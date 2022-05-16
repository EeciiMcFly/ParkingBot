using Microsoft.EntityFrameworkCore;

namespace MorionParkingBot.Users;

[Index(nameof(TelegramUserId))]
public class UserData
{
	public long Id { get; set; }

	public long TelegramUserId { get; set; }

	public List<LicenseInfo> LicenseInfos { get; set; }
}