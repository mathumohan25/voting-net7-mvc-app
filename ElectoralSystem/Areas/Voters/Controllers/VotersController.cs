using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectoralSystem.Data;
using ElectoralSystem.Models.Entities;
using ElectoralSystem.Repositories.Interfaces;
using ElectoralSystem.Models.RequestDtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace ElectoralSystem.Areas.Voters.Controllers
{
    [Area("Voters")]
    [Authorize(Roles = "Voter")]
    [Route("~/Voters/Voters/")]
    public class VotersController : Controller
    {
        private readonly IAsyncRepository<Voter> _voterRepository;
        private readonly IAsyncRepository<State> _stateRepository;
        private readonly IAsyncRepository<Candidate> _candidateRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public VotersController(IAsyncRepository<Voter> repository, 
            IAsyncRepository<State> repository1, IAsyncRepository<Candidate> repository2,
            IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _voterRepository = repository;
            _stateRepository = repository1;
            _candidateRepository = repository2;
            _userManager = userManager;
            _mapper = mapper;
        }
        [HttpGet("Index/{id:guid}")]
        public IActionResult Index(Guid id)
        {
            return View();
        }
        [HttpGet("Details/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
                return NotFound();
            var voter = await _voterRepository.GetAsync(x => x.UserId == id.ToString(), null, nameof(Voter.State), false);
            if (voter == null || voter.Count <= 0)
                return View("Index");
            return View(voter.First());
        }
        [HttpGet("Register/{id:guid}")]
        public async Task<IActionResult> Register(Guid id)
        {
            var voter = await _voterRepository.GetAsync(x => x.UserId == id.ToString());
            if (voter.Count > 0)
            {
                TempData["error"] = "You are already applied for voting request!!";
                return View("Index");
            }
            NewVoterDto newVoterDto = new NewVoterDto();
            newVoterDto.StateList = await _stateRepository.GetAllAsync();
            return View(newVoterDto);
        }
        [HttpPost("Register/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterPost([FromRoute]Guid id,NewVoterDto voterRequest)
        {            
            
            //if (ModelState.IsValid)
            {
                try
                {
                    State seat = await _stateRepository.GetByIdAsync(voterRequest.SelectedStateId);
                    if(seat != null) 
                    {
                        Voter voter = _mapper.Map<Voter>(voterRequest);
                        voter.UserId = id.ToString();
                        voter.StateId = seat.Id;
                        voter.State = seat;
                        await _voterRepository.AddAsync(voter);
                    }
                    TempData["success"] = "Voter Registeration successful";

                }
                catch (Exception e)
                {
                    TempData["error"] = "Not able to regiter the voter details";
                }
               
            }
            return View("Index");

        }

        [HttpGet("AddVote/{id:guid}")]
        public async Task<IActionResult> AddVote([FromRoute] Guid id)
        {
            var voters = await _voterRepository.GetAsync(x => x.UserId == id.ToString(), null, nameof(Voter.State), false);
            if (voters == null || voters.Count <= 0)
            {
                TempData["error"] = "Not even registered for voting!!";
                return View("Index");
            }
            var voter = voters.First();
            if(!voter.IsEligible)
            {
                //ViewBag.IsEligible = true;
                TempData["error"] = "You are not eligible to vote!!";
                return View("Index");
            }
            else if(voter.IsVoted)
            {
                //ViewBag.IsVoted = true;
                TempData["error"] = "You already voted!!";
                return View("Index");
            }
            AddVoteRequest addVoteRequest = new AddVoteRequest();
            addVoteRequest.VoterId = voter.Id;
            addVoteRequest.StateName = voter.State.Name;
            List<Expression<Func<Candidate, object>>> includes = new List<Expression<Func<Candidate, object>>>
            {
                b => b.Party,
                b => b.Party.PartySymbol
            };
            addVoteRequest.Candidates = await _candidateRepository.GetAsync(x => x.StateId == voter.StateId
                , null, includes, false);
            return View(addVoteRequest);
        }
        [HttpPost("AddVote/{id:guid}")]
        [ActionName("AddVote")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVotePost([FromRoute] Guid id, [Bind("VoterId,SelectedCandidateId,StateName")] AddVoteRequest request)
        {
            //if (ModelState.IsValid)
            {
                try
                {                    
                    var voters = await _voterRepository.GetAsync(x=>x.UserId == id.ToString());
                    if (voters == null || voters.Count <=0)
                        throw new Exception();
                    var voter = voters.First();
                    if (voter == null || voter.IsVoted || !voter.IsEligible)
                        throw new Exception();
                    voter.IsVoted = true;
                    var candidate = await _candidateRepository.GetByIdAsync(request.SelectedCandidateId);
                    if (candidate == null)
                        throw new Exception();
                    candidate.VotesCount++;
                    await _candidateRepository.UpdateAsync(candidate);
                    await _voterRepository.UpdateAsync(voter);
                    TempData["success"] = "Voted successfully";
                }
                catch (Exception e)
                {
                    TempData["error"] = "Not able to your vote";
                }                
            }
            return View("Index");
        }       
    }
}
