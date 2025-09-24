-- ================================================================
-- RESET COMPLET ET RECONSTRUCTION PROPRE - PHASE 1 & 2
-- Script de vérification, suppression et reconstruction
-- ================================================================

USE iit_university_db;

-- 1. VÉRIFICATION DE L'ÉTAT ACTUEL
SELECT '=== ÉTAT ACTUEL DES TABLES ===' as INFO;

SELECT 'Tables existantes:' as INFO;
SHOW TABLES;

SELECT 'Nombre d''étudiants:' as INFO, COUNT(*) as count FROM etudiant;
SELECT 'Nombre de classes:' as INFO, COUNT(*) as count FROM classe;
SELECT 'Nombre de filières:' as INFO, COUNT(*) as count FROM filiere;
SELECT 'Nombre d''inscriptions:' as INFO, COUNT(*) as count FROM inscription;
SELECT 'Nombre de paiements:' as INFO, COUNT(*) as count FROM paiement;
SELECT 'Nombre de services:' as INFO, COUNT(*) as count FROM service;

-- Vérifier structure des tables principales
SELECT 'Structure table etudiant:' as INFO;
DESCRIBE etudiant;

SELECT 'Structure table classe:' as INFO;
DESCRIBE classe;

-- ================================================================
-- 2. SUPPRESSION DE TOUTES LES DONNÉES (en respectant les FK)
-- ================================================================

SELECT '=== SUPPRESSION DES DONNÉES ===' as INFO;

-- Désactiver temporairement les vérifications de clés étrangères
SET FOREIGN_KEY_CHECKS = 0;

-- Vider toutes les tables dans l'ordre inverse des dépendances
TRUNCATE TABLE souscription_service;
TRUNCATE TABLE delegue;
TRUNCATE TABLE paiement;
TRUNCATE TABLE inscription;
TRUNCATE TABLE scolarite;
TRUNCATE TABLE service;
TRUNCATE TABLE etudiant;
TRUNCATE TABLE classe;
TRUNCATE TABLE filiere;

-- Réactiver les vérifications de clés étrangères
SET FOREIGN_KEY_CHECKS = 1;

SELECT 'Données supprimées - Vérification:' as INFO;
SELECT 'Étudiants:' as table_name, COUNT(*) as count FROM etudiant
UNION ALL
SELECT 'Classes:', COUNT(*) FROM classe
UNION ALL
SELECT 'Filières:', COUNT(*) FROM filiere
UNION ALL
SELECT 'Inscriptions:', COUNT(*) FROM inscription
UNION ALL
SELECT 'Paiements:', COUNT(*) FROM paiement;

-- ================================================================
-- 3. AJOUT DES COLONNES MANQUANTES (Phase 1 + Phase 2)
-- ================================================================

SELECT '=== AJOUT DES COLONNES MANQUANTES ===' as INFO;

-- Vérifier et ajouter id_classe dans etudiant si nécessaire
SELECT 'Ajout id_classe dans etudiant...' as INFO;
SET @column_exists = (
    SELECT COUNT(*) 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = 'iit_university_db' 
    AND TABLE_NAME = 'etudiant' 
    AND COLUMN_NAME = 'id_classe'
);

SET @sql = IF(@column_exists = 0,
    'ALTER TABLE etudiant ADD COLUMN id_classe INT NULL AFTER prenom',
    'SELECT "Colonne id_classe existe déjà" as message'
);
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Vérifier et ajouter id_filiere dans etudiant si nécessaire
SELECT 'Ajout id_filiere dans etudiant...' as INFO;
SET @column_exists = (
    SELECT COUNT(*) 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = 'iit_university_db' 
    AND TABLE_NAME = 'etudiant' 
    AND COLUMN_NAME = 'id_filiere'
);

SET @sql = IF(@column_exists = 0,
    'ALTER TABLE etudiant ADD COLUMN id_filiere INT NULL AFTER id_classe',
    'SELECT "Colonne id_filiere existe déjà" as message'
);
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Ajouter niveau dans etudiant si nécessaire
SELECT 'Ajout niveau dans etudiant...' as INFO;
SET @column_exists = (
    SELECT COUNT(*) 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = 'iit_university_db' 
    AND TABLE_NAME = 'etudiant' 
    AND COLUMN_NAME = 'niveau'
);

