using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuAPI.Models
{
    public abstract class MongoDocument
    {
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        public Guid Id { get; set; }

        public DateTime LastChangedAt { get; set; }
    }
}
