using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class SecurityCodeMap : EntityTypeConfiguration<SecurityCodeEntity>
    {
        public SecurityCodeMap()
        {
            #region 表、主键
            //表
            this.ToTable("Extend_SecurityCode");
            //主键
            this.HasKey(t => t.SecurityId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
