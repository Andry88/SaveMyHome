using SaveMyHome.Models;

namespace SaveMyHome.Infrastructure.Repository.Abstract
{
    public interface IProblemRepository : IRepository<Problem>
    {
        int GetProblemIdByName(string name);
    }
}
