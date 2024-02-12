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
using ElectoralSystem.Models.RequestDtos;
using ElectoralSystem.Repositories;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

namespace ElectoralSystem.Areas.ECs.Controllers
{
    [Area("ECs")]
    [Authorize(Roles = "EC_Admin")]
    public class CandidatesController : Controller
    {
        private readonly IAsyncRepository<Party> _partyRepository;
        private readonly IAsyncRepository<State> _stateRepository;
        private readonly IAsyncRepository<Candidate> _candidateRepository;
        private readonly IMapper _mapper;

        public CandidatesController(IAsyncRepository<Party> repository1, 
            IAsyncRepository<Candidate> repository2, IAsyncRepository<State> repository3, IMapper mapper)
        {
            _partyRepository = repository1;
            _candidateRepository = repository2;
            _stateRepository = repository3;
            _mapper = mapper;
        }

        public async Task<IActionResult> List()
        {
            List<Expression<Func<Candidate, object>>> includes = new List<Expression<Func<Candidate, object>>>
            {
                c=> c.State,
                b => b.Party,
                b => b.Party.PartySymbol
            };
            var candidates = await _candidateRepository.GetAllAsync(includes);
            return View(candidates);
        }

        [HttpGet]
        public async Task<IActionResult> AddNew()
        {
            NewCandidateDto newCandidatedto = new NewCandidateDto();
            newCandidatedto.StateList = await _stateRepository.GetAllAsync();
            List<Expression<Func<Party, object>>> includes = new List<Expression<Func<Party, object>>>
            {
                c=> c.PartySymbol
            };
            newCandidatedto.PartyList = await _partyRepository.GetAllAsync(includes);
            return View(newCandidatedto);
        }
        [HttpPost]
        [ActionName("AddNew")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNew2(NewCandidateDto candidateReq)
        {
            //if (ModelState.IsValid)
            {
                try
                {
                    State seat = await _stateRepository.GetByIdAsync(candidateReq.SelectedStateId);
                    Party party = await _partyRepository.GetByIdAsync(candidateReq.SelectedPartyId);
                    if (seat != null && party!=null)
                    {
                        Candidate candidate = _mapper.Map<Candidate>(candidateReq);
                        candidate.Party = party;
                        candidate.State = seat;
                        await _candidateRepository.AddAsync(candidate);
                    }
                    TempData["success"] = "Candidate registered successfully";

                }
                catch (Exception e)
                {
                    TempData["error"] = "Not able to regiter the candidate details";
                }

            }
            return RedirectToAction("List");

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var candidate = await _candidateRepository.GetByIdAsync(id.Value);
            if (candidate == null)
                return NotFound();            
            NewCandidateDto newCandidatedto = _mapper.Map<NewCandidateDto>(candidate);   
            //newCandidatedto.Id = candidate.Id;
            //newCandidatedto.Name = candidate.Name;
            newCandidatedto.StateList = await _stateRepository.GetAllAsync();
            newCandidatedto.PartyList = await _partyRepository.GetAllAsync();
            newCandidatedto.SelectedPartyId = candidate.PartyId;
            newCandidatedto.SelectedStateId = candidate.StateId;
            return View(newCandidatedto);
        }
        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit2(NewCandidateDto candidateReq)
        {
            //if (ModelState.IsValid)
            {
                try
                {
                    State seat = await _stateRepository.GetByIdAsync(candidateReq.SelectedStateId);
                    Party party = await _partyRepository.GetByIdAsync(candidateReq.SelectedPartyId);
                    if (seat != null && party != null)
                    {
                        Candidate candidate = await _candidateRepository.GetByIdAsync(candidateReq.Id);
                        candidate.State = seat;
                        candidate.Name = candidateReq.Name;
                        candidate.Party = party;
                        await _candidateRepository.UpdateAsync(candidate);
                    }
                    TempData["success"] = "Candidate Updated successfully";

                }
                catch (Exception e)
                {
                    TempData["error"] = "Not able to Update the candidate details";
                }

            }
            return RedirectToAction("List");

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            List<Expression<Func<Candidate, object>>> includes = new List<Expression<Func<Candidate, object>>>
            {
                c=> c.State,
                b => b.Party,
            };
            var candidate = await _candidateRepository.GetAsync(x=> x.Id == id.Value, null, includes,false);
            if (candidate == null || candidate.Count <=0)
                return NotFound();
            return View(candidate.First());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete2(int? id)
        {
            if (id == null)
                return NotFound();
            var candidate = await _candidateRepository.GetByIdAsync(id.Value);
            if (candidate == null)
                return NotFound();
            else
            {
                try
                {
                    await _candidateRepository.DeleteAsync(candidate);
                    TempData["success"] = "Candidate entry deleted successfully";
                }
                catch (Exception e)
                {
                    TempData["error"] = "Not able to delete the candidate entry in DB";
                }
                return RedirectToAction("List");
            }
        }
    }
}
