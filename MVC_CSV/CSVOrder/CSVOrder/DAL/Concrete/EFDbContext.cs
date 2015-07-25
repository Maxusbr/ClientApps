using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CSVOrder.DAL.Concrete
{
    public class EFDbContext : DbContext
    {
        public EFDbContext(string connectionString)
            : base(nameOrConnectionString: connectionString)
        { }
    }
}
