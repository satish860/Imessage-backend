using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using IMessager.Api.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Claims;

namespace IMessager.Api
{
    public class HttpRequestInterceptor : DefaultHttpRequestInterceptor
    {
        private readonly IMongoDatabase database;
        private readonly IMongoCollection<Session.Session> sessionCollection;

        public HttpRequestInterceptor(IMongoDatabase database)
        {
            this.database = database;
            this.sessionCollection = database.GetCollection<Session.Session>("Session");
        }

        public override ValueTask OnCreateAsync(HttpContext context,
            IRequestExecutor requestExecutor,
            IQueryRequestBuilder requestBuilder,
            CancellationToken cancellationToken)
        {
            var req = context.Request;
            var sessionToken = req.Cookies
                        .FirstOrDefault(p => p.Key == "next-auth.session-token")!.Value;
           
            if (sessionToken != null)
            {
                var entity = sessionCollection
                    .Find(doc => doc.SessionToken == sessionToken)
                    .FirstOrDefault();
                if (entity != null)
                {
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                    claimsIdentity.AddClaim(new Claim("userId", entity.UserId));
                    context.User.AddIdentity(claimsIdentity);
                }

            }

            return base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
        }
    }
}
