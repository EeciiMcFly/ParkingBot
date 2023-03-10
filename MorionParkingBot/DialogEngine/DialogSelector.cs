using MorionParkingBot.Dialogs;

namespace MorionParkingBot.DialogEngine;

public interface IDialogSelector
{
    IDialog<BotContext> SelectDialog(BotContext botContext);
}

public class DialogSelector : IDialogSelector
{
    private IDialog<BotContext>[] dialogs;
    private DefaultDialog defaultDialog;

    public DialogSelector(IDialog<BotContext>[] dialogs, DefaultDialog defaultDialog)
    {
        this.dialogs = dialogs.OrderByDescending(e => e.Priority).ToArray();
        this.defaultDialog = defaultDialog;
    }

    public IDialog<BotContext> SelectDialog(BotContext botContext)
    {
        foreach (var dialog in dialogs)
        {
            if (dialog.CanProcess(botContext))
                return dialog;
        }

        return defaultDialog;
    }
}