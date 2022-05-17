namespace MorionParkingBot.Constants;

public static class MessageConstants
{
	public const string StartMessage =
		"Вас приветствует бот Parkoscop. С помощью данного бота вы сможете узнать о наличии свободных парковочных мест.";

	public const string InactiveLicenseStartMessage = "Ваша подсписка не активирована. Введите промокод для активации.";

	public const string InactiveLicenseUseCode = "У вас нет активной подписки. Введите промокод.";

	public const string ActiveLicenseInfoUseCode =
		"У вас активна подписка. Оставшееся количество дней: {0}. Вы можете ввести промокод для продления лицензии.";

	public const string PromoCodeNotExist = "Некорректный промокод. Попробуйте еще раз.";

	public const string PromoCodeAlreadyActivated = "Промокод уже активирован. Попробуйте другой.";

	public const string PromoCodeActivated = "Промокод успешно активирован. Ваша лицензия обновлена.";

	public const string ChooseParking = "Выберите парковку на которой хотите найти место";

	public const string NoParking = "На парковке {0} свободное место не найдено";

	public const string ParkingFound = "Нашел парковочное место. Вот фото:";
}