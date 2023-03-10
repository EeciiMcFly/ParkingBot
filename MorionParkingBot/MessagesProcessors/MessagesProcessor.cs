using MorionParkingBot.Frames;
using MorionParkingBot.MessageQueue;

namespace MorionParkingBot.MessagesProcessors;

public class MessagesProcessor
{
	//private readonly ChatId _myChatId = new("340612851");

	private readonly IOutputMessageQueue outputMessageQueue;

	private CancellationTokenSource cancellationTokenSource;

	public MessagesProcessor(IOutputMessageQueue outputMessageQueue)
	{
		this.outputMessageQueue = outputMessageQueue;
	}

	public async Task ProcessMessage(BotContext botContext)
	{
		if (botContext.MessageText == "/start")
		{
			await ProcessStartMessageAsync(botContext);
		}
	}

	private async Task ProcessStartMessageAsync(BotContext botContext)
	{
		var firstFrameState = new FrameState
		{
			ChatId = botContext.ChatId,
			MessageText = "Привет это Йога бот!",
		};

		var frameStateList = new List<FrameState>
		{
			firstFrameState
		};

		foreach (var currentState in frameStateList)
		{
			outputMessageQueue.AddMessage(currentState);
		}
	}
}