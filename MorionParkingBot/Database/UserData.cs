using Microsoft.EntityFrameworkCore;

namespace MorionParkingBot.Database;

[Index(nameof(TelegramUserId))]
public class UserData
{
	public long Id { get; set; }

	public long TelegramUserId { get; set; }

	public LicenseInfo LicenseInfo { get; set; }
}