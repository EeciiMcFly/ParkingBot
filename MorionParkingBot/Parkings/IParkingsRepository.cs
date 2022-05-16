namespace MorionParkingBot.Parkings;

public interface IParkingsRepository
{
	Task<List<ParkingData>> GetAllParkingsAsync();

	Task<ParkingData> GetParkingByNameAsync(string parkingName);
}