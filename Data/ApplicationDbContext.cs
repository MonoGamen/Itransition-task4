using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<DateUser> DateUsers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public void InsertRegistrationDate(string id, DateTime date)
        {
            DateUsers.Add(new DateUser() { Id = id, RegistrationDate = date});
            SaveChanges();
        }

        public async Task UpdateLoginDate(string id, DateTime date)
        {
            var user = await DateUsers.FirstAsync(u => u.Id == id);
            user.LastLoginDate = date;
            SaveChanges();
        }

        public static string GetNpgsqlConnectionString(string databaseUrl, bool dev)
        {
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/')
            };

            if (!dev)
            {
                builder.Pooling = true;
                builder.SslMode = SslMode.Require;
                builder.TrustServerCertificate = true;
            }

            return builder.ToString();
        }
    }
}
