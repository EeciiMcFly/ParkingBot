namespace MorionParkingBot.Users;

public interface IUsersService
{
	public Task<UserData> GetOrCreateUserAsync(long telegramUserId);

	public Task UpdateUserAsync(UserData user);
}