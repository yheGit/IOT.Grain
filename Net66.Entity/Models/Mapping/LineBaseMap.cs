using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Net66.Entity.Models.Mapping
{
    public class LineBaseMap : EntityTypeConfiguration<LineBase>
    {
        public LineBaseMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.HeapNumber)
                .HasMaxLength(50);

            this.Property(t => t.LineCode)
                .HasMaxLength(50);

            this.Property(t => t.StampTime)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("LineBase");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.HeapNumber).HasColumnName("HeapNumber");
            this.Property(t => t.LineCode).HasColumnName("LineCode");
            this.Property(t => t.StampTime).HasColumnName("StampTime");
            this.Property(t => t.LSequence).HasColumnName("LSequence");
        }
    }
}
