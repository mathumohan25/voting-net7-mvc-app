using ElectoralSystem.Models.Entities;

namespace ElectoralSystem.Models.RequestDtos
{
    public class AddVoteRequest
    {
        public int VoterId { get; set; }
        public IReadOnlyList<Candidate> Candidates { get; set; }

        public int SelectedCandidateId { get; set; }

        public string StateName { get; set; }
    }
}
