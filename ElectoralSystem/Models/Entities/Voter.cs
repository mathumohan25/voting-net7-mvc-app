using ElectoralSystem.Models.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectoralSystem.Models.Entities
{
    public class Voter : User
    {
        public string VoterId { get; set; }

        [ForeignKey("State")]
        public int StateId { get; set; }
        public virtual State State { get; set; }
        public bool IsEligible { get; set; }
        public bool IsVoted { get; set; }

        // Foreign Key Relationship to IdentityUser
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
    }
}
