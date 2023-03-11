using YogaBot.Constants;
using YogaBot.DialogEngine;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using Telegram.Bot.Types.ReplyMarkups;
using YogaBot.Storage.Arrangements;
using YogaBot.Storage.UserArrangementRelations;
using YogaBot.Storage.Users;

namespace YogaBot.Dialogs;

public class ArrangementDialog : IDialog<BotContext>
{
    private readonly IOutputMessageQueue outputMessageQueue;
    private readonly IDialogStateSetter dialogStateSetter;
    private readonly IArrangementRepository arrangementRepository;
    private readonly IUserArrangementRelationsRepository userArrangementRelationsRepository;
    private readonly IUsersRepository usersRepository;
    
    public ArrangementDialog(IOutputMessageQueue outputMessageQueue, IDialogStateSetter dialogStateSetter, 
        IArrangementRepository arrangementRepository, IUsersRepository usersRepository, IUserArrangementRelationsRepository userArrangementRelationsRepository)
    {
        this.outputMessageQueue = outputMessageQueue;
        this.dialogStateSetter = dialogStateSetter;
        this.arrangementRepository = arrangementRepository;
        this.usersRepository = usersRepository;
        this.userArrangementRelationsRepository = userArrangementRelationsRepository;
    }

    public async void StartDialog(BotContext context)
    {
        /*var eventCount = 1;
        var ggg = eventCount > 6 ? ConstructPluralFindParkingFrame() : ConstructSingleFindParkingFrame();*/

        var innerUserId = await usersRepository.GetUserAsync(context.TelegramUserId)
            ?? throw new Exception("You do not exist");

        var relations = await userArrangementRelationsRepository.GetRelationsForUserAsync(innerUserId.Id);
        var arrangements = await Task.WhenAll(relations.Select(x => arrangementRepository.GetArrangementAsync(x.ArrangementId)));

        var ikm = new InlineKeyboardMarkup(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.Back)}
                .Concat(arrangements.Select(x => InlineKeyboardButton.WithCallbackData(x.Name, CallbackDataConstants.SelectArrangement + "/" + x.Id)))
        });
        
        var answer = new FrameState
        {
            ChatId = context.ChatId,
            MessageId = context.MessageId,
            MessageType = MessageType.Change,
            MessageText = "Тут твои мероприятия",
            Ikm = ikm
        };

        dialogStateSetter.SetState(context.TelegramUserId, ProcessArrangementId);
        outputMessageQueue.AddMessage(answer);
    }

    public bool CanProcess(BotContext context)
    {
        if (context.CallbackData == null)
            return false;

        return context.CallbackData.Equals(CallbackDataConstants.MyActivities);
    }

    public async void ProcessArrangementId(BotContext context)
    {
        if (context.CallbackData.Equals(CallbackDataConstants.Back))
        {
            dialogStateSetter.ClearState(context.TelegramUserId);
            return;
        }

        if (context.CallbackData.Contains(CallbackDataConstants.SelectArrangement))
        {
            var arrangementGuid = new Guid(context.CallbackData.Split('/')[1]);
            var arrangement = await arrangementRepository.GetArrangementAsync(arrangementGuid);
            
            var ikm = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Запланировать занятие", CallbackDataConstants.CreateEvent + '/' + arrangementGuid),
                    InlineKeyboardButton.WithCallbackData("Посмотреть запланированные занятия", CallbackDataConstants.GetEvents + '/' + arrangementGuid),
                    InlineKeyboardButton.WithCallbackData("Удалить занятие", CallbackDataConstants.DeleteEvent + '/' + arrangementGuid),
                },
            });
            var answer = new FrameState
            {
                ChatId = context.ChatId,
                MessageId = context.MessageId,
                MessageType = MessageType.Change,
                MessageText = arrangement.Name,
                Ikm = ikm
            };
            
            outputMessageQueue.AddMessage(answer);
        }
    }

    public int Priority => 10;

    /*public FrameState ConstructSingleFindParkingFrame()
    {
        var buttonsArray = new InlineKeyboardButton[parkingDatas.Count + 1];
        for (int i = 0; i < parkingDatas.Count; i++)
        {
            var parkingIdQueryData = $"parkingId:{parkingDatas[i].Id}";
            buttonsArray[i] = InlineKeyboardButton.WithCallbackData($"{parkingDatas[i].Name}", parkingIdQueryData);
        }

        buttonsArray[parkingDatas.Count] =
            InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.BackToMainMenu);

        var finalList = new List<List<InlineKeyboardButton>>();
        for (int i = 0; i < buttonsArray.Length; i++)
        {
            if (i % 3 == 0)
            {
                finalList.Add(new List<InlineKeyboardButton>());
            }
            finalList.Last().Add(buttonsArray[i]);
        }

        var ikm = new InlineKeyboardMarkup(finalList);

        var frameState = new FrameState
        {
            ChatId = chatId,
            MessageText = MessageConstants.ChooseParking,
            MessageId = messageId,
            Ikm = ikm
        };

        return frameState;
    }
    
    public FrameState ConstructPluralFindParkingFrame()
    {
        var buttonsArray = new InlineKeyboardButton[6];
        for (int i = 0; i < 4; i++)
        {
            var parkingIdQueryData = $"parkingId:{parkingDatas[i].Id}";
            buttonsArray[i] = InlineKeyboardButton.WithCallbackData($"{parkingDatas[i].Name}", parkingIdQueryData);
        }
	
        buttonsArray[4] =
            InlineKeyboardButton.WithCallbackData("В меню", CallbackDataConstants.BackToMainMenu);
        var nextPortionQuery = $"next:{2}";
        buttonsArray[5] =
            InlineKeyboardButton.WithCallbackData("Далее", nextPortionQuery);
	
        var finalList = new List<List<InlineKeyboardButton>>();
        for (int i = 0; i < buttonsArray.Length; i++)
        {
            if (i % 3 == 0)
            {
                finalList.Add(new List<InlineKeyboardButton>());
            }
            finalList.Last().Add(buttonsArray[i]);
        }
		
        var ikm = new InlineKeyboardMarkup(finalList);
	
        var frameState = new FrameState
        {
            ChatId = chatId,
            MessageText = MessageConstants.ChooseParking,
            MessageId = messageId,
            Ikm = ikm
        };
	
        return frameState;
    }*/
}