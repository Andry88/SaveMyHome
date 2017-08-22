using SaveMyHome.Models;

namespace SaveMyHome.Infrastructure.Repository.Abstract
{
    public interface IClientProfileRepository : IRepository<ClientProfile>
    {
        ClientProfile CurrentClientProfile { get; }
        ClientProfile GetUserWithProfile(string id);
    }
}
