using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using RouletteAPI.Models;
using System;

namespace RouletteAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            // Prueba la conexión al inicializar el contexto
            TestConnection();
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();
        }

        private void TestConnection()
        {
            string connectionString = Database.GetConnectionString();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("✅ Conexión exitosa a MySQL.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Error de conexión: " + ex.Message);
                }
            }
        }
    }
}
