using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Net66.Entity.Models.Mapping
{
    public class HeapMap : EntityTypeConfiguration<Heap>
    {
        public HeapMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Number)
                .HasMaxLength(50);

            this.Property(t => t.Location)
                .HasMaxLength(50);

            this.Property(t => t.G_Number)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Heap");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Number).HasColumnName("Number");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.G_Number).HasColumnName("G_Number");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
        }
    }
}
