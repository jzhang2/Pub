using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class CommentsMap : EntityTypeConfiguration<CommentsEntity>
    {
        public CommentsMap()
        {
            #region ������
            //��
            this.ToTable("Extend_Comments");
            //����
            this.HasKey(t => t.CommentsId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
