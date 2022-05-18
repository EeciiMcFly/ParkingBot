namespace MorionParkingBot;

public class BotContext
{
	public long ChatId { get; set; }

	public int MessageId { get; set; }

	public string MessageText { get; set; }
	
	public string CallbackData { get; set; }

	public long TelegramUserId { get; set; }
}