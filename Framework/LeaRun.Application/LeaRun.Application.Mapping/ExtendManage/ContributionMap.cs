using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class ContributionMap : EntityTypeConfiguration<ContributionEntity>
    {
        public ContributionMap()
        {
            #region ������
            //��
            this.ToTable("Extend_Contribution");
            //����
            this.HasKey(t => t.ContributionId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
