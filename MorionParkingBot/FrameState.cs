using Telegram.Bot.Types.ReplyMarkups;

namespace MorionParkingBot;

public class FrameState
{
	public long ChatId { get; set; }
	
	public int MessageId { get; set; }
	
	public string MessageText { get; set; }
	
	public InlineKeyboardMarkup Ikm { get; set; }
}