using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CsQuery;
using LeaRun.Application.Busines.PublicInfoManage;
using LeaRun.Application.Code;
using LeaRun.Application.Entity.BaseManage;
using LeaRun.Application.Entity.ExtendManage;
using LeaRun.Application.Entity.PublicInfoManage;
using LeaRun.Application.Web.Controllers;
using LeaRun.Data.Repository;
using LeaRun.Util;
using LeaRun.Util.WebControl;
using Microsoft.Office.Interop.Word;
using Config = LeaRun.Util.Config;

namespace LeaRun.Application.Web.Areas.ExtendManage.Controllers {
    public class FileInfo {
        public string fileName { get; set; }
        public string filePath { get; set; }
    }
    public class BooksController : MvcControllerBase {
        private NewsBLL newsBLL = new NewsBLL();
        private string baseUrl = Config.GetValue("EBookSite");
        #region 视图功能
        /// <summary>
        /// 新闻管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Index() {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Ebook() {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult EForm() {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult PBook() {
            return View();
        }

        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult PBookForm() {
            return View();
        }
        /// <summary>
        /// 新闻表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Form() {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 新闻列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson) {
            var watch = CommonHelper.TimerStart();
            var data = newsBLL.GetPageList(pagination, queryJson);
            var JsonData = new {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 新闻实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue) {
            var data = newsBLL.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除新闻
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult RemoveForm(string keyValue) {
            newsBLL.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存新闻表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="newsEntity">新闻实体</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [ValidateInput(false)]
        public ActionResult SaveForm(string keyValue, NewsEntity newsEntity) {
            newsBLL.SaveForm(keyValue, newsEntity);
            return Success("操作成功。");
        }
        [HttpPost]
        [AjaxOnly]
        [ValidateInput(false)]
        public ActionResult SaveBook(string keyValue, NewsEntity newsEntity) {
            var result = "";
            try {
                int totalPagesCount = 1;
                var ePath = string.Format("/Resource/EBook/{0}/", Guid.NewGuid());
                if (string.IsNullOrEmpty(keyValue) && string.IsNullOrEmpty(newsEntity.FilePath)) {
                    return Error("上传书籍失败，请上传书签文档。");
                }
                var import = true;
                var entity = new NewsEntity();
                if (!string.IsNullOrEmpty(keyValue)) {
                    newsEntity.Modify(keyValue);
                    var book = newsBLL.GetEntity(keyValue);
                    if (book != null) {
                        entity = book;
                    }
                }
                else {
                    newsEntity.Create();
                }

                var tableList = new List<BookTableEntity>();
                if (!string.IsNullOrEmpty(newsEntity.FilePath) && newsEntity.FilePath != entity.FilePath) {
                    import = ImportBook(Server.MapPath("~" + newsEntity.FilePath), ref totalPagesCount, ePath);
                    if (import) {
                        newsEntity.PageCount = totalPagesCount;
                        for (int i = 1; i <= newsEntity.PageCount; i++) {
                            string url = string.Format(baseUrl + ePath + i + ".html");
                            var dom = CQ.CreateFromUrl(url);
                            var table = dom.Select("[docparttype='Table of Contents'] a");
                            if (tableList.Count == 0 && table.Length > 0) {
                                newsEntity.BookTablePage = i;
                                tableList.AddRange(table.Select(domObject => new BookTableEntity() { BookTableId = Guid.NewGuid().ToString(), Toc = domObject["href"], NewsId = newsEntity.NewsId }));
                            }

                            foreach (BookTableEntity key in tableList) {
                                var mark = dom.Select("a[name='" + key.Toc.Replace("#", "") + "']");
                                if (mark.Length > 0) {
                                    key.Page = i;
                                }
                            }
                        }
                    }
                }
                if (import) {
                    if (!string.IsNullOrEmpty(newsEntity.FilePath)) {
                        newsEntity.EPath = newsEntity.FilePath == entity.FilePath ? entity.EPath : ePath;
                    }
                    IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
                    try {
                        if (!string.IsNullOrEmpty(keyValue)) {
                            db.Update(newsEntity);
                            if (tableList.Count > 0) {
                                db.Delete<BookTableEntity>(t => t.NewsId == newsEntity.NewsId);
                                db.Insert(tableList);
                            }
                        }
                        else {
                            db.Insert(newsEntity);
                            if (tableList.Count > 0) {
                                db.Insert(tableList);
                            }
                        }
                        db.Commit();
                        return Success("操作成功。");
                    }
                    catch (Exception) {
                        db.Rollback();
                        throw;
                    }
                }
                else {
                    return Error("上传书籍失败，请检查文档是否正确。");
                }
            }
            catch (Exception ee) {
                return Error(ee.Message);
            }
        }

        public bool ImportBook(object path, ref int totalPagesCount, string ePath) {
            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            object missObj = Missing.Value;
            object saveChanges = WdSaveOptions.wdDoNotSaveChanges;
            try {
                string fullPath = Server.MapPath("~" + ePath);
                Directory.CreateDirectory(fullPath);


                object ReadOnly = false;
                object Visible = false;

                Microsoft.Office.Interop.Word.Document doc = app.Documents.Open(ref path, ref missObj, ref ReadOnly,
                    ref missObj, ref missObj, ref missObj, ref missObj, ref missObj, ref missObj, ref missObj,
                    ref missObj, ref Visible, ref missObj, ref missObj, ref missObj, ref missObj);

                object wdWhat = WdGoToItem.wdGoToPage;
                object wdWhich = WdGoToDirection.wdGoToAbsolute;

                totalPagesCount = doc.ComputeStatistics(WdStatistic.wdStatisticPages, ref missObj);

                int pageNumber = 1;
                object currentPageIndex = pageNumber;
                Range range = doc.Range(ref missObj, ref missObj);
                Range pageRange = doc.Range(ref missObj, ref missObj);
                object CurrentPageNumber;
                object NextPageNumber;
                object Start;
                object End;
                object what = WdGoToItem.wdGoToPage;
                object which = WdGoToDirection.wdGoToFirst;
                do {
                    doc.Activate();
                    CurrentPageNumber = (Convert.ToInt32(pageNumber.ToString()));
                    NextPageNumber = (Convert.ToInt32((pageNumber + 1).ToString()));

                    // Get start position of current page
                    Start = app.Selection.GoTo(ref what, ref which, ref CurrentPageNumber, ref missObj).Start;

                    // Get end position of current page                                
                    End = app.Selection.GoTo(ref what, ref which, ref NextPageNumber, ref missObj).End;

                    // Get text
                    if (Convert.ToInt32(Start.ToString()) != Convert.ToInt32(End.ToString()))
                        doc.Range(ref Start, ref End).Select();
                    else
                        doc.Range(ref Start).Select();
                    app.Selection.Copy();

                    // Generates new doc files.
                    Document newDoc = app.Documents.Add(ref missObj, ref missObj, ref missObj, ref missObj);
                    //newDoc.Content.FormattedText = pageRange.FormattedText;
                    newDoc.PageSetup.PageWidth = doc.PageSetup.PageWidth;
                    newDoc.PageSetup.PageHeight = doc.PageSetup.PageHeight;
                    app.Selection.PasteAndFormat(Microsoft.Office.Interop.Word.WdRecoveryType.wdPasteDefault);

                    //Microsoft.Office.Interop.Word.Application word1 = new Microsoft.Office.Interop.Word.Application();
                    //object outputFileName = doc.Name.Replace(".docx", ".html");
                    object fileFormat = WdSaveFormat.wdFormatHTML;

                    object newDocName = Server.MapPath("~" + ePath + pageNumber + ".html");
                    newDoc.Fields.Update();
                    newDoc.WebOptions.Encoding = Microsoft.Office.Core.MsoEncoding.msoEncodingUTF8;
                    newDoc.SaveAs(ref newDocName, ref fileFormat, ref missObj, ref missObj, ref missObj, ref missObj,
                        ref missObj, ref missObj, ref missObj, ref missObj, ref missObj, ref missObj, ref missObj,
                        ref missObj, ref missObj, ref missObj);
                    ((Microsoft.Office.Interop.Word._Document)newDoc).Close(ref saveChanges, ref missObj, ref missObj);
                    newDoc = null;

                    pageNumber++;
                    if ((int)currentPageIndex == totalPagesCount) {
                        try {
                            app.Documents.Close(ref saveChanges, ref missObj, ref missObj);
                        }
                        catch (Exception) {
                        }
                    }
                } while (pageNumber <= totalPagesCount);
                app.Quit(ref saveChanges, ref missObj, ref missObj);
                return true;
            }
            catch (Exception ex) {
                app.Quit(ref saveChanges, ref missObj, ref missObj);
                throw ex;
            }
        }

        public bool ImportFullBook(object path, ref int totalPagesCount, string ePath) {
            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            object missObj = Missing.Value;
            object saveChanges = WdSaveOptions.wdDoNotSaveChanges;
            try {
                string fullPath = Server.MapPath("~" + ePath);
                Directory.CreateDirectory(fullPath);


                object ReadOnly = false;
                object Visible = false;

                Microsoft.Office.Interop.Word.Document doc = app.Documents.Open(ref path, ref missObj, ref ReadOnly,
                    ref missObj, ref missObj, ref missObj, ref missObj, ref missObj, ref missObj, ref missObj,
                    ref missObj, ref Visible, ref missObj, ref missObj, ref missObj, ref missObj);

                doc.Activate();

                object fileFormat = WdSaveFormat.wdFormatHTML;

                object newDocName = Server.MapPath("~" + ePath + 1 + ".html");
                doc.Fields.Update();
                doc.WebOptions.Encoding = Microsoft.Office.Core.MsoEncoding.msoEncodingUTF8;
                doc.SaveAs(ref newDocName, ref fileFormat, ref missObj, ref missObj, ref missObj, ref missObj,
                        ref missObj, ref missObj, ref missObj, ref missObj, ref missObj, ref missObj, ref missObj,
                        ref missObj, ref missObj, ref missObj);
                ((Microsoft.Office.Interop.Word._Document)doc).Close(ref saveChanges, ref missObj, ref missObj);

                try {
                    app.Documents.Close(ref saveChanges, ref missObj, ref missObj);
                }
                catch (Exception) {
                }
                app.Quit(ref saveChanges, ref missObj, ref missObj);
                return true;
            }
            catch (Exception ex) {
                app.Quit(ref saveChanges, ref missObj, ref missObj);
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file) {
            if (file != null && file.ContentType.StartsWith("image/")) {
                string FileEextension = Path.GetExtension(file.FileName);
                string virtualPath = string.Format("/Resource/BookFile/{0}{1}", Guid.NewGuid(), FileEextension);
                string fullFileName = Server.MapPath("~" + virtualPath);
                //创建文件夹，保存文件
                string path = Path.GetDirectoryName(fullFileName);
                Directory.CreateDirectory(path);
                file.SaveAs(fullFileName);

                return Success("上传成功。", virtualPath);
            }
            else {
                return HttpNotFound();
            }
        }


        public ActionResult UploadFile() {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            //没有文件上传，直接返回
            if (files[0].ContentLength == 0 || string.IsNullOrEmpty(files[0].FileName)) {
                return HttpNotFound();
            }
            string FileEextension = Path.GetExtension(files[0].FileName);
            string virtualPath = string.Format("/Resource/BookFile/{0}{1}", Guid.NewGuid(), FileEextension);
            string fullFileName = Server.MapPath("~" + virtualPath);
            //创建文件夹，保存文件
            string path = Path.GetDirectoryName(fullFileName);
            Directory.CreateDirectory(path);
            files[0].SaveAs(fullFileName);

            return Success("上传成功。", virtualPath);
        }

        [HandlerFrontLogin(LoginMode.Enforce, LoginType.FrontEnd)]
        public ActionResult UploadDocument() {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            //没有文件上传，直接返回
            if (files[0].ContentLength == 0 || string.IsNullOrEmpty(files[0].FileName)) {
                return HttpNotFound();
            }
            string FileEextension = Path.GetExtension(files[0].FileName);
            string virtualPath = string.Format("/Resource/BookFile/{0}{1}", Guid.NewGuid(), FileEextension);
            string fullFileName = Server.MapPath("~" + virtualPath);
            //创建文件夹，保存文件
            string path = Path.GetDirectoryName(fullFileName);
            Directory.CreateDirectory(path);
            files[0].SaveAs(fullFileName);

            return Success("上传成功。", new FileInfo() { fileName = files[0].FileName, filePath = virtualPath });
        }
        /// <summary>
        /// 禁用账户
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult DisabledAccount(string keyValue) {
            newsBLL.UpdateState(keyValue, 0);
            return Success("书籍下架成功。");
        }
        /// <summary>
        /// 启用账户
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult EnabledAccount(string keyValue) {
            newsBLL.UpdateState(keyValue, 1);
            return Success("书籍上架成功。");
        }

        [HttpPost]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Disrecommed(string keyValue) {
            newsBLL.UpdateRecommed(keyValue, 0);
            return Success("书籍取消推荐成功。");
        }
        /// <summary>
        /// 启用账户
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Recommed(string keyValue) {
            newsBLL.UpdateRecommed(keyValue, 1);
            return Success("书籍推荐成功。");
        }
        #endregion
    }

}
