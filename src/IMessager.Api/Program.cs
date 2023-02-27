using IMessager.Api.Types;

namespace IMessager.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services
                   .AddGraphQLServer()
                   .AddQueryType<Query>()
                   .AddMutationType<UserMutations>();
            var app = builder.Build();

            app.MapGraphQL();

            app.Run();
        }
    }
}