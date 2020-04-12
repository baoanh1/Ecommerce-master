using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ecommerce.Data.EF
{
    class EcommerceContextFactory : IDesignTimeDbContextFactory<EcommerceDbContext>
    {
        public EcommerceDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
                .AddJsonFile("appsettings.json").Build();
            var conectionstring = configuration.GetConnectionString("mySQLConnectionString");

            //var optionsBuilder = new DbContextOptionsBuilder<EcommerceDbContext>();
            //optionsBuilder.UseMySql(conectionstring);

            var optionsBuilder = new DbContextOptionsBuilder<EcommerceDbContext>();
            optionsBuilder.UseSqlite("Data Source=Ecomerce.db");

            return new EcommerceDbContext(optionsBuilder.Options);
        }
    }
}
