using LeaRun.Application.Entity.CustomerManage;
using LeaRun.Application.IService.CustomerManage;
using LeaRun.Application.Service.CustomerManage;
using LeaRun.Util.WebControl;
using System.Collections.Generic;
using System;
using LeaRun.Application.Entity.BaseManage;
using LeaRun.Util;

namespace LeaRun.Application.Busines.CustomerManage
{
    /// <summary>
    /// �� �� 6.1
    /// Copyright (c) 2013-2016 �Ϻ�������Ϣ�������޹�˾
    /// �� �����ܴ���
    /// �� �ڣ�2016-03-14 09:47
    /// �� �����ͻ���Ϣ
    /// </summary>
    public class CustomerBLL
    {
        private ICustomerService service = new CustomerService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<CustomerEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public CustomerEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region ��֤����
        /// <summary>
        /// �ͻ����Ʋ����ظ�
        /// </summary>
        /// <param name="fullName">����</param>
        /// <param name="keyValue">����</param>
        /// <returns></returns>
        public bool ExistFullName(string fullName, string keyValue)
        {
            return service.ExistFullName(fullName, keyValue);
        }
        public bool ExistEmail(string fullName, string keyValue) {
            return service.ExistEmail(fullName, keyValue);
        }
        public bool ExistMobile(string fullName, string keyValue) {
            return service.ExistMobile(fullName, keyValue);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, CustomerEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// �����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity">ʵ��</param>
        /// <param name="moduleId">ģ��</param>
        public void SaveForm(string keyValue, CustomerEntity entity, string moduleId)
        {
            try
            {
                service.SaveForm(keyValue, entity, moduleId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public CustomerEntity CheckLogin(string username, string password) {
            CustomerEntity userEntity = service.CheckLogin(username);
            if (userEntity != null) {
                if (userEntity.EnabledMark == 1) {
                    string dbPassword = Md5Helper.MD5(DESEncrypt.Encrypt(password.ToLower(), userEntity.Secretkey).ToLower(), 32).ToLower();
                    if (dbPassword == userEntity.Password) {
                        DateTime LastVisit = DateTime.Now;
                        //int LogOnCount = (userEntity.LogOnCount).ToInt() + 1;
                        //if (userEntity.LastVisit != null) {
                        //    userEntity.PreviousVisit = userEntity.LastVisit.ToDate();
                        //}
                        //userEntity.LastVisit = LastVisit;
                        //userEntity.LogOnCount = LogOnCount;
                        //userEntity.UserOnLine = 1;
                        //service.UpdateEntity(userEntity);
                        return userEntity;
                    }
                    else {
                        throw new Exception("������˻�����ƥ��");
                        //throw new Exception("������˻�����ƥ��" + "|" + password + "|" + dbPassword);
                    }
                }
                else {
                    throw new Exception("�˻�����ϵͳ����,����ϵ����Ա");
                }
            }
            else {
                throw new Exception("�˻������ڣ�����������");
            }
        }

    }
}
