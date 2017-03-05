using System;
using LeaRun.Application.Code;

namespace LeaRun.Application.Entity.ExtendManage
{
    public class CustomizationEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// CustomizationId
        /// </summary>
        /// <returns></returns>
        public string CustomizationId { get; set; }
        /// <summary>
        /// ���ͣ�1-����2-����3-��ͼ��̬4-��ͼ�Ļ�5-������6-ʵ����7-ҵ�����8-�������ǣ�
        /// </summary>
        /// <returns></returns>
        public int? TypeId { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        public string Category { get; set; }
        /// <summary>
        /// Size
        /// </summary>
        /// <returns></returns>
        public string Size { get; set; }
        /// <summary>
        /// Material
        /// </summary>
        /// <returns></returns>
        public string Material { get; set; }
        /// <summary>
        /// Frame
        /// </summary>
        /// <returns></returns>
        public string Frame { get; set; }
        /// <summary>
        /// Contact
        /// </summary>
        /// <returns></returns>
        public string Contact { get; set; }

        public string Contactor { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        /// <returns></returns>
        public string Description { get; set; }
        /// <summary>
        /// ContactTime
        /// </summary>
        /// <returns></returns>
        public string ContactTime { get; set; }
        /// <summary>
        /// ɾ�����
        /// </summary>
        /// <returns></returns>
        public int? DeleteMark { get; set; }
        /// <summary>
        /// ��Ч��־
        /// </summary>
        /// <returns></returns>
        public int? EnabledMark { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        /// <returns></returns>
        public string Status { get; set; }
        /// <summary>
        /// Reply
        /// </summary>
        /// <returns></returns>
        public string Reply { get; set; }
        /// <summary>
        /// �ͻ�����
        /// </summary>
        /// <returns></returns>
        public string CustomerId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �����û�����
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        public string CreateUserName { get; set; }
        /// <summary>
        /// �޸�����
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// �޸��û�����
        /// </summary>
        /// <returns></returns>
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �޸��û�
        /// </summary>
        /// <returns></returns>
        public string ModifyUserName { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.CustomizationId = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            if (OperatorProvider.Provider.Current() != null && OperatorProvider.Provider.Current().UserId != null)
            {
                this.CreateUserId = OperatorProvider.Provider.Current().UserId;
                this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            }
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.CustomizationId = keyValue;
            this.ModifyDate = DateTime.Now;
            if (OperatorProvider.Provider.Current() != null && OperatorProvider.Provider.Current().UserId != null)
            {
                this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
                this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
            }
        }
        #endregion
    }
}