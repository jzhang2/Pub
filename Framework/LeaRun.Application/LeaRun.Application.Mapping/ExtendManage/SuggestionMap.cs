using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class SuggestionMap : EntityTypeConfiguration<SuggestionEntity>
    {
        public SuggestionMap()
        {
            #region ������
            //��
            this.ToTable("Extend_Suggestion");
            //����
            this.HasKey(t => t.SuggestionId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
