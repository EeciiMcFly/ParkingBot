﻿using Microsoft.EntityFrameworkCore;

namespace MorionParkingBot.Users;

public class UsersRepository : IUsersRepository
{
	private readonly UserDbContext _userDbContext;

	public UsersRepository(UserDbContext userDbContext)
	{
		_userDbContext = userDbContext;
	}

	public async Task<UserData?> GetUserAsync(long telegramUserId)
	{
		var userData = await _userDbContext.Users.Include(x => x.LicenseInfos).FirstOrDefaultAsync(data => data.TelegramUserId == telegramUserId);

		return userData;
	}

	public async Task AddNewUserAsync(UserData userData)
	{
		await _userDbContext.Users.AddAsync(userData);

		await _userDbContext.SaveChangesAsync();
	}

	public async Task UpdateUserAsync(UserData user)
	{
		_userDbContext.Users.Update(user);

		await _userDbContext.SaveChangesAsync();
	}

	public async Task AddNewLicenseAsync(LicenseInfo licenseInfo)
	{
		await _userDbContext.LicenseInfos.AddAsync(licenseInfo);

		await _userDbContext.SaveChangesAsync();
	}

	public async Task UpdateLicenseAsync(LicenseInfo licenseInfo)
	{
		_userDbContext.LicenseInfos.Update(licenseInfo);

		await _userDbContext.SaveChangesAsync();
	}
}