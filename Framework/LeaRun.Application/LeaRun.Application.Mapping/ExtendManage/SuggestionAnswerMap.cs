using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class SuggestionAnswerMap : EntityTypeConfiguration<SuggestionAnswerEntity>
    {
        public SuggestionAnswerMap()
        {
            #region 表、主键
            //表
            this.ToTable("Extend_SuggestionAnswer");
            //主键
            this.HasKey(t => t.AnswerId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
