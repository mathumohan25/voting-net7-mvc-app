using ElectoralSystem.Models.Common;
using ElectoralSystem.Models.Entities;

namespace ElectoralSystem.Models.RequestDtos
{
    public class NewVoterDto : User
    {
        public string VoterId { get; set; }

        public int SelectedStateId { get; set; }

        public IReadOnlyList<State> StateList { get; set; }
    }
}
