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
using ElectoralSystem.Models.RequestDtos;
using ElectoralSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

namespace ElectoralSystem.Areas.ECs.Controllers
{
    [Area("ECs")]
    [Authorize(Roles = "EC_Admin")]
    public class PartiesController : Controller
    {
        private readonly IAsyncRepository<Party> _partyRepository;
        private readonly IAsyncRepository<PartySymbol> _partySymbolsRepository;
        private readonly IMapper _mapper;

        public PartiesController(IAsyncRepository<Party> repository1,
            IAsyncRepository<PartySymbol> repository2, IMapper mapper)
        {
            _partyRepository = repository1;
            _partySymbolsRepository = repository2;
            _mapper = mapper;
        }

        public async Task<IActionResult> List()
        {
            List<Expression<Func<Party, object>>> includes = new List<Expression<Func<Party, object>>>
            {
                c=> c.PartySymbol
            };
            var parties = await _partyRepository.GetAllAsync(includes);
            return View(parties);
        }

        [HttpGet]
        public async Task<IActionResult> AddNew()
        {
            NewPartyDto newPartydto = new NewPartyDto();
            newPartydto.PartySymbolsList = await _partySymbolsRepository.GetAllAsync();
            return View(newPartydto);
        }
        [HttpPost]
        [ActionName("AddNew")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNew2(NewPartyDto PartyReq)
        {
            //if (ModelState.IsValid)
            {
                try
                {
                    PartySymbol partySymbol = await _partySymbolsRepository.GetByIdAsync(PartyReq.SelectedPartySymbolId);
                    if (partySymbol != null)
                    {
                        Party Party = _mapper.Map<Party>(PartyReq);
                        Party.PartySymbol = partySymbol;
                        await _partyRepository.AddAsync(Party);
                    }
                    TempData["success"] = "Party registered successfully";

                }
                catch (Exception e)
                {
                    TempData["error"] = "Not able to regiter the Party details";
                }

            }
            return RedirectToAction("List");

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var party = await _partyRepository.GetByIdAsync(id.Value);
            if (party == null)
                return NotFound();
            NewPartyDto newPartydto = _mapper.Map<NewPartyDto>(party);    
            newPartydto.PartySymbolsList = await _partySymbolsRepository.GetAllAsync();
            newPartydto.SelectedPartySymbolId = party.PartySymbolId;
            return View(newPartydto);
        }
        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit2(NewPartyDto PartyReq)
        {
            //if (ModelState.IsValid)
            {
                try
                {
                    PartySymbol party = await _partySymbolsRepository.GetByIdAsync(PartyReq.SelectedPartySymbolId);
                    if (party != null)
                    {
                        Party Party = await _partyRepository.GetByIdAsync(PartyReq.Id);
                        Party.Name = PartyReq.Name;
                        Party.PartySymbol = party;
                        await _partyRepository.UpdateAsync(Party);
                    }
                    TempData["success"] = "Party Updated successfully";

                }
                catch (Exception e)
                {
                    TempData["error"] = "Not able to Update the Party details";
                }

            }
            return RedirectToAction("List");

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var party = await _partyRepository.GetAsync(x => x.Id == id.Value, null, nameof(Party.PartySymbol), false);
            if (party == null || party.Count <= 0)
                return NotFound();
            return View(party.First());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete2(int? id)
        {
            if (id == null)
                return NotFound();
            var party = await _partyRepository.GetAsync(x=> x.Id == id.Value, null,nameof(Party.PartySymbol),false);
            if (party == null || party.Count <=0)
                return NotFound();
            else
            {
                try
                {
                    await _partyRepository.DeleteAsync(party.First());
                    TempData["success"] = "Party entry deleted successfully";
                }
                catch (Exception e)
                {
                    TempData["error"] = "Not able to delete the Party entry in DB";
                }
                return RedirectToAction("List");
            }
        }
    }
}
