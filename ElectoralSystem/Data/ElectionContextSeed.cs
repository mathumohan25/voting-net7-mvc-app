using ElectoralSystem.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace ElectoralSystem.Data
{
    public class ElectionContextSeed
    {
        public static async Task SeedAsync(ElectionDbContext electionDbContext, IServiceProvider services)
        {
            ILogger<ElectionContextSeed> logger = services.GetRequiredService<ILogger<ElectionContextSeed>>();
            UserManager<IdentityUser> userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var hasher = services.GetRequiredService<IPasswordHasher<IdentityUser>>();


            var userSeeder = new UserSeeder(userManager, roleManager, new PasswordHasherService(hasher));
            userSeeder.CreateRoles().Wait();
            var results = userSeeder.SeedUsers().Result;
            if (!electionDbContext.Symbols.Any())
            {
                var symbols = GetPartySymbols();
                var states = GetStates();
                var parties = GetParties(symbols);
                var candidates = GetCandidates(parties,states);
                var voters = GetVoters(states, results);
                var Ecs = GetElectionCommisioners(results);

                electionDbContext.Symbols.AddRange(symbols);
                electionDbContext.States.AddRange(states);
                electionDbContext.Parties.AddRange(parties);
                electionDbContext.Candidates.AddRange(candidates);
                electionDbContext.Voters.AddRange(voters);

                await electionDbContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(ElectionDbContext).Name);
            }
        }

        private static IEnumerable<State> GetStates()
        {
            return new List<State>
            {
                new State() { Name = "State_1", NumberOfSeats = 25 },
                new State() { Name = "State_2", NumberOfSeats = 28 },
                new State() { Name = "State_3", NumberOfSeats = 26 }
            };
        }
        private static IEnumerable<PartySymbol> GetPartySymbols()
        {
            return new List<PartySymbol>
            {
                new PartySymbol(){ SymbolName = "Sun",SymbolURI="file://sun_symbol"},
                new PartySymbol(){ SymbolName = "Moon",SymbolURI="file://moon_symbol"},
                new PartySymbol(){ SymbolName = "Star",SymbolURI="file://star_symbol"}
            };
        }
        private static IEnumerable<Party> GetParties(IEnumerable<PartySymbol> symbols)
        {
            return new List<Party>
            {
                new Party(){ Name = "ADA", PartySymbol = symbols.First()},
                new Party(){ Name = "DDA", PartySymbol = symbols.ElementAt(1)}
            };
        }
        private static IEnumerable<Candidate> GetCandidates(IEnumerable<Party> parties, IEnumerable<State> states)
        {
            return new List<Candidate>
            {
                new Candidate() { Name = "Candidate_1", Party = parties.First(), State = states.First() , VotesCount = 0 },
                new Candidate() { Name = "Candidate_2", Party = parties.ElementAt(1), State = states.ElementAt(1) , VotesCount = 0 }
            };
        }
        private static IEnumerable<Voter> GetVoters(IEnumerable<State> states, IList<IdentityUser> results)
        {
            return new List<Voter>
            {
                new Voter() { VoterId = "Voter_001", FirstName = "John", LastName = "W", EmailAddress = "johnW@gmail.com", 
                        AddressLine = "XXXXXX", Country = "IND", State = states.First() , City = "CHN" , ZipCode = "12343", UserId = results[1].Id },
                new Voter() { VoterId = "Voter_002", FirstName = "James", LastName = "W", EmailAddress = "jamesW@gmail.com",
                            AddressLine = "XXXXXX", Country = "IND", State = states.ElementAt(1) , City = "BNG" , ZipCode = "22343", UserId = results[2].Id },
            };
        }

        private static IEnumerable<ElectionCommissioner> GetElectionCommisioners(IList<IdentityUser> results)
        {
            return new List<ElectionCommissioner>
            {
                new ElectionCommissioner() {  FirstName = "Adam", LastName = "W", EmailAddress = "Adam@gmail.com",
                        AddressLine = "XXXXXX", Country = "IND", City = "CHN" , ZipCode = "12343", UserId = results[0].Id },
                new ElectionCommissioner() { FirstName = "Michaels", LastName = "W", EmailAddress = "Michaels@gmail.com",
                            AddressLine = "XXXXXX", Country = "IND" , City = "BNG" , ZipCode = "22343",UserId = results[0].Id},
            };
        }
    }
}
