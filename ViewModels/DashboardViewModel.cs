using BugTracker.Models;

namespace BugTracker.ViewModels
{
    public class DashboardViewModel
    {
        public List<List<Ticket>> TicketLists { get; set; }
        public List<int[]> ProjectProgress {get;set;}
        public List<string> ProjectTitles { get; set; }

        public List<string> ProjectColors = new List<string>()
        {
            "#DD0000",
            "#FFCC00",
            "#00CC00",
            "#00FFFF",
            "#0000FF",
            "#FF00FF",
            "#800000",
            "#808000",
            "#008000",
            "#008080",
            "#000080",
            "#800080",
        };
    }
}
