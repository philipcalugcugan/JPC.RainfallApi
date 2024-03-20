using JPC.Application.RainfallService;
using JPC.Application.Shared.Rainfall.Configuration;
using JPC.Application.Shared.Rainfall.Dto;
using Microsoft.Extensions.Options;
using Moq;

namespace JPC.RainfallApi.Tests
{
    public class RainfallApiTest
    {
        [Fact]
        public async Task GetRainfallReadingsAsync_ReturnsReadings_WhenSuccessful()
        {
            // Arrange
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockOptions = new Mock<IOptions<RainfallApiConfiguration>>();
            var mockHttpClient = new Mock<HttpClient>();
            var cancellationToken = new CancellationToken(false);
            var stationId = "3680";
            var count = 5;

            var config = new RainfallApiConfiguration
            {
                GetByStationIdUrl = "http://environment.data.gov.uk/flood-monitoring/id/stations/"
            };
            mockOptions.Setup(apiconfig => apiconfig.Value).Returns(config);
            mockHttpClientFactory.Setup(client => client.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

            var service = new RainfallService(mockHttpClient.Object, mockOptions.Object.Value);

            // Act
            var result = await service.GetRainfallReadingsAsync(stationId, count, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RainfallReadingResponse>(result);
        }

        [Fact]
        public async Task GetRainfallReadingsAsync_ThrowsRainfallApiException_WhenBadRequest()
        {
            // Arrange
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockOptions = new Mock<IOptions<RainfallApiConfiguration>>();
            var mockHttpClient = new Mock<HttpClient>();
            var cancellationToken = new CancellationToken(false);
            var stationId = "1";
            var count = 5;

            var config = new RainfallApiConfiguration
            {
                GetByStationIdUrl = "http://environment.data.gov.uk/flood-monitoring/id/stations/"
            };
            mockOptions.Setup(apiconfig => apiconfig.Value).Returns(config);
            mockHttpClientFactory.Setup(client => client.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

            var service = new RainfallService(mockHttpClient.Object, mockOptions.Object.Value);

            // Act & Assert
            await Assert.ThrowsAsync<RainfallApiException>(() => service.GetRainfallReadingsAsync(stationId, count, cancellationToken));
        }
    }
}