using YogaBot.Constants;
using YogaBot.DialogEngine;
using YogaBot.Frames;
using YogaBot.MessageQueue;
using Telegram.Bot.Types.ReplyMarkups;

namespace YogaBot.Dialogs;

public class ActivityDialog : IDialog<BotContext>
{
    private readonly IOutputMessageQueue outputMessageQueue;
    private readonly IDialogStateSetter dialogStateSetter;

    public ActivityDialog(IOutputMessageQueue outputMessageQueue, IDialogStateSetter dialogStateSetter)
    {
        this.outputMessageQueue = outputMessageQueue;
        this.dialogStateSetter = dialogStateSetter;
    }

    public void StartDialog(BotContext context)
    {
        /*var eventCount = 1;
        var ggg = eventCount > 6 ? ConstructPluralFindParkingFrame() : ConstructSingleFindParkingFrame();*/

        var ikm = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Назад", CallbackDataConstants.Back),
            },
        });
        var answer = new FrameState
        {
            ChatId = context.ChatId,
            MessageId = context.MessageId,
            MessageType = MessageType.Change,
            MessageText = "Тут твои события",
            Ikm = ikm
        };

        dialogStateSetter.SetState(context.TelegramUserId, ProcessEventId);
        outputMessageQueue.AddMessage(answer);
    }

    public bool CanProcess(BotContext context)
    {
        if (context.CallbackData == null)
            return false;

        return context.CallbackData.Equals(CallbackDataConstants.MyActivities);
    }

    public void ProcessEventId(BotContext context)
    {
        if (context.CallbackData.Equals(CallbackDataConstants.Back))
        {
            dialogStateSetter.ClearState(context.TelegramUserId);
            return;
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