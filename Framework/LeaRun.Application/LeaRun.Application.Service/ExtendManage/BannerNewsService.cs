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
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<BannerNewsEntity> GetList(string queryJson) {
            return this.BaseRepository().IQueryable().ToList();
        }
        public IEnumerable<BannerNewsEntity> GetPageList(Pagination pagination, string queryJson) {
            var expression = LinqExtensions.True<BannerNewsEntity>();
            var queryParam = queryJson.ToJObject();

            //查询条件
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
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public BannerNewsEntity GetEntity(string keyValue) {
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
