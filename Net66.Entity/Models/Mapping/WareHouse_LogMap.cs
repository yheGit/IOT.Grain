using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Net66.Entity.Models.Mapping
{
    public class WareHouse_LogMap : EntityTypeConfiguration<WareHouse_Log>
    {
        public WareHouse_LogMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.WH_Content)
                .HasMaxLength(50);

            this.Property(t => t.ProducePlace)
                .HasMaxLength(50);

            this.Property(t => t.Mgr)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("WareHouse_Log");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.WH_ID).HasColumnName("WH_ID");
            this.Property(t => t.WH_Content).HasColumnName("WH_Content");
            this.Property(t => t.ProducePlace).HasColumnName("ProducePlace");
            this.Property(t => t.ReceiptDate).HasColumnName("ReceiptDate");
            this.Property(t => t.InputDate).HasColumnName("InputDate");
            this.Property(t => t.Moisture).HasColumnName("Moisture");
            this.Property(t => t.StoreType).HasColumnName("StoreType");
            this.Property(t => t.Incomplete).HasColumnName("Incomplete");
            this.Property(t => t.StoreLevel).HasColumnName("StoreLevel");
            this.Property(t => t.Capacity).HasColumnName("Capacity");
            this.Property(t => t.Impurity).HasColumnName("Impurity");
            this.Property(t => t.Mgr).HasColumnName("Mgr");
        }
    }
}
