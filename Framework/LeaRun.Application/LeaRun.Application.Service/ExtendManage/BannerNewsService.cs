using LeaRun.Application.Entity.ExtendManage;
using LeaRun.Application.IService.ExtendManage;
using LeaRun.Data.Repository;
using LeaRun.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using LeaRun.Util;
using LeaRun.Util.Extension;

namespace LeaRun.Application.Service.ExtendManage {
    public class BannerNewsService : RepositoryFactory<BannerNewsEntity>, BannerNewsIService {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<BannerNewsEntity> GetList(string queryJson) {
            return this.BaseRepository().IQueryable().ToList();
        }
        public IEnumerable<BannerNewsEntity> GetPageList(Pagination pagination, string queryJson) {
            var expression = LinqExtensions.True<BannerNewsEntity>();
            var queryParam = queryJson.ToJObject();

            //��ѯ����
            if (!queryParam["keyword"].IsEmpty()) {
                string keyord = queryParam["keyword"].ToString();
                expression = expression.And(t => t.Title.Contains(keyord));
            }
            if (!queryParam["Type"].IsEmpty()) {
                string Type = queryParam["Type"].ToString();
                expression = expression.And(t => t.Type.ToString()== Type);
            }
            //expression = expression.And(t => t.UserId != "System");
            return this.BaseRepository().FindList(expression, pagination);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public BannerNewsEntity GetEntity(string keyValue) {
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
        public void SaveForm(string keyValue, BannerNewsEntity entity) {
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
    }
}
