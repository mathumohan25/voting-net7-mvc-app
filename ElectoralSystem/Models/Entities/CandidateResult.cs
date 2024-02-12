using ElectoralSystem.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectoralSystem.Models.Entities
{
    public class CandidateResult : EntityBase
    {

        public string Name { get; set; }
        public int VotesCountResult { get; set; }
        public string  PartyName { get; set; }
        public string PartySymbol { get; set; }

        [ForeignKey("ElectionResult")]
        public int ElectionResultId { get; set; }
        public ElectionResult Result { get; set; }

    }
}
