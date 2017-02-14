using System;
using LeaRun.Application.Code;

namespace LeaRun.Application.Entity.ExtendManage
{
    public class CommentsEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// CommentsId
        /// </summary>
        /// <returns></returns>
        public string CommentsId { get; set; }
        /// <summary>
        /// 客户主键
        /// </summary>
        /// <returns></returns>
        public string CustomerId { get; set; }
        /// <summary>
        /// 客户主键
        /// </summary>
        /// <returns></returns>
        public string ReplyCustomerId { get; set; }
        /// <summary>
        /// 新闻主键
        /// </summary>
        /// <returns></returns>
        public string NewsId { get; set; }
        /// <summary>
        /// ParentId
        /// </summary>
        /// <returns></returns>
        public string ParentId { get; set; }
        /// <summary>
        /// Comments
        /// </summary>
        /// <returns></returns>
        public string Comments { get; set; }
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
            this.CommentsId = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.CommentsId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}