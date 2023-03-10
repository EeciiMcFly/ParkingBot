namespace MorionParkingBot.Users;

public interface IUsersRepository
{
	public Task<UserData?> GetUserAsync(long telegramUserId);

	public Task AddNewUserAsync(UserData userData);

	public Task UpdateUserAsync(UserData user);
}