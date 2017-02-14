using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaRun.Application.Busines.PublicInfoManage;
using LeaRun.Application.Code;
using LeaRun.Application.Entity.BaseManage;
using LeaRun.Application.Entity.PublicInfoManage;
using LeaRun.Util;
using LeaRun.Util.WebControl;

namespace LeaRun.Application.Web.Areas.ExtendManage.Controllers
{
    public class BooksController : MvcControllerBase {
        private NewsBLL newsBLL = new NewsBLL();

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
        public ActionResult UploadImage(HttpPostedFileBase file) {
            if (file != null && file.ContentType.StartsWith("image/"))
            {
                string FileEextension = Path.GetExtension(file.FileName);
                string virtualPath = string.Format("/Resource/BookFile/{0}{1}", Guid.NewGuid(), FileEextension);
                string fullFileName = Server.MapPath("~" + virtualPath);
                //创建文件夹，保存文件
                string path = Path.GetDirectoryName(fullFileName);
                Directory.CreateDirectory(path);
                file.SaveAs(fullFileName);

                return Success("上传成功。", virtualPath);
            }
            else
            {
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
