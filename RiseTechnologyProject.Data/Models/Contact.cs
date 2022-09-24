using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseTechnologyProject.Data.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string ObjectId { get; set; }
    }
}
