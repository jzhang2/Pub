using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class BannerNewsMap : EntityTypeConfiguration<BannerNewsEntity>
    {
        public BannerNewsMap()
        {
            #region 表、主键
            //表
            this.ToTable("Extend_BannerNews");
            //主键
            this.HasKey(t => t.BannerNewsId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
