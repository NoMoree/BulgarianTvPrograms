using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirst.Model;

namespace CodeFirst.DataLayer
{
    public class ProgramTvContext: DbContext
    {
        //public ProgramTvContext()
        //    : base("BloggingDB")
        //{
        //}

        public DbSet<TvProgram> TvPrograms { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Show> Shows { get; set; }
    }
}
