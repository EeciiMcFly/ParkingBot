using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace YogaBot.Storage.Users;

[Index(nameof(TelegramUserId))]
public class User
{
	[Key]
	public long UserId { get; set; }

	public long TelegramUserId { get; set; }
}