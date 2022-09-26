using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseTechnologyProject.Data.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }
        public int UUID { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsOkey { get; set; }
    }
}
