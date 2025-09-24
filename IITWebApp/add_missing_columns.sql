-- Script pour ajouter les colonnes manquantes dans la base de données
-- Exécuter ces commandes une par une dans MySQL

-- Ajouter la colonne niveau_experience à la table enseignant
ALTER TABLE enseignant ADD COLUMN niveau_experience VARCHAR(20);

-- Ajouter la colonne annee_academique à la table cours
ALTER TABLE cours ADD COLUMN annee_academique VARCHAR(9);

-- Ajouter la colonne duree_heures à la table absence
ALTER TABLE absence ADD COLUMN duree_heures DECIMAL(4,2);

-- Ajouter la colonne seance_absence à la table absence
ALTER TABLE absence ADD COLUMN seance_absence VARCHAR(50);

-- Ajouter les colonnes manquantes à la table honoraire_enseignant
ALTER TABLE honoraire_enseignant ADD COLUMN montant_base DECIMAL(10,2);
ALTER TABLE honoraire_enseignant ADD COLUMN primes DECIMAL(10,2);
ALTER TABLE honoraire_enseignant ADD COLUMN retenues DECIMAL(10,2);
ALTER TABLE honoraire_enseignant ADD COLUMN statut_paiement VARCHAR(20) DEFAULT 'en_attente';

-- Vérifier l'ajout des colonnes
DESCRIBE enseignant;
DESCRIBE cours;
DESCRIBE absence;
DESCRIBE honoraire_enseignant;
