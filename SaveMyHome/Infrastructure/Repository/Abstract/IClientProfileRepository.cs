using SaveMyHome.Models;

namespace SaveMyHome.Infrastructure.Repository.Abstract
{
    public interface IClientProfileRepository : IRepository<ClientProfile>
    {
        ClientProfile CurrentUserProfile { get; }
        ClientProfile GetUserWithProfile(string id);
    }
}
