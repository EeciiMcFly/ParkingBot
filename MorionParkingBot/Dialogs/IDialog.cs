namespace MorionParkingBot.Dialogs;

public interface IDialog<TContext> where TContext : class
{
    void StartDialog(TContext context);
    bool CanProcess(TContext context);
    int Priority { get; }
}