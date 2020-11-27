using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DocumentsKM.Models
{
    public class MarkGeneralData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int MarkId { get; set; }

        public MarkGeneralDataSection[] Sections { get; set; }
    }
}