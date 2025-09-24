-- Requêtes SQL à exécuter dans phpMyAdmin de WampServer
-- Base de données: iit_university_db

-- 1. Ajouter la colonne niveau_experience à la table enseignant
ALTER TABLE `enseignant` ADD `niveau_experience` VARCHAR(20) NULL;

-- 2. Ajouter la colonne annee_academique à la table cours
ALTER TABLE `cours` ADD `annee_academique` VARCHAR(9) NULL;

-- 3. Ajouter les colonnes manquantes à la table absence
ALTER TABLE `absence` ADD `duree_heures` DECIMAL(4,2) NULL;
ALTER TABLE `absence` ADD `seance_absence` VARCHAR(50) NULL;

-- 4. Ajouter les colonnes manquantes à la table honoraire_enseignant
ALTER TABLE `honoraire_enseignant` ADD `montant_base` DECIMAL(10,2) NULL;
ALTER TABLE `honoraire_enseignant` ADD `primes` DECIMAL(10,2) NULL;
ALTER TABLE `honoraire_enseignant` ADD `retenues` DECIMAL(10,2) NULL;
ALTER TABLE `honoraire_enseignant` ADD `statut_paiement` VARCHAR(20) DEFAULT 'en_attente';

-- 5. Vérifier l'ajout des colonnes (optionnel)
SHOW COLUMNS FROM `enseignant`;
SHOW COLUMNS FROM `cours`;
SHOW COLUMNS FROM `absence`;
SHOW COLUMNS FROM `honoraire_enseignant`;
