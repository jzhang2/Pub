using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class CommentsMap : EntityTypeConfiguration<CommentsEntity>
    {
        public CommentsMap()
        {
            #region 表、主键
            //表
            this.ToTable("Extend_Comments");
            //主键
            this.HasKey(t => t.CommentsId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
