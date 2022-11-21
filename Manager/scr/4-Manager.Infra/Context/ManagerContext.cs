using Microsoft.EntityFrameworkCore;
using Manager.Domain.Entities;
using Manager.Infra.Mappings;

namespace Manager.Infra.Context{
    public class ManagerContext : DbContext{
        public ManagerContext(){}

        public ManagerContext(DbContextOptions<ManagerContext> options) : base(options){}

        //para migrations
        /* esse trecho foi incluido no appsettings.json
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
            optionsBuilder.UseSqlServer(@"Server=CLEOMILSON_NB\SQLEXPRESS01;Database=usermanagerapi;Trusted_Connection=True;");
            //Server=CLEOMILSON_NB\SQLEXPRESS01;Database=usermanagerapi;User Id=Cleomilson_NB\CleomilsonSales;Password=cleo2705;  --conexão do curso não deu certo na minha configuração do sqlserver
        }*/

        public virtual DbSet<User> Users {get; set; }

        protected override void OnModelCreating(ModelBuilder builder){
            builder.ApplyConfiguration(new UserMap());
        }

    }
}