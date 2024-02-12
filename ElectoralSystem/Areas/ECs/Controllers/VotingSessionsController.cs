using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectoralSystem.Data;
using ElectoralSystem.Models.Entities;
using AutoMapper;
using ElectoralSystem.Repositories.Interfaces;
using ElectoralSystem.Repositories;
using System.Linq.Expressions;
using ElectoralSystem.Models.RequestDtos;

namespace ElectoralSystem.Areas.ECs.Controllers
{
    [Area("ECs")]
    public class VotingSessionsController : Controller
    {
        private readonly IAsyncRepository<VotingSession> _voteSessionRepository;
        private readonly IAsyncRepository<State> _stateRepository;
        private readonly IAsyncRepository<ElectionResult> _electionResultRepository;
        private readonly IAsyncRepository<Candidate> _candidateRepository;
        private readonly IAsyncRepository<CandidateResult> _candidateResultRepository;
        private readonly IMapper _mapper;

        public VotingSessionsController(IAsyncRepository<VotingSession> repository,
            IAsyncRepository<ElectionResult> repository2, IAsyncRepository<State> staterepository,
            IAsyncRepository<Candidate> candidaterepository,
            IAsyncRepository<CandidateResult> candidateResultRepository, IMapper mapper)
        {
            _voteSessionRepository = repository;
            _electionResultRepository = repository2;
            _candidateRepository = candidaterepository;
            _stateRepository = staterepository;
            _candidateResultRepository = candidateResultRepository;
            _mapper = mapper;
        }

        // GET: ECs/VotingSessions
        public async Task<IActionResult> List()
        {
            List<Expression<Func<VotingSession, object>>> includes = new List<Expression<Func<VotingSession, object>>>
            {
                c=> c.State                
            };
            var sessions = await _voteSessionRepository.GetAllAsync(includes);
            return View(sessions);
        }

        // GET: ECs/VotingSessions/Details/5
        public async Task<IActionResult> Result(int? id)
        {
            if (id == null)
                return NotFound();
            List<Expression<Func<VotingSession, object>>> includes = new List<Expression<Func<VotingSession, object>>>
            {
                c=> c.State,
                b => b.Result,
                d => d.Result.CandidateResults
            };
            var sessions = await _voteSessionRepository.GetAsync(x => x.Id == id.Value, null, includes, false);
            if (sessions == null || sessions.Count <= 0)
                return NotFound();            
            return View(sessions.First());
        }

        [HttpGet]
        public async Task<IActionResult> Start()
        {
            NewVotingSessionDto dto = new NewVotingSessionDto();
            dto.StateList = await _stateRepository.GetAllAsync();
            return View(dto);
        }
        [HttpPost]
        [ActionName("Start")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Start2(NewVotingSessionDto dto)
        {
            //if (ModelState.IsValid)
            {
                try
                {
                    State state = await _stateRepository.GetByIdAsync(dto.SelectedStateId);
                    if (state != null)
                    {
                        VotingSession session = new VotingSession();
                        session.StartDate = DateTime.Now;
                        session.State = state;
                        await _voteSessionRepository.AddAsync(session);
                    }
                    TempData["success"] = "Voting session Started successfully";

                }
                catch (Exception e)
                {
                    TempData["error"] = "Not able to start the Voting session";
                }

            }
            return RedirectToAction("List");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Stop(int? id)
        {
            //if (ModelState.IsValid)
            {
                try
                {
                    var sessions = await _voteSessionRepository.GetAsync(x => x.Id == id,
                        null, nameof(State), false);
                    if(sessions == null || sessions.Count <= 0)
                        return RedirectToAction("List");
                    var session = sessions[0];
                    List<Expression<Func<Candidate, object>>> includes = new List<Expression<Func<Candidate, object>>>
                    {
                        c=> c.State,
                        b => b.Party,
                        b => b.Party.PartySymbol
                    };
                    var candidates = await _candidateRepository.GetAsync(x => x.StateId == session.State.Id, 
                        null, includes, false);
                    ElectionResult result = new ElectionResult();
                    result.VotingSession = session;
                    result.VotingSessionId = session.Id;
                    result = await _electionResultRepository.AddAsync(result);
                    var collection = new List<CandidateResult>();                    
                    foreach (var candidate in candidates)
                    {
                        var cndResult = new CandidateResult
                        {
                            Name = candidate.Name,
                            PartyName = candidate.Party.Name,
                            PartySymbol = candidate.Party.PartySymbol.SymbolName,
                            VotesCountResult = candidate.VotesCount.Value,
                        };
                        cndResult.ElectionResultId = result.Id;                        
                        candidate.VotesCount = 0;
                        var tem =  await _candidateResultRepository.AddAsync(cndResult);
                        collection.Add(tem);
                        await _candidateRepository.UpdateAsync(candidate);
                    }                    
                    result.CandidateResults = collection;
                    await _electionResultRepository.UpdateAsync(result);
                    session.EndDate = DateTime.Now;
                    
                    await _voteSessionRepository.UpdateAsync(session);
                    TempData["success"] = "Voting session ended successfully";

                }
                catch (Exception e)
                {
                    TempData["error"] = "Not able to stop the Voting session";
                }

            }
            return RedirectToAction("List");

        }
    }
}
