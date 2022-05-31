namespace MorionParkingBot.Metrics;

public interface IUsingRepository
{
	Task SaveUsingAsync(UsingModel usingModel);
}