using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Net66.Entity.Models.Mapping
{
    public class HumidityMap : EntityTypeConfiguration<Humidity>
    {
        public HumidityMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            //this.Property(t => t.ReceiverId)
            //    .HasMaxLength(50);

            this.Property(t => t.StampTime)
                .HasMaxLength(50);
            this.Property(t => t.G_Number)
              .HasMaxLength(50);
            this.Property(t => t.WH_Number)
              .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Humidity");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Humility).HasColumnName("Humility");
            this.Property(t => t.Temp).HasColumnName("Temp");
            this.Property(t => t.StampTime).HasColumnName("StampTime");
            this.Property(t => t.ReceiverId).HasColumnName("ReceiverId");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.G_Number).HasColumnName("G_Number");
            this.Property(t => t.WH_Number).HasColumnName("WH_Number");
            this.Property(t => t.RealHeart).HasColumnName("RealHeart");
        }
    }
}
