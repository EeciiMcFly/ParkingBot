namespace YogaBot.Storage.Users;

public interface IUsersService
{
	Task<User> GetOrCreateUserAsync(long telegramUserId);

	Task UpdateUserAsync(User user);
}

public class UsersService : IUsersService
{
	private readonly IUsersRepository _usersRepository;

	public UsersService(IUsersRepository usersRepository)
	{
		_usersRepository = usersRepository;
	}

	public async Task<User> GetOrCreateUserAsync(long telegramUserId)
	{
		var user = await _usersRepository.GetUserAsync(telegramUserId);
		if (user == null)
		{
			var newUserData = new User
			{
				TelegramUserId = telegramUserId,
			};

			await _usersRepository.AddUserAsync(newUserData);

			var createdUser = await _usersRepository.GetUserAsync(telegramUserId);

			await _usersRepository.UpdateUserAsync(createdUser);
			user = createdUser;
		}

		return user;
	}

	public async Task UpdateUserAsync(User user)
	{
		await _usersRepository.UpdateUserAsync(user);
	}
}