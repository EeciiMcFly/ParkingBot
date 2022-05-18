namespace MorionParkingBot.PromoCodes;

public interface IPromoCodeService
{
	Task<ActivationResult> ActivatePromoCodeAsync(string codeString, BotContext botContext);
}