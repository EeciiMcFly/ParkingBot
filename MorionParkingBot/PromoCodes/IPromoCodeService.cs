
using MorionParkingBot.Database;

namespace MorionParkingBot.PromoCodes;

public interface IPromoCodeService
{
	Task<ActivationResult> ActivatePromoCodeAsync(UserData user, string codeString);
}