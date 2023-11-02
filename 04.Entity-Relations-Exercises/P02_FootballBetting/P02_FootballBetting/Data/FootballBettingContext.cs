using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data
{
    public class FootballBettingContext: DbContext
    {
        public FootballBettingContext()
        {
        }
        public FootballBettingContext(DbContextOptions<FootballBettingContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
         
        }
    }
}
