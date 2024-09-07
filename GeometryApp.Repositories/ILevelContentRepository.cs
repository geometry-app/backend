using System;
using System.Threading.Tasks;
using GeometryDashAPI.Server;
using GeometryDashAPI.Server.Responses;

namespace GeometryApp.Repositories;

public interface ILevelContentRepository
{
    Task Save(int id, int statusCode, int httpStatusCode, DateTime checkDate, ServerResponse<LevelResponse> level);
}
