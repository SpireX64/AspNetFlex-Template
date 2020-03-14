using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetFlex.DatabaseStore.Contexts.App.Entities
{
    public class UserDbEntity
    {
        public Guid Id { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public long RegistrationDate { get; set; }

        public class EntityConfiguration : IEntityTypeConfiguration<UserDbEntity>
        {
            public void Configure(EntityTypeBuilder<UserDbEntity> builder)
            {
                builder.ToTable("users").HasKey(e => e.Id);
                builder.HasIndex(e => e.Email);

                builder.Property(e => e.Id).HasColumnName("id");
                builder.Property(e => e.Name).HasColumnName("name");
                builder.Property(e => e.Email).HasColumnName("email");
                builder.Property(e => e.PasswordHash).HasColumnName("pass_hash");
                builder.Property(e => e.RegistrationDate).HasColumnName("reg_date");
            }
        }
    }
}