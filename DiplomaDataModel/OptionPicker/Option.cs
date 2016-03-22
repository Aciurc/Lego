using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DiplomaDataModel
{
    public class Option
    {
        [Key]
        public int OptionId { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }
}