
namespace FlashSale.InventoryManager.Application.Interfaces
{
    public interface ICurrentUser
    {
        Guid GetUserId();

        string GetUserName();

        string GetUserRole();

        bool IsInRole(string role);

        bool IsAdmin();

    }
}