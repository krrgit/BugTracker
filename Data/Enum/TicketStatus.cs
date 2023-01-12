using System.ComponentModel;

namespace BugTracker.Data.Enum
{
	public enum TicketStatus
	{
		Resolved,
		New,
		Open,
        [Description("In Progress")]
        InProgress,
        [Description("Add. Info Req'd")]
        AddInfoReqd
	}
}
