using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class ContributionMap : EntityTypeConfiguration<ContributionEntity>
    {
        public ContributionMap()
        {
            #region 表、主键
            //表
            this.ToTable("Extend_Contribution");
            //主键
            this.HasKey(t => t.ContributionId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
