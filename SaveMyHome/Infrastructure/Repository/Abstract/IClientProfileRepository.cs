using SaveMyHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveMyHome.Infrastructure.Repository.Abstract
{
    public interface IClientProfileRepository : IRepository<ClientProfile>
    {
        ClientProfile CurrentUserProfile { get; }
        ClientProfile GetUserWithProfile(string id);
    }
}
