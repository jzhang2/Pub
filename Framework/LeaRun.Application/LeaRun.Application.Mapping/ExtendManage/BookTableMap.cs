using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class BookTableMap : EntityTypeConfiguration<BookTableEntity>
    {
        public BookTableMap()
        {
            #region 表、主键
            //表
            this.ToTable("Extend_BookTable");
            //主键
            this.HasKey(t => t.BookTableId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
