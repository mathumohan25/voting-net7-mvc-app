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
    public class ElectionResultsRepository : RepositoryBase<ElectionResult>
    {
        public ElectionResultsRepository(ElectionDbContext dbContext) : base(dbContext)
        {
        }       
    }
}
