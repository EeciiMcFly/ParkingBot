﻿namespace MorionParkingBot.Users;

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
			var newLicenseInfo = new LicenseInfo
			{
				UserId = createdUser.Id,
				User = createdUser,
				ExpirationTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, DateTimeKind.Utc)
			};

			await _usersRepository.AddNewLicenseAsync(newLicenseInfo);
			createdUser.LicenseInfos = new List<LicenseInfo> {newLicenseInfo};
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