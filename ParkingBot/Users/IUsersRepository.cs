namespace ParkingBot.Users;

public interface IUsersRepository
{
	Task GetUserAsync(long telegramUserId);
}