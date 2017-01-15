using LeaRun.Application.Entity.ExtendManage;
using LeaRun.Util.WebControl;
using System.Collections.Generic;
using LeaRun.Application.Entity.PublicInfoManage;

namespace LeaRun.Application.IService.ExtendManage
{
    public interface BannerNewsIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<BannerNewsEntity> GetList(string queryJson);
        IEnumerable<BannerNewsEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        BannerNewsEntity GetEntity(string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, BannerNewsEntity entity);
        #endregion
    }
}
