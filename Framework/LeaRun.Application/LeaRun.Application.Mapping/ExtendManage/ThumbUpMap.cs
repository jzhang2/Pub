using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class ThumbUpMap : EntityTypeConfiguration<ThumbUpEntity>
    {
        public ThumbUpMap()
        {
            #region 表、主键
            //表
            this.ToTable("Extend_ThumbUp");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
