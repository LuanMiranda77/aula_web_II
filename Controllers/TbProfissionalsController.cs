using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_aula_1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.ConstrainedExecution;

namespace App_aula_1.Controllers
{
   public enum Plano {
        MedicoTotal = 1,
        MedicoParcial =2,
        Nutricionista =3

    }
    public class TbProfissionalsController : Controller
    {
        private readonly db_aula_webContext _context;

        public TbProfissionalsController(db_aula_webContext context)
        {
            _context = context;
        }

        // GET: TbProfissionals
        public async Task<IActionResult> Index()
        {
            //var db_aula_webContext = _context.TbProfissional.Include(t => t.IdCidadeNavigation).Include(t => t.IdContratoNavigation).ThenInclude(t=>t.IdPlanoNavigation).Include(t => t.IdTipoAcessoNavigation);
            var db_aula_webContext = (from pro in _context.TbProfissional
                                      where (Plano)pro.IdContratoNavigation.IdPlano == Plano.MedicoTotal
                                      select new TbProfissionalDTO { 
                                          IdProfissional =pro.IdProfissional,
                                          Nome = pro.Nome,
                                          NomeCidade = pro.IdCidadeNavigation.Nome,
                                          NomePlano = pro.IdContratoNavigation.IdPlanoNavigation.Nome,
                                          Cpf = pro.Cpf,
                                          Especialidade = pro.Especialidade,
                                          CrmCrn = pro.CrmCrn,
                                          Logradouro = pro.Logradouro,
                                          Numero = pro.Numero,
                                          Cep = pro.Cep,
                                          Ddd1 = pro.Ddd1,
                                          Ddd2 = pro.Ddd2,
                                          Telefone1 = pro.Telefone1,
                                          Telefone2 = pro.Telefone2,
                                          Salario = pro.Salario,
                                      });
            return View(db_aula_webContext);
        }

        // GET: TbProfissionals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbProfissional = await _context.TbProfissional
                .Include(t => t.IdCidadeNavigation)
                .Include(t => t.IdContratoNavigation)
                .Include(t => t.IdTipoAcessoNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdProfissional == id);
            if (tbProfissional == null)
            {
                return NotFound();
            }

            return View(tbProfissional);
        }

        // GET: TbProfissionals/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome");
            ViewData["IdPlano"] = new SelectList(_context.TbPlano, "IdPlano", "Nome");
            ViewData["IdTipoAcesso"] = new SelectList(_context.TbTipoAcesso, "IdTipoAcesso", "Nome");
            return View();
        }

        // POST: TbProfissionals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoProfissional,IdTipoAcesso,IdCidade,IdUser,Nome,Cpf,CrmCrn,Especialidade,Logradouro,Numero,Bairro,Cep,Ddd1,Ddd2,Telefone1,Telefone2,Salario")] TbProfissional tbProfissional, [Bind("IdPlano")] TbContrato IdContratoNavigation)
        {
            try
            {

                ModelState.Remove("IdUser");
                ModelState.Remove("IdContrato");
                if (ModelState.IsValid)
                {
                    IdContratoNavigation.DataInicio = DateTime.UtcNow;
                    IdContratoNavigation.DataFim = IdContratoNavigation.DataInicio.Value.AddMonths(2);
                    _context.Add(IdContratoNavigation);
                    await _context.SaveChangesAsync();

                    var userManager = HttpContext.RequestServices.GetService<UserManager<IdentityUser>>();
                    if (userManager != null)
                    {
                        var email = User.Identity?.Name;
                        if (email != null)
                        {
                            var user = await userManager.FindByEmailAsync(email);
                            if (user != null)
                            {
                                tbProfissional.IdUser = user.Id;
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                    tbProfissional.IdContrato = IdContratoNavigation.IdContrato;
                    _context.Add(tbProfissional);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", "Impossivel salvar" + e.ToString());
            }
            ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome", tbProfissional.IdCidade);
            ViewData["IdPlano"] = new SelectList(_context.TbPlano, "IdPlano", "Nome", tbProfissional.IdContratoNavigation.IdPlanoNavigation);
            ViewData["IdTipoAcesso"] = new SelectList(_context.TbTipoAcesso, "IdTipoAcesso", "Nome", tbProfissional.IdTipoAcesso);
            return View(tbProfissional);
        }

        // GET: TbProfissionals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Erro", "Home");
            }

            //var tbProfissional = await _context.TbProfissional.FindAsync(id);
            var tbProfissional = await _context.TbProfissional.Include(t=>t.IdContratoNavigation).FirstOrDefaultAsync(s=>s.IdProfissional==id);
            if (tbProfissional == null)
            {
                return RedirectToAction("Erro", "Home");
            }
            ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome", tbProfissional.IdCidade);
            ViewData["IdPlano"] = new SelectList(_context.TbPlano, "IdPlano", "Nome", tbProfissional.IdContratoNavigation.IdPlano);
            ViewData["IdTipoAcesso"] = new SelectList(_context.TbTipoAcesso, "IdTipoAcesso", "Nome", tbProfissional.IdTipoAcesso);
            return View(tbProfissional);
        }

        // POST: TbProfissionals/Edit/5
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
            var tbProfissional = await _context.TbProfissional.Include(t => t.IdContratoNavigation).FirstOrDefaultAsync(prof => prof.IdProfissional == id);

            if (tbProfissional == null)
            {
                return RedirectToAction("Erro", "Home");
            }
            if (await TryUpdateModelAsync<TbProfissional>(
            tbProfissional,
            "",
               s=> s.IdTipoProfissional, s => s.IdTipoAcesso, s => s.IdCidade, s => s.IdUser, s => s.Nome, s => s.Cpf,
               s => s.CrmCrn, s => s.Especialidade, s => s.Logradouro,
               s => s.Numero, s => s.Bairro, s => s.Cep, s => s.Ddd1,
               s => s.Ddd2, s => s.Telefone1,s => s.Telefone2, s => s.Salario))
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
            ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome", tbProfissional.IdCidade);
            ViewData["IdPlano"] = new SelectList(_context.TbPlano, "IdPlano", "Nome", tbProfissional.IdContratoNavigation.IdPlano);
            ViewData["IdTipoAcesso"] = new SelectList(_context.TbTipoAcesso, "IdTipoAcesso", "Nome", tbProfissional.IdTipoAcesso);
            return View(tbProfissional);
        }

        // GET: TbProfissionals/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangeError = false)
        {
            if (id == null)
            {
                return RedirectToAction("Erro", "Home");
            }

            var tbProfissional = await _context.TbProfissional
                .Include(t => t.IdCidadeNavigation)
                .ThenInclude(p => p.IdEstadoNavigation)
                .Include(t => t.IdTipoAcessoNavigation)
                .Include(t => t.IdContratoNavigation)
                .ThenInclude(p => p.IdPlanoNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdProfissional == id);
            if (tbProfissional == null)
            {
                return RedirectToAction("Erro", "Home");
            }
            if (saveChangeError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete erro";
            }

            return View(tbProfissional);
        }

        // POST: TbProfissionals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbProfissional = await _context.TbPaciente.FindAsync(id);
            if (tbProfissional == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.TbPaciente.Remove(tbProfissional);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangeError = true });
            }
        }
    }
}
