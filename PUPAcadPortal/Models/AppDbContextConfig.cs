using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Services;
using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace PUPAcadPortal.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string originalConnectionString = DBConnectService.ConnectionString;
                //string originalConnectionString = "Server = localhost; Database=defaultdb;Uid=root;Pwd=1234;";

                var builder = new MySqlConnectionStringBuilder(originalConnectionString)
                {
                    ConnectionTimeout = 300
                };

                string modifiedConnectionString = builder.ConnectionString;

                optionsBuilder.UseMySQL(modifiedConnectionString, options =>
                {
                    options.CommandTimeout(300);

                    options.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: System.TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                });
            }
        }
    }
}
