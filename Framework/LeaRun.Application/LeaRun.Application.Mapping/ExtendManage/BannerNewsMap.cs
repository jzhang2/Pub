using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class BannerNewsMap : EntityTypeConfiguration<BannerNewsEntity>
    {
        public BannerNewsMap()
        {
            #region ������
            //��
            this.ToTable("Extend_BannerNews");
            //����
            this.HasKey(t => t.BannerNewsId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
