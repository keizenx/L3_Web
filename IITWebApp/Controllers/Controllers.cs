using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IITWebApp.Data;
using IITWebApp.Models;
using IITWebApp.Services;
using System.Linq;

namespace IITWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalEtudiants = await _context.Etudiants.CountAsync();
            ViewBag.TotalInscriptions = await _context.Inscriptions.CountAsync();
            ViewBag.TotalPaiements = await _context.Paiements.CountAsync();
            ViewBag.TotalServices = await _context.Services.CountAsync();

            return View();
        }

        // GET: Home/Phase1Dashboard
        public async Task<IActionResult> Phase1Dashboard()
        {
            // Statistiques pour le tableau de bord Phase 1
            ViewBag.TotalEtudiants = await _context.Etudiants.CountAsync();
            ViewBag.TotalInscriptions = await _context.Inscriptions.CountAsync();
            ViewBag.TotalPaiements = await _context.Paiements.CountAsync();
            ViewBag.TotalServices = await _context.Services.CountAsync();
            ViewBag.TotalArrieres = await _context.Paiements.CountAsync(p => p.Statut != "paye");

            // Calculer le total de scolarité due (en utilisant les données existantes)
            var totalScolariteDue = await _context.Inscriptions
                .Where(i => i.AnneeAcademique == "2024-2025")
                .SumAsync(i => i.MontantInscription);
            ViewBag.TotalScolariteDue = totalScolariteDue;

            // Récupérer les données pour les graphiques
            var inscriptionsParMois = await _context.Inscriptions
                .Where(i => i.DateInscription.Year == DateTime.Now.Year)
                .GroupBy(i => i.DateInscription.Month)
                .Select(g => new { Mois = g.Key, Nombre = g.Count() })
                .ToListAsync();

            var paiementsParMois = await _context.Paiements
                .Where(p => p.DatePaiement.Year == DateTime.Now.Year)
                .GroupBy(p => p.DatePaiement.Month)
                .Select(g => new { Mois = g.Key, Montant = g.Sum(p => p.Montant) })
                .ToListAsync();

            ViewBag.InscriptionsParMois = inscriptionsParMois;
            ViewBag.PaiementsParMois = paiementsParMois;

            // Récupérer les étudiants récents
            var etudiantsRecents = await _context.Etudiants
                .OrderByDescending(e => e.DateCreation)
                .Take(3)
                .Select(e => new {
                    Matricule = e.Matricule,
                    NomComplet = e.Nom + " " + e.Prenom,
                    ClasseActuelle = e.Niveau
                })
                .ToListAsync();
            ViewBag.EtudiantsRecents = etudiantsRecents;

            // Récupérer les paiements récents
            var paiementsRecents = await _context.Paiements
                .Include(p => p.Etudiant)
                .OrderByDescending(p => p.DatePaiement)
                .Take(3)
                .Select(p => new {
                    EtudiantNom = p.Etudiant.Nom + " " + p.Etudiant.Prenom,
                    Montant = p.Montant,
                    DatePaiement = p.DatePaiement,
                    Statut = p.Statut == "paye" ? "Validé" : "En attente"
                })
                .ToListAsync();
            ViewBag.PaiementsRecents = paiementsRecents;

            return View("~/Views/Home/Phase1Dashboard.cshtml");
        }
    }

    public class EtudiantsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMatriculeService _matriculeService;

        public EtudiantsController(ApplicationDbContext context, IMatriculeService matriculeService)
        {
            _context = context;
            _matriculeService = matriculeService;
        }

        // GET: Etudiants
        public async Task<IActionResult> Index()
        {
            return View(await _context.Etudiants
                .Include(e => e.Inscriptions)
                .ThenInclude(i => i.Filiere)
                .OrderBy(e => e.Nom)
                .ThenBy(e => e.Prenom)
                .ToListAsync());
        }

        // GET: Etudiants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var etudiant = await _context.Etudiants
                .Include(e => e.Inscriptions)
                .ThenInclude(i => i.Filiere)
                .FirstOrDefaultAsync(m => m.IdEtudiant == id);

            if (etudiant == null)
            {
                return NotFound();
            }

            return View(etudiant);
        }

        // Méthode helper pour créer la liste des niveaux
        private SelectList GetNiveauxSelectList()
        {
            try
            {
                Console.WriteLine("[DEBUG] Entering GetNiveauxSelectList");
                var niveaux = new[]
                {
                    new { Value = "L1", Text = "Licence 1 (L1)" },
                    new { Value = "L2", Text = "Licence 2 (L2)" },
                    new { Value = "L3", Text = "Licence 3 (L3)" },
                    new { Value = "M1", Text = "Master 1 (M1)" },
                    new { Value = "M2", Text = "Master 2 (M2)" }
                };
                Console.WriteLine($"[DEBUG] Created niveaux array with {niveaux.Length} items");
                var selectList = new SelectList(niveaux, "Value", "Text");
                Console.WriteLine("[DEBUG] Created SelectList successfully");
                return selectList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception in GetNiveauxSelectList: {ex.Message}");
                Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        // GET: Etudiants/Create
        public IActionResult Create()
        {
            try
            {
                Console.WriteLine("[DEBUG] Entering EtudiantsController.Create GET method");
                
                var viewModel = new CreateEtudiantViewModel();
                Console.WriteLine("[DEBUG] Created CreateEtudiantViewModel");
                
                ViewBag.Niveaux = GetNiveauxSelectList();
                Console.WriteLine("[DEBUG] Set ViewBag.Niveaux");
                
                Console.WriteLine("[DEBUG] Returning view");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log l'erreur pour le debugging
                Console.WriteLine($"[ERROR] Exception in Create GET: {ex.Message}");
                Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
                
                // Retourner une vue d'erreur ou rediriger
                TempData["Error"] = $"Erreur lors du chargement de la page : {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: Etudiants/Create
        [HttpPost]
        // [ValidateAntiForgeryToken] // Temporairement retiré pour debugging
        public async Task<IActionResult> Create(CreateEtudiantViewModel viewModel)
        {
            Console.WriteLine("[DEBUG] Entering EtudiantsController.Create POST method");
            
            // Log raw form data
            Console.WriteLine("[DEBUG] Raw form data:");
            foreach (var key in Request.Form.Keys)
            {
                Console.WriteLine($"[DEBUG] Form[{key}] = '{Request.Form[key]}'");
            }
            
            Console.WriteLine($"[DEBUG] Model received: Nom={viewModel?.Nom}, Prenom={viewModel?.Prenom}, Niveau={viewModel?.Niveau}");
            Console.WriteLine($"[DEBUG] ModelState.IsValid: {ModelState.IsValid}");
            
            // Log all received data
            if (viewModel != null)
            {
                Console.WriteLine($"[DEBUG] Full model data:");
                Console.WriteLine($"  - Niveau: '{viewModel.Niveau}'");
                Console.WriteLine($"  - Nom: '{viewModel.Nom}'");
                Console.WriteLine($"  - Prenom: '{viewModel.Prenom}'");
                Console.WriteLine($"  - Sexe: '{viewModel.Sexe}'");
                Console.WriteLine($"  - DateNaissance: {viewModel.DateNaissance}");
                Console.WriteLine($"  - Email: '{viewModel.Email}'");
                Console.WriteLine($"  - Telephone: '{viewModel.Telephone}'");
            }
            
            // Check ModelState details
            if (!ModelState.IsValid)
            {
                Console.WriteLine("[DEBUG] ModelState errors:");
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state.Errors.Count > 0)
                    {
                        Console.WriteLine($"[DEBUG] - {key}: {string.Join(", ", state.Errors.Select(e => e.ErrorMessage))}");
                    }
                }
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("[DEBUG] ModelState is valid - creating etudiant");
                    // Générer automatiquement le matricule
                    var matricule = await _matriculeService.GenerateMatriculeAsync(viewModel.Niveau);
                    Console.WriteLine($"[DEBUG] Generated matricule: {matricule}");

                    // Créer l'entité Etudiant à partir du ViewModel
                    var etudiant = new Etudiant
                    {
                        Matricule = matricule,
                        Nom = viewModel.Nom,
                        Prenom = viewModel.Prenom,
                        DateNaissance = viewModel.DateNaissance,
                        LieuNaissance = viewModel.LieuNaissance,
                        Sexe = viewModel.Sexe,
                        Adresse = viewModel.Adresse,
                        Telephone = viewModel.Telephone,
                        Email = viewModel.Email,
                        Nationalite = viewModel.Nationalite,
                        Niveau = viewModel.Niveau, // AJOUT DE LA LIGNE MANQUANTE
                        Statut = "actif",
                        DateCreation = DateTime.Now
                    };

                    // Vérifier si l'email existe déjà (si fourni)
                    if (!string.IsNullOrEmpty(etudiant.Email))
                    {
                        var existingEmail = await _context.Etudiants.FirstOrDefaultAsync(e => e.Email == etudiant.Email);
                        if (existingEmail != null)
                        {
                            Console.WriteLine("[DEBUG] Email already exists");
                            ModelState.AddModelError("Email", "Cet email existe déjà.");
                            ViewBag.Niveaux = GetNiveauxSelectList();
                            return View(viewModel);
                        }
                    }
                    
                    _context.Add(etudiant);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("[DEBUG] Etudiant saved successfully");
                    TempData["Message"] = $"Étudiant créé avec succès. Matricule généré : {matricule}";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DEBUG] Exception in Create POST: {ex.Message}");
                    Console.WriteLine($"[DEBUG] Exception stack trace: {ex.StackTrace}");
                    ModelState.AddModelError("", $"Erreur lors de la création : {ex.Message}");
                    ViewBag.Niveaux = GetNiveauxSelectList();
                    return View(viewModel);
                }
            }
            
            Console.WriteLine("[DEBUG] ModelState is invalid - returning view with errors");
            // Afficher les erreurs de validation
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"[DEBUG] Validation error: {error.ErrorMessage}");
            }
            
            ViewBag.Niveaux = GetNiveauxSelectList();
            
            return View(viewModel);
        }

        // GET: Etudiants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var etudiant = await _context.Etudiants.FindAsync(id);
            if (etudiant == null)
            {
                return NotFound();
            }
            
            return View(etudiant);
        }

        // POST: Etudiants/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEtudiant,Matricule,Nom,Prenom,DateNaissance,LieuNaissance,Sexe,Adresse,Telephone,Email,Nationalite")] Etudiant etudiant)
        {
            if (id != etudiant.IdEtudiant)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Récupérer l'étudiant existant pour préserver les champs non modifiables
                    var existingEtudiant = await _context.Etudiants.FindAsync(id);
                    if (existingEtudiant == null)
                    {
                        return NotFound();
                    }

                    // Mettre à jour seulement les champs modifiables
                    existingEtudiant.Nom = etudiant.Nom;
                    existingEtudiant.Prenom = etudiant.Prenom;
                    existingEtudiant.DateNaissance = etudiant.DateNaissance;
                    existingEtudiant.LieuNaissance = etudiant.LieuNaissance;
                    existingEtudiant.Sexe = etudiant.Sexe;
                    existingEtudiant.Adresse = etudiant.Adresse;
                    existingEtudiant.Telephone = etudiant.Telephone;
                    existingEtudiant.Email = etudiant.Email;
                    existingEtudiant.Nationalite = etudiant.Nationalite;

                    _context.Update(existingEtudiant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EtudiantExists(etudiant.IdEtudiant))
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
            return View(etudiant);
        }

        // GET: Etudiants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var etudiant = await _context.Etudiants
                .Include(e => e.Inscriptions)
                .FirstOrDefaultAsync(m => m.IdEtudiant == id);
            if (etudiant == null)
            {
                return NotFound();
            }

            return View(etudiant);
        }

        // POST: Etudiants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var etudiant = await _context.Etudiants.FindAsync(id);
            if (etudiant != null)
            {
                _context.Etudiants.Remove(etudiant);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EtudiantExists(int id)
        {
            return _context.Etudiants.Any(e => e.IdEtudiant == id);
        }
    }

    public class InscriptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InscriptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Inscriptions
        public async Task<IActionResult> Index()
        {
            var inscriptions = await _context.Inscriptions
                .Include(i => i.Etudiant)
                .Include(i => i.Filiere)
                .OrderByDescending(i => i.DateInscription)
                .ToListAsync();
            return View(inscriptions);
        }

        // GET: Inscriptions/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdEtudiant"] = new SelectList(await _context.Etudiants.OrderBy(e => e.Nom).ToListAsync(), "IdEtudiant", "NomComplet");
            ViewData["IdClasse"] = new SelectList(await _context.Classes.OrderBy(c => c.NomClasse).ToListAsync(), "IdClasse", "NomClasse");
            ViewData["IdFiliere"] = new SelectList(await _context.Filieres.OrderBy(f => f.NomFiliere).ToListAsync(), "IdFiliere", "NomFiliere");
            return View();
        }

        // POST: Inscriptions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEtudiant,IdClasse,IdFiliere,AnneeAcademique,DateInscription,TypeInscription,MontantInscription")] Inscription inscription)
        {
            if (ModelState.IsValid)
            {
                // Vérifier si l'étudiant n'est pas déjà inscrit pour cette année académique
                var existingInscription = await _context.Inscriptions
                    .FirstOrDefaultAsync(i => i.IdEtudiant == inscription.IdEtudiant && i.AnneeAcademique == inscription.AnneeAcademique);
                
                if (existingInscription != null)
                {
                    ModelState.AddModelError("IdEtudiant", "Cet étudiant est déjà inscrit pour cette année académique.");
                    ViewData["IdEtudiant"] = new SelectList(await _context.Etudiants.OrderBy(e => e.Nom).ToListAsync(), "IdEtudiant", "NomComplet");
                    ViewData["IdClasse"] = new SelectList(await _context.Classes.OrderBy(c => c.NomClasse).ToListAsync(), "IdClasse", "NomClasse");
                    ViewData["IdFiliere"] = new SelectList(await _context.Filieres.OrderBy(f => f.NomFiliere).ToListAsync(), "IdFiliere", "NomFiliere");
                    return View(inscription);
                }

                inscription.Statut = "en attente";
                _context.Add(inscription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["IdEtudiant"] = new SelectList(await _context.Etudiants.OrderBy(e => e.Nom).ToListAsync(), "IdEtudiant", "NomComplet");
            ViewData["IdClasse"] = new SelectList(await _context.Classes.OrderBy(c => c.NomClasse).ToListAsync(), "IdClasse", "NomClasse");
            ViewData["IdFiliere"] = new SelectList(await _context.Filieres.OrderBy(f => f.NomFiliere).ToListAsync(), "IdFiliere", "NomFiliere");
            return View(inscription);
        }

        // GET: Inscriptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscription = await _context.Inscriptions.FindAsync(id);
            if (inscription == null)
            {
                return NotFound();
            }

            ViewData["IdEtudiant"] = new SelectList(await _context.Etudiants.OrderBy(e => e.Nom).ToListAsync(), "IdEtudiant", "NomComplet");
            ViewData["IdClasse"] = new SelectList(await _context.Classes.OrderBy(c => c.NomClasse).ToListAsync(), "IdClasse", "NomClasse");
            ViewData["IdFiliere"] = new SelectList(await _context.Filieres.OrderBy(f => f.NomFiliere).ToListAsync(), "IdFiliere", "NomFiliere");
            return View(inscription);
        }

        // POST: Inscriptions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdInscription,IdEtudiant,IdClasse,IdFiliere,AnneeAcademique,DateInscription,TypeInscription,MontantInscription,Statut")] Inscription inscription)
        {
            if (id != inscription.IdInscription)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inscription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InscriptionExists(inscription.IdInscription))
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

            ViewData["IdEtudiant"] = new SelectList(await _context.Etudiants.OrderBy(e => e.Nom).ToListAsync(), "IdEtudiant", "NomComplet");
            ViewData["IdClasse"] = new SelectList(await _context.Classes.OrderBy(c => c.NomClasse).ToListAsync(), "IdClasse", "NomClasse");
            ViewData["IdFiliere"] = new SelectList(await _context.Filieres.OrderBy(f => f.NomFiliere).ToListAsync(), "IdFiliere", "NomFiliere");
            return View(inscription);
        }

        // GET: Inscriptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscription = await _context.Inscriptions
                .Include(i => i.Etudiant)
                .Include(i => i.Filiere)
                .FirstOrDefaultAsync(m => m.IdInscription == id);
            if (inscription == null)
            {
                return NotFound();
            }

            return View(inscription);
        }

        // POST: Inscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inscription = await _context.Inscriptions.FindAsync(id);
            if (inscription != null)
            {
                _context.Inscriptions.Remove(inscription);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InscriptionExists(int id)
        {
            return _context.Inscriptions.Any(e => e.IdInscription == id);
        }

        // GET: Inscriptions/Reinscription
        public async Task<IActionResult> Reinscription()
        {
            // Récupérer les étudiants avec leurs informations pour la réinscription
            var etudiants = await _context.Etudiants
                .Select(e => new {
                    e.IdEtudiant,
                    e.Matricule,
                    NomComplet = e.Nom + " " + e.Prenom,
                    e.Niveau,
                    e.Email
                })
                .OrderBy(e => e.Matricule)
                .ToListAsync();

            ViewBag.Etudiants = etudiants;
            return View("~/Views/Inscriptions/Reinscription.cshtml");
        }

        // POST: Inscriptions/Reinscription
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reinscription(ReinscriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Vérifier que l'étudiant existe
                    var etudiant = await _context.Etudiants.FirstOrDefaultAsync(e => e.Matricule == model.EtuMatricule);
                    if (etudiant == null)
                    {
                        ModelState.AddModelError("EtuMatricule", "Étudiant non trouvé");
                        return View(model);
                    }

                    // Vérifier s'il n'est pas déjà inscrit pour cette année
                    var existingInscription = await _context.Inscriptions
                        .FirstOrDefaultAsync(i => i.IdEtudiant == etudiant.IdEtudiant && i.AnneeAcademique == model.AnAcadem);

                    if (existingInscription != null)
                    {
                        ModelState.AddModelError("AnAcadem", "Cet étudiant est déjà inscrit pour cette année académique");
                        return View(model);
                    }

                    // Créer la nouvelle inscription
                    var inscription = new Inscription
                    {
                        IdEtudiant = etudiant.IdEtudiant,
                        IdFiliere = 1, // COMPUTER par défaut
                        IdClasse = 2, // L2GL par défaut
                        AnneeAcademique = model.AnAcadem,
                        DateInscription = model.DateInscrip,
                        MontantInscription = model.TotalScolarite,
                        TypeInscription = "réinscription",
                        Statut = "en_attente"
                    };

                    _context.Inscriptions.Add(inscription);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = $"Réinscription de {etudiant.Nom} {etudiant.Prenom} effectuée avec succès !";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erreur lors de la réinscription : {ex.Message}");
                }
            }

            // Recharger les données en cas d'erreur
            var etudiants = await _context.Etudiants
                .Select(e => new {
                    e.IdEtudiant,
                    e.Matricule,
                    NomComplet = e.Nom + " " + e.Prenom,
                    e.Niveau,
                    e.Email
                })
                .OrderBy(e => e.Matricule)
                .ToListAsync();

            ViewBag.Etudiants = etudiants;
            return View(model);
        }

        // GET: Inscriptions/Promotion
        public async Task<IActionResult> Promotion()
        {
            // Récupérer les statistiques de promotion (en utilisant les classes existantes)
            var statsPromotion = _context.Inscriptions
                .Include(i => i.Classe)
                .Where(i => i.AnneeAcademique == "2024-2025")
                .AsEnumerable()
                .GroupBy(i => i.Classe != null ? i.Classe.NomClasse : "Non assignée")
                .Select(g => new {
                    Classe = g.Key,
                    Nombre = g.Count()
                })
                .ToList();

            ViewBag.StatsPromotion = statsPromotion;

            // Récupérer les étudiants éligibles à la promotion
            var etudiantsEligibles = await _context.Etudiants
                .Where(e => e.Niveau.StartsWith("L1") || e.Niveau.StartsWith("L2"))
                .Select(e => new {
                    e.IdEtudiant,
                    e.Matricule,
                    NomComplet = e.Nom + " " + e.Prenom,
                    e.Niveau,
                    e.Email,
                    // Calculer la nouvelle classe
                    NouvelleClasse = e.Niveau.Replace("L1", "L2").Replace("L2", "L3")
                })
                .OrderBy(e => e.Matricule)
                .ToListAsync();

            ViewBag.EtudiantsEligibles = etudiantsEligibles;
            return View("~/Views/Inscriptions/Promotion.cshtml");
        }
    }

    public class PaiementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaiementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Paiements
        public async Task<IActionResult> Index()
        {
            var paiements = await _context.Paiements
                .Include(p => p.Etudiant)
                .OrderByDescending(p => p.DatePaiement)
                .ToListAsync();
            return View(paiements);
        }

        // GET: Paiements/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdEtudiant"] = new SelectList(await _context.Etudiants.OrderBy(e => e.Nom).ToListAsync(), "IdEtudiant", "NomComplet");
            return View();
        }

        // POST: Paiements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEtudiant,TypePaiement,Montant,MethodePaiement,ReferencePaiement")] Paiement paiement)
        {
            if (ModelState.IsValid)
            {
                paiement.DatePaiement = DateTime.Now;
                paiement.Statut = "valide";

                _context.Add(paiement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdEtudiant"] = new SelectList(await _context.Etudiants.OrderBy(e => e.Nom).ToListAsync(), "IdEtudiant", "NomComplet");
            return View(paiement);
        }

        // GET: Paiements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paiement = await _context.Paiements
                .Include(p => p.Etudiant)
                .FirstOrDefaultAsync(m => m.IdPaiement == id);
            if (paiement == null)
            {
                return NotFound();
            }

            return View(paiement);
        }

        // GET: Paiements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paiement = await _context.Paiements.FindAsync(id);
            if (paiement == null)
            {
                return NotFound();
            }

            ViewData["IdEtudiant"] = new SelectList(await _context.Etudiants.OrderBy(e => e.Nom).ToListAsync(), "IdEtudiant", "NomComplet", paiement.IdEtudiant);
            return View(paiement);
        }

        // POST: Paiements/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPaiement,IdEtudiant,TypePaiement,Montant,DatePaiement,MethodePaiement,ReferencePaiement,Statut")] Paiement paiement)
        {
            if (id != paiement.IdPaiement)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paiement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaiementExists(paiement.IdPaiement))
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

            ViewData["IdEtudiant"] = new SelectList(await _context.Etudiants.OrderBy(e => e.Nom).ToListAsync(), "IdEtudiant", "NomComplet", paiement.IdEtudiant);
            return View(paiement);
        }

        // GET: Paiements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paiement = await _context.Paiements
                .Include(p => p.Etudiant)
                .FirstOrDefaultAsync(m => m.IdPaiement == id);
            if (paiement == null)
            {
                return NotFound();
            }

            return View(paiement);
        }

        // POST: Paiements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paiement = await _context.Paiements.FindAsync(id);
            if (paiement != null)
            {
                _context.Paiements.Remove(paiement);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PaiementExists(int id)
        {
            return _context.Paiements.Any(e => e.IdPaiement == id);
        }

        // GET: Paiements/Arrieres
        public async Task<IActionResult> Arrieres()
        {
            // Récupérer les paiements en retard avec les informations des étudiants
            var paiementsEnRetard = await _context.Paiements
                .Include(p => p.Etudiant)
                .Where(p => p.Statut != "paye" && p.DatePaiement < DateTime.Now.AddDays(-30))
                .ToListAsync();

            // Transformer les données côté client pour éviter les expressions LINQ non traduisibles
            var arrieres = paiementsEnRetard
                .Select(p => {
                    var dureeRetard = (DateTime.Now - p.DatePaiement).Days;
                    var statut = p.Statut == "paye" ? "Payé" : dureeRetard > 90 ? "Critique" : "Modéré";

                    return new {
                        p.IdPaiement,
                        p.Montant,
                        p.DatePaiement,
                        EtudiantMatricule = p.Etudiant?.Matricule ?? "",
                        EtudiantNom = p.Etudiant?.Nom ?? "",
                        EtudiantPrenom = p.Etudiant?.Prenom ?? "",
                        EtudiantEmail = p.Etudiant?.Email ?? "",
                        DureeRetard = dureeRetard,
                        Statut = statut
                    };
                })
                .OrderByDescending(p => p.DureeRetard)
                .ToList();

            // Statistiques
            ViewBag.TotalArrieres = arrieres.Count;
            ViewBag.MontantTotalDu = arrieres.Sum(a => a.Montant);
            ViewBag.ArrieresCritiques = arrieres.Count(a => a.Statut == "Critique");
            ViewBag.ArrieresModeres = arrieres.Count(a => a.Statut == "Modéré");

            return View("~/Views/Paiements/Arrieres.cshtml", arrieres);
        }
    }

    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            // Récupérer d'abord tous les services
            var servicesList = await _context.Services.ToListAsync();

            // Calculer les statistiques pour chaque service en utilisant des requêtes séparées
            var services = servicesList
                .Select(s => new {
                    s.IdService,
                    s.NomService,
                    s.Description,
                    s.Prix,
                    s.TypeService,
                    s.Statut,
                    SouscriptionsActives = _context.SouscriptionServices
                        .Where(ss => ss.IdService == s.IdService && ss.Statut == "actif")
                        .Count(),
                    RevenusMensuels = _context.SouscriptionServices
                        .Where(ss => ss.IdService == s.IdService && ss.Statut == "actif")
                        .Sum(ss => (decimal?)s.Prix) ?? 0
                })
                .OrderBy(s => s.TypeService)
                .ThenBy(s => s.NomService)
                .ToList();

            // Statistiques basées sur les données récupérées
            ViewBag.TotalServices = services.Count;
            ViewBag.ServicesActifs = services.Count(s => s.Statut == "actif");
            ViewBag.TotalSouscriptions = services.Sum(s => s.SouscriptionsActives);
            ViewBag.RevenusMensuels = services.Sum(s => s.RevenusMensuels);

            // Statistiques alternatives basées sur les données récupérées
            ViewBag.TotalServicesAlt = services.Count;
            ViewBag.ServicesActifsAlt = services.Count(s => s.Statut == "actif");
            ViewBag.TotalSouscriptionsAlt = services.Sum(s => s.SouscriptionsActives);
            ViewBag.RevenusMensuelsAlt = services.Sum(s => s.RevenusMensuels);

            // Convertir en liste de services pour la vue
            var serviceList = servicesList.Select(s => new Service {
                IdService = s.IdService,
                NomService = s.NomService,
                Description = s.Description,
                Prix = s.Prix,
                TypeService = s.TypeService,
                Statut = s.Statut,
                DateCreation = s.DateCreation
            }).ToList();

            return View("~/Views/Services/Index.cshtml", serviceList);
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.SouscriptionServices)
                .ThenInclude(ss => ss.Etudiant)
                .FirstOrDefaultAsync(s => s.IdService == id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomService,Description,Prix,TypeService")] Service service)
        {
            if (ModelState.IsValid)
            {
                service.DateCreation = DateTime.Now;
                service.Statut = "actif";

                _context.Services.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }
    }
}

