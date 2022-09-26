using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseTechnologyProject.Data.Dto
{
    public class ReportsDto
    {
        public int UUID { get; set; }
        public string Location { get; set; }
        public int RegisteredLocationCount { get; set; }
        public int RegisteredPhoneNumberCount { get; set; }
    }
}
