using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IITWebApp.Data;
using IITWebApp.Models;

namespace IITWebApp.Controllers
{
    public class DiagnosticController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiagnosticController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var diagnostics = new
                {
                    DatabaseConnection = "✅ Connecté",
                    TablesCount = new
                    {
                        Etudiants = await _context.Etudiants.CountAsync(),
                        Classes = await _context.Classes.CountAsync(),
                        Filieres = await _context.Filieres.CountAsync(),
                        Services = await _context.Services.CountAsync()
                    },
                    SampleData = new
                    {
                        FirstStudent = await _context.Etudiants.FirstOrDefaultAsync(),
                        FirstClass = await _context.Classes.FirstOrDefaultAsync(),
                        FirstFiliere = await _context.Filieres.FirstOrDefaultAsync()
                    }
                };

                ViewBag.Diagnostics = diagnostics;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> TestCreate()
        {
            try
            {
                // Test simple de création d'étudiant
                var newStudent = new Etudiant
                {
                    Matricule = "TEST2025001",
                    Nom = "TEST",
                    Prenom = "Utilisateur",
                    DateNaissance = new DateTime(2000, 1, 1),
                    LieuNaissance = "Test City",
                    Sexe = "M",
                    Nationalite = "Test",
                    Niveau = "L1",
                    Statut = "actif"
                };

                _context.Etudiants.Add(newStudent);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Étudiant créé avec succès", id = newStudent.IdEtudiant });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message, innerException = ex.InnerException?.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> TestRead(int id = 1)
        {
            try
            {
                var student = await _context.Etudiants
                    .Include(e => e.Classe)
                    .Include(e => e.Inscriptions)
                    .ThenInclude(i => i.Filiere)
                    .FirstOrDefaultAsync(e => e.IdEtudiant == id);

                if (student == null)
                {
                    return Json(new { success = false, message = "Étudiant non trouvé" });
                }

                return Json(new { 
                    success = true, 
                    student = new {
                        student.IdEtudiant,
                        student.Matricule,
                        student.Nom,
                        student.Prenom,
                        student.Niveau,
                        Classe = student.Classe?.NomClasse,
                        InscriptionsCount = student.Inscriptions.Count
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> TestUpdate([FromBody] dynamic data)
        {
            try
            {
                int id = data.id;
                string newNom = data.nom;

                var student = await _context.Etudiants.FindAsync(id);
                if (student == null)
                {
                    return Json(new { success = false, message = "Étudiant non trouvé" });
                }

                student.Nom = newNom;
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Étudiant mis à jour" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> TestDelete(int id)
        {
            try
            {
                var student = await _context.Etudiants.FindAsync(id);
                if (student == null)
                {
                    return Json(new { success = false, message = "Étudiant non trouvé" });
                }

                _context.Etudiants.Remove(student);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Étudiant supprimé" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}
