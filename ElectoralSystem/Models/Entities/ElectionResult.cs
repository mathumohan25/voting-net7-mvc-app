using ElectoralSystem.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralSystem.Models.Entities
{
    public class ElectionResult : EntityBase
    {
        [ForeignKey("VotingSession")]
        public int VotingSessionId { get; set; }

        public VotingSession VotingSession { get; set; }
        public IEnumerable<CandidateResult> CandidateResults { get; set; }
    }
}
