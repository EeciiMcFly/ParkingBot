namespace MorionParkingBot.Database;

public interface IUsersRepository
{
	public Task<UserData?> GetUserAsync(long telegramUserId);

	public Task AddNewUserAsync(UserData userData);

	public Task UpdateUserAsync(UserData user);

	public Task AddNewLicenseAsync(LicenseInfo licenseInfo);

	public Task UpdateLicenseAsync(LicenseInfo licenseInfo);
}