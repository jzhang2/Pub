﻿using LeaRun.Application.Code;
using System;

namespace LeaRun.Application.Entity.PublicInfoManage
{
    /// <summary>
    /// 版 本 6.1
    /// Copyright (c) 2013-2016 上海力软信息技术有限公司
    /// 创建人：佘赐雄
    /// 日 期：2015.12.7 16:40
    /// 描 述：新闻中心
    /// </summary>
    public class NewsEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 新闻主键
        /// </summary>		
        public string NewsId { get; set; }
        /// <summary>
        /// 类型（1-新闻2-公告3-西图动态4-地图文化5-电子书6-实体书7-业务服务8-关于我们）
        /// </summary>		
        public int? TypeId { get; set; }

        public int? BookTablePage { get; set; }
        /// <summary>
        /// 父级主键
        /// </summary>		
        public string ParentId { get; set; }
        /// <summary>
        /// 所属类别
        /// </summary>		
        public string Category { get; set; }

        public decimal? Price { get; set; }
        /// <summary>
        /// 完整标题
        /// </summary>		
        public string FullHead { get; set; }
        public string EPath { get; set; }
        /// <summary>
        /// 标题颜色
        /// </summary>		
        public string FullHeadColor { get; set; }
        /// <summary>
        /// 简略标题
        /// </summary>		
        public string BriefHead { get; set; }
        /// <summary>
        /// 作者
        /// </summary>		
        public string AuthorName { get; set; }
        /// <summary>
        /// 编辑
        /// </summary>		
        public string CompileName { get; set; }
        /// <summary>
        /// Tag词
        /// </summary>		
        public string TagWord { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>		
        public string Keyword { get; set; }
        /// <summary>
        /// 来源
        /// </summary>		
        public string SourceName { get; set; }
        /// <summary>
        /// 来源地址
        /// </summary>		
        public string SourceAddress { get; set; }
        /// <summary>
        /// 新闻内容
        /// </summary>		
        public string NewsContent { get; set; }
        public string MusicPath { get; set; }
        public string FilePath { get; set; }
        public string Attachment { get; set; }
        public string AttachmentName { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>		
        public int? PV { get; set; }
        public int? PageCount { get; set; }
        public int? CanViewCount { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>		
        public DateTime? ReleaseTime { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>		
        public int? SortCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>		
        public int? DeleteMark { get; set; }
        /// <summary>
        /// 有效标志
        /// </summary>		
        public int? EnabledMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        public string Description { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>		
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>		
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>		
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>		
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>		
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>		
        public string ModifyUserName { get; set; }

        public string NewsImage { get; set; }

        public string NewsLink { get; set; }

        public string BuyLink { get; set; }

        public int? IsRecommend { get; set; }


        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.NewsId = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            this.ReleaseTime = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.DeleteMark = 0;
            this.EnabledMark = 1;
            this.PV = 0;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.NewsId = keyValue;
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
