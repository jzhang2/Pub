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
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<BookMarkEntity> GetPageList(Pagination pagination, string queryJson) {
            return this.BaseRepository().FindList(pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<BookMarkEntity> GetList(string queryJson) {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public BookMarkEntity GetEntity(string keyValue) {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue) {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
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
