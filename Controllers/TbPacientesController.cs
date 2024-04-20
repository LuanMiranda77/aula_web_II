using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_aula_1.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace App_aula_1.Controllers
{
    [Authorize(Roles = "GerenteGeral, Medico, Nutricionista")]
    public class TbPacientesController : Controller
    {
        private readonly db_aula_webContext _context;

        public TbPacientesController(db_aula_webContext context)
        {
            _context = context;
        }

        // GET: TbPacientes
        public async Task<IActionResult> Index()
        {
            // Carregar id de usuario logado
            string IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Buscar o profissional associado ao usuário logado
            var profissionalLogado = await _context.TbProfissional.FirstOrDefaultAsync(p => p.IdUser == IdUser);

            if (profissionalLogado == null)
            {
                return RedirectToAction("Erro", "Home");
            }

            //Retorna a lista de paciente por profissional logado
            var pacientesCadastradosPorProfissional = await _context.TbPaciente
               .Where(p => _context.TbMedicoPaciente.Any(mp => mp.IdProfissional == profissionalLogado.IdProfissional && mp.IdPaciente == p.IdPaciente))
               .Include(t => t.IdCidadeNavigation)
               .ToListAsync();

            return View(pacientesCadastradosPorProfissional);
        }

        // GET: TbPacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Erro", "Home");
            }

            var tbPaciente = await _context.TbPaciente
                .Include(t => t.IdCidadeNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdPaciente == id);
            if (tbPaciente == null)
            {
                return RedirectToAction("Erro", "Home");
            }

            return View(tbPaciente);
        }

        // GET: TbPacientes/Create
        public IActionResult Create()
        {
            ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome");
            return View();
        }

        // POST: TbPacientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Rg,Cpf,DataNascimento,NomeResponsavel,Sexo,Etnia,Endereco,Bairro,IdCidade,TelResidencial,TelComercial,TelCelular,Profissao,FlgAtleta,FlgGestante")] TbPaciente tbPaciente, TbMedicoPaciente tbMedicoPaciente)
        {
            try { 
                if (ModelState.IsValid)
                {
                    _context.Add(tbPaciente);
                    await _context.SaveChangesAsync();

                    // Carregar id de usuario logado
                    string IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    // Buscar o profissional associado ao usuário logado
                    var profissionalLogado = await _context.TbProfissional.FirstOrDefaultAsync(p => p.IdUser == IdUser);

                    if (profissionalLogado == null)
                    {
                        return RedirectToAction("Erro", "Home");
                    }

                    tbMedicoPaciente.IdProfissional = profissionalLogado.IdProfissional;
                    tbMedicoPaciente.IdPaciente = tbPaciente.IdPaciente;
                    _context.Add(tbMedicoPaciente);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException e) {
                ModelState.AddModelError("", "Impossivel salvar"+e.ToString());
            }
            ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "IdCidade", tbPaciente.IdCidade);
            return View(tbPaciente);
        }

        // GET: TbPacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Erro", "Home");
            }

            var tbPaciente = await _context.TbPaciente.FindAsync(id);
            if (tbPaciente == null)
            {
                return RedirectToAction("Erro", "Home");
            }
            ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome", tbPaciente.IdCidade);
            return View(tbPaciente);
        }

        // POST: TbPacientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Erro", "Home");
            }
            var tbPaciente = await _context.TbPaciente.FirstOrDefaultAsync(paciente =>paciente.IdPaciente==id);

            if (tbPaciente == null)
            {
                return RedirectToAction("Erro", "Home");
            }

            if (await TryUpdateModelAsync<TbPaciente>(
                tbPaciente, 
                "", 
                c=> c.Nome, c => c.Rg, c => c.Cpf, c => c.DataNascimento, c => c.NomeResponsavel, c => c.Sexo, c => c.Etnia, 
                c => c.Endereco, c => c.Bairro, c => c.IdCidade, c => c.TelResidencial, c => c.TelComercial, c => c.TelCelular,
                c => c.Profissao, c => c.FlgAtleta, c => c.FlgGestante))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError("", "Erro ao editar paciente" + ex.ToString());
                }
            }
            ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome", tbPaciente.IdCidade);
            return View(tbPaciente);
        }

        // GET: TbPacientes/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangeError=false)
        {
            if (id == null)
            {
                return RedirectToAction("Erro", "Home");
            }

            var tbPaciente = await _context.TbPaciente
                .Include(t => t.IdCidadeNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdPaciente == id);
            if (tbPaciente == null)
            {
                return RedirectToAction("Erro", "Home");
            }
            if (saveChangeError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete erro";
            }

            return View(tbPaciente);
        }

        // POST: TbPacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbPaciente = await _context.TbPaciente.FindAsync(id);
            if (tbPaciente == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.TbPaciente.Remove(tbPaciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangeError = true });
            }
        }

        private bool TbPacienteExists(int id)
        {
            return _context.TbPaciente.Any(e => e.IdPaciente == id);
        }
    }
}
