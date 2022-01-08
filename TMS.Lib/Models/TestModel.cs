using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Lib.Models
{
    public class TestModel
    {
        [BsonId]
        public ObjectId MongoId { get; set; }
        public ulong Entity { get; set; }
        public ulong Type { get; set; }
        public DateTime Time { get; set; }
    }
}
