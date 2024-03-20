using JPC.Application.Shared.Rainfall.Dto;

namespace JPC.Application.RainfallService
{
    public interface IRainfallService
    {
        Task<RainfallReadingResponse> GetRainfallReadingsAsync(string stationId, int count, CancellationToken cancellationToken);
    }
}
