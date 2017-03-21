using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class BookTableMap : EntityTypeConfiguration<BookTableEntity>
    {
        public BookTableMap()
        {
            #region ������
            //��
            this.ToTable("Extend_BookTable");
            //����
            this.HasKey(t => t.BookTableId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
