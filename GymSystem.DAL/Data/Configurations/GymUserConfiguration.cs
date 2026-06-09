using GymSystem.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.Configurations
{
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name)
                .HasColumnType("varchar").HasMaxLength(50);

            builder.Property(x => x.Email)
                .HasColumnType("varchar").HasMaxLength(100);

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Phone).IsUnique();

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint(
                      "PhoneCheck",
                      "Phone Like '010%' Or Phone Like '011%' Or Phone Like '012%' Or Phone Like '015%'");

                tb.HasCheckConstraint(
                    "EmailCheck",
                    "Email Like '%_@_%._%'");
            });

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(x => x.City).HasColumnType("Varchar").HasMaxLength(30);
                address.Property(x => x.Streat).HasColumnType("Varchar").HasMaxLength(30);
            });

        }
    }
}
