using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Net66.Entity.Models.Mapping
{
    public class SensorMap : EntityTypeConfiguration<Sensor>
    {
        public SensorMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.SensorId)
                .HasMaxLength(50);

            this.Property(t => t.CRC)
                .HasMaxLength(50);

            this.Property(t => t.Label)
                .HasMaxLength(50);

            this.Property(t => t.Collector)
                .HasMaxLength(50);


            this.Property(t => t.GuidID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sensor");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SensorId).HasColumnName("SensorId");
            this.Property(t => t.CRC).HasColumnName("CRC");
            this.Property(t => t.Label).HasColumnName("Label");
            this.Property(t => t.Sequen).HasColumnName("Sequen");
            this.Property(t => t.Collector).HasColumnName("Collector");
            this.Property(t => t.Direction_X).HasColumnName("Direction_X");
            this.Property(t => t.Direction_Y).HasColumnName("Direction_Y");
            this.Property(t => t.Direction_Z).HasColumnName("Direction_Z");
            this.Property(t => t.MaxTemp).HasColumnName("MaxTemp");
            this.Property(t => t.MinTemp).HasColumnName("MinTemp");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.GuidID).HasColumnName("GuidID");//
            this.Property(t => t.IsBad).HasColumnName("IsBad");//
        }
    }
}
