using SixLabors.ImageSharp;
using Telegram.Bot.Types.ReplyMarkups;
using YogaBot.Models.Poll;

namespace YogaBot.Frames;

public class FrameState
{
	public MessageType MessageType { get; set; } = MessageType.Send;

	public long ChatId { get; set; }

	public int MessageId { get; set; }

	public string MessageText { get; set; }

	public InlineKeyboardMarkup? Ikm { get; set; }

	public List<Image> Images { get; set; }

	public Poll? Poll { get; init; }
}