// ===== PHASE 2 - NOUVEAUX CONTRÔLEURS =====

public class EnseignantsController : Controller
{
    private readonly ApplicationDbContext _context;

    public EnseignantsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Enseignants
    public async Task<IActionResult> Index()
    {
        return View(await _context.Enseignants.OrderBy(e => e.Nom).ToListAsync());
    }

    // GET: Enseignants/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var enseignant = await _context.Enseignants
            .Include(e => e.Cours)
            .FirstOrDefaultAsync(m => m.IdEnseignant == id);

        if (enseignant == null)
        {
            return NotFound();
        }

        return View(enseignant);
    }

    // GET: Enseignants/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Enseignants/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdEnseignant,Nom,Prenom,Email,Telephone,Specialite,Grade,NiveauExperience")] Enseignant enseignant)
    {
        if (ModelState.IsValid)
        {
            enseignant.DateCreation = DateTime.Now;
            enseignant.Statut = "actif";

            _context.Add(enseignant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(enseignant);
    }

    // GET: Enseignants/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var enseignant = await _context.Enseignants.FindAsync(id);
        if (enseignant == null)
        {
            return NotFound();
        }
        return View(enseignant);
    }

    // POST: Enseignants/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdEnseignant,Matricule,Nom,Prenom,Email,Telephone,Specialite,Grade,NiveauExperience,Statut")] Enseignant enseignant)
    {
        if (id != enseignant.IdEnseignant)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(enseignant);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnseignantExists(enseignant.IdEnseignant))
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
        return View(enseignant);
    }

    // GET: Enseignants/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var enseignant = await _context.Enseignants
            .FirstOrDefaultAsync(m => m.IdEnseignant == id);
        if (enseignant == null)
        {
            return NotFound();
        }

        return View(enseignant);
    }

    // POST: Enseignants/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var enseignant = await _context.Enseignants.FindAsync(id);
        if (enseignant != null)
        {
            _context.Enseignants.Remove(enseignant);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EnseignantExists(int id)
    {
        return _context.Enseignants.Any(e => e.IdEnseignant == id);
    }
}

