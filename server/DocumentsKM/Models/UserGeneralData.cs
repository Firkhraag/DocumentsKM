using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DocumentsKM.Models
{
    public class UserGeneralData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int UserId { get; set; }

        public UserGeneralDataSection[] Sections { get; set; }
    }
}