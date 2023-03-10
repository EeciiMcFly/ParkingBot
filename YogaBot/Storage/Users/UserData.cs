using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.Users;

[Index(nameof(TelegramUserId))]
public class UserData
{
	public long Id { get; set; }

	public long TelegramUserId { get; set; }
}