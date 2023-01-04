using System.ComponentModel;

namespace BugTracker.Data.Enum
{
	public enum TicketType
	{
		[Description("Bugs/Errors")]
		BugsOrErrors,
		[Description("Feature Request")]
		FeatureRequest,
		[Description("Other Comment")]
		OtherComment,
		[Description("Training/Document Request")]
		Training
	}

}
