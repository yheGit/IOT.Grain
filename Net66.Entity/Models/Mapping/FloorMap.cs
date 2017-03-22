using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Net66.Entity.Models.Mapping
{
    public class FloorMap : EntityTypeConfiguration<Floor>
    {
        public FloorMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Number)
                .HasMaxLength(50);

            this.Property(t => t.Location)
                .HasMaxLength(50);

            this.Property(t => t.WH_Number)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Floor");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Number).HasColumnName("Number");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.WH_Number).HasColumnName("WH_Number");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
        }
    }
}
