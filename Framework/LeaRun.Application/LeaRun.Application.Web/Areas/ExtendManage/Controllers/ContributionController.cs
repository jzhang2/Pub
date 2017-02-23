using LeaRun.Application.Entity.ExtendManage;
using LeaRun.Application.Busines.ExtendManage;
using LeaRun.Util;
using LeaRun.Util.WebControl;
using System.Web.Mvc;
using LeaRun.Application.Code;

namespace LeaRun.Application.Web.Areas.ExtendManage.Controllers {
    public class ContributionController : MvcControllerBase {
        private ContributionBLL contributionbll = new ContributionBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ContributionIndex() {
            return View();
        }
        [HttpGet]
        public ActionResult Reply() {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ContributionForm() {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson) {
            var data = contributionbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue) {
            var data = contributionbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue) {
            contributionbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, ContributionEntity entity) {
            contributionbll.SaveForm(keyValue, entity);
            return Success("投稿成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerFrontLogin(LoginMode.Enforce, LoginType.FrontEnd)]
        public ActionResult SaveFormFront(string keyValue, ContributionEntity entity) {
            entity.CustomerId = OperatorProvider.Provider.Current().UserId;
            var result = contributionbll.SaveEntity(keyValue, entity);
            return Success("投稿成功。", result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveReply(string keyValue, ContributionEntity entity) {
            contributionbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public void DownloadFile(string keyValue) {
            var data = contributionbll.GetEntity(keyValue);
            string filename = Server.UrlDecode(data.FileName);//返回客户端文件名称
            string filepath = this.Server.MapPath(data.FilePath);
            if (FileDownHelper.FileExists(filepath)) {
                FileDownHelper.DownLoadold(filepath, filename);
            }
        }
        [HttpPost]
        [HandlerFrontLogin(LoginMode.Enforce, LoginType.FrontEnd)]
        public void Download(string keyValue) {
            var data = contributionbll.GetEntity(keyValue);
            string filename = Server.UrlDecode(data.FileName);//返回客户端文件名称
            string filepath = this.Server.MapPath(data.FilePath);
            if (FileDownHelper.FileExists(filepath)) {
                FileDownHelper.DownLoadold(filepath, filename);
            }
        }
        #endregion
    }
}
