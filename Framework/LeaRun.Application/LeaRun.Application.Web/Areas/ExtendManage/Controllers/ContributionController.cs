using LeaRun.Application.Entity.ExtendManage;
using LeaRun.Application.Busines.ExtendManage;
using LeaRun.Util;
using LeaRun.Util.WebControl;
using System.Web.Mvc;
using LeaRun.Application.Code;

namespace LeaRun.Application.Web.Areas.ExtendManage.Controllers {
    public class ContributionController : MvcControllerBase {
        private ContributionBLL contributionbll = new ContributionBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
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
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ContributionForm() {
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
            var data = contributionbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue) {
            var data = contributionbll.GetEntity(keyValue);
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
            contributionbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, ContributionEntity entity) {
            contributionbll.SaveForm(keyValue, entity);
            return Success("Ͷ��ɹ���");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerFrontLogin(LoginMode.Enforce, LoginType.FrontEnd)]
        public ActionResult SaveFormFront(string keyValue, ContributionEntity entity) {
            entity.CustomerId = OperatorProvider.Provider.Current().UserId;
            var result = contributionbll.SaveEntity(keyValue, entity);
            return Success("Ͷ��ɹ���", result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveReply(string keyValue, ContributionEntity entity) {
            contributionbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        [HttpPost]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public void DownloadFile(string keyValue) {
            var data = contributionbll.GetEntity(keyValue);
            string filename = Server.UrlDecode(data.FileName);//���ؿͻ����ļ�����
            string filepath = this.Server.MapPath(data.FilePath);
            if (FileDownHelper.FileExists(filepath)) {
                FileDownHelper.DownLoadold(filepath, filename);
            }
        }
        [HttpPost]
        [HandlerFrontLogin(LoginMode.Enforce, LoginType.FrontEnd)]
        public void Download(string keyValue) {
            var data = contributionbll.GetEntity(keyValue);
            string filename = Server.UrlDecode(data.FileName);//���ؿͻ����ļ�����
            string filepath = this.Server.MapPath(data.FilePath);
            if (FileDownHelper.FileExists(filepath)) {
                FileDownHelper.DownLoadold(filepath, filename);
            }
        }
        #endregion
    }
}
