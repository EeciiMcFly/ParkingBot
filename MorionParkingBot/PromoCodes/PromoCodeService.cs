using MorionParkingBot.Users;
using ILogger = Serilog.ILogger;

namespace MorionParkingBot.PromoCodes;

public class PromoCodeService : IPromoCodeService
{
	private readonly IPromoCodeRepository _promoCodeRepository;
	private readonly IUsersService _usersService;
	private readonly ILogger _logger;

	public PromoCodeService(IPromoCodeRepository promoCodeRepository,
		IUsersService usersService,
		ILogger logger)
	{
		_promoCodeRepository = promoCodeRepository;
		_usersService = usersService;
		_logger = logger;
	}

	public async Task<ActivationResult> ActivatePromoCodeAsync(string codeString, BotContext botContext)
	{
		_logger.Information($"Start process promocode - {codeString}");
		var user = await _usersService.GetOrCreateUserAsync(botContext.TelegramUserId);
		_logger.Information($"User for process promocode - {user.Id}");
		var promoCode = await _promoCodeRepository.GetPromoCodeAsync(codeString);

		if (promoCode == null)
		{
			_logger.Information($"Promocode {codeString} not exist");
			return ActivationResult.CodeNotExist;
		}

		if (promoCode.IsActivated)
		{
			_logger.Information($"Promocode {codeString} already activated");
			return ActivationResult.AlreadyActivated;
		}

		_logger.Debug($"ExpirationTime - {user.LicenseInfos.First().ExpirationTime}. Now - {DateTime.UtcNow}");
		var isLicenseActive = user.LicenseInfos.First().ExpirationTime > DateTime.UtcNow;
		_logger.Debug($"isLicenseActive - {isLicenseActive}");
		var licenseStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0,
			DateTimeKind.Utc);
		if (isLicenseActive)
			licenseStartTime = user.LicenseInfos.First().ExpirationTime;

		_logger.Information($"PromoCodeType - {promoCode.PromoCodeType}");
		if (promoCode.PromoCodeType == PromoCodeType.ForMonth)
			user.LicenseInfos.First().ExpirationTime = licenseStartTime.AddMonths(1);
		if (promoCode.PromoCodeType == PromoCodeType.ForYear)
			user.LicenseInfos.First().ExpirationTime = licenseStartTime.AddYears(1);
		promoCode.IsActivated = true;
		promoCode.UserId = user.Id;
		_logger.Information($"New ExpirationTime - {user.LicenseInfos.First().ExpirationTime}");
		await _usersService.UpdateUserAsync(user);
		await _promoCodeRepository.UpdatePromoCodeAsync(promoCode);

		return ActivationResult.SuccessesActivation;
	}
}