using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RADLAB.Data.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RADLABDBContext>
    {
        public RADLABDBContext CreateDbContext(string[] args)
        {
            string connectionString = "Data Source=.;Initial Catalog=RADLAB;Persist Security Info=True;User ID=sa;Password=ondata";

            var builder = new DbContextOptionsBuilder<RADLABDBContext>();
            
            builder.UseSqlServer(connectionString);

            return new RADLABDBContext(builder.Options);
        }
    }
}