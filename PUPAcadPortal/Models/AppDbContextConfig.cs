using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseMySQL(DBConnectService.ConnectionString);
    }
}
