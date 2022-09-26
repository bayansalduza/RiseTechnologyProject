using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseTechnologyProject.Data.Dto
{
    public class ReportsDto
    {
        [BsonElement("_id")]
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int UUID { get; set; }
        public string Location { get; set; }
        public int RegisteredLocationCount { get; set; }
        public int RegisteredPhoneNumberCount { get; set; }
    }
}
