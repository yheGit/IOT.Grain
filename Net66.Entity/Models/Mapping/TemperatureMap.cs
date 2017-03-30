using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Net66.Entity.Models.Mapping
{
    public class TemperatureMap : EntityTypeConfiguration<Temperature>
    {
        public TemperatureMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.PId)
                .HasMaxLength(50);

            this.Property(t => t.StampTime)
               .HasMaxLength(50);

            this.Property(t => t.WH_Number)
              .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Temperature");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Temp).HasColumnName("Temp");
            this.Property(t => t.StampTime).HasColumnName("StampTime");
            this.Property(t => t.PId).HasColumnName("PId");
            this.Property(t => t.RealHeart).HasColumnName("RealHeart");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.WH_Number).HasColumnName("WH_Number");
        }
    }
}
