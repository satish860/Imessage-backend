using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Claims;

namespace IMessager.Api.Types
{
    public class UserMutations
    {
        private readonly IMongoDatabase database;
        private readonly IMongoCollection<Session.User> sessionCollection;

        public UserMutations(IMongoDatabase database)
        {
            this.database = database;
            this.sessionCollection = database.GetCollection<Session.User>("User");
        }

        public CreateUserNameResponse CreateUserName(string username, ClaimsPrincipal claimsPrincipal)
        {
            var identity = claimsPrincipal
                        .Identities
                        .FirstOrDefault(identity => identity.AuthenticationType == JwtBearerDefaults.AuthenticationScheme);
            var user = identity!.Claims.FirstOrDefault(p => p.Type == "userId")!.Value;
            if (user != null)
            {
                var entity = sessionCollection
                  .Find(doc => doc.Id == ObjectId.Parse(user))
                  .FirstOrDefault();
                if (entity != null && entity.UserName == null)
                {
                    entity.UserName = username;
                    var filter = Builders<Session.User>
                        .Filter.Eq(s => s.Id, ObjectId.Parse(user));
                    sessionCollection.ReplaceOne(filter, entity);
                }
            }
            return new CreateUserNameResponse { Sucess = true };
        }
    }
}
