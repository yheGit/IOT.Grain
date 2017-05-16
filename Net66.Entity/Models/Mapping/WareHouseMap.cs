using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Net66.Entity.Models.Mapping
{
    public class WareHouseMap : EntityTypeConfiguration<WareHouse>
    {
        public WareHouseMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Number)
                .HasMaxLength(50);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.Location)
                .HasMaxLength(500);

            this.Property(t => t.UserId)
                .HasMaxLength(50);

            this.Property(t => t.OrgId)
               .HasMaxLength(36);

            this.Property(t => t.OrgCode)
               .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("WareHouse");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Number).HasColumnName("Number");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.InSideTemperature).HasColumnName("InSideTemperature");
            this.Property(t => t.OutSideTemperature).HasColumnName("OutSideTemperature");
            this.Property(t => t.AverageTemperature).HasColumnName("AverageTemperature");
            this.Property(t => t.Maximumemperature).HasColumnName("Maximumemperature");
            this.Property(t => t.MinimumTemperature).HasColumnName("MinimumTemperature");
            this.Property(t => t.StampTime).HasColumnName("StampTime");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.BadPoints).HasColumnName("BadPoints");//
            this.Property(t => t.Width).HasColumnName("Width");
            this.Property(t => t.Height).HasColumnName("Height");
            this.Property(t => t.depth).HasColumnName("depth");
            this.Property(t => t.OrgId).HasColumnName("OrgId");
            this.Property(t => t.OrgCode).HasColumnName("OrgCode");
        }
    }
}
