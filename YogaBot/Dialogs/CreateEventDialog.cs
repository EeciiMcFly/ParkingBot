using YogaBot.Constants;
using YogaBot.DialogEngine;
using YogaBot.Frames;
using YogaBot.MessageQueue;

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
        var message = "Введите дату события и стоимость в формате: \n 10.03.2022 15:00\n1500";
        var answer = new FrameState
        {
            ChatId = context.ChatId,
            MessageText = message,
        };

        //TODO userId вместо TelegramUserId
        dialogStateSetter.SetState(context.TelegramUserId, ProcessEventData);
        outputMessageQueue.AddMessage(answer);
    }

    public bool CanProcess(BotContext context)
    {
        if (context.CallbackData == null)
            return false;
            
        return context.CallbackData.Equals(CallbackDataConstants.CreateEvent);
    }

    public int Priority => 10;

    private async void ProcessEventData(BotContext context)
    {
        //парсим данные и создаем событие
    }
}