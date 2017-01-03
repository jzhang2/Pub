using System;
using LeaRun.Application.Entity.ExtendManage;
using LeaRun.Application.Busines.ExtendManage;
using LeaRun.Util;
using LeaRun.Util.WebControl;
using System.Web.Mvc;
using LeaRun.Application.Code;
using LeaRun.Application.IService.ExtendManage;
using LeaRun.Application.Service.ExtendManage;

namespace LeaRun.Application.Web.Areas.ExtendManage.Controllers {
    public class SuggestionController : MvcControllerBase {
        private SuggestionBLL suggestionbll = new SuggestionBLL();
        private SuggestionAnswerService suggestionAnswerIService = new SuggestionAnswerService();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index() {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form() {
            return View();
        }
        /// <summary>
        /// 回复
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Reply() {
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
            var data = suggestionbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue) {
            var data = suggestionbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取回复
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetReply(string keyValue) {
            var data = suggestionAnswerIService.GetEntityBySuggestion(keyValue);
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
            suggestionbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SuggestionEntity entity) {
            suggestionbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveReply(string keyValue, string reply) {
            SuggestionAnswerEntity suggestionAnswerEntity = suggestionAnswerIService.GetEntityBySuggestion(keyValue) ??
                                                            new SuggestionAnswerEntity();
            suggestionAnswerEntity.SuggestionId = keyValue;
            suggestionAnswerEntity.Contents = reply;
            suggestionAnswerIService.SaveForm(suggestionAnswerEntity.AnswerId, suggestionAnswerEntity);
            SuggestionEntity suggestionEntity = suggestionbll.GetEntity(keyValue);
            suggestionEntity.IsReply = 1;
            suggestionbll.SaveForm(keyValue,suggestionEntity);
            return Success("操作成功。");
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
            if (keyValue == "System") {
                throw new Exception("当前账户不禁用");
            }
            suggestionbll.UpdateState(keyValue, 0);
            return Success("账户禁用成功。");
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
            suggestionbll.UpdateState(keyValue, 1);
            return Success("账户启用成功。");
        }
        #endregion
    }
}
