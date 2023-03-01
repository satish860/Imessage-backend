using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IMessager.Api.Session
{
    public class Session
    {
        public ObjectId Id { get; set; }

        [BsonElement("sessionToken")]
        public string SessionToken { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("expires")]
        public DateTime Expires { get; set; }
    }
}
