using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class ThumbUpMap : EntityTypeConfiguration<ThumbUpEntity>
    {
        public ThumbUpMap()
        {
            #region ������
            //��
            this.ToTable("Extend_ThumbUp");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
