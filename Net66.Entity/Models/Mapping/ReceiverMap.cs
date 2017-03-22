using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Net66.Entity.Models.Mapping
{
    public class ReceiverMap : EntityTypeConfiguration<Receiver>
    {
        public ReceiverMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.CPUId)
                .HasMaxLength(50);

            this.Property(t => t.G_Number)
                .HasMaxLength(50);

            this.Property(t => t.F_Number)
                .HasMaxLength(50);

            this.Property(t => t.W_Number)
               .HasMaxLength(50);

            this.Property(t => t.GuidID)
               .HasMaxLength(50);

            this.Property(t => t.InstallDate)
              .HasMaxLength(50);

            this.Property(t => t.IPAddress)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Receiver");
            this.Property(t => t.ID).HasColumnName("ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity); 
            this.Property(t => t.CPUId).HasColumnName("CPUId");
            this.Property(t => t.G_Number).HasColumnName("G_Number");
            this.Property(t => t.F_Number).HasColumnName("F_Number");
            this.Property(t => t.W_Number).HasColumnName("W_Number");
            this.Property(t => t.GuidID).HasColumnName("GuidID");
            this.Property(t => t.IPAddress).HasColumnName("IPAddress");
            this.Property(t => t.InstallDate).HasColumnName("InstallDate");
            this.Property(t => t.Temperature).HasColumnName("Temperature");
            this.Property(t => t.Humidity).HasColumnName("Humidity");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
        }
    }
}
