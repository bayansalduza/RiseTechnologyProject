using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RiseTechnologyProject.Data.Models
{
    public class User
    {
        [Key]
        public int UUID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        [ForeignKey("Contact_FK_User")]
        public int ContactID { get; set; }
        public Contact Contact { get; set; }
    }
}
