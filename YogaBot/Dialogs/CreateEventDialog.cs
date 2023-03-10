using YogaBot.Constants;
using YogaBot.DialogEngine;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using YogaBot.Storage.Events;

namespace YogaBot.Dialogs;

public class CreateEventDialog : IDialog<BotContext>
{
    private readonly IOutputMessageQueue outputMessageQueue;
    private readonly IDialogStateSetter dialogStateSetter;

    public CreateEventDialog(IOutputMessageQueue outputMessageQueue, IDialogStateSetter dialogStateSetter)
    {
        this.outputMessageQueue = outputMessageQueue;
        this.dialogStateSetter = dialogStateSetter;
    }

    public void StartDialog(BotContext context)
    {
        var message = "Введите название события, дату и стоимость в формате: \nЙога \n10.03.2022 15:00\n1500";
        var answer = new FrameState
        {
            ChatId = context.ChatId,
            MessageText = message,
        };

        //TODO userId вместо TelegramUserId
        dialogStateSetter.SetState(context.TelegramUserId, ctx => ProcessArrangementData(ctx, context.CallbackData));
        outputMessageQueue.AddMessage(answer);
    }

    public bool CanProcess(BotContext context)
    {
        if (context.CallbackData == null)
            return false;
            
        return context.CallbackData.Contains(CallbackDataConstants.CreateEvent);
    }

    public int Priority => 10;

    private async void ProcessArrangementData(BotContext context, string callbackData)
    {
        var eventInfo = context.MessageText.Split("\n").Select(x => x.Trim()).ToList();

        var newEvent = new EventData
        {
            Id = BitConverter.ToInt64(Guid.NewGuid().ToByteArray()),
            ArrangementId = 0,//(long)Convert.ToDouble(callbackData),
            Cost = Convert.ToInt32(eventInfo[2]),
            Name = eventInfo[0],
            Date = DateTime.Parse(eventInfo[1])
        };
    }
}