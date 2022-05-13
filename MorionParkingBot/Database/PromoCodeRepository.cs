using Microsoft.EntityFrameworkCore;

namespace MorionParkingBot.Database;

public class PromoCodeRepository : IPromoCodeRepository
{
	private readonly PromoCodeDbContext _promoCodeDbContext;

	public PromoCodeRepository(PromoCodeDbContext promoCodeDbContext)
	{
		_promoCodeDbContext = promoCodeDbContext;
	}

	public async Task<PromoCodeData> GetPromoCodeAsync(string codeString)
	{
		var promoCode = await _promoCodeDbContext.PromoCodes
			.FirstOrDefaultAsync(code => code.CodeString.Equals(codeString));

		return promoCode;
	}

	public async Task UpdatePromoCodeAsync(PromoCodeData promoCodeData)
	{
		_promoCodeDbContext.PromoCodes.Update(promoCodeData);

		await _promoCodeDbContext.SaveChangesAsync();
	}
}