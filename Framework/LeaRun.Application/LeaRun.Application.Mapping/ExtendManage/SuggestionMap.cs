using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class SuggestionMap : EntityTypeConfiguration<SuggestionEntity>
    {
        public SuggestionMap()
        {
            #region 表、主键
            //表
            this.ToTable("Extend_Suggestion");
            //主键
            this.HasKey(t => t.SuggestionId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
