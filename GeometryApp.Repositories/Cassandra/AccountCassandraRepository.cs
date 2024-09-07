using System.Threading.Tasks;
using GeometryApp.Common.Models.Cassandra;

namespace GeometryApp.Repositories.Cassandra
{
    public class AccountCassandraRepository : CassandraRepository, IAccountRepository
    {
        public AccountCassandraRepository(GeometryAppCassandra context) : base(context)
        {
            CreateIfNotExists<AccountEntity>();
        }

        public async Task Add(AccountEntity account)
        {
            await GetMapper().InsertAsync(account);
        }

        public async Task<AccountEntity> GetLast(int id)
        {
            return await GetMapper().FirstOrDefaultAsync<AccountEntity>("WHERE id = ? LIMIT 1", id);
        }
    }
}
