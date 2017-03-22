using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Net66.Entity.Models.Mapping
{
    public class OrganizationMap : EntityTypeConfiguration<Organization>
    {
        public OrganizationMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.OrgCode)
                .HasMaxLength(50);

            this.Property(t => t.OrgName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Organization");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.OrgCode).HasColumnName("OrgCode");
            this.Property(t => t.OrgName).HasColumnName("OrgName");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.AddDate).HasColumnName("AddDate");
            this.Property(t => t.ModifyDate).HasColumnName("ModifyDate");
        }
    }
}
