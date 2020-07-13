namespace xCBLSoapWebService.M4PL.Electrolux.DeliveryUpdateRequest
{
	public class DeliveryUpdate
	{
		public string ServiceProviderID { get; set; }
		public string OrderNumber { get; set; }
		public string OrderDate { get; set; }
		public string SPTransactionID { get; set; }
		public string InstallStatus { get; set; }
		public string InstallStatusTS { get; set; }
		public string PlannedInstallDate { get; set; }
		public string ScheduledInstallDate { get; set; }
		public string ActualInstallDate { get; set; }
		public string RescheduledInstallDate { get; set; }
		public string RescheduleReason { get; set; }
		public string CancelDate { get; set; }
		public string CancelReason { get; set; }
		public Exceptions Exceptions { get; set; }
		public string UserNotes { get; set; }
		public string OrderURL { get; set; }
		public POD POD { get; set; }
		public string AdditionalComments { get; set; }
		public OrderLineDetail OrderLineDetail { get; set; }
	}
}