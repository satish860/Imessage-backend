using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Claims;

namespace IMessager.Api.Types
{
    public class User
    {
        public string? Id { get; set; }

        public string? UserName { get; set; }
    }

    public class Query
    {
        private readonly IMongoDatabase database;
        private readonly IMongoCollection<Session.User> sessionCollection;

        public Query(IMongoDatabase database)
        {
            this.database = database;
            this.sessionCollection = database.GetCollection<Session.User>("User");
        }

        public async Task<IEnumerable<User>> SearchUsers(string username, ClaimsPrincipal claimsPrincipal)
        {
            var identity = claimsPrincipal
                        .Identities
                        .FirstOrDefault(identity => identity.AuthenticationType == JwtBearerDefaults.AuthenticationScheme);
            var userId = identity!.Claims.FirstOrDefault(p => p.Type == "userId")!.Value;
            var searchBuilder = Builders<Session.User>
                .Filter
                .Regex(u => u.UserName, new BsonRegularExpression(username, "i"));
            var notInSameuser = Builders<Session.User>
                .Filter
                .Ne(p=>p.Id,ObjectId.Parse(userId));
            var filter = Builders<Session.User>
                .Filter
                .And(searchBuilder, notInSameuser);
            var results = await this.sessionCollection.Find(filter).ToListAsync();
            var users = results.Select(p =>
             {
                 return new User
                 {
                     Id = p.Id.ToString(),
                     UserName = p.UserName!
                 };
             });
            return users;
        }
    }
}
