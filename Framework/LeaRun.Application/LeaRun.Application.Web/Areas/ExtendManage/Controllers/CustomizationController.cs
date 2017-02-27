using System.Linq;
using LeaRun.Application.Entity.ExtendManage;
using LeaRun.Application.Busines.ExtendManage;
using LeaRun.Util;
using LeaRun.Util.WebControl;
using System.Web.Mvc;
using LeaRun.Application.Busines.BaseManage;
using LeaRun.Application.Busines.CustomerManage;
using LeaRun.Application.Code;
using LeaRun.Application.Service.ExtendManage;
using LeaRun.Util.Extension;

namespace LeaRun.Application.Web.Areas.ExtendManage.Controllers
{
    public class CustomizationController : MvcControllerBase
    {
        private CustomizationBLL customizationbll = new CustomizationBLL();
        private CustomerBLL customerBll = new CustomerBLL();
        private CustomizationService service = new CustomizationService();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Customiza() {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
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
        public ActionResult GetPageListJson(Pagination pagination, string queryJson) {
            var queryParam = queryJson.ToJObject();
            var data = customizationbll.GetList(queryJson);
            if (!queryParam["TypeId"].IsEmpty()) {
                data = data.Where(t => t.TypeId.ToString() == queryParam["TypeId"].ToString());
            }
            if (!queryParam["FullHead"].IsEmpty())
            {
                var key = queryParam["FullHead"].ToString();
                data = data.Where(t => t.Frame == key|| t.Description == key || t.Material == key || t.Size == key || t.Contact == key);
            }
            var users = customerBll.GetList();
            var result = (from a1 in data
                          join user in users on a1.CreateUserId equals user.CustomerId into cu
                          from c in cu.DefaultIfEmpty() select new {a1.Category,a1.Contact,a1.ContactTime,a1.CreateDate,a1.CustomizationId,a1.Description,a1.Frame,a1.Material,a1.Size, FullName = c==null? "":c.FullName, Email = c == null ? "" : c.Email, Mobile = c == null ? "" : c.Mobile}).ToList();
            pagination.records = result.Count;
            var list =
                result.OrderByDescending(x => x.CreateDate)
                    .Skip(pagination.rows * (pagination.page - 1))
                    .Take(pagination.rows);
            var res = new {
                rows = list,
                pagination.total,
                pagination.page,
                pagination.records
            };
            return ToJsonResult(res);
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = customizationbll.GetEntity(keyValue);
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
        public ActionResult RemoveForm(string keyValue)
        {
            customizationbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, CustomizationEntity entity)
        {
            customizationbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }

        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerFrontLogin(LoginMode.Ignore, LoginType.FrontEnd)]
        public ActionResult SaveFormFront(string keyValue, CustomizationEntity entity) {
            service.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
