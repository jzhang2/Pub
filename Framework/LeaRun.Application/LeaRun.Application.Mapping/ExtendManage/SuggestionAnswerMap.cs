using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class SuggestionAnswerMap : EntityTypeConfiguration<SuggestionAnswerEntity>
    {
        public SuggestionAnswerMap()
        {
            #region ������
            //��
            this.ToTable("Extend_SuggestionAnswer");
            //����
            this.HasKey(t => t.AnswerId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
