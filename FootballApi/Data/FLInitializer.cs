using Microsoft.EntityFrameworkCore;
using FootballApi.Models;
using System.Diagnostics;
using System.Numerics;
using Microsoft.Extensions.DependencyModel.Resolution;

namespace FootballApi.Data
{
    public class FLInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            FootballContext context = applicationBuilder.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<FootballContext>();
            try
            {
                //Delete the database if you need to apply a new Migration
                context.Database.EnsureDeleted();
                //Create the database if it does not exist and apply the Migration
                context.Database.Migrate();
                Random random = new(99);

                if (!context.Leagues.Any())
                {
                    context.Leagues.AddRange(
                        new League
                        {
                            ID = "AB",
                            Name = "Alberta League"
                        },
                        new League
                        {
                            ID = "ON",
                            Name = "Ontario League"
                        },
                        new League
                        {
                            ID = "QC",
                            Name = "Quebec League"
                        }
                        ) ; 
                        context.SaveChanges();
                }

                if (!context.Teams.Any())
                {
                    context.Teams.AddRange(
                        new Team
                        {
                            Name = "Toronto tigers",
                            Budget = 6000,
                            LeagueID = "ON"
                        },
                        new Team
                        {
                            Name = "Laval Rocket",
                            Budget = 6500,
                            LeagueID = "QC"
                        },
                        new Team
                        {
                            Name = "Niagara titens",
                            Budget = 7200,
                            LeagueID = "ON"
                        },
                        new Team
                        {
                            Name = "Montreal Roasters",
                            Budget = 6600,
                            LeagueID = "QC"
                        },
                        new Team
                        {
                            Name = "Calgary Flames" ,
                            Budget = 7000,
                            LeagueID = "AB"
                        },
                        new Team
                        {
                            Name = "Edmonton Kings",
                            Budget = 8000,
                            LeagueID = "AB"
                        }
                        );
                    context.SaveChanges();
                }

                if (!context.Players.Any())
                {
                    context.Players.AddRange(
                        new Player 
                        {
                            FirstName = "Jhon",
                            LastName = "Doe",
                            DOB = new DateTime(1955,09,01),
                            EMail = "123@abc.com",
                            FeePaid = 150.00,
                            Jersey = "22"

                        },
                        new Player
                        {
                            FirstName = "Harry",
                            LastName = "Mikelson",
                            DOB = new DateTime(1995,06,07),
                            EMail = "163@abc.com",
                            FeePaid = 130.00,
                            Jersey = "15",
                        },
                        new Player
                        {
                            FirstName = "Sam",
                            LastName = "Doe",
                            Jersey = "21",
                            DOB = new DateTime(1991,09,02),
                            FeePaid = 160.00,
                            EMail = "122@abc.com"
                        },
                        new Player
                        {
                            FirstName = "Mit",
                            LastName = "Patel",
                            Jersey = "26",
                            DOB = new DateTime(1993,09,03),
                            FeePaid = 133.00,
                            EMail = "177@abc.com"
                        },
                        new Player
                        {
                            FirstName = "Aman",
                            LastName = "Gill",
                            Jersey = "17",
                            DOB = new DateTime(1994,09,16),
                            FeePaid = 146.00,
                            EMail = "223@abc.com"
                        },
                        new Player
                        {
                            FirstName = "Harsh",
                            LastName = "Shah",
                            Jersey = "08",
                            DOB = new DateTime(1995, 09, 01),
                            FeePaid = 166.00,
                            EMail = "134@abc.com"
                        },
                        new Player
                        {
                            FirstName = "Henry",
                            LastName = "House",
                            Jersey = "14",
                            DOB = new DateTime(1996, 09, 11),
                            FeePaid = 144.00,
                            EMail = "166@abc.com"
                        },
                        new Player
                        {
                            FirstName = "Gautam",
                            LastName = "Jadeja",
                            Jersey = "23",
                            DOB = new DateTime(1991, 09, 07),
                            FeePaid = 199.00,
                            EMail = "187@abc.com"
                        },
                        new Player
                        {
                            FirstName = "Navjot",
                            LastName = "Dhillon",
                            Jersey = "18",
                            DOB = new DateTime(1996, 09, 05),
                            FeePaid = 152.00,
                            EMail = "146@abc.com"
                        },
                        new Player
                        {
                            FirstName = "Ayush",
                            LastName = "Pandit",
                            Jersey = "06",
                            DOB = new DateTime(1997, 09, 20),
                            FeePaid = 133,
                            EMail = "113@abc.com"
                        },
                        new Player
                        {
                            FirstName = "Arpit",
                            LastName = "Gandhi",
                            Jersey = "04",
                            DOB = new DateTime(1992, 09, 03),
                            FeePaid = 150.00,
                            EMail = "101@abc.com"
                        }, new Player
                        {
                            FirstName = "Swapnil",
                            LastName = "Shah",
                            Jersey = "16",
                            DOB = new DateTime(1989, 09, 16),
                            FeePaid = 160.00,
                            EMail = "100@abc.com"
                        }, new Player
                        {
                            FirstName = "Faiz",
                            LastName = "Sheikh",
                            Jersey = "02",
                            DOB = new DateTime(1932, 08, 01),
                            FeePaid = 170.00,
                            EMail = "112@abc.com"
                        }
                        );
                        context.SaveChanges();
                }

                //int[] playerIDs = context.Players.Select(s => s.ID).ToArray();
                //int playerIDCount = playerIDs.Length;
                //int[] teamIDs = context.Teams.Select(s => s.ID).ToArray();
                //int teamIDCount = teamIDs.Length;

                //if (!context.PlayerTeams.Any())
                //{
                //    //i loops through the primary keys of the Patients
                //    //j is just a counter so we add some Conditions to a Patient
                //    //k lets us step through all Conditions so we can make sure each gets used
                //    int k = 0;//Start with the first Condition
                //    foreach (int i in playerIDs)
                //    {
                //        int howMany = random.Next(1, 6);//Add up to 6 teams
                //        for (int j = 1; j <= howMany; j++)
                //        {
                //            k = (k >= teamIDCount) ? 0 : k;//Resets counter k to 0 if we have run out of Conditions
                //            PlayerTeam ds = new PlayerTeam()
                //            {
                //                PlayerId = i,
                //                TeamId = teamIDs[k]
                //            };
                //            k++;
                //            context.PlayerTeams.Add(ds);
                //        }
                //        context.SaveChanges();
                //    }
                //}
                if(!context.PlayerTeams.Any())
                {
                    Random r = new Random();
                    foreach (Player pp in context.Players)
                    {
                        context.PlayerTeams.Add(new PlayerTeam()
                        {
                            PlayerId = pp.ID,
                            TeamId = r.Next(1, 6)
                        });
                    }
                    context.SaveChanges();
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.GetBaseException().Message);
            }
        }
    }
}
