using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using GeometryApp.Repositories.Entities;

namespace GeometryApp.Repositories.Cassandra
{
    public class LevelCassandraRepository : ILevelRepository
    {
        private readonly Mapper mapper;
        private readonly Table<LevelEntity> table;

        public LevelCassandraRepository(GeometryAppCassandra context, GeometryAppMappings mappings)
        {
            context.GetSession().CreateKeyspaceIfNotExists(mappings.Keyspace);
            var configuration = new MappingConfiguration().Define(mappings);
            table = new Table<LevelEntity>(context.GetSession(), configuration);
            table.CreateIfNotExists();

            mapper = new Mapper(context.GetSession(), configuration);
        }

        public async Task Add(LevelEntity level)
        {
            await mapper.InsertAsync(level);
        }

        public async Task<LevelEntity> GetLast(int id)
        {
            return await mapper.FirstOrDefaultAsync<LevelEntity>("WHERE id = ? LIMIT 1", id);
        }

        public async Task<IEnumerable<LevelEntity>> Get(int id)
        {
            return await mapper.FetchAsync<LevelEntity>("WHERE id = ?", id);
        }

        public async Task<bool> Contains(int id)
        {
            return (await GetLast(id)) != null;
        }

        public async IAsyncEnumerable<int> SelectAllIds()
        {
            IPage<LevelEntity> page = null;
            while (page == null || page.Count != 0)
            {
                page = await mapper.FetchPageAsync<LevelEntity>(2, page?.PagingState, $"SELECT id FROM {table.KeyspaceName}.{table.Name}", Array.Empty<object>());
                foreach (var p in page)
                    yield return p.Id;
                if (page.PagingState == null)
                    break;
            }
        }

        public async Task<IEnumerable<LevelEntity>> SelectAll()
        {
            return await mapper.FetchAsync<LevelEntity>("SELECT * FROM geometryapp.levels");
        }
    }
}
