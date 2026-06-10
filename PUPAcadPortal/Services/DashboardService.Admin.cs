using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using PUPAcadPortal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Services
{
    public partial class DashboardService
    {
        public async Task<List<ActivityLog>> GetActivityLogsAsync(string role)
        {
            if (role.ToLower() == "admin")
            {
                using (var context = new AppDbContext())
                {
                    return await context.ActivityLogs
                        .Include(a =>a.User)
                        .OrderByDescending(a => a.Timestamp)
                        .Take(5)
                        .ToListAsync();
                }
            }
            else
                return [];
        }
    }
}
