using SixLabors.ImageSharp;
using Telegram.Bot.Types.ReplyMarkups;

namespace YogaBot.Frames;

public class FrameState
{
	public long ChatId { get; set; }
	
	public int MessageId { get; set; }
	
	public string MessageText { get; set; }
	
	public InlineKeyboardMarkup? Ikm { get; set; }
	
	public List<Image> Images { get; set; }
}