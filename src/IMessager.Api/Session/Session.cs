using MongoDB.Bson;

namespace IMessager.Api.Session
{
    public class Session
    {
        public ObjectId Id { get; set; }

        public string SessionToken { get; set; }

        public string UserId { get; set; }

        public DateTime Expires { get; set; }
    }
}