SET @sql = IF(@column_exists = 0,
    'ALTER TABLE etudiant ADD COLUMN niveau VARCHAR(20) DEFAULT "L1" AFTER id_filiere',
    'SELECT "Colonne niveau existe déjà" as message'
);
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- ================================================================
-- 4. CRÉATION DES TABLES PHASE 2 SI NÉCESSAIRES
-- ================================================================

SELECT '=== CRÉATION DES TABLES PHASE 2 ===' as INFO;

-- Table ENSEIGNANT
CREATE TABLE IF NOT EXISTS enseignant (
    id_enseignant INT PRIMARY KEY AUTO_INCREMENT,
    matricule VARCHAR(20) UNIQUE NOT NULL,
    nom VARCHAR(50) NOT NULL,
    prenom VARCHAR(50) NOT NULL,
    date_naissance DATE,
    sexe ENUM('M', 'F') NOT NULL,
    adresse TEXT,
    telephone VARCHAR(20),
    email VARCHAR(100) UNIQUE,
    specialite VARCHAR(100),
    statut ENUM('permanent', 'contractuel', 'vacataire') DEFAULT 'contractuel',
    date_embauche DATE,
    salaire_base DECIMAL(10,2),
    date_creation TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_enseignant_matricule (matricule),
    INDEX idx_enseignant_nom (nom, prenom)
);

-- Table COURS
CREATE TABLE IF NOT EXISTS cours (
    id_cours INT PRIMARY KEY AUTO_INCREMENT,
    nom_cours VARCHAR(100) NOT NULL,
    code_cours VARCHAR(20) UNIQUE NOT NULL,
    description TEXT,
    id_filiere INT NOT NULL,
    niveau VARCHAR(20) NOT NULL,
    semestre INT NOT NULL,
    coefficient INT DEFAULT 1,
    heures_total INT NOT NULL,
    statut ENUM('actif', 'inactif') DEFAULT 'actif',
    FOREIGN KEY (id_filiere) REFERENCES filiere(id_filiere) ON DELETE CASCADE,
    INDEX idx_cours_code (code_cours),
    INDEX idx_cours_filiere (id_filiere),
    INDEX idx_cours_niveau (niveau)
);

-- Table ABSENCE
CREATE TABLE IF NOT EXISTS absence (
    id_absence INT PRIMARY KEY AUTO_INCREMENT,
    id_etudiant INT NOT NULL,
    id_cours INT NOT NULL,
    date_absence DATE NOT NULL,
    heure_debut TIME,
    heure_fin TIME,
    type_absence ENUM('justifiée', 'non_justifiée') DEFAULT 'non_justifiée',
    motif TEXT,
    statut ENUM('validée', 'en_attente', 'refusée') DEFAULT 'en_attente',
    date_creation TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_etudiant) REFERENCES etudiant(id_etudiant) ON DELETE CASCADE,
    FOREIGN KEY (id_cours) REFERENCES cours(id_cours) ON DELETE CASCADE,
    INDEX idx_absence_etudiant (id_etudiant),
    INDEX idx_absence_cours (id_cours),
    INDEX idx_absence_date (date_absence)
);

-- Table HONORAIRE_ENSEIGNANT
CREATE TABLE IF NOT EXISTS honoraire_enseignant (
    id_honoraire INT PRIMARY KEY AUTO_INCREMENT,
    id_enseignant INT NOT NULL,
    id_cours INT NOT NULL,
    mois INT NOT NULL,
    annee INT NOT NULL,
    heures_effectuees INT NOT NULL,
    taux_horaire DECIMAL(8,2) NOT NULL,
    montant_total DECIMAL(10,2) NOT NULL,
    statut ENUM('calculé', 'validé', 'payé') DEFAULT 'calculé',
    date_creation TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_enseignant) REFERENCES enseignant(id_enseignant) ON DELETE CASCADE,
    FOREIGN KEY (id_cours) REFERENCES cours(id_cours) ON DELETE CASCADE,
    INDEX idx_honoraire_enseignant (id_enseignant),
    INDEX idx_honoraire_mois (mois, annee)
);

SELECT 'Tables Phase 2 créées avec succès!' as INFO;

-- ================================================================
-- 5. VÉRIFICATION FINALE DE LA STRUCTURE
-- ================================================================

SELECT '=== VÉRIFICATION FINALE ===' as INFO;

SELECT 'Structure finale table etudiant:' as INFO;
DESCRIBE etudiant;

SELECT 'Toutes les tables disponibles:' as INFO;
SHOW TABLES;

SELECT 'RESET TERMINÉ - PRÊT POUR LES DONNÉES PROPRES!' as MESSAGE;
