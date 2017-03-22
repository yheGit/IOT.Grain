using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Net66.Entity.Models.Mapping
{
    public class AlarmMap : EntityTypeConfiguration<Alarm>
    {
        public AlarmMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ShortCode)
                .HasMaxLength(50);

            this.Property(t => t.CpuId)
                .HasMaxLength(50);

            this.Property(t => t.SensorId)
                .HasMaxLength(50);

            this.Property(t => t.DateValue)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Alarm");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.ShortCode).HasColumnName("ShortCode");
            this.Property(t => t.CpuId).HasColumnName("CpuId");
            this.Property(t => t.SensorId).HasColumnName("SensorId");
            this.Property(t => t.DateValue).HasColumnName("DateValue");
            this.Property(t => t.StampTime).HasColumnName("StampTime");
        }
    }
}
