using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseTechnologyProject.Data.Dto
{
    public class GetAllContactDto
    {
        public int UUID { get; set; }
        public List<ContactDto> Contacts { get; set; }
    }
}
