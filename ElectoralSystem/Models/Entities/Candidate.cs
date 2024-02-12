using ElectoralSystem.Models.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectoralSystem.Models.Entities
{
    public class Candidate : EntityBase
    {
        //public IEnumerable<Seat> Seats { get; set; }

        public string Name { get; set; }

        [ForeignKey("State")]
        public int StateId { get; set; }
        public State State { get; set; }


        [ForeignKey("Party")]
        public int PartyId { get; set; }
        public Party Party { get; set; }        

        public int? VotesCount {  get; set; }

    }
}
