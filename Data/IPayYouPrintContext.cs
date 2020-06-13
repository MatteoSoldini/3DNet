using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IPayYouPrint.Models;

namespace IPayYouPrint.Data
{
    public class IPayYouPrintContext : DbContext
    {
        public IPayYouPrintContext (DbContextOptions<IPayYouPrintContext> options)
            : base(options)
        {
        }

        public DbSet<IPayYouPrint.Models.User> User { get; set; }

        public DbSet<IPayYouPrint.Models._3dModel> _3dModel { get; set; }
    }
}
