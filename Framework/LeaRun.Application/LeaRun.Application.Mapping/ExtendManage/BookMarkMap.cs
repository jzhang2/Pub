using LeaRun.Application.Entity.ExtendManage;
using System.Data.Entity.ModelConfiguration;

namespace LeaRun.Application.Mapping.ExtendManage
{
    public class BookMarkMap : EntityTypeConfiguration<BookMarkEntity>
    {
        public BookMarkMap()
        {
            #region 表、主键
            //表
            this.ToTable("Extend_BookMark");
            //主键
            this.HasKey(t => t.BookMarkId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
