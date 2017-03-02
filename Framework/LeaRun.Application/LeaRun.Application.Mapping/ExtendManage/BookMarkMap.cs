using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class BookMarkMap : EntityTypeConfiguration<BookMarkEntity>
    {
        public BookMarkMap()
        {
            #region ������
            //��
            this.ToTable("Extend_BookMark");
            //����
            this.HasKey(t => t.BookMarkId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
