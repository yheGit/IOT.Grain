using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Net66.Entity.Models.Mapping
{
    public class GranaryMap : EntityTypeConfiguration<Granary>
    {
        public GranaryMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Number)
                .HasMaxLength(50);

            this.Property(t => t.Code)
              .HasMaxLength(50);

            this.Property(t => t.Location)
                .HasMaxLength(50);

            this.Property(t => t.StampTime)
                .HasMaxLength(50);

            this.Property(t => t.WH_Number)
                .HasMaxLength(50);
            this.Property(t => t.Name)
              .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Granary");
            this.Property(t => t.ID).HasColumnName("ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity); ;
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Number).HasColumnName("Number");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.AverageTemperature).HasColumnName("AverageTemperature");
            this.Property(t => t.AverageHumidity).HasColumnName("AverageHumidity");
            this.Property(t => t.MaxiTemperature).HasColumnName("MaxiTemperature");
            this.Property(t => t.MinTemperature).HasColumnName("MinTemperature");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.PID).HasColumnName("PID");
            this.Property(t => t.WH_ID).HasColumnName("WH_ID");
            this.Property(t => t.WH_Number).HasColumnName("WH_Number");
            this.Property(t => t.BadPoints).HasColumnName("BadPoints");
            this.Property(t => t.StampTime).HasColumnName("StampTime");
            this.Property(t => t.Sort).HasColumnName("Sort");


        }
    }
}
