using System;
using LeaRun.Application.Code;

namespace LeaRun.Application.Entity.ExtendManage
{
    public class SecurityCodeEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// SecurityId
        /// </summary>
        /// <returns></returns>
        public string SecurityId { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        /// <returns></returns>
        public string Email { get; set; }
        /// <summary>
        /// SecurityCode
        /// </summary>
        /// <returns></returns>
        public string SecurityCode { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.SecurityId = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
                                }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.SecurityId = keyValue;
                                            }
        #endregion
    }
}