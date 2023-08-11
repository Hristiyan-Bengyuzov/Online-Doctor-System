namespace OnlineDoctorSystem.Services.Scraping
{
	public interface IDoctorScraperService
	{
		Task<int> Import(int count, int townId);
	}
}
