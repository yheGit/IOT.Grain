using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Net66.Entity.Models.Mapping
{
    public class SensorBaseMap : EntityTypeConfiguration<SensorBase>
    {
        public SensorBaseMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.SCpu)
                .HasMaxLength(50);

            this.Property(t => t.SLineCode)
                .HasMaxLength(50);

            this.Property(t => t.StampTime)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SensorBase");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SCpu).HasColumnName("SCpu");
            this.Property(t => t.SSequen).HasColumnName("SSequen");
            this.Property(t => t.SLineCode).HasColumnName("SLineCode");
            this.Property(t => t.StampTime).HasColumnName("StampTime");
        }
    }
}
