using System;
using LeaRun.Application.Code;

namespace LeaRun.Application.Entity.ExtendManage {
    public class SuggestionEntity : BaseEntity {
        #region 实体成员
        /// <summary>
        /// SuggestionId
        /// </summary>
        /// <returns></returns>
        public string SuggestionId { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <returns></returns>
        public string Title { get; set; }
        /// <summary>
        /// Contents
        /// </summary>
        /// <returns></returns>
        public string Contents { get; set; }
        /// <summary>
        /// CusId
        /// </summary>
        /// <returns></returns>
        public string CusId { get; set; }
        /// <summary>
        /// 分类1-西图动态-2-地图文化
        /// </summary>
        /// <returns></returns>
        public int? Category { get; set; }
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
        /// IsReply
        /// </summary>
        /// <returns></returns>
        public int? IsReply { get; set; }
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
        public override void Create() {
            this.SuggestionId = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            if (OperatorProvider.Provider.Current() != null && OperatorProvider.Provider.Current().UserId != null) {
                this.CreateUserId = OperatorProvider.Provider.Current().UserId;
                this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            }
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue) {
            this.SuggestionId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}