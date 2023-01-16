using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace RPG_game_dotnet.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
                new Skill{ Id = 1, Name="Fireball", Damage = 28 },
                new Skill{ Id = 2, Name="Star Falling", Damage = 41 },
                new Skill{ Id = 3, Name="Dark lightning of Dul Guldur", Damage = 33 }
            );
        }

        public DbSet<Character> Characters => Set<Character>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Weapon> Weapons => Set<Weapon>();
        public DbSet<Skill> Skills => Set<Skill>();
    }
}