using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineDoctorSystem.Services.Data.Interfaces;

namespace OnlineDoctorSystem.Services.Data
{
    public class ConsultationsBackgroundService : BackgroundService, IConsultationsBackgroundService
    {
        private readonly IServiceProvider services;

        public ConsultationsBackgroundService(IServiceProvider services)
        {
            this.services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = services.CreateScope())
                {
                    var consultationsService = scope.ServiceProvider.GetRequiredService<IConsultationsService>();
                    await consultationsService.UpdateConsultationsWhenCompleted();
                }

                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }
    }
}