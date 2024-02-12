using ElectoralSystem.Models.Common;
using ElectoralSystem.Models.Entities;

namespace ElectoralSystem.Models.RequestDtos
{
    public class NewVotingSessionDto
    {
        public int Id { get; set; } = 0;
        public int SelectedStateId { get; set; }

        public IReadOnlyList<State> StateList { get; set; }
    }
}
