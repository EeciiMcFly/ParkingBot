using Microsoft.EntityFrameworkCore;

namespace MorionParkingBot.Database;

[Index(nameof(Id))]
public class LicenseInfo
{
	public long Id { get; set; }
	
	public long UserId { get; set; }
	
	public UserData User { get; set; }
	
	public DateTime ExpirationTime { get; set; }
}