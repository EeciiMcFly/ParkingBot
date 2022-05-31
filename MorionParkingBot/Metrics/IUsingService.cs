using MorionParkingBot.Users;

namespace MorionParkingBot.Metrics;

public interface IUsingService
{
	Task CreateUsingAsync(UserData user);
}