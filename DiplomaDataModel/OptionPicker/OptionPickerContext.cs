using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DiplomaDataModel
{
    public class OptionPickerContext : DbContext
    {
        public OptionPickerContext() : base("DefaultConnection")
        {

        }

        public DbSet<Option> Options { get; set; }
        public DbSet<YearTerm> YearTerms { get; set; }
        public DbSet<Choice> Choices { get; set; }
    }
}