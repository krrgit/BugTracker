using BugTracker.Models;
using Microsoft.AspNetCore.Identity;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using System.Diagnostics;
using System.Security.Cryptography;

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

		public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				//Roles
				var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
				if (!await roleManager.RoleExistsAsync(UserRoles.ProjectManager))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.ProjectManager));
				if (!await roleManager.RoleExistsAsync(UserRoles.Developer))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.Developer));
				if (!await roleManager.RoleExistsAsync(UserRoles.Submitter))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.Submitter));

				//Users
				var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<Member>>();
				string adminUserEmail = "kevindev@gmail.com";

				var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
				if (adminUser == null)
				{
					var newAdminUser = new Member()
					{
						UserName = "kevindev",
						FirstName = "Kevin",
						LastName = "Dev",
						Email = adminUserEmail,
						EmailConfirmed = true,
					};
					await userManager.CreateAsync(newAdminUser, "Coding@1234?");
					await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
				}

				// Submitter
				{
					string appUserEmail = "submitter@demo.com";

					var appUser = await userManager.FindByEmailAsync(appUserEmail);
					if (appUser == null)
					{
						var newAppUser = new Member()
						{
							UserName = "submitter",
							FirstName = "Submitter",
							LastName = "Demo",
							Email = appUserEmail,
							EmailConfirmed = true,
						};
						await userManager.CreateAsync(newAppUser, "Coding@1234?");
						await userManager.AddToRoleAsync(newAppUser, UserRoles.Submitter);
					}
				}

				// Developer
				{
					string appUserEmail = "developer@demo.com";

					var appUser = await userManager.FindByEmailAsync(appUserEmail);
					if (appUser == null)
					{
						var newAppUser = new Member()
						{
							UserName = "developer",
							FirstName = "Developer",
							LastName = "Demo",
							Email = appUserEmail,
							EmailConfirmed = true,
						};
						await userManager.CreateAsync(newAppUser, "Coding@1234?");
						await userManager.AddToRoleAsync(newAppUser, UserRoles.Developer);
					}
				}

				// ProjectManager
				{
					string appUserEmail = "manager@demo.com";

					var appUser = await userManager.FindByEmailAsync(appUserEmail);
					if (appUser == null)
					{
						var newAppUser = new Member()
						{
							UserName = "manager",
							FirstName = "Manager",
							LastName = "Demo",
							Email = appUserEmail,
							EmailConfirmed = true,
						};
						await userManager.CreateAsync(newAppUser, "Coding@1234?");
						await userManager.AddToRoleAsync(newAppUser, UserRoles.ProjectManager);
					}
				}
			}
		}

		public static async void SeedRandomUsers(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				//Roles
				var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
				if (!await roleManager.RoleExistsAsync(UserRoles.ProjectManager))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.ProjectManager));
				if (!await roleManager.RoleExistsAsync(UserRoles.Developer))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.Developer));
				if (!await roleManager.RoleExistsAsync(UserRoles.Submitter))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.Submitter));

				string[] FirstNames = { "Maria", "Nushi", "Mohammed", "Jose", "Wei", "Ahmed", "Yan", "Ali", "John", "David", "Li", "Ana", "Ying", "Michael", "Juan", "Mary", "Robert" };
				string[] LastNames = { "Wang", "Li", "Zhang", "Chen", "Liu", "Devi", "Yang", "Huang", "Singh", "Wu", "Kumar", "Xu", "Ali", "Zhao", "Nguyen", "Garcia", "Begum" };
				string[] Roles = { "submitter", "submitter", "submitter", "submitter", "developer", "developer", "developer", "manager" };

				//Users
				var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<Member>>();
				int users = 20;
				var rand = new Random();
				for (int i =0;i<users;++i)
				{
					string first = FirstNames[rand.Next(FirstNames.Length)];
					string last = LastNames[rand.Next(LastNames.Length)];
					string email = first + last + "@email.com";

					var user = await userManager.FindByEmailAsync(email); 

					if (user == null)
					{
						var newUser = new Member()
						{
							UserName = first + last,
							FirstName = first,
							LastName = last,
							Email = email,
							EmailConfirmed = true,
						};

						var role = i < 25 ? UserRoles.Submitter : (i < 47 ? UserRoles.Developer : UserRoles.ProjectManager);
						await userManager.CreateAsync(newUser, "Coding@1111?");
						await userManager.AddToRoleAsync(newUser, UserRoles.Submitter);
					}
				}
			}
		}
	}
}
