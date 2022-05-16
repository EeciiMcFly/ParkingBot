namespace MorionParkingBot.PromoCodes;

public class PromoCodeData
{
	public long Id { get; set; }
	
	public string CodeString { get; set; }
	
	public PromoCodeType PromoCodeType { get; set; }
	
	public bool IsActivated { get; set; }
}