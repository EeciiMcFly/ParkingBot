using System.Collections.Concurrent;

namespace YogaBot.DialogEngine;

public interface IDialogStateSetter
{
    void SetState(long userId, Action<BotContext> action);
    void ClearState(long userId);
}

public interface IDialogStateStorage
{
    Action<BotContext>? GetUserState(long userId);
}

public class DialogStateStorage : IDialogStateStorage, IDialogStateSetter
{
    private ConcurrentDictionary<long, Action<BotContext>> storage = new();

    public void SetState(long userId, Action<BotContext> action)
    {
        storage.AddOrUpdate(userId, action, (_, _) => action);
    }

    public void ClearState(long userId)
    {
        storage.TryRemove(userId, out _);
    }

    public Action<BotContext>? GetUserState(long userId)
    {
        storage.TryGetValue(userId, out var action);
        return action;
    }
}