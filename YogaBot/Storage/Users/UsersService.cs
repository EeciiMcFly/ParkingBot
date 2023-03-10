namespace YogaBot.Storage.Users;

public interface IUsersService
{
	Task<UserData> GetOrCreateUserAsync(long telegramUserId);

	Task UpdateUserAsync(UserData user);
}

public class UsersService : IUsersService
{
	private readonly IUsersRepository _usersRepository;

	public UsersService(IUsersRepository usersRepository)
	{
		_usersRepository = usersRepository;
	}

	public async Task<UserData> GetOrCreateUserAsync(long telegramUserId)
	{
		var user = await _usersRepository.GetUserAsync(telegramUserId);
		if (user == null)
		{
			var newUserData = new UserData
			{
				TelegramUserId = telegramUserId,
			};

			await _usersRepository.AddNewUserAsync(newUserData);

			var createdUser = await _usersRepository.GetUserAsync(telegramUserId);

			await _usersRepository.UpdateUserAsync(createdUser);
			user = createdUser;
		}

		return user;
	}

	public async Task UpdateUserAsync(UserData userData)
	{
		await _usersRepository.UpdateUserAsync(userData);
	}
}