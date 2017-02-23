using System;
using LeaRun.Application.Code;

namespace LeaRun.Application.Entity.ExtendManage {
    public class SuggestionEntity : BaseEntity {
        #region ʵ���Ա
        /// <summary>
        /// SuggestionId
        /// </summary>
        /// <returns></returns>
        public string SuggestionId { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <returns></returns>
        public string Title { get; set; }
        /// <summary>
        /// Contents
        /// </summary>
        /// <returns></returns>
        public string Contents { get; set; }
        /// <summary>
        /// CusId
        /// </summary>
        /// <returns></returns>
        public string CusId { get; set; }
        /// <summary>
        /// ����1-��ͼ��̬-2-��ͼ�Ļ�
        /// </summary>
        /// <returns></returns>
        public int? Category { get; set; }
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
        /// IsReply
        /// </summary>
        /// <returns></returns>
        public int? IsReply { get; set; }
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
        public override void Create() {
            this.SuggestionId = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            if (OperatorProvider.Provider.Current() != null && OperatorProvider.Provider.Current().UserId != null) {
                this.CreateUserId = OperatorProvider.Provider.Current().UserId;
                this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            }
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue) {
            this.SuggestionId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}