using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.Users;

public interface IUsersRepository
{
	Task<UserData?> GetUserAsync(long telegramUserId);

	Task AddUserAsync(UserData userData);

	Task UpdateUserAsync(UserData user);
}

public class UsersRepository : IUsersRepository
{
	private readonly StorageDbContext _storageDbContext;

	public UsersRepository(StorageDbContext storageDbContext)
	{
		_storageDbContext = storageDbContext;
	}

	public async Task<UserData?> GetUserAsync(long telegramUserId)
	{
		var userData = await _storageDbContext.Users.FirstOrDefaultAsync(data => data.TelegramUserId == telegramUserId);

		return userData;
	}

	public async Task AddUserAsync(UserData userData)
	{
		await _storageDbContext.Users.AddAsync(userData);

		await _storageDbContext.SaveChangesAsync();
	}

	public async Task UpdateUserAsync(UserData user)
	{
		_storageDbContext.Users.Update(user);

		await _storageDbContext.SaveChangesAsync();
	}
}