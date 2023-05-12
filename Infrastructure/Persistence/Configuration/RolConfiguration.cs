namespace Infrastructure.Persistence.Configuration
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    internal class RolConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("Rol");

            builder.Property(p => p.Id).IsRequired();

            builder.HasKey(p => p.Id);
        }
    }
}
