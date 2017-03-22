using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Net66.Entity.Models.Mapping
{
    public class UserInfoMap : EntityTypeConfiguration<UserInfo>
    {
        public UserInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.LoginName)
                .HasMaxLength(50);

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.LoginPwd)
                .HasMaxLength(50);

            this.Property(t => t.OrgId)
                .HasMaxLength(50);

            this.Property(t => t.mobile)
                .HasMaxLength(50);

            this.Property(t => t.tel)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("UserInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.LoginName).HasColumnName("LoginName");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.LoginPwd).HasColumnName("LoginPwd");
            this.Property(t => t.OrgId).HasColumnName("OrgId");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.mobile).HasColumnName("mobile");
            this.Property(t => t.tel).HasColumnName("tel");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
        }
    }
}
