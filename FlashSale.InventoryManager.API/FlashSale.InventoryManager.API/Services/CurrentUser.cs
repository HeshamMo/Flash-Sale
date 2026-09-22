
using FlashSale.InventoryManager.API.Extensinos;
using FlashSale.InventoryManager.Application.Interfaces;

namespace FlashSale.InventoryManager.API.Services
{
    public class CurrentUser:ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        private HttpRequest GetRequest()
        {
            return _httpContextAccessor.HttpContext?.Request
                ?? throw new UnauthorizedAccessException(
                    "No active HTTP request.");
        }

        public Guid GetUserId()
        {
            var userId = GetRequest().GetUserId();

            if(!Guid.TryParse(userId, out var parsedUserId))
                throw new UnauthorizedAccessException("Invalid user ID.");

            return parsedUserId;
        }

        public string GetUserName()
        {
            var username = GetRequest().GetUsername();

            if(string.IsNullOrWhiteSpace(username))
                throw new UnauthorizedAccessException("Username is missing.");

            return username;
        }

        public string GetUserRole()
        {
            var role = GetRequest().GetUserRole();

            if(string.IsNullOrWhiteSpace(role))
                throw new UnauthorizedAccessException("User role is missing.");

            return role;
        }

        public bool IsInRole(string role)
        {
            return GetUserRole().Equals(
                role,
                StringComparison.OrdinalIgnoreCase);
        }

        public bool IsAdmin()
        {
            return IsInRole("Admin");
        }



    }
}
