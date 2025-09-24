using System.ComponentModel.DataAnnotations;

namespace IITWebApp.Models
{
    // ViewModel pour la création d'étudiants sans les champs automatiques
    public class CreateEtudiantViewModel
    {
        [Required(ErrorMessage = "Le niveau d'études est obligatoire")]
        [Display(Name = "Niveau d'études")]
        public string Niveau { get; set; } = string.Empty;

        [Display(Name = "Matricule")]
        [StringLength(20, ErrorMessage = "Le matricule ne peut pas dépasser 20 caractères")]
        public string Matricule { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [StringLength(50, ErrorMessage = "Le prénom ne peut pas dépasser 50 caractères")]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date de naissance est obligatoire")]
        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }

        [StringLength(100, ErrorMessage = "Le lieu de naissance ne peut pas dépasser 100 caractères")]
        public string? LieuNaissance { get; set; }

        [Required(ErrorMessage = "Le sexe est obligatoire")]
        [StringLength(1, ErrorMessage = "Le sexe doit être M ou F")]
        public string Sexe { get; set; } = string.Empty;

        public string? Adresse { get; set; }

        [StringLength(20, ErrorMessage = "Le téléphone ne peut pas dépasser 20 caractères")]
        public string? Telephone { get; set; }

        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        [StringLength(100, ErrorMessage = "L'email ne peut pas dépasser 100 caractères")]
        public string? Email { get; set; }

        [StringLength(50, ErrorMessage = "La nationalité ne peut pas dépasser 50 caractères")]
        public string Nationalite { get; set; } = "Ivoirien";
    }

    // ViewModel pour l'édition d'étudiants
    public class EditEtudiantViewModel : CreateEtudiantViewModel
    {
        public int IdEtudiant { get; set; }
        public string Statut { get; set; } = "actif";
        public DateTime DateCreation { get; set; }
    }

    // ViewModel pour les délégués sans les champs historiques obligatoires
    public class CreateDelegueViewModel
    {
        [Required]
        public int IdEtudiant { get; set; }

        [Required]
        public int IdClasse { get; set; }

        [Required]
        [StringLength(20)]
        public string Fonction { get; set; } = string.Empty;

        [Required]
        [StringLength(9)]
        public string AnneeAcademique { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? DateElection { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateDebut { get; set; }

        [DataType(DataType.Date)]  
        public DateTime? DateFin { get; set; }
    }

    // ViewModel pour les souscriptions de service
    public class CreateSouscriptionServiceViewModel
    {
        [Required]
        public int IdEtudiant { get; set; }

        [Required]
        public int IdService { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateDebut { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateFin { get; set; }

        [StringLength(200)]
        public string? MotifFin { get; set; }
    }

    // ViewModel pour la réinscription des étudiants
    public class ReinscriptionViewModel
    {
        [Required(ErrorMessage = "Le matricule de l'étudiant est obligatoire")]
        [StringLength(20, ErrorMessage = "Le matricule ne peut pas dépasser 20 caractères")]
        [Display(Name = "Matricule de l'Étudiant")]
        public string EtuMatricule { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'année académique est obligatoire")]
        [StringLength(9, ErrorMessage = "L'année académique ne peut pas dépasser 9 caractères")]
        [Display(Name = "Année Académique")]
        public string AnAcadem { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date d'inscription est obligatoire")]
        [DataType(DataType.Date)]
        [Display(Name = "Date d'Inscription")]
        public DateTime DateInscrip { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Le montant de scolarité est obligatoire")]
        [Range(0, double.MaxValue, ErrorMessage = "Le montant doit être positif")]
        [Display(Name = "Montant de Scolarité")]
        public decimal TotalScolarite { get; set; } = 1200000;

        [Display(Name = "Services Additionnels")]
        public List<string> Services { get; set; } = new List<string>();

        // Propriétés calculées
        [Display(Name = "Nom de l'Étudiant")]
        public string? NomEtudiant { get; set; }

        [Display(Name = "Prénom de l'Étudiant")]
        public string? PrenomEtudiant { get; set; }

        [Display(Name = "Classe Actuelle")]
        public string? ClasseActuelle { get; set; }

        [Display(Name = "Moyenne")]
        public decimal? Moyenne { get; set; }

        [Display(Name = "Statut d'Éligibilité")]
        public string? StatutEligibilite { get; set; }
    }
}