using BugTracker.Models;

namespace BugTracker.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScpe = applicationBuilder.ApplicationServices.CreateScope()) 
            {
                var context = serviceScpe.ServiceProvider.GetService<AppDBContext>();

                context.Database.EnsureCreated();

                if (!context.Projects.Any())
                {
                    // Projects
                    context.Projects.AddRange(new List<Project>()
                    {
                        new Project()
                        {
                            Title = "Project 1",
                            Description = "This is the description of the first project",
                        },
                        new Project()
                        {
                            Title = "Project 2",
                            Description = "This is the description of the second project",
                        },new Project()
                        {
                            Title = "Project 3",
                            Description = "This is the description of the third project",
                        }
                    });
                    context.SaveChanges();
                    // Tickets
                    /*if (!context.Tickets.Any())
                    {
                        context.Tickets.AddRange(new List<Ticket>()
                        { 
                            new Ticket() {
                                Title = "Ticket 1",
                                Description = "This is the first ticket for project 1",
                                Priority = Enum.TicketPriority.Low,
                                Status = Enum.TicketStatus.New,
                                Type = Enum.TicketType.BugOrError,
                                ProjectId = 1
                            },
                            new Ticket() {
                                Title = "Ticket 2",
                                Description = "This is the first ticket for project 1",
                                Priority = Enum.TicketPriority.High,
                                Status = Enum.TicketStatus.Resolved,
                                Type = Enum.TicketType.BugOrError,
                                ProjectId = 1
                            },
                            new Ticket() {
                                Title = "Ticket 1",
                                Description = "This is the first ticket for project 2",
                                Priority = Enum.TicketPriority.Medium,
                                Status = Enum.TicketStatus.Open,
                                Type = Enum.TicketType.BugOrError,
                                ProjectId = 2
                            },
                            new Ticket() {
                                Title = "Ticket 2",
                                Description = "This is the first ticket for project 2",
                                Priority = Enum.TicketPriority.High,
                                Status = Enum.TicketStatus.InProgress,
                                Type = Enum.TicketType.BugOrError,
                                ProjectId = 2
                            }

                        });
                        context.SaveChanges();
                    }*/
                }
            }
        }
    }
}