public class CoursController : Controller
{
    private readonly ApplicationDbContext _context;

    public CoursController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Cours
    public async Task<IActionResult> Index()
    {
        var cours = await _context.Cours
            .Include(c => c.Enseignant)
            .Include(c => c.Classe)
            .Include(c => c.Filiere)
            .OrderBy(c => c.NomCours)
            .ToListAsync();
        return View(cours);
    }

    // GET: Cours/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cours = await _context.Cours
            .Include(c => c.Enseignant)
            .Include(c => c.Classe)
            .Include(c => c.Filiere)
            .FirstOrDefaultAsync(m => m.IdCours == id);

        if (cours == null)
        {
            return NotFound();
        }

        return View(cours);
    }

    // GET: Cours/Create
    public async Task<IActionResult> Create()
    {
        ViewData["IdEnseignant"] = new SelectList(await _context.Enseignants.OrderBy(e => e.Nom).ToListAsync(), "IdEnseignant", "NomComplet");
        ViewData["IdClasse"] = new SelectList(await _context.Classes.OrderBy(c => c.NomClasse).ToListAsync(), "IdClasse", "NomClasse");
        ViewData["IdFiliere"] = new SelectList(await _context.Filieres.OrderBy(f => f.NomFiliere).ToListAsync(), "IdFiliere", "NomFiliere");
        return View();
    }

    // POST: Cours/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdCours,NomCours,CodeCours,Description,IdEnseignant,IdClasse,IdFiliere,NombreHeures,Coefficient,Semestre,AnneeAcademique")] Cours cours)
    {
        if (ModelState.IsValid)
        {
            cours.DateCreation = DateTime.Now;
            cours.Statut = "actif";

            _context.Add(cours);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["IdEnseignant"] = new SelectList(await _context.Enseignants.OrderBy(e => e.Nom).ToListAsync(), "IdEnseignant", "NomComplet");
        ViewData["IdClasse"] = new SelectList(await _context.Classes.OrderBy(c => c.NomClasse).ToListAsync(), "IdClasse", "NomClasse");
        ViewData["IdFiliere"] = new SelectList(await _context.Filieres.OrderBy(f => f.NomFiliere).ToListAsync(), "IdFiliere", "NomFiliere");
        return View(cours);
    }

    // GET: Cours/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cours = await _context.Cours.FindAsync(id);
        if (cours == null)
        {
            return NotFound();
        }

        ViewData["IdEnseignant"] = new SelectList(await _context.Enseignants.OrderBy(e => e.Nom).ToListAsync(), "IdEnseignant", "NomComplet");
        ViewData["IdClasse"] = new SelectList(await _context.Classes.OrderBy(c => c.NomClasse).ToListAsync(), "IdClasse", "NomClasse");
        ViewData["IdFiliere"] = new SelectList(await _context.Filieres.OrderBy(f => f.NomFiliere).ToListAsync(), "IdFiliere", "NomFiliere");
        return View(cours);
    }

    // POST: Cours/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdCours,NomCours,CodeCours,Description,IdEnseignant,IdClasse,IdFiliere,NombreHeures,Coefficient,Semestre,AnneeAcademique,Statut")] Cours cours)
    {
        if (id != cours.IdCours)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(cours);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoursExists(cours.IdCours))
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

        ViewData["IdEnseignant"] = new SelectList(await _context.Enseignants.OrderBy(e => e.Nom).ToListAsync(), "IdEnseignant", "NomComplet");
        ViewData["IdClasse"] = new SelectList(await _context.Classes.OrderBy(c => c.NomClasse).ToListAsync(), "IdClasse", "NomClasse");
        ViewData["IdFiliere"] = new SelectList(await _context.Filieres.OrderBy(f => f.NomFiliere).ToListAsync(), "IdFiliere", "NomFiliere");
        return View(cours);
    }

    // GET: Cours/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cours = await _context.Cours
            .Include(c => c.Enseignant)
            .Include(c => c.Classe)
            .Include(c => c.Filiere)
            .FirstOrDefaultAsync(m => m.IdCours == id);

        if (cours == null)
        {
            return NotFound();
        }

        return View(cours);
    }

    // POST: Cours/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var cours = await _context.Cours.FindAsync(id);
        if (cours != null)
        {
            _context.Cours.Remove(cours);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CoursExists(int id)
    {
        return _context.Cours.Any(e => e.IdCours == id);
    }
}

public class AbsencesController : Controller
{
    private readonly ApplicationDbContext _context;

    public AbsencesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Absences
    public async Task<IActionResult> Index()
    {
        var absences = await _context.Absences
            .Include(a => a.Etudiant)
            .Include(a => a.Cours)
            .OrderByDescending(a => a.DateAbsence)
            .ToListAsync();
        return View(absences);
    }

    // GET: Absences/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var absence = await _context.Absences
            .Include(a => a.Etudiant)
            .Include(a => a.Cours)
            .FirstOrDefaultAsync(m => m.IdAbsence == id);

        if (absence == null)
        {
            return NotFound();
        }

        return View(absence);
    }

    // GET: Absences/Create
    public async Task<IActionResult> Create()
    {
        ViewData["IdEtudiant"] = new SelectList(await _context.Etudiants.OrderBy(e => e.Nom).ToListAsync(), "IdEtudiant", "NomComplet");
        ViewData["IdCours"] = new SelectList(await _context.Cours.OrderBy(c => c.NomCours).ToListAsync(), "IdCours", "NomCours");
        return View();
    }

    // POST: Absences/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdAbsence,IdEtudiant,IdCours,DateAbsence,TypeAbsence,Motif,Justifiee,DureeHeures")] Absence absence)
    {
        if (ModelState.IsValid)
        {
            absence.DateCreation = DateTime.Now;

            _context.Add(absence);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["IdEtudiant"] = new SelectList(await _context.Etudiants.OrderBy(e => e.Nom).ToListAsync(), "IdEtudiant", "NomComplet");
        ViewData["IdCours"] = new SelectList(await _context.Cours.OrderBy(c => c.NomCours).ToListAsync(), "IdCours", "NomCours");
        return View(absence);
    }

    // GET: Absences/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var absence = await _context.Absences.FindAsync(id);
        if (absence == null)
        {
            return NotFound();
        }

        ViewData["IdEtudiant"] = new SelectList(await _context.Etudiants.OrderBy(e => e.Nom).ToListAsync(), "IdEtudiant", "NomComplet");
        ViewData["IdCours"] = new SelectList(await _context.Cours.OrderBy(c => c.NomCours).ToListAsync(), "IdCours", "NomCours");
        return View(absence);
    }

    // POST: Absences/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdAbsence,IdEtudiant,IdCours,DateAbsence,TypeAbsence,Motif,Justifiee,DureeHeures")] Absence absence)
    {
        if (id != absence.IdAbsence)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(absence);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AbsenceExists(absence.IdAbsence))
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

        ViewData["IdEtudiant"] = new SelectList(await _context.Etudiants.OrderBy(e => e.Nom).ToListAsync(), "IdEtudiant", "NomComplet");
        ViewData["IdCours"] = new SelectList(await _context.Cours.OrderBy(c => c.NomCours).ToListAsync(), "IdCours", "NomCours");
        return View(absence);
    }

    // GET: Absences/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var absence = await _context.Absences
            .Include(a => a.Etudiant)
            .Include(a => a.Cours)
            .FirstOrDefaultAsync(m => m.IdAbsence == id);

        if (absence == null)
        {
            return NotFound();
        }

        return View(absence);
    }

    // POST: Absences/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var absence = await _context.Absences.FindAsync(id);
        if (absence != null)
        {
            _context.Absences.Remove(absence);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AbsenceExists(int id)
    {
        return _context.Absences.Any(e => e.IdAbsence == id);
    }
}

public class HonoraireEnseignantsController : Controller
{
    private readonly ApplicationDbContext _context;

    public HonoraireEnseignantsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: HonoraireEnseignants
    public async Task<IActionResult> Index()
    {
        var honoraires = await _context.HonoraireEnseignants
            .Include(h => h.Enseignant)
            .Include(h => h.Cours)
            .OrderByDescending(h => h.Mois)
            .ToListAsync();
        return View(honoraires);
    }

    // GET: HonoraireEnseignants/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var honoraire = await _context.HonoraireEnseignants
            .Include(h => h.Enseignant)
            .Include(h => h.Cours)
            .FirstOrDefaultAsync(m => m.IdHonoraire == id);

        if (honoraire == null)
        {
            return NotFound();
        }

        return View(honoraire);
    }

    // GET: HonoraireEnseignants/Create
    public async Task<IActionResult> Create()
    {
        ViewData["IdEnseignant"] = new SelectList(await _context.Enseignants.OrderBy(e => e.Nom).ToListAsync(), "IdEnseignant", "NomComplet");
        ViewData["IdCours"] = new SelectList(await _context.Cours.OrderBy(c => c.NomCours).ToListAsync(), "IdCours", "NomCours");
        return View();
    }

    // POST: HonoraireEnseignants/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdHonoraire,IdEnseignant,IdCours,Mois,Annee,MontantBase,TauxHoraire,NombreHeures,Primes,Retenues,MontantTotal,StatutPaiement")] HonoraireEnseignant honoraireEnseignant)
    {
        if (ModelState.IsValid)
        {
            honoraireEnseignant.DateCreation = DateTime.Now;

            _context.Add(honoraireEnseignant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["IdEnseignant"] = new SelectList(await _context.Enseignants.OrderBy(e => e.Nom).ToListAsync(), "IdEnseignant", "NomComplet");
        ViewData["IdCours"] = new SelectList(await _context.Cours.OrderBy(c => c.NomCours).ToListAsync(), "IdCours", "NomCours");
        return View(honoraireEnseignant);
    }

    // GET: HonoraireEnseignants/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var honoraireEnseignant = await _context.HonoraireEnseignants.FindAsync(id);
        if (honoraireEnseignant == null)
        {
            return NotFound();
        }

        ViewData["IdEnseignant"] = new SelectList(await _context.Enseignants.OrderBy(e => e.Nom).ToListAsync(), "IdEnseignant", "NomComplet");
        ViewData["IdCours"] = new SelectList(await _context.Cours.OrderBy(c => c.NomCours).ToListAsync(), "IdCours", "NomCours");
        return View(honoraireEnseignant);
    }

    // POST: HonoraireEnseignants/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdHonoraire,IdEnseignant,IdCours,Mois,Annee,MontantBase,TauxHoraire,NombreHeures,Primes,Retenues,MontantTotal,StatutPaiement")] HonoraireEnseignant honoraireEnseignant)
    {
        if (id != honoraireEnseignant.IdHonoraire)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(honoraireEnseignant);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HonoraireEnseignantExists(honoraireEnseignant.IdHonoraire))
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

        ViewData["IdEnseignant"] = new SelectList(await _context.Enseignants.OrderBy(e => e.Nom).ToListAsync(), "IdEnseignant", "NomComplet");
        ViewData["IdCours"] = new SelectList(await _context.Cours.OrderBy(c => c.NomCours).ToListAsync(), "IdCours", "NomCours");
        return View(honoraireEnseignant);
    }

    // GET: HonoraireEnseignants/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var honoraireEnseignant = await _context.HonoraireEnseignants
            .Include(h => h.Enseignant)
            .Include(h => h.Cours)
            .FirstOrDefaultAsync(m => m.IdHonoraire == id);

        if (honoraireEnseignant == null)
        {
            return NotFound();
        }

        return View(honoraireEnseignant);
    }

    // POST: HonoraireEnseignants/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var honoraireEnseignant = await _context.HonoraireEnseignants.FindAsync(id);
        if (honoraireEnseignant != null)
        {
            _context.HonoraireEnseignants.Remove(honoraireEnseignant);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool HonoraireEnseignantExists(int id)
    {
        return _context.HonoraireEnseignants.Any(e => e.IdHonoraire == id);
    }
}
