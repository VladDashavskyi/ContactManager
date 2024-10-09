using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ContactManager.DAL.Entity;


namespace ContactManager.DAL.EntityTypeConfigurations
{
    class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder
               .ToTable("Person", "dbo");
            builder
               .HasKey(e => e.Id);

            builder
               .Property(e => e.Name)
               .HasMaxLength(200);
            
            builder
               .Property(e => e.Phone)
               .HasMaxLength(30);
                       
   
        }
    }
}