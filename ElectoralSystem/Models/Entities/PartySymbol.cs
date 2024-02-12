using ElectoralSystem.Models.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ElectoralSystem.Models.Entities
{
    public class PartySymbol : EntityBase
    {
        public string SymbolName { get; set; }

        public string SymbolURI { get; set; }
        //public byte[] SymbolData { get; set; }
    }
}
