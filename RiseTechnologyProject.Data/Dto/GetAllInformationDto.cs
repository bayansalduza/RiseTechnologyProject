using RiseTechnologyProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseTechnologyProject.Data.Dto
{
    public class GetAllInformationDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }  
        public List<ContactDto> ContactDtos { get; set; }  
    }
}
