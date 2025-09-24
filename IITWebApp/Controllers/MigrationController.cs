using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IITWebApp.Data;
using IITWebApp.Models;
using IITWebApp.Services;

namespace IITWebApp.Controllers
{
    public class MigrationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMatriculeService _matriculeService;

        public MigrationController(ApplicationDbContext context, IMatriculeService matriculeService)
        {
            _context = context;
            _matriculeService = matriculeService;
        }

        // Action pour vérifier la connexion à la base de données
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                var count = await _context.Etudiants.CountAsync();
                ViewBag.Message = $"✅ Connexion réussie ! {count} étudiants trouvés dans la base.";
                ViewBag.MessageType = "success";

                // Lister les premiers étudiants pour debug
                var etudiants = await _context.Etudiants.Take(5).ToListAsync();
                ViewBag.Etudiants = etudiants;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"❌ Erreur de connexion : {ex.Message}";
                ViewBag.MessageType = "danger";
                ViewBag.Details = ex.InnerException?.Message;
                return View();
            }
        }

        // Action pour corriger les niveaux des étudiants existants
        public async Task<IActionResult> FixNiveaux()
        {
            try
            {
                var etudiants = await _context.Etudiants.Where(e => e.Niveau == null || e.Niveau == "").ToListAsync();
                int updated = 0;

                foreach (var etudiant in etudiants)
                {
                    // Extraire le niveau du matricule existant
                    if (etudiant.Matricule.Contains("L1"))
                        etudiant.Niveau = "L1";
                    else if (etudiant.Matricule.Contains("L2"))
                        etudiant.Niveau = "L2";
                    else if (etudiant.Matricule.Contains("L3"))
                        etudiant.Niveau = "L3";
                    else if (etudiant.Matricule.Contains("M1"))
                        etudiant.Niveau = "M1";
                    else if (etudiant.Matricule.Contains("M2"))
                        etudiant.Niveau = "M2";
                    else
                        etudiant.Niveau = "L1"; // Par défaut

                    updated++;
                }

                await _context.SaveChangesAsync();

                ViewBag.Message = $"✅ {updated} étudiants mis à jour avec leur niveau.";
                ViewBag.MessageType = "success";

                return View("TestConnection");
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"❌ Erreur lors de la mise à jour : {ex.Message}";
                ViewBag.MessageType = "danger";
                return View("TestConnection");
            }
        }

        // Action pour tester la création d'un étudiant
        public async Task<IActionResult> TestCreateEtudiant()
        {
            try
            {
                // Générer un matricule
                var matricule = await _matriculeService.GenerateMatriculeAsync("L1");

                // Créer un étudiant de test
                var etudiant = new Etudiant
                {
                    Matricule = matricule,
                    Nom = "Test",
                    Prenom = "Utilisateur",
                    DateNaissance = new DateTime(2000, 1, 1),
                    Sexe = "M",
                    Nationalite = "Ivoirien",
                    Niveau = "L1",
                    Statut = "actif",
                    DateCreation = DateTime.Now
                };

                _context.Etudiants.Add(etudiant);
                await _context.SaveChangesAsync();

                ViewBag.Message = $"✅ Étudiant de test créé avec le matricule : {matricule}";
                ViewBag.MessageType = "success";

                return View("TestConnection");
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"❌ Erreur lors de la création : {ex.Message}";
                ViewBag.MessageType = "danger";
                ViewBag.Details = ex.InnerException?.Message;
                return View("TestConnection");
            }
        }

        // Action pour voir toutes les tables
        public async Task<IActionResult> ViewAllTables()
        {
            try
            {
                var stats = new Dictionary<string, int>
                {
                    {"Étudiants", await _context.Etudiants.CountAsync()},
                    {"Classes", await _context.Classes.CountAsync()},
                    {"Filières", await _context.Filieres.CountAsync()},
                    {"Inscriptions", await _context.Inscriptions.CountAsync()},
                    {"Services", await _context.Services.CountAsync()},
                    {"Paiements", await _context.Paiements.CountAsync()},
                    {"Délégués", await _context.Delegues.CountAsync()},
                    {"Enseignants", await _context.Enseignants.CountAsync()},
                    {"Cours", await _context.Cours.CountAsync()}
                };

                ViewBag.Stats = stats;
                ViewBag.Message = "✅ Statistiques des tables chargées";
                ViewBag.MessageType = "info";

                return View("TestConnection");
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"❌ Erreur lors du chargement : {ex.Message}";
                ViewBag.MessageType = "danger";
                return View("TestConnection");
            }
        }
    }
}