using ElectoralSystem.Models.Common;
using ElectoralSystem.Models.Entities;

namespace ElectoralSystem.Models.RequestDtos
{
    public class NewCandidateDto 
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }

        public int SelectedPartyId { get; set; }

        public IReadOnlyList<Party> PartyList { get; set; }
        public int SelectedStateId { get; set; }

        public IReadOnlyList<State> StateList { get; set; }
    }
}
