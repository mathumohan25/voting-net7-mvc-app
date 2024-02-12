using ElectoralSystem.Models.Common;
using ElectoralSystem.Models.Entities;

namespace ElectoralSystem.Models.RequestDtos
{
    public class NewPartyDto
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }

        public int SelectedPartySymbolId { get; set; }

        public IReadOnlyList<PartySymbol> PartySymbolsList { get; set; }
    }
}
