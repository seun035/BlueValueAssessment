using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueValueAssessment.Core.Documents
{
    public class BaseDocument
    {
        [BsonElement("_id")]
        public Guid Id { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public DateTime? ModifiedDateUtc { get; set; }
    }
}
