using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class CustomizationMap : EntityTypeConfiguration<CustomizationEntity>
    {
        public CustomizationMap()
        {
            #region 表、主键
            //表
            this.ToTable("Extend_Customization");
            //主键
            this.HasKey(t => t.CustomizationId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
