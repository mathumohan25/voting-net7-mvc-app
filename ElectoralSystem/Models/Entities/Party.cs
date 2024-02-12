using ElectoralSystem.Models.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectoralSystem.Models.Entities
{
    public class Party : EntityBase
    {
        public string Name { get; set; }

        [ForeignKey("PartySymbol")]
        public int PartySymbolId { get; set; }
        public PartySymbol? PartySymbol { get; set; }

        public IEnumerable<Candidate> Candidates { get; set; }
    }
}
