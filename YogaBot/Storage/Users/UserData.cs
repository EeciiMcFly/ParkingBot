using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.Users;

[Index(nameof(TelegramUserId))]
public class UserData
{
	public Guid Id { get; set; }

	public long TelegramUserId { get; set; }
}