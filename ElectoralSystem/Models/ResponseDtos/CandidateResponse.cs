using ElectoralSystem.Models.Entities;

namespace ElectoralSystem.Models.ResponseDtos
{
    public class CandidateResponse
    {
        public int CandidateId { get; set; }

        public string Name { get; set; }
        //public IReadOnlyList<Seat> ContestingSeats { get; set; }
        public Party PartyName { get; set; }
    }
}
