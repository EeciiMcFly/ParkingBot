namespace MorionParkingBot;

public static class MessageConstants
{
	public const string StartMessage =
		"Вас приветствует бот Parkoscop. С помощью данного бота вы сможете узнать о наличии свободных парковочных мест.";

	public const string InactiveLicenseStartMessage = "Ваша подсписка не активирована. Введите промокод для активации.";

	public const string InactiveLicenseUseCode = "У вас нет активной подписки. Введите промокод.";

	public const string ActiveLicenseInfoUseCode =
		"У вас активна подписка. Оставшееся количество дней: {0}. Вы можете ввести промокод для продления лицензии.";
}