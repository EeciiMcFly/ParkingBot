namespace MorionParkingBot.Database;

public interface IPromoCodeRepository
{
	Task<PromoCodeData> GetPromoCodeAsync(string codeString);

	Task UpdatePromoCodeAsync(PromoCodeData promoCodeData);
}