using LeaRun.Application.Entity.ExtendManage;
using LeaRun.Application.IService.ExtendManage;
using LeaRun.Data.Repository;
using LeaRun.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System;
using LeaRun.Application.Entity.BaseManage;
using LeaRun.Util.Extension;

namespace LeaRun.Application.Service.ExtendManage {
    public class BookMarkService : RepositoryFactory<BookMarkEntity>, BookMarkIService {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<BookMarkEntity> GetPageList(Pagination pagination, string queryJson) {
            return this.BaseRepository().FindList(pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<BookMarkEntity> GetList(string queryJson) {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public BookMarkEntity GetEntity(string keyValue) {
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
        public void SaveForm(string keyValue, BookMarkEntity entity) {
            if (!string.IsNullOrEmpty(keyValue)) {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }

        public List<BookMarkEntity> GetList(string userId, string newsId, string page,string type) {
            var expression = LinqExtensions.True<BookMarkEntity>();
            if (!string.IsNullOrEmpty(newsId)) {
                expression = expression.And(t => t.NewsId.Equals(newsId));
            }
            if (!string.IsNullOrEmpty(page)) {
                expression = expression.And(t => t.Page.ToString() == page);
            }
            if (!string.IsNullOrEmpty(type)) {
                expression = expression.And(t => t.Type.ToString() == type);
            }
            return this.BaseRepository().IQueryable(expression).OrderBy(t => t.Page).ToList();
        }
        #endregion
    }
}
