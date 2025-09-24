using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IITWebApp.Models
{
    public class Etudiant
    {
        [Key]
        public int IdEtudiant { get; set; }

        [Required]
        [StringLength(20)]
        public string Matricule { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Nom { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Prenom { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }

        [StringLength(100)]
        public string? LieuNaissance { get; set; }

        [Required]
        [StringLength(1)]
        public string Sexe { get; set; } = string.Empty;

        public string? Adresse { get; set; }

        [StringLength(20)]
        public string? Telephone { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(50)]
        public string Nationalite { get; set; } = "Ivoirien";

        [Required]
        [StringLength(20)]
        public string Niveau { get; set; } = string.Empty;

        // Clé étrangère vers la classe
        public int? IdClasse { get; set; }

        [Required]
        [StringLength(20)]
        public string Statut { get; set; } = "actif";

        public DateTime? DateCreation { get; set; }

        // Propriétés calculées
        public string NomComplet => $"{Nom} {Prenom}";

        // Navigation properties
        public virtual Classe? Classe { get; set; }
        public virtual ICollection<Inscription> Inscriptions { get; set; } = new List<Inscription>();
        public virtual ICollection<SouscriptionService> SouscriptionServices { get; set; } = new List<SouscriptionService>();
        public virtual ICollection<Delegue> Delegues { get; set; } = new List<Delegue>();
        public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();
    }

    public class Classe
    {
        [Key]
        public int IdClasse { get; set; }

        [Required]
        [StringLength(50)]
        public string NomClasse { get; set; } = string.Empty;

        // Clé étrangère vers filière
        [Required]
        public int IdFiliere { get; set; }

        [Required]
        [StringLength(20)]
        public string Niveau { get; set; } = string.Empty;

        public int? CapaciteMax { get; set; } = 30;

        [Required]
        [StringLength(20)]
        public string Statut { get; set; } = "actif";

        public DateTime? DateCreation { get; set; }

        // Propriétés calculées
        public string NomComplet => $"{NomClasse} - {Niveau}";

        // Navigation properties
        public virtual Filiere? Filiere { get; set; }
        public virtual ICollection<Etudiant> Etudiants { get; set; } = new List<Etudiant>();
        public virtual ICollection<Inscription> Inscriptions { get; set; } = new List<Inscription>();
        public virtual ICollection<Delegue> Delegues { get; set; } = new List<Delegue>();
        public virtual ICollection<Cours> Cours { get; set; } = new List<Cours>();
    }

    public class Filiere
    {
        [Key]
        public int IdFiliere { get; set; }

        [Required]
        [StringLength(100)]
        public string NomFiliere { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public int DureeEtudes { get; set; } = 3;

        [Required]
        [StringLength(20)]
        public string Statut { get; set; } = "actif";

        public DateTime? DateCreation { get; set; }

        // Navigation properties
        public virtual ICollection<Classe> Classes { get; set; } = new List<Classe>();
        public virtual ICollection<Scolarite> Scolarites { get; set; } = new List<Scolarite>();
        public virtual ICollection<Inscription> Inscriptions { get; set; } = new List<Inscription>();
        public virtual ICollection<Cours> Cours { get; set; } = new List<Cours>();
    }

    public class Scolarite
    {
        [Key]
        public int IdScolarite { get; set; }

        [Required]
        public int IdFiliere { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MontantAnnuel { get; set; }

        [Required]
        [StringLength(9)]
        public string AnneeAcademique { get; set; } = string.Empty;

        [StringLength(20)]
        public string Statut { get; set; } = "active";

        // Navigation property
        [ForeignKey("IdFiliere")]
        public virtual Filiere Filiere { get; set; } = null!;
    }

    public class Inscription
    {
        [Key]
        public int IdInscription { get; set; }

        [Required]
        public int IdEtudiant { get; set; }

        [Required]
        public int IdFiliere { get; set; }

        [Required]
        public int IdClasse { get; set; }

        [Required]
        [StringLength(20)]
        public string AnneeAcademique { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateInscription { get; set; }

        [StringLength(50)]
        public string? TypeInscription { get; set; } = "normale";

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MontantInscription { get; set; }

        // Note: ClasseCode et TotalRegle sont gérés via les relations avec les tables Classe et les calculs

        [Required]
        [StringLength(20)]
        public string Statut { get; set; } = "en_cours";

        // Navigation properties
        [ForeignKey("IdEtudiant")]
        public virtual Etudiant? Etudiant { get; set; }

        [ForeignKey("IdFiliere")]
        public virtual Filiere? Filiere { get; set; }

        [ForeignKey("IdClasse")]
        public virtual Classe? Classe { get; set; }
    }

    public class Service
    {
        [Key]
        public int IdService { get; set; }

        [Required]
        [StringLength(100)]
        public string NomService { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Prix { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeService { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Statut { get; set; } = "actif";

        public DateTime? DateCreation { get; set; }

        // Navigation property
        public virtual ICollection<SouscriptionService> SouscriptionServices { get; set; } = new List<SouscriptionService>();
    }

    public class SouscriptionService
    {
        [Key]
        public int IdSouscription { get; set; }

        [Required]
        public int IdEtudiant { get; set; }

        [Required]
        public int IdService { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateSouscription { get; set; }

        [Required]
        [StringLength(20)]
        public string Statut { get; set; } = "actif";

        // Navigation properties
        [ForeignKey("IdEtudiant")]
        public virtual Etudiant? Etudiant { get; set; }

        [ForeignKey("IdService")]
        public virtual Service? Service { get; set; }
    }

    public class Paiement
    {
        [Key]
        public int IdPaiement { get; set; }

        [Required]
        public int IdEtudiant { get; set; }

        public int? IdInscription { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Montant { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DatePaiement { get; set; }

        [Required]
        [StringLength(50)]
        public string MethodePaiement { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string TypePaiement { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ReferencePaiement { get; set; }

        [Required]
        [StringLength(20)]
        public string Statut { get; set; } = "valide";

        // Navigation properties
        [ForeignKey("IdEtudiant")]
        public virtual Etudiant? Etudiant { get; set; }

        [ForeignKey("IdInscription")]
        public virtual Inscription? Inscription { get; set; }
    }

    public class Delegue
    {
        [Key]
        public int IdDelegue { get; set; }

        [Required]
        public int IdEtudiant { get; set; }

        [Required]
        public int IdClasse { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeDelegue { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateNomination { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateFinMandat { get; set; }

        [Required]
        [StringLength(20)]
        public string Statut { get; set; } = "actif";

        // Navigation properties
        [ForeignKey("IdEtudiant")]
        public virtual Etudiant? Etudiant { get; set; }

        [ForeignKey("IdClasse")]
        public virtual Classe? Classe { get; set; }
    }

    // ===== PHASE 2 - NOUVEAUX MODÈLES =====

    public class Enseignant
    {
        [Key]
        public int IdEnseignant { get; set; }

        [Required]
        [StringLength(20)]
        public string Matricule { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Nom { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Prenom { get; set; } = string.Empty;

        [Required]
        [StringLength(1)]
        public string Sexe { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? DateNaissance { get; set; }

        [StringLength(100)]
        public string? LieuNaissance { get; set; }

        [StringLength(50)]
        public string? Nationalite { get; set; } = "Ivoirien";

        public string? Adresse { get; set; }

        [StringLength(20)]
        public string? Telephone { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(100)]
        public string? Specialite { get; set; }

        [StringLength(50)]
        public string? Grade { get; set; }

        [StringLength(20)]
        public string? NiveauExperience { get; set; }

        [Required]
        [StringLength(20)]
        public string Statut { get; set; } = "actif";

        [DataType(DataType.Date)]
        public DateTime? DateEmbauche { get; set; }

        public DateTime? DateCreation { get; set; }

        // Propriété calculée
        [NotMapped]
        public string NomComplet => $"{Nom} {Prenom}";

        // Navigation properties
        public virtual ICollection<Cours> Cours { get; set; } = new List<Cours>();
        public virtual ICollection<HonoraireEnseignant> HonoraireEnseignants { get; set; } = new List<HonoraireEnseignant>();
    }

    public class Cours
    {
        [Key]
        public int IdCours { get; set; }

        [Required]
        [StringLength(100)]
        public string NomCours { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string CodeCours { get; set; } = string.Empty;

        [Required]
        public int IdEnseignant { get; set; }

        [Required]
        public int IdClasse { get; set; }

        public int? IdFiliere { get; set; }

        [Required]
        public int VolumeHoraire { get; set; }

        public int? NombreHeures { get; set; }

        [Required]
        public int Coefficient { get; set; } = 1;

        [Required]
        [StringLength(50)]
        public string TypeCours { get; set; } = string.Empty;

        [StringLength(10)]
        public string? Semestre { get; set; }

        [StringLength(9)]
        public string? AnneeAcademique { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(20)]
        public string Statut { get; set; } = "actif";

        public DateTime? DateCreation { get; set; }

        // Navigation properties
        [ForeignKey("IdEnseignant")]
        public virtual Enseignant? Enseignant { get; set; }

        [ForeignKey("IdClasse")]
        public virtual Classe? Classe { get; set; }

        [ForeignKey("IdFiliere")]
        public virtual Filiere? Filiere { get; set; }

        public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();
        public virtual ICollection<HonoraireEnseignant> HonoraireEnseignants { get; set; } = new List<HonoraireEnseignant>();
    }

    public class Absence
    {
        [Key]
        public int IdAbsence { get; set; }

        [Required]
        public int IdEtudiant { get; set; }

        [Required]
        public int IdCours { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateAbsence { get; set; }

        [Required]
        [StringLength(50)]
        public string SeanceAbsence { get; set; } = string.Empty;

        [DataType(DataType.Time)]
        public TimeSpan? HeureDebut { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? HeureFin { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        public decimal? DureeHeures { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeAbsence { get; set; } = string.Empty;

        public bool? Justifiee { get; set; } = false;

        public string? Motif { get; set; }

        public DateTime? DateCreation { get; set; }

        // Navigation properties
        [ForeignKey("IdEtudiant")]
        public virtual Etudiant? Etudiant { get; set; }

        [ForeignKey("IdCours")]
        public virtual Cours? Cours { get; set; }
    }

    public class HonoraireEnseignant
    {
        [Key]
        public int IdHonoraire { get; set; }

        [Required]
        public int IdEnseignant { get; set; }

        [Required]
        public int IdCours { get; set; }

        [Required]
        [StringLength(20)]
        public string Mois { get; set; } = string.Empty;

        [Required]
        public int Annee { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal HeuresEffectuees { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? NombreHeures { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TauxHoraire { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? MontantBase { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Primes { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Retenues { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? MontantTotal { get; set; }

        [Required]
        [StringLength(20)]
        public string Statut { get; set; } = "en_attente";

        [StringLength(20)]
        public string? StatutPaiement { get; set; } = "En attente";

        public DateTime? DateCreation { get; set; }

        // Navigation properties
        [ForeignKey("IdEnseignant")]
        public virtual Enseignant? Enseignant { get; set; }

        [ForeignKey("IdCours")]
        public virtual Cours? Cours { get; set; }
    }
}
