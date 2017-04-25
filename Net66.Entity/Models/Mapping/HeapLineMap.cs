using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Net66.Entity.Models.Mapping
{
    public class HeapLineMap : EntityTypeConfiguration<HeapLine>
    {
        public HeapLineMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.HeapNumber)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("HeapLine");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.HeapNumber).HasColumnName("HeapNumber");
            this.Property(t => t.Counts).HasColumnName("Counts");
            this.Property(t => t.Sort).HasColumnName("Sort");
        }
    }
}
