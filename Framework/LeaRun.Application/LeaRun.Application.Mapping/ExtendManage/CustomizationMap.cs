using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class CustomizationMap : EntityTypeConfiguration<CustomizationEntity>
    {
        public CustomizationMap()
        {
            #region ������
            //��
            this.ToTable("Extend_Customization");
            //����
            this.HasKey(t => t.CustomizationId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
