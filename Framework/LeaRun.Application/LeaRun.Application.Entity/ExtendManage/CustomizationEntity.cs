using System;
using LeaRun.Application.Code;

namespace LeaRun.Application.Entity.ExtendManage
{
    public class CustomizationEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// CustomizationId
        /// </summary>
        /// <returns></returns>
        public string CustomizationId { get; set; }
        /// <summary>
        /// 类型（1-新闻2-公告3-西图动态4-地图文化5-电子书6-实体书7-业务服务8-关于我们）
        /// </summary>
        /// <returns></returns>
        public int? TypeId { get; set; }
        /// <summary>
        /// 所属类别
        /// </summary>
        /// <returns></returns>
        public string Category { get; set; }
        /// <summary>
        /// Size
        /// </summary>
        /// <returns></returns>
        public string Size { get; set; }
        /// <summary>
        /// Material
        /// </summary>
        /// <returns></returns>
        public string Material { get; set; }
        /// <summary>
        /// Frame
        /// </summary>
        /// <returns></returns>
        public string Frame { get; set; }
        /// <summary>
        /// Contact
        /// </summary>
        /// <returns></returns>
        public string Contact { get; set; }

        public string Contactor { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        /// <returns></returns>
        public string Description { get; set; }
        /// <summary>
        /// ContactTime
        /// </summary>
        /// <returns></returns>
        public string ContactTime { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        /// <returns></returns>
        public int? DeleteMark { get; set; }
        /// <summary>
        /// 有效标志
        /// </summary>
        /// <returns></returns>
        public int? EnabledMark { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        /// <returns></returns>
        public string Status { get; set; }
        /// <summary>
        /// Reply
        /// </summary>
        /// <returns></returns>
        public string Reply { get; set; }
        /// <summary>
        /// 客户主键
        /// </summary>
        /// <returns></returns>
        public string CustomerId { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>
        /// <returns></returns>
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        public string ModifyUserName { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.CustomizationId = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            if (OperatorProvider.Provider.Current() != null && OperatorProvider.Provider.Current().UserId != null)
            {
                this.CreateUserId = OperatorProvider.Provider.Current().UserId;
                this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            }
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.CustomizationId = keyValue;
            this.ModifyDate = DateTime.Now;
            if (OperatorProvider.Provider.Current() != null && OperatorProvider.Provider.Current().UserId != null)
            {
                this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
                this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
            }
        }
        #endregion
    }
}