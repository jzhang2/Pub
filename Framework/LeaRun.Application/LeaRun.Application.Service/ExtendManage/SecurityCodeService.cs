using LeaRun.Application.Entity.ExtendManage;
using LeaRun.Application.IService.ExtendManage;
using LeaRun.Data.Repository;
using LeaRun.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using LeaRun.Application.Entity.BaseManage;
using LeaRun.Util.Extension;

namespace LeaRun.Application.Service.ExtendManage {
    public class SecurityCodeService : RepositoryFactory<SecurityCodeEntity>, SecurityCodeIService {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SecurityCodeEntity> GetList(string queryJson) {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SecurityCodeEntity GetEntity(string keyValue) {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue) {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SecurityCodeEntity entity) {
            if (!string.IsNullOrEmpty(keyValue)) {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion

        public SecurityCodeEntity GetSecurityCode(string mobileCode) {
            var expression = LinqExtensions.True<SecurityCodeEntity>();
            expression = expression.And(t => t.Email == mobileCode);
            return this.BaseRepository().IQueryable(expression).OrderByDescending(t => t.CreateDate).FirstOrDefault();
        }

        public SecurityCodeEntity GetCode(string mobileCode) {
            var expression = LinqExtensions.True<SecurityCodeEntity>();
            expression = expression.And(t => t.SecurityCode == mobileCode);
            return this.BaseRepository().IQueryable(expression).OrderByDescending(t => t.CreateDate).FirstOrDefault();
        }
    }
}
