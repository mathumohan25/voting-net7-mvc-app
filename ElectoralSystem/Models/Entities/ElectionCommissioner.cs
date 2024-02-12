using ElectoralSystem.Models.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectoralSystem.Models.Entities
{
    public class ElectionCommissioner : User
    {
        // Foreign Key Relationship to IdentityUser
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
    }
}
