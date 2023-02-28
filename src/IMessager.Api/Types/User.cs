using System.Security.Claims;

namespace IMessager.Api.Types
{
    public class User
    {
        public string Id { get; set; }

        public string UserName { get; set; }
    }

    public class Query
    {
        public IEnumerable<User> SearchUsers(string username)
        {
            return Enumerable.Empty<User>();
        }
    }

    public class UserMutations
    {
        public CreateUserNameResponse CreateUserName(string username,ClaimsPrincipal claimsPrincipal)
        {
            return new CreateUserNameResponse { Sucess = true };
        }
    }

    public class CreateUserNameResponse
    {
        public bool Sucess { get; set; }

        public string? Error { get; set; }
    }
}
