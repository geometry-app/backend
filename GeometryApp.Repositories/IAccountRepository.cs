using System.Threading.Tasks;
using GeometryApp.Common.Models.Cassandra;

namespace GeometryApp.Repositories
{
    public interface IAccountRepository
    {
        Task Add(AccountEntity account);
        Task<AccountEntity> GetLast(int id);
    }
}
