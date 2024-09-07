using System;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Mapping;
using GeometryApp.Repositories.Entities.Users;

namespace GeometryApp.Repositories.Cassandra
{
    public class UserCassandraRepository : CassandraRepository, IUserRepository
    {
        private readonly GeometryAppCassandra cassandra;
        private readonly Mapper mapper;

        public UserCassandraRepository(GeometryAppCassandra context) : base(context)
        {
            cassandra = context;
            var session = cassandra.GetSession();
            
            CreateIfNotExists<UserEntity>();
            CreateIfNotExists<UserNameMap>();
            CreateIfNotExists<UserAccountIdMap>();
            CreateIfNotExists<Token>();
            CreateIfNotExists<LoginCodeEntity>();

            mapper = new Mapper(session);
        }
        
        public async Task<UserEntity> Create(string userName, int accountId, int userId)
        {
            var entity = new UserEntity()
            {
                Id = Guid.NewGuid(),
                AccountId = accountId,
                UserId = userId,
                UserName = userName
            };
            var name = new UserNameMap() {Id = entity.Id, UserName = entity.UserName};
            var account = new UserAccountIdMap() {Id = entity.Id, AccountId = entity.AccountId};
            var batch = mapper.CreateBatch(BatchType.Logged);
            batch.Insert(entity);
            batch.Insert(name);
            batch.Insert(account);
            await mapper.ExecuteAsync(batch);
            return entity;
        }

        public async Task<UserEntity> GetByUserName(string userName)
        {
            return await Get<UserNameMap, string>("WHERE userName = ?", userName, x => x.Id).ConfigureAwait(false);
        }

        public async Task<UserEntity> GetByAccountId(int accountId)
        {
            return await Get<UserAccountIdMap, int>("WHERE accountId = ?", accountId, x => x.Id).ConfigureAwait(false);
        }

        public async Task<UserEntity> GetByToken(string token)
        {
            return await Get<Token, string>("WHERE rawToken = ?", token, x => x.UserId).ConfigureAwait(false);
        }

        public async Task CreateAccessToken(Token token)
        {
            await mapper.InsertAsync(token);
        }

        public Task UpdateName(UserEntity user, string userName)
        {
            // todo: I am sooo lazy :)
            return Task.CompletedTask;
        }

        public async Task AddCode(LoginCodeEntity codeEntity)
        {
            await GetMapper().InsertAsync(codeEntity);
        }

        public async Task<LoginCodeEntity> GetSentCode(string userName, Guid key)
        {
            return await GetMapper().FirstOrDefaultAsync<LoginCodeEntity>("WHERE username = ? AND key = ?", userName, key);
        }

        private async Task<UserEntity> Get<TBetween, TBValue>(string tail, TBValue value, Func<TBetween, Guid> get)
        {
            var between = await mapper.SingleOrDefaultAsync<TBetween>(tail, value).ConfigureAwait(false);
            if (between == null)
                return null;
            return await mapper.SingleOrDefaultAsync<UserEntity>("WHERE id = ?", get(between)).ConfigureAwait(false);
        }
    }
}
