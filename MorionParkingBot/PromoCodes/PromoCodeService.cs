using MorionParkingBot.Database;
using MorionParkingBot.Users;

namespace MorionParkingBot.PromoCodes;

public class PromoCodeService : IPromoCodeService
{
	private readonly IPromoCodeRepository _promoCodeRepository;
	private readonly IUsersService _usersService;

	public PromoCodeService(IPromoCodeRepository promoCodeRepository, 
		IUsersService usersService)
	{
		_promoCodeRepository = promoCodeRepository;
		_usersService = usersService;
	}

	public async Task<ActivationResult> ActivatePromoCodeAsync(UserData user, string codeString)
	{
		var promoCode = await _promoCodeRepository.GetPromoCodeAsync(codeString);

		if (promoCode == null)
			return ActivationResult.CodeNotExist;

		if (promoCode.IsActivated)
			return ActivationResult.AlreadyActivated;

		var isLicenseActive = user.LicenseInfo.ExpirationTime > DateTime.UtcNow;
		var licenseStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0,
			DateTimeKind.Utc);
		if (isLicenseActive)
			licenseStartTime = user.LicenseInfo.ExpirationTime;
			
		if (promoCode.PromoCodeType == PromoCodeType.ForMonth)
			user.LicenseInfo.ExpirationTime = licenseStartTime.AddMonths(1);
		if (promoCode.PromoCodeType == PromoCodeType.ForYear)
			user.LicenseInfo.ExpirationTime = licenseStartTime.AddYears(1);
		promoCode.IsActivated = true;
		await _usersService.UpdateUserAsync(user);
		await _promoCodeRepository.UpdatePromoCodeAsync(promoCode);

		return ActivationResult.SuccessesActivation;
	}
}