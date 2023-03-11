﻿using Telegram.Bot.Types.ReplyMarkups;
using YogaBot.Constants;
using YogaBot.DialogEngine;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using YogaBot.Storage.Arrangements;
using YogaBot.Storage.Events;

namespace YogaBot.Dialogs.EventProcessings;

public class CreateEventDialog : IDialog<BotContext>
{
    private readonly IOutputMessageQueue outputMessageQueue;
    private readonly IDialogStateSetter dialogStateSetter;
    private readonly IEventsRepository eventsRepository;
    private readonly IArrangementRepository arrangementRepository;

    public CreateEventDialog(IOutputMessageQueue outputMessageQueue, IDialogStateSetter dialogStateSetter, 
        IEventsRepository eventsRepository, IArrangementRepository arrangementRepository)
    {
        this.outputMessageQueue = outputMessageQueue;
        this.dialogStateSetter = dialogStateSetter;
        this.eventsRepository = eventsRepository;
        this.arrangementRepository = arrangementRepository;
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
        var arrangementGuid = Int64.Parse(callbackData.Split('/')[1]);
        
        var arrangementEvents = await eventsRepository.GetEventsForArrangementAsync(arrangementGuid);

        if (arrangementEvents.FirstOrDefault(x => x.Date == date) == null)
        {
            var newEvent = new Event
            {
                ArrangementId = Convert.ToInt64(callbackData.Split('/')[1]),
                Cost = Convert.ToInt32(eventInfo[2]),
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

        var arrangement = await arrangementRepository.GetArrangementAsync(arrangementGuid);

        var ikm = new InlineKeyboardMarkup(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData("Удалить занятие", CallbackDataConstants.DeleteEvent + '/' + arrangementGuid)},
            new[] {InlineKeyboardButton.WithCallbackData("Посмотреть запланированные занятия", CallbackDataConstants.GetEvents + '/' + arrangementGuid)},
            new[] {InlineKeyboardButton.WithCallbackData("Рассчитать стоимость", CallbackDataConstants.CalculatePrice + '/' + arrangementGuid)},
            new[] {InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.AllActivities)}
        });
        var answer = new FrameState
        {
            ChatId = context.ChatId,
            MessageType = MessageType.Send,
            MessageText = message,
            Ikm = ikm
        };
            
        outputMessageQueue.AddMessage(answer);
    }
}