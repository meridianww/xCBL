namespace xCBLSoapWebService
{
	public class ShippingScheduleResponse : ShippingSchedule
	{
		public string ScheduleResponseID { get; set; }
		public string ScheduleResponseIssueDate { get; set; }
		public string ScheduleResponseOrderNumber { get; set; }
		public string ScheduleResponsePurposeCoded { get; set; }
		public string ScheduleResponseTypeCoded { get; set; }
	}
}