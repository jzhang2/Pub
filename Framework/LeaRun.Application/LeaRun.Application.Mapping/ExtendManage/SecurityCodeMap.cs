using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class SecurityCodeMap : EntityTypeConfiguration<SecurityCodeEntity>
    {
        public SecurityCodeMap()
        {
            #region ������
            //��
            this.ToTable("Extend_SecurityCode");
            //����
            this.HasKey(t => t.SecurityId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
