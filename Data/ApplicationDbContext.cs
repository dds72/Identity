using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<ApplicationUser>()
			   .Property(u => u.Id)
			   .HasMaxLength(40);

			builder.Entity<IdentityRole>()
				  .Property(r => r.Id)
				  .HasMaxLength(40);

			builder.Entity<IdentityUserLogin<string>>()
				.Property(l => l.LoginProvider)
				.HasMaxLength(40);

			builder.Entity<IdentityUserLogin<string>>()
				.Property(l => l.ProviderKey)
				.HasMaxLength(40);

			builder.Entity<IdentityUserToken<string>>()
				.Property(t => t.LoginProvider)
				.HasMaxLength(40);

			builder.Entity<IdentityUserToken<string>>()
				.Property(t => t.Name)
				.HasMaxLength(40);

			foreach (var entity in builder.Model.GetEntityTypes())
			{
				// Replace table names
				entity.Relational().TableName = entity.Relational().TableName.ToSnakeCase();

				// Replace column names            
				foreach (var property in entity.GetProperties())
				{
					property.Relational().ColumnName = property.Name.ToSnakeCase();
				}

				foreach (var key in entity.GetKeys())
				{
					key.Relational().Name = key.Relational().Name.ToSnakeCase();
				}

				foreach (var key in entity.GetForeignKeys())
				{
					key.Relational().Name = key.Relational().Name.ToSnakeCase();
				}

				foreach (var index in entity.GetIndexes())
				{
					index.Relational().Name = index.Relational().Name.ToSnakeCase();
				}
			}
		}
	}
}
