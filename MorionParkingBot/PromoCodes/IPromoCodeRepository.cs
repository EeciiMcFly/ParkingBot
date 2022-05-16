namespace MorionParkingBot.PromoCodes;

public interface IPromoCodeRepository
{
	Task<PromoCodeData> GetPromoCodeAsync(string codeString);

	Task UpdatePromoCodeAsync(PromoCodeData promoCodeData);
}