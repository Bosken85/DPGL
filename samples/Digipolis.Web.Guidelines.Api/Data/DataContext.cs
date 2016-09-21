using Digipolis.Web.Guidelines.Api.Data.Entiteiten;
using Microsoft.EntityFrameworkCore;

namespace Digipolis.Web.Guidelines.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            :base(options)
        { }

        public DbSet<Value> Values { get; set; }
    }
}
