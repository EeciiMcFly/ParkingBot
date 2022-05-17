namespace MorionParkingBot.Parkings;

public interface IParkingsService
{
	Task<List<ParkingData>> GetParkingForIkmAsync();

	Task<FindParkingResult> FindParkingAsync(long parkingId);
}