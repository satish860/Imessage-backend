using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IMessager.Api.Session
{
    public class User
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("username")]
        public string? UserName { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("emailVerified")]
        public bool? EmailVerfied { get; set; }

        [BsonElement("image")]
        public string? Image { get; set; }
    }
}
