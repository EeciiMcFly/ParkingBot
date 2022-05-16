namespace MorionParkingBot.Parkings;

public interface IParkingsService
{
	Task<List<ParkingData>> GetParkingForIkmAsync();
}