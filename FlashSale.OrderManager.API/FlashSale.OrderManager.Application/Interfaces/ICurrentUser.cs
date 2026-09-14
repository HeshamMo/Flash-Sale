using FlashSale.OrderManager.Domain.Models;

namespace FlashSale.OrderManager.Application.Interfaces
{
    public interface ICurrentUser
    {
        Guid GetUserId();

        string GetUserName();

        string GetUserRole();

        bool IsInRole(string role);

        bool IsAdmin();

        bool IsOrderOwner(Order order);

        bool IsAdminOrOrderOwner(Order order);
    }
}