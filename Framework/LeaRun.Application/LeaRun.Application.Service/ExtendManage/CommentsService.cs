using LeaRun.Application.Entity.ExtendManage;
using LeaRun.Application.IService.ExtendManage;
using LeaRun.Data.Repository;
using LeaRun.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using LeaRun.Application.Entity.CustomerManage;
using LeaRun.Util.Extension;

namespace LeaRun.Application.Service.ExtendManage
{
    public class CommentsService : RepositoryFactory<CommentsEntity>, CommentsIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<CommentsEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }

        public IEnumerable<CommentsEntity> GetList(string CustomerId, string NewsId) {
            var expression = LinqExtensions.True<CommentsEntity>();
            expression = expression.And(t => t.NewsId == NewsId);
            if (!string.IsNullOrEmpty(CustomerId)) {
                expression = expression.And(t => t.CustomerId != CustomerId);
            }
            return this.BaseRepository().IQueryable(expression).OrderBy(t=>t.CreateDate);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CommentsEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CommentsEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, CommentsEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        public CommentsEntity SaveFormData(string keyValue, CommentsEntity entity) {
            if (!string.IsNullOrEmpty(keyValue)) {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
            return entity;
        }
        #endregion
    }
}
