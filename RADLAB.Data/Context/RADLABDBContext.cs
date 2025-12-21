using RADLAB.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RADLAB.Data.Context
{
    public class RADLABDBContext : DbContext
    {
        public RADLABDBContext(DbContextOptions<RADLABDBContext> options) : base(options)
        {

        }

        public virtual DbSet<KisiModel> Kisi { get; set; }
        public virtual DbSet<KisiBirimModel> KisiBirim { get; set; }
        public virtual DbSet<KisiRolModel> KisiRol { get; set; }
        public virtual DbSet<RolModel> Rol { get; set; }
        public virtual DbSet<RolYetkiModel> RolYetki { get; set; }
        public virtual DbSet<YetkiModel> Yetki { get; set; }
    }
}
