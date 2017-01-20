using LeaRun.Application.Entity.ExtendManage;
using LeaRun.Application.IService.ExtendManage;
using LeaRun.Data.Repository;
using LeaRun.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace LeaRun.Application.Service.ExtendManage {
    public class SuggestionService : RepositoryFactory<SuggestionEntity>, SuggestionIService {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SuggestionEntity> GetList(string queryJson) {
            return this.BaseRepository().IQueryable().ToList();
        }
        public IEnumerable<SuggestionEntity> GetUserSuggestion(string userId) {
            return this.BaseRepository().IQueryable(t => t.CreateUserId == userId).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SuggestionEntity GetEntity(string keyValue) {
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
        public void SaveForm(string keyValue, SuggestionEntity entity) {
            if (!string.IsNullOrEmpty(keyValue)) {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        public void UpdateState(string keyValue, int State) {
            SuggestionEntity userEntity = new SuggestionEntity();
            userEntity.Modify(keyValue);
            userEntity.EnabledMark = State;
            this.BaseRepository().Update(userEntity);
        }
        #endregion
    }
}
