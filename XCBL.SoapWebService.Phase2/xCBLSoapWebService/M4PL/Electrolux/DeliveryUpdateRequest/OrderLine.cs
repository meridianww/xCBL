namespace xCBLSoapWebService.M4PL.Electrolux.DeliveryUpdateRequest
{
	public class OrderLine
	{
		public int LineNumber { get; set; }
		public string ItemNumber { get; set; }
		public string ItemInstallStatus { get; set; }
		public string UserNotes { get; set; }
		public string ItemInstallComments { get; set; }
		public Exceptions Exceptions { get; set; }
	}
}