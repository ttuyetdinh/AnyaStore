using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Services.CouponAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace AnyaStore.Services.CouponAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void DBMigration(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<AppDbContext>();
                if (db.Database.GetPendingMigrations().Any())
                {
                    db.Database.Migrate();
                }
            }
        }
    }
}