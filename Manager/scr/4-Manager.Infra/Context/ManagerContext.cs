using Microsoft.EntityFrameworkCore;
using Manager.Domain.Entities;
using Manager.Infra.Mappings;

namespace Manager.Infra.Context{
    public class ManagerContext : DbContext{
        public ManagerContext(){}

        public ManagerContext(DbContextOptions<ManagerContext> options) : base(options){}

        //para migrations
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
            optionsBuilder.UseMySql("Server=localhost;Port=3306;Database=usermanagerapi;Uid=root;Pwd=root");
        }

        public virtual DbSet<User> Users {get; set; }

        protected override void OnModelCreating(ModelBuilder builder){
            builder.ApplyConfiguration(new UserMap());
        }

    }
}