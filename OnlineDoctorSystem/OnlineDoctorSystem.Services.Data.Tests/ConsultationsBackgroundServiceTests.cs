using Microsoft.Extensions.DependencyInjection;
using Moq;
using OnlineDoctorSystem.Services.Data;
using OnlineDoctorSystem.Services.Data.Interfaces;

namespace OnlineDoctorSystem.Tests.Services.Data
{
	[TestFixture]
	public class ConsultationsBackgroundServiceTests
	{
		[Test]
		public async Task ExecuteAsync_ServiceIsCalled()
		{
			var mockConsultationsService = new Mock<IConsultationsService>();

			var serviceCollection = new ServiceCollection();
			serviceCollection.AddSingleton(mockConsultationsService.Object);

			var serviceProvider = serviceCollection.BuildServiceProvider();

			var mockScope = new Mock<IServiceScope>();
			mockScope.Setup(scope => scope.ServiceProvider).Returns(serviceProvider);

			var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
			mockServiceScopeFactory.Setup(factory => factory.CreateScope()).Returns(mockScope.Object);

			var backgroundService = new ConsultationsBackgroundService(serviceProvider);

			using (var cts = new CancellationTokenSource())
			{
				var cancellationToken = cts.Token;
				cts.CancelAfter(TimeSpan.FromSeconds(1)); // Cancel the test after 1 second

				await backgroundService.StartAsync(cancellationToken);
				await backgroundService.StopAsync(cancellationToken);

				mockConsultationsService.Verify(service => service.UpdateConsultationsWhenCompleted(), Times.AtLeastOnce);
			}
		}
	}
}