using Microsoft.EntityFrameworkCore;
using Manager.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manager.Infra.Mappings{
    //mapping, seria a ligação (DE x Para) entre a classe com a tabela no banco de dados
    public class UserMap : IEntityTypeConfiguration<User>{
        public void Configure(EntityTypeBuilder<User> builder){
            builder.ToTable("User");

            //builder.Haskey(x => x.Id); //não funcionou no dotnet 5.0, mas primarykey foi criada com sucesso

            builder.Property(x => x.Id)
                    .UseIdentityColumn()//auto incremento do SqlServer
                    .HasColumnType("BIGINT");

            builder.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(180)
                    .HasColumnName("name")
                    .HasColumnType("VARCHAR(180)");

            builder.Property(x => x.Password)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("password")
                    .HasColumnType("VARCHAR(30)");

            builder.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(180)
                    .HasColumnName("email")
                    .HasColumnType("VARCHAR(180)");

        }
    }
}