using ElectoralSystem.Repositories.Interfaces;
using ElectoralSystem.Models.Common;
using ElectoralSystem.Models.Entities;
using ElectoralSystem.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralSystem.Repositories
{
    public class PartySymbolRepository : RepositoryBase<PartySymbol>
    {
        public PartySymbolRepository(ElectionDbContext dbContext) : base(dbContext)
        {
        }       
    }
}
