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

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index() {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form() {
            return View();
        }
        /// <summary>
        /// �ظ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Reply() {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson) {
            var data = suggestionbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue) {
            var data = suggestionbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ�ظ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetReply(string keyValue) {
            var data = suggestionAnswerIService.GetEntityBySuggestion(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue) {
            suggestionbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, SuggestionEntity entity) {
            suggestionbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
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
            return Success("�����ɹ���");
        }

        /// <summary>
        /// �����˻�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult DisabledAccount(string keyValue) {
            if (keyValue == "System") {
                throw new Exception("��ǰ�˻�������");
            }
            suggestionbll.UpdateState(keyValue, 0);
            return Success("�˻����óɹ���");
        }
        /// <summary>
        /// �����˻�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult EnabledAccount(string keyValue) {
            suggestionbll.UpdateState(keyValue, 1);
            return Success("�˻����óɹ���");
        }
        #endregion
    }
}
