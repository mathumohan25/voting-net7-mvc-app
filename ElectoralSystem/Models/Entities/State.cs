using ElectoralSystem.Models.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ElectoralSystem.Models.Entities
{
    public class State : EntityBase
    {
        public string Name { get; set; }

        [Range(1,60)]
        public int NumberOfSeats { get; set; }

    }
}
