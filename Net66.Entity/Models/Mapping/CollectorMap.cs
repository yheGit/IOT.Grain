using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Net66.Entity.Models.Mapping
{
    public class CollectorMap : EntityTypeConfiguration<Collector>
    {
        public CollectorMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.CPUId)
                .HasMaxLength(50);

            this.Property(t => t.HeapNumber)
                .HasMaxLength(50);

            this.Property(t => t.GuidID)
                .HasMaxLength(50);

            this.Property(t => t.InstallDate)
              .HasMaxLength(50);

            //this.Property(t => t.R_Number)
            //   .HasMaxLength(50);

            this.Property(t => t.SensorIdArr)
                .HasMaxLength(4000);

            // Table & Column Mappings
            this.ToTable("Collector");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CPUId).HasColumnName("CPUId");
            this.Property(t => t.R_Code).HasColumnName("R_Code");
            this.Property(t => t.HeapNumber).HasColumnName("HeapNumber");
            this.Property(t => t.InstallDate).HasColumnName("InstallDate");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Voltage).HasColumnName("Voltage");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.SensorIdArr).HasColumnName("SensorIdArr");
            this.Property(t => t.Sublayer).HasColumnName("Sublayer");
            this.Property(t => t.GuidID).HasColumnName("GuidID");
            this.Property(t => t.BadPoints).HasColumnName("BadPoints");
            this.Property(t => t.Column).HasColumnName("Column");
        }
    }
}
