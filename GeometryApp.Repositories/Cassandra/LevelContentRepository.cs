// using System;
// using System.Text;
// using System.Threading.Tasks;
// using Cassandra.Data.Linq;
// using Cassandra.Mapping;
// using Folleach.Storage;
// using GeometryApp.Repositories.Entities;
// using GeometryDashAPI.Server;
// using GeometryDashAPI.Server.Enums;
// using GeometryDashAPI.Server.Responses;

// namespace GeometryApp.Repositories.Cassandra;

// public class LevelContentRepository : ILevelContentRepository
// {
//     private readonly IStorage storage;
//     private readonly Mapper mapper;

//     public LevelContentRepository(GeometryAppCassandra context, GeometryAppMappings mappings, IStorage storage)
//     {
//         this.storage = storage;
//         var configuration = new MappingConfiguration().Define(mappings);
//         mapper = new Mapper(context.GetSession(), configuration);
//         new Table<LevelEntity>(context.GetSession(), configuration).CreateIfNotExists();
//     }

//     public async Task Save(int id, int statusCode, int httpStatusCode, DateTime checkDate, ServerResponse<LevelResponse> level)
//     {
//         var storageId = Guid.NewGuid();
//         var content = Encoding.UTF8.GetBytes(level.GetRawOrDefault());
//         var result = await storage.Insert(storageId, content);
//         if (!result.Status)
//             throw new InvalidOperationException($"not saved to storage: {id}");
//         var entity = GetLevelEntity(level, id, checkDate, statusCode, storageId, httpStatusCode);
//         await mapper.InsertAsync(entity);
//     }

//     public static LevelEntity GetLevelEntity(
//             ServerResponse<LevelResponse> response,
//             int id,
//             DateTime checkDate,
//             int statusCode,
//             Guid storageId,
//             int httpStatusCode)
//         {
//             var levelResponse = response.GetResultOrDefault();
//             var level = levelResponse?.Level;
//             if (level?.WithoutLoaded.Count > 0)
//                 throw new Exception($"Find unnecessary properties: {string.Join(", ", level.WithoutLoaded)}.");
//             return new LevelEntity()
//             {
//                 Id = id,
//                 CheckDate = checkDate.Truncate(TimeSpan.TicksPerMillisecond),
//                 StatusCode = statusCode,
//                 StorageId = storageId,
//                 HttpStatusCode = httpStatusCode,
//                 Difficulty = (int)(level?.Difficulty ?? Difficulty.Hard),
//                 Downloads = level?.Downloads ?? 0,
//                 Likes = level?.Likes ?? 0,
//                 Name = level?.Name,
//                 Version = level?.Version ?? 0,
//                 RawDate = level?.UploadDateTime,
//                 AuthorUserId = level?.AuthorUserId ?? 0,
//                 Description = level?.Description,
//                 Auto = level?.Auto ?? false,
//                 Coins = level?.Coins ?? 0,
//                 Epic = level?.Epic ?? false,
//                 Extra = level?.ExtraString,
//                 Length = level?.Length ?? 0,
//                 Objects = level?.Objects ?? 0,
//                 Stars = level?.Stars ?? 0,
//                 CoinsVerified = level?.CoinsVerified ?? false,
//                 CopiedId = level?.CopiedId ?? 0,
//                 DemonDifficulty = (int)(level?.DemonDifficulty ?? DemonDifficulty.Hard),
//                 EditorTime = level?.EditorTime,
//                 FeatureScore = level?.FeatureScore ?? 0,
//                 GameVersion = level?.GameVersion ?? 0,
//                 IsDemon = level?.Demon ?? false,
//                 LowDetail = level?.LowDetail ?? false,
//                 MusicId = level?.MusicId ?? 0,
//                 OfficialSong = level?.OfficialSong ?? 0,
//                 RawPassword = level?.RawPassword,
//                 SlaveDifficulty = (int)(level?.DifficultyIcon ?? DifficultyIcon.Auto),
//                 StarsRequested = level?.StarsRequested ?? 0,
//                 TwoPlayer = level?.TwoPlayer ?? false,
//                 EditorTimeSum = level?.EditorTimeCopies,
//                 RawUpdateDate = level?.SecondDateTime
//             };
//         }
// }
