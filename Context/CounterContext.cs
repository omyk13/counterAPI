using counterAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace counterAPI.Context
{
    public class CounterContext:DbContext
    {
        public CounterContext(DbContextOptions<CounterContext> options)
            : base(options) 
        { 
                   
        }

        public DbSet<Counter> Counters { get; set; } = null!;
    }
}
