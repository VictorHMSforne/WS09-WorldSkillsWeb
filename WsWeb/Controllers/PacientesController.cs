using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WsWeb.Context;
using WsWeb.Models;

namespace WsWeb.Controllers
{
    public class PacientesController : Controller
    {
        private readonly AppDbContext _context;

        public PacientesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Pacientes
        public async Task<IActionResult> Index(string nomeMedico)
        {
            /*Método com o objetivo de Pesquisar o Paciente em específico e verifica se o que o Usuário inseriu não é nulo, caso seja,
            irá retornar toda a agenda*/
            var pesquisarMedico = _context.Pacientes.AsQueryable();
            if (nomeMedico != null)
            {
                pesquisarMedico = pesquisarMedico.Where(p => p.Medico.NomeMedico == nomeMedico);
            }
            ViewData["MedicoNome"] = new SelectList(_context.Medicos, "NomeMedico", "NomeMedico");
            return View(await pesquisarMedico.ToListAsync());


            //var appDbContext = _context.Pacientes.Include(p => p.Medico);
            //return View(await appDbContext.ToListAsync());
        }

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .Include(p => p.Medico)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // GET: Pacientes/Create
        public IActionResult Create()
        {
            ViewData["MedicoId"] = new SelectList(_context.Medicos, "Id", "NomeMedico");
            return View();
        }

        // POST: Pacientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomePaciente,MedicoId,Dia,Horario")] Paciente paciente)
        {
            //Faz a verificação no BD para ver se o médico está com a agenda ocupada
            var agendamentoMedico = await _context.Pacientes.FirstOrDefaultAsync(m => m.MedicoId == paciente.MedicoId &&
                                                                                m.Dia == paciente.Dia &&
                                                                                m.Horario == paciente.Horario);
            //Caso a agenda esteja ocupada, irá retornar uma mensagem
            if (agendamentoMedico != null)
            {
                ModelState.AddModelError("MedicoOcupado", "O Médico já está agendado para esse dia e horário");
                ViewData["MedicoId"] = new SelectList(_context.Medicos, "Id", "NomeMedico", paciente.MedicoId);
                return View(paciente);
            }
            //Caso a agenda esteja desocupada, irá adicionar ao BD
            if (ModelState.IsValid)
            {
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MedicoId"] = new SelectList(_context.Medicos, "Id", "NomeMedico", paciente.MedicoId);
            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            ViewData["MedicoId"] = new SelectList(_context.Medicos, "Id", "Especialidade", paciente.MedicoId);
            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomePaciente,MedicoId,Dia,Horario")] Paciente paciente)
        {
            if (id != paciente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MedicoId"] = new SelectList(_context.Medicos, "Id", "Especialidade", paciente.MedicoId);
            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .Include(p => p.Medico)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pacientes == null)
            {
                return Problem("Entity set 'AppDbContext.Pacientes'  is null.");
            }
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteExists(int id)
        {
          return _context.Pacientes.Any(e => e.Id == id);
        }
    }
}
