using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyaStore.Services.ProductAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace AnyaStore.Services.ProductAPI.Extensions
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