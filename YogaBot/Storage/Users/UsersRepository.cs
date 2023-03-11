﻿using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.Users;

public interface IUsersRepository
{
	Task<User?> GetUserAsync(long telegramUserId);

	Task AddUserAsync(User user);

	Task UpdateUserAsync(User user);
}

public class UsersRepository : IUsersRepository
{
	private readonly StorageDbContext _storageDbContext;

	public UsersRepository(StorageDbContext storageDbContext)
	{
		_storageDbContext = storageDbContext;
	}

	public async Task<User?> GetUserAsync(long telegramUserId)
	{
		var userData = await _storageDbContext.Users.FirstOrDefaultAsync(data => data.TelegramUserId == telegramUserId);

		return userData;
	}

	public async Task AddUserAsync(User user)
	{
		await _storageDbContext.Users.AddAsync(user);

		await _storageDbContext.SaveChangesAsync();
	}

	public async Task UpdateUserAsync(User user)
	{
		_storageDbContext.Users.Update(user);

		await _storageDbContext.SaveChangesAsync();
	}
}