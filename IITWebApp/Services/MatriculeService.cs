using IITWebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace IITWebApp.Services
{
    public interface IMatriculeService
    {
        Task<string> GenerateMatriculeAsync(string niveau);
        bool IsValidMatricule(string matricule);
        string ExtractNiveauFromMatricule(string matricule);
        string ExtractAnneeFromMatricule(string matricule);
    }

    public class MatriculeService : IMatriculeService
    {
        private readonly ApplicationDbContext _context;

        public MatriculeService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Génère automatiquement un matricule au format IIT2025L1001, IIT2025L2001, etc.
        /// </summary>
        /// <param name="niveau">L1, L2, L3, M1, M2</param>
        /// <returns>Matricule généré</returns>
        public async Task<string> GenerateMatriculeAsync(string niveau)
        {
            var currentYear = DateTime.Now.Year;
            var baseMatricule = $"IIT{currentYear}{niveau}";

            // Récupérer le dernier matricule pour ce niveau et cette année
            var lastMatricule = await _context.Etudiants
                .Where(e => e.Matricule.StartsWith(baseMatricule))
                .OrderByDescending(e => e.Matricule)
                .Select(e => e.Matricule)
                .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (!string.IsNullOrEmpty(lastMatricule))
            {
                // Extraire le numéro de la fin du matricule
                var numberPart = lastMatricule.Substring(baseMatricule.Length);
                if (int.TryParse(numberPart, out int currentNumber))
                {
                    nextNumber = currentNumber + 1;
                }
            }

            // Format : IIT2025L1001, IIT2025L2001, etc.
            return $"{baseMatricule}{nextNumber:D3}";
        }

        /// <summary>
        /// Valide le format du matricule
        /// </summary>
        /// <param name="matricule">Matricule à valider</param>
        /// <returns>True si valide</returns>
        public bool IsValidMatricule(string matricule)
        {
            if (string.IsNullOrEmpty(matricule) || matricule.Length != 12)
                return false;

            // Format attendu : IIT2025L1001
            if (!matricule.StartsWith("IIT"))
                return false;

            // Vérifier l'année (4 chiffres)
            if (!int.TryParse(matricule.Substring(3, 4), out int year))
                return false;

            if (year < 2020 || year > 2030)
                return false;

            // Vérifier le niveau (L1, L2, L3, M1, M2)
            var niveau = matricule.Substring(7, 2);
            var niveauxValides = new[] { "L1", "L2", "L3", "M1", "M2" };
            if (!niveauxValides.Contains(niveau))
                return false;

            // Vérifier le numéro (3 chiffres)
            if (!int.TryParse(matricule.Substring(9, 3), out int numero))
                return false;

            if (numero < 1 || numero > 999)
                return false;

            return true;
        }

        /// <summary>
        /// Extrait le niveau du matricule
        /// </summary>
        /// <param name="matricule">Matricule</param>
        /// <returns>Niveau (L1, L2, etc.)</returns>
        public string ExtractNiveauFromMatricule(string matricule)
        {
            if (IsValidMatricule(matricule))
            {
                return matricule.Substring(7, 2);
            }
            return string.Empty;
        }

        /// <summary>
        /// Extrait l'année du matricule
        /// </summary>
        /// <param name="matricule">Matricule</param>
        /// <returns>Année</returns>
        public string ExtractAnneeFromMatricule(string matricule)
        {
            if (IsValidMatricule(matricule))
            {
                return matricule.Substring(3, 4);
            }
            return string.Empty;
        }
    }
}