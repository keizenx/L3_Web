using Microsoft.EntityFrameworkCore;
using IITWebApp.Models;

namespace IITWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Etudiant> Etudiants { get; set; }
        public DbSet<Classe> Classes { get; set; }
        public DbSet<Filiere> Filieres { get; set; }
        public DbSet<Scolarite> Scolarites { get; set; }
        public DbSet<Inscription> Inscriptions { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<SouscriptionService> SouscriptionServices { get; set; }
        public DbSet<Paiement> Paiements { get; set; }
        public DbSet<Delegue> Delegues { get; set; }

        // PHASE 2 - Nouveaux DbSet
        public DbSet<Enseignant> Enseignants { get; set; }
        public DbSet<Cours> Cours { get; set; }
        public DbSet<Absence> Absences { get; set; }
        public DbSet<HonoraireEnseignant> HonoraireEnseignants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration des tables
            modelBuilder.Entity<Etudiant>(entity =>
            {
                entity.ToTable("etudiant");
                entity.HasKey(e => e.IdEtudiant);
                entity.Property(e => e.IdEtudiant).HasColumnName("id_etudiant");
                entity.Property(e => e.Matricule).HasColumnName("matricule").HasMaxLength(20).IsRequired();
                entity.Property(e => e.Nom).HasColumnName("nom").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Prenom).HasColumnName("prenom").HasMaxLength(50).IsRequired();
                entity.Property(e => e.DateNaissance).HasColumnName("date_naissance").IsRequired();
                entity.Property(e => e.LieuNaissance).HasColumnName("lieu_naissance").HasMaxLength(100);
                entity.Property(e => e.Sexe).HasColumnName("sexe").HasMaxLength(1).IsRequired();
                entity.Property(e => e.Adresse).HasColumnName("adresse").HasColumnType("text");
                entity.Property(e => e.Telephone).HasColumnName("telephone").HasMaxLength(20);
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(100);
                entity.Property(e => e.Nationalite).HasColumnName("nationalite").HasMaxLength(50).HasDefaultValue("Ivoirien");
                entity.Property(e => e.Niveau).HasColumnName("niveau").HasMaxLength(20).IsRequired();
                entity.Property(e => e.Statut).HasColumnName("statut").HasMaxLength(20).HasDefaultValue("actif").IsRequired();
                entity.Property(e => e.DateCreation).HasColumnName("date_creation");
                
                // Relation optionnelle avec classe (peut être null)
                entity.Property(e => e.IdClasse).HasColumnName("id_classe");
                entity.HasOne(e => e.Classe)
                      .WithMany(c => c.Etudiants)
                      .HasForeignKey(e => e.IdClasse)
                      .OnDelete(DeleteBehavior.SetNull);
                      
                entity.HasIndex(e => e.Matricule).IsUnique();
                entity.HasIndex(e => new { e.Nom, e.Prenom });
            });

            modelBuilder.Entity<Classe>(entity =>
            {
                entity.ToTable("classe");
                entity.HasKey(e => e.IdClasse);
                entity.Property(e => e.IdClasse).HasColumnName("id_classe");
                entity.Property(e => e.NomClasse).HasColumnName("nom_classe").HasMaxLength(50).IsRequired();
                entity.Property(e => e.IdFiliere).HasColumnName("id_filiere").IsRequired();
                entity.Property(e => e.Niveau).HasColumnName("niveau").HasMaxLength(20).IsRequired();
                entity.Property(e => e.CapaciteMax).HasColumnName("capacite_max").HasDefaultValue(30);
                entity.Property(e => e.Statut).HasColumnName("statut").HasMaxLength(20).HasDefaultValue("actif").IsRequired();
                entity.Property(e => e.DateCreation).HasColumnName("date_creation");
                
                // Relation avec filière
                entity.HasOne(c => c.Filiere)
                      .WithMany(f => f.Classes)
                      .HasForeignKey(c => c.IdFiliere)
                      .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasIndex(e => e.Niveau);
                entity.HasIndex(e => e.IdFiliere);
            });

            modelBuilder.Entity<Filiere>(entity =>
            {
                entity.ToTable("filiere");
                entity.HasKey(e => e.IdFiliere);
                entity.Property(e => e.IdFiliere).HasColumnName("id_filiere");
                entity.Property(e => e.NomFiliere).HasColumnName("nom_filiere").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
                entity.Property(e => e.DureeEtudes).HasColumnName("duree_etudes").HasDefaultValue(3).IsRequired();
                entity.Property(e => e.Statut).HasColumnName("statut").HasMaxLength(20).HasDefaultValue("actif").IsRequired();
                entity.Property(e => e.DateCreation).HasColumnName("date_creation");
                
                entity.HasIndex(e => e.NomFiliere);
            });

            modelBuilder.Entity<Scolarite>(entity =>
            {
                entity.ToTable("scolarite");
                entity.HasKey(e => e.IdScolarite);
                entity.Property(e => e.IdScolarite).HasColumnName("id_scolarite");
                entity.Property(e => e.IdFiliere).HasColumnName("id_filiere").IsRequired();
                entity.Property(e => e.MontantAnnuel).HasColumnName("montant_annuel").HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.AnneeAcademique).HasColumnName("annee_academique").HasMaxLength(9).IsRequired();
                entity.Property(e => e.Statut).HasColumnName("statut").HasMaxLength(20);
                
                entity.HasOne(e => e.Filiere)
                    .WithMany(f => f.Scolarites)
                    .HasForeignKey(e => e.IdFiliere)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasIndex(e => e.AnneeAcademique);
            });

            modelBuilder.Entity<Inscription>(entity =>
            {
                entity.ToTable("inscription");
                entity.HasKey(e => e.IdInscription);
                entity.Property(e => e.IdInscription).HasColumnName("id_inscription");
                entity.Property(e => e.IdEtudiant).HasColumnName("id_etudiant").IsRequired();
                entity.Property(e => e.IdClasse).HasColumnName("id_classe").IsRequired();
                entity.Property(e => e.IdFiliere).HasColumnName("id_filiere").IsRequired();
                entity.Property(e => e.AnneeAcademique).HasColumnName("annee_academique").HasMaxLength(9).IsRequired();
                entity.Property(e => e.DateInscription).HasColumnName("date_inscription").IsRequired();
                entity.Property(e => e.TypeInscription).HasColumnName("type_inscription").HasMaxLength(20).IsRequired();
                entity.Property(e => e.Statut).HasColumnName("statut").HasMaxLength(20);
                entity.Property(e => e.MontantInscription).HasColumnName("montant_inscription").HasColumnType("decimal(10,2)").IsRequired();
                
                entity.HasOne(e => e.Etudiant)
                    .WithMany(et => et.Inscriptions)
                    .HasForeignKey(e => e.IdEtudiant)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Classe)
                    .WithMany(c => c.Inscriptions)
                    .HasForeignKey(e => e.IdClasse)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Filiere)
                    .WithMany(f => f.Inscriptions)
                    .HasForeignKey(e => e.IdFiliere)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasIndex(e => e.AnneeAcademique);
                entity.HasIndex(e => e.IdEtudiant);
                entity.HasIndex(e => e.Statut);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("service");
                entity.HasKey(e => e.IdService);
                entity.Property(e => e.IdService).HasColumnName("id_service");
                entity.Property(e => e.NomService).HasColumnName("nom_service").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
                entity.Property(e => e.Prix).HasColumnName("prix").HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.TypeService).HasColumnName("type_service").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Statut).HasColumnName("statut").HasMaxLength(20).HasDefaultValue("actif").IsRequired();
                entity.Property(e => e.DateCreation).HasColumnName("date_creation");
                
                entity.HasIndex(e => e.NomService);
            });

            modelBuilder.Entity<SouscriptionService>(entity =>
            {
                entity.ToTable("souscription_service");
                entity.HasKey(e => e.IdSouscription);
                entity.Property(e => e.IdSouscription).HasColumnName("id_souscription");
                entity.Property(e => e.IdEtudiant).HasColumnName("id_etudiant").IsRequired();
                entity.Property(e => e.IdService).HasColumnName("id_service").IsRequired();
                entity.Property(e => e.DateSouscription).HasColumnName("date_souscription").IsRequired();
                entity.Property(e => e.Statut).HasColumnName("statut").HasMaxLength(20).HasDefaultValue("actif").IsRequired();
                
                entity.HasOne(e => e.Etudiant)
                    .WithMany(et => et.SouscriptionServices)
                    .HasForeignKey(e => e.IdEtudiant)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Service)
                    .WithMany(s => s.SouscriptionServices)
                    .HasForeignKey(e => e.IdService)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Paiement>(entity =>
            {
                entity.ToTable("paiement");
                entity.HasKey(e => e.IdPaiement);
                entity.Property(e => e.IdPaiement).HasColumnName("id_paiement");
                entity.Property(e => e.IdEtudiant).HasColumnName("id_etudiant").IsRequired();
                entity.Property(e => e.IdInscription).HasColumnName("id_inscription");
                entity.Property(e => e.Montant).HasColumnName("montant").HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.DatePaiement).HasColumnName("date_paiement").IsRequired();
                entity.Property(e => e.MethodePaiement).HasColumnName("methode_paiement").HasMaxLength(50).IsRequired();
                entity.Property(e => e.TypePaiement).HasColumnName("type_paiement").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ReferencePaiement).HasColumnName("reference_paiement").HasMaxLength(100);
                entity.Property(e => e.Statut).HasColumnName("statut").HasMaxLength(20).HasDefaultValue("valide").IsRequired();
                
                entity.HasOne(e => e.Etudiant)
                    .WithMany()
                    .HasForeignKey(e => e.IdEtudiant)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Inscription)
                    .WithMany()
                    .HasForeignKey(e => e.IdInscription)
                    .OnDelete(DeleteBehavior.SetNull);
                    
                entity.HasIndex(e => e.DatePaiement);
                entity.HasIndex(e => e.TypePaiement);
            });

            modelBuilder.Entity<Delegue>(entity =>
            {
                entity.ToTable("delegue");
                entity.HasKey(e => e.IdDelegue);
                entity.Property(e => e.IdDelegue).HasColumnName("id_delegue");
                entity.Property(e => e.IdEtudiant).HasColumnName("id_etudiant").IsRequired();
                entity.Property(e => e.IdClasse).HasColumnName("id_classe").IsRequired();
                entity.Property(e => e.TypeDelegue).HasColumnName("type_delegue").HasMaxLength(50).IsRequired();
                entity.Property(e => e.DateNomination).HasColumnName("date_nomination").IsRequired();
                entity.Property(e => e.DateFinMandat).HasColumnName("date_fin_mandat");
                entity.Property(e => e.Statut).HasColumnName("statut").HasMaxLength(20).HasDefaultValue("actif").IsRequired();
                
                entity.HasOne(e => e.Etudiant)
                    .WithMany(et => et.Delegues)
                    .HasForeignKey(e => e.IdEtudiant)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Classe)
                    .WithMany(c => c.Delegues)
                    .HasForeignKey(e => e.IdClasse)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasIndex(e => new { e.IdClasse, e.TypeDelegue });
            });

            // ===== PHASE 2 - CONFIGURATIONS =====

            modelBuilder.Entity<Enseignant>(entity =>
            {
                entity.ToTable("enseignant");
                entity.HasKey(e => e.IdEnseignant);
                entity.Property(e => e.IdEnseignant).HasColumnName("id_enseignant");
                entity.Property(e => e.Matricule).HasColumnName("matricule").HasMaxLength(20).IsRequired();
                entity.Property(e => e.Nom).HasColumnName("nom").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Prenom).HasColumnName("prenom").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Sexe).HasColumnName("sexe").HasMaxLength(1).IsRequired();
                entity.Property(e => e.DateNaissance).HasColumnName("date_naissance");
                entity.Property(e => e.LieuNaissance).HasColumnName("lieu_naissance").HasMaxLength(100);
                entity.Property(e => e.Nationalite).HasColumnName("nationalite").HasMaxLength(50).HasDefaultValue("Ivoirien");
                entity.Property(e => e.Adresse).HasColumnName("adresse").HasColumnType("text");
                entity.Property(e => e.Telephone).HasColumnName("telephone").HasMaxLength(20);
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(100);
                entity.Property(e => e.Specialite).HasColumnName("specialite").HasMaxLength(100);
                entity.Property(e => e.Grade).HasColumnName("grade").HasMaxLength(50);
                entity.Property(e => e.NiveauExperience).HasColumnName("niveau_experience").HasMaxLength(20);
                entity.Property(e => e.Statut).HasColumnName("statut").HasMaxLength(20).HasDefaultValue("actif").IsRequired();
                entity.Property(e => e.DateEmbauche).HasColumnName("date_embauche");
                entity.Property(e => e.DateCreation).HasColumnName("date_creation");

                entity.HasIndex(e => e.Matricule).IsUnique();
                entity.HasIndex(e => new { e.Nom, e.Prenom });
            });

            modelBuilder.Entity<Cours>(entity =>
            {
                entity.ToTable("cours");
                entity.HasKey(e => e.IdCours);
                entity.Property(e => e.IdCours).HasColumnName("id_cours");
                entity.Property(e => e.NomCours).HasColumnName("nom_cours").HasMaxLength(100).IsRequired();
                entity.Property(e => e.CodeCours).HasColumnName("code_cours").HasMaxLength(20).IsRequired();
                entity.Property(e => e.IdEnseignant).HasColumnName("id_enseignant").IsRequired();
                entity.Property(e => e.IdClasse).HasColumnName("id_classe").IsRequired();
                entity.Property(e => e.IdFiliere).HasColumnName("id_filiere");
                entity.Property(e => e.VolumeHoraire).HasColumnName("volume_horaire").IsRequired();
                entity.Property(e => e.NombreHeures).HasColumnName("nombre_heures");
                entity.Property(e => e.Coefficient).HasColumnName("coefficient").HasDefaultValue(1).IsRequired();
                entity.Property(e => e.TypeCours).HasColumnName("type_cours").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Semestre).HasColumnName("semestre").HasMaxLength(10);
                entity.Property(e => e.AnneeAcademique).HasColumnName("annee_academique").HasMaxLength(9);
                entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(500);
                entity.Property(e => e.Statut).HasColumnName("statut").HasMaxLength(20).HasDefaultValue("actif").IsRequired();
                entity.Property(e => e.DateCreation).HasColumnName("date_creation");

                entity.HasIndex(e => e.CodeCours).IsUnique();
                entity.HasIndex(e => new { e.IdEnseignant, e.IdClasse });

                // Relations
                entity.HasOne(d => d.Enseignant)
                    .WithMany(p => p.Cours)
                    .HasForeignKey(d => d.IdEnseignant)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Classe)
                    .WithMany(p => p.Cours)
                    .HasForeignKey(d => d.IdClasse)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Filiere)
                    .WithMany(p => p.Cours)
                    .HasForeignKey(d => d.IdFiliere)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Absence>(entity =>
            {
                entity.ToTable("absence");
                entity.HasKey(e => e.IdAbsence);
                entity.Property(e => e.IdAbsence).HasColumnName("id_absence");
                entity.Property(e => e.IdEtudiant).HasColumnName("id_etudiant").IsRequired();
                entity.Property(e => e.IdCours).HasColumnName("id_cours").IsRequired();
                entity.Property(e => e.DateAbsence).HasColumnName("date_absence").IsRequired();
                entity.Property(e => e.SeanceAbsence).HasColumnName("seance_absence").HasMaxLength(50).IsRequired();
                entity.Property(e => e.DureeHeures).HasColumnName("duree_heures").HasColumnType("decimal(4,2)");
                entity.Property(e => e.TypeAbsence).HasColumnName("type_absence").HasMaxLength(20).HasDefaultValue("Non justifiee").IsRequired();
                entity.Property(e => e.Motif).HasColumnName("motif").HasColumnType("text");
                entity.Property(e => e.Justifiee).HasColumnName("justifiee").HasDefaultValue(false).IsRequired();
                entity.Property(e => e.DateCreation).HasColumnName("date_creation");

                entity.HasIndex(e => new { e.IdEtudiant, e.DateAbsence });
                entity.HasIndex(e => new { e.IdCours, e.DateAbsence });

                // Relations
                entity.HasOne(d => d.Etudiant)
                    .WithMany(p => p.Absences)
                    .HasForeignKey(d => d.IdEtudiant)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Cours)
                    .WithMany(p => p.Absences)
                    .HasForeignKey(d => d.IdCours)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<HonoraireEnseignant>(entity =>
            {
                entity.ToTable("honoraire_enseignant");
                entity.HasKey(e => e.IdHonoraire);
                entity.Property(e => e.IdHonoraire).HasColumnName("id_honoraire");
                entity.Property(e => e.IdEnseignant).HasColumnName("id_enseignant").IsRequired();
                entity.Property(e => e.IdCours).HasColumnName("id_cours").IsRequired();
                entity.Property(e => e.Mois).HasColumnName("mois").IsRequired();
                entity.Property(e => e.Annee).HasColumnName("annee").IsRequired();
                entity.Property(e => e.HeuresEffectuees).HasColumnName("heures_effectuees").IsRequired();
                entity.Property(e => e.NombreHeures).HasColumnName("nombre_heures").HasColumnType("decimal(5,2)");
                entity.Property(e => e.TauxHoraire).HasColumnName("taux_horaire").HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.MontantBase).HasColumnName("montant_base").HasColumnType("decimal(10,2)");
                entity.Property(e => e.Primes).HasColumnName("primes").HasColumnType("decimal(10,2)");
                entity.Property(e => e.Retenues).HasColumnName("retenues").HasColumnType("decimal(10,2)");
                entity.Property(e => e.MontantTotal).HasColumnName("montant_total").HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.StatutPaiement).HasColumnName("statut_paiement").HasMaxLength(20).HasDefaultValue("En attente");
                entity.Property(e => e.DateCreation).HasColumnName("date_creation");

                entity.HasIndex(e => new { e.IdEnseignant, e.Mois, e.Annee });

                // Relations
                entity.HasOne(d => d.Enseignant)
                    .WithMany(p => p.HonoraireEnseignants)
                    .HasForeignKey(d => d.IdEnseignant)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Cours)
                    .WithMany(p => p.HonoraireEnseignants)
                    .HasForeignKey(d => d.IdCours)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
