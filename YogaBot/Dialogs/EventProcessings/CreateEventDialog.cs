using Telegram.Bot.Types.ReplyMarkups;
using YogaBot.Constants;
using YogaBot.DialogEngine;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using YogaBot.Models.KeyboardBuilder;
using YogaBot.Storage.Events;

namespace YogaBot.Dialogs.EventProcessings;

public class CreateEventDialog : IDialog<BotContext>
{
    private readonly IOutputMessageQueue outputMessageQueue;
    private readonly IDialogStateSetter dialogStateSetter;
    private readonly IEventsRepository eventsRepository;
    private readonly IKeyboardBuilder keyboardBuilder;

    public CreateEventDialog(IOutputMessageQueue outputMessageQueue,
        IDialogStateSetter dialogStateSetter,
        IEventsRepository eventsRepository,
        IKeyboardBuilder keyboardBuilder)
    {
        this.outputMessageQueue = outputMessageQueue;
        this.dialogStateSetter = dialogStateSetter;
        this.eventsRepository = eventsRepository;
        this.keyboardBuilder = keyboardBuilder;
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
        var date = DateTime.Parse(eventInfo[1]).ToUniversalTime();
        var message = string.Empty;
        var arrangementId = Int64.Parse(callbackData.Split('/')[1]);

        var arrangementEvents = await eventsRepository.GetEventsForArrangementAsync(arrangementId);

        if (arrangementEvents.FirstOrDefault(x => x.Date == date) == null)
        {
            var newEvent = new Event
            {
                ArrangementId = Convert.ToInt64(callbackData.Split('/')[1]),
                Cost = Convert.ToInt32(eventInfo[2].Replace(" ", "")),
                Name = eventInfo[0],
                Date = DateTime.Parse(eventInfo[1]).ToUniversalTime()
            };
            await eventsRepository.AddEventAsync(newEvent);
            message = "Событие успешно добавлено";
        }
        else
        {
            message = "Событие на данное время уже запланировано";
        }

        var ikm = await keyboardBuilder.BuildForArrangementFrameAsync(context, arrangementId);

        var answer = new FrameState
        {
            ChatId = context.ChatId,
            MessageType = MessageType.Send,
            MessageText = message,
            Ikm = ikm.ToArray()
        };

        outputMessageQueue.AddMessage(answer);
    }
}