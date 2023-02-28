using IMessager.Api.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace IMessager.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var MyAllowSpecificOrigins = "_FRONTEND";
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy
                                      .SetIsOriginAllowed((_) => true)
                                      .WithOrigins("http://localhost:3000")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials();
                                  });
            });
            builder.Services.AddAuthorization();
            builder.Services
                   .AddGraphQLServer()
                   .AddHttpRequestInterceptor<HttpRequestInterceptor>()
                   .AddAuthorization()
                   .AddQueryType<Query>()
                   .AddMutationType<UserMutations>();
            var app = builder.Build();



            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.MapGraphQL();

            app.Run();
        }
    }
}