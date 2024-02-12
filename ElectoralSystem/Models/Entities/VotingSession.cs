using ElectoralSystem.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralSystem.Models.Entities
{
    public class VotingSession : EntityBase
    {
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }

        [ForeignKey("State")]
        public int StateId { get; set; }
        public State State { get; set; }

        [ForeignKey("ElectionResult")]
        public int ElectionResultId { get; set; }
        public ElectionResult Result { get; set; }

        public bool IsCompleted
        {
            get
            {
                return EndDate != DateTime.MinValue;
            }
        }
        //public IEnumerable<Candidate> Candidates { get; set; }
    }
}
