using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IITWebApp.Data;
using IITWebApp.Models;

namespace IITWebApp.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Test de connexion
                ViewBag.ConnectionTest = "Connexion réussie !";
                
                // Compter les enregistrements
                ViewBag.TotalEtudiants = await _context.Etudiants.CountAsync();
                ViewBag.TotalClasses = await _context.Classes.CountAsync();
                ViewBag.TotalFilieres = await _context.Filieres.CountAsync();
                ViewBag.TotalServices = await _context.Services.CountAsync();
                ViewBag.TotalEnseignants = await _context.Enseignants.CountAsync();
                ViewBag.TotalCours = await _context.Cours.CountAsync();
                
                // Tester les niveaux disponibles
                var niveauxDisponibles = await _context.Classes
                    .Select(c => c.Niveau)
                    .Distinct()
                    .OrderBy(n => n)
                    .ToListAsync();
                ViewBag.NiveauxDisponibles = string.Join(", ", niveauxDisponibles);
                
                // Derniers étudiants
                var derniersEtudiants = await _context.Etudiants
                    .OrderByDescending(e => e.DateCreation)
                    .Take(5)
                    .Select(e => new { e.Matricule, e.Nom, e.Prenom, e.Niveau })
                    .ToListAsync();
                ViewBag.DerniersEtudiants = derniersEtudiants;
                
            }
            catch (Exception ex)
            {
                ViewBag.ConnectionTest = $"Erreur de connexion : {ex.Message}";
                ViewBag.ErrorDetails = ex.ToString();
            }

            return View();
        }

        public async Task<IActionResult> TestCrud()
        {
            try
            {
                var result = new
                {
                    Phase1 = new
                    {
                        Etudiants = await _context.Etudiants.CountAsync(),
                        Classes = await _context.Classes.CountAsync(),
                        Filieres = await _context.Filieres.CountAsync(),
                        Inscriptions = await _context.Inscriptions.CountAsync(),
                        Services = await _context.Services.CountAsync(),
                        Paiements = await _context.Paiements.CountAsync(),
                        Delegues = await _context.Delegues.CountAsync()
                    },
                    Phase2 = new
                    {
                        Enseignants = await _context.Enseignants.CountAsync(),
                        Cours = await _context.Cours.CountAsync(),
                        Absences = await _context.Absences.CountAsync(),
                        Honoraires = await _context.HonoraireEnseignants.CountAsync()
                    }
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> TestNiveaux()
        {
            try
            {
                var niveaux = new[]
                {
                    new { Value = "L1", Text = "Licence 1 (L1)" },
                    new { Value = "L2", Text = "Licence 2 (L2)" },
                    new { Value = "L3", Text = "Licence 3 (L3)" },
                    new { Value = "M1", Text = "Master 1 (M1)" },
                    new { Value = "M2", Text = "Master 2 (M2)" }
                };

                ViewBag.NiveauxTest = niveaux;
                
                return Json(new { 
                    success = true, 
                    niveaux = niveaux,
                    message = "Niveaux chargés avec succès"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}
