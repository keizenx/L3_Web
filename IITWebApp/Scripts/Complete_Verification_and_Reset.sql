-- ================================================================
-- SCRIPT COMPLET : VÉRIFICATION ET RESET PHASE 1 + PHASE 2
-- IIT University Management System
-- ================================================================

USE iit_university_db;

-- ================================================================
-- ÉTAPE 1: VÉRIFICATION DE LA STRUCTURE ACTUELLE
-- ================================================================

SELECT '=== VÉRIFICATION DES TABLES EXISTANTES ===' as INFO;
SHOW TABLES;

-- Vérification structure Phase 1
SELECT '=== STRUCTURE TABLE FILIERE ===' as INFO;
DESCRIBE filiere;

SELECT '=== STRUCTURE TABLE CLASSE ===' as INFO;
DESCRIBE classe;

SELECT '=== STRUCTURE TABLE ETUDIANT ===' as INFO;
DESCRIBE etudiant;

SELECT '=== STRUCTURE TABLE INSCRIPTION ===' as INFO;
DESCRIBE inscription;

SELECT '=== STRUCTURE TABLE PAIEMENT ===' as INFO;
DESCRIBE paiement;

SELECT '=== STRUCTURE TABLE SERVICE ===' as INFO;
DESCRIBE service;

SELECT '=== STRUCTURE TABLE SOUSCRIPTION_SERVICE ===' as INFO;
DESCRIBE souscription_service;

SELECT '=== STRUCTURE TABLE SCOLARITE ===' as INFO;
DESCRIBE scolarite;

-- Vérification structure Phase 2
SELECT '=== STRUCTURE TABLE ENSEIGNANT ===' as INFO;
DESCRIBE enseignant;

SELECT '=== STRUCTURE TABLE COURS ===' as INFO;
DESCRIBE cours;

SELECT '=== STRUCTURE TABLE ABSENCE ===' as INFO;
DESCRIBE absence;

SELECT '=== STRUCTURE TABLE HONORAIRE_ENSEIGNANT ===' as INFO;
DESCRIBE honoraire_enseignant;

SELECT '=== STRUCTURE TABLE DELEGUE ===' as INFO;
DESCRIBE delegue;

-- Vérification des données existantes
SELECT '=== COMPTAGE DES DONNÉES ACTUELLES ===' as INFO;
SELECT 'filiere' as table_name, COUNT(*) as count FROM filiere
UNION ALL
SELECT 'classe' as table_name, COUNT(*) as count FROM classe
UNION ALL
SELECT 'etudiant' as table_name, COUNT(*) as count FROM etudiant
UNION ALL
SELECT 'inscription' as table_name, COUNT(*) as count FROM inscription
UNION ALL
SELECT 'paiement' as table_name, COUNT(*) as count FROM paiement
UNION ALL
SELECT 'service' as table_name, COUNT(*) as count FROM service
UNION ALL
SELECT 'souscription_service' as table_name, COUNT(*) as count FROM souscription_service
UNION ALL
SELECT 'scolarite' as table_name, COUNT(*) as count FROM scolarite
UNION ALL
SELECT 'enseignant' as table_name, COUNT(*) as count FROM enseignant
UNION ALL
SELECT 'cours' as table_name, COUNT(*) as count FROM cours
UNION ALL
SELECT 'absence' as table_name, COUNT(*) as count FROM absence
UNION ALL
SELECT 'honoraire_enseignant' as table_name, COUNT(*) as count FROM honoraire_enseignant
UNION ALL
SELECT 'delegue' as table_name, COUNT(*) as count FROM delegue;

-- ================================================================
-- ÉTAPE 2: SUPPRESSION DE TOUTES LES DONNÉES
-- ================================================================

SELECT '=== DÉBUT SUPPRESSION DES DONNÉES ===' as INFO;

-- Désactiver les vérifications de clés étrangères
SET FOREIGN_KEY_CHECKS = 0;

-- Suppression des données dans l'ordre inverse des dépendances
TRUNCATE TABLE delegue;
TRUNCATE TABLE honoraire_enseignant;
TRUNCATE TABLE absence;
TRUNCATE TABLE cours;
TRUNCATE TABLE enseignant;
TRUNCATE TABLE scolarite;
TRUNCATE TABLE souscription_service;
TRUNCATE TABLE service;
TRUNCATE TABLE paiement;
TRUNCATE TABLE inscription;
TRUNCATE TABLE etudiant;
TRUNCATE TABLE classe;
TRUNCATE TABLE filiere;

-- Réactiver les vérifications de clés étrangères
SET FOREIGN_KEY_CHECKS = 1;

SELECT '=== DONNÉES SUPPRIMÉES ===' as INFO;

-- ================================================================
-- ÉTAPE 3: SUPPRESSION ET RECRÉATION DES TABLES
-- ================================================================

SELECT '=== DÉBUT SUPPRESSION DES TABLES ===' as INFO;

-- Désactiver les vérifications de clés étrangères
SET FOREIGN_KEY_CHECKS = 0;

-- Suppression des tables dans l'ordre inverse des dépendances
DROP TABLE IF EXISTS delegue;
DROP TABLE IF EXISTS honoraire_enseignant;
DROP TABLE IF EXISTS absence;
DROP TABLE IF EXISTS cours;
DROP TABLE IF EXISTS enseignant;
DROP TABLE IF EXISTS scolarite;
DROP TABLE IF EXISTS souscription_service;
DROP TABLE IF EXISTS service;
DROP TABLE IF EXISTS paiement;
DROP TABLE IF EXISTS inscription;
DROP TABLE IF EXISTS etudiant;
DROP TABLE IF EXISTS classe;
DROP TABLE IF EXISTS filiere;

-- Réactiver les vérifications de clés étrangères
SET FOREIGN_KEY_CHECKS = 1;

SELECT '=== TABLES SUPPRIMÉES ===' as INFO;

-- ================================================================
-- ÉTAPE 4: RECRÉATION DES TABLES PHASE 1
-- ================================================================

SELECT '=== CRÉATION DES TABLES PHASE 1 ===' as INFO;

-- Table filiere (Phase 1)
CREATE TABLE filiere (
    id_filiere INT AUTO_INCREMENT PRIMARY KEY,
    nom_filiere VARCHAR(100) NOT NULL,
    description TEXT,
    duree_etudes INT NOT NULL DEFAULT 3,
    statut VARCHAR(20) NOT NULL DEFAULT 'actif',
    date_creation TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table classe (Phase 1)
CREATE TABLE classe (
    id_classe INT AUTO_INCREMENT PRIMARY KEY,
    nom_classe VARCHAR(50) NOT NULL,
    id_filiere INT NOT NULL,
    niveau VARCHAR(20) NOT NULL,
    capacite_max INT DEFAULT 30,
    statut VARCHAR(20) NOT NULL DEFAULT 'actif',
    date_creation TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_filiere) REFERENCES filiere(id_filiere) ON DELETE CASCADE
);

-- Table etudiant (Phase 1)
CREATE TABLE etudiant (
    id_etudiant INT AUTO_INCREMENT PRIMARY KEY,
    matricule VARCHAR(20) UNIQUE NOT NULL,
    nom VARCHAR(50) NOT NULL,
    prenom VARCHAR(50) NOT NULL,
    date_naissance DATE NOT NULL,
    lieu_naissance VARCHAR(100),
    sexe VARCHAR(1) NOT NULL CHECK (sexe IN ('M', 'F')),
    nationalite VARCHAR(50) DEFAULT 'Ivoirien',
    adresse TEXT,
    telephone VARCHAR(20),
    email VARCHAR(100),
    id_classe INT,
    niveau VARCHAR(20) NOT NULL,
    statut VARCHAR(20) NOT NULL DEFAULT 'actif',
    date_creation TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_classe) REFERENCES classe(id_classe) ON DELETE SET NULL
);

-- Table inscription (Phase 1)
CREATE TABLE inscription (
    id_inscription INT AUTO_INCREMENT PRIMARY KEY,
    id_etudiant INT NOT NULL,
    id_filiere INT NOT NULL,
    id_classe INT NOT NULL,
    annee_academique VARCHAR(20) NOT NULL,
    date_inscription DATE NOT NULL,
    type_inscription VARCHAR(50) DEFAULT 'normale',
    montant_inscription DECIMAL(10,2) NOT NULL,
    statut VARCHAR(20) NOT NULL DEFAULT 'en_cours',
    FOREIGN KEY (id_etudiant) REFERENCES etudiant(id_etudiant) ON DELETE CASCADE,
    FOREIGN KEY (id_filiere) REFERENCES filiere(id_filiere) ON DELETE CASCADE,
    FOREIGN KEY (id_classe) REFERENCES classe(id_classe) ON DELETE CASCADE
);

-- Table service (Phase 1)
CREATE TABLE service (
    id_service INT AUTO_INCREMENT PRIMARY KEY,
    nom_service VARCHAR(100) NOT NULL,
    description TEXT,
    prix DECIMAL(10,2) NOT NULL,
    type_service VARCHAR(50) NOT NULL,
    statut VARCHAR(20) NOT NULL DEFAULT 'actif',
    date_creation TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table paiement (Phase 1)
CREATE TABLE paiement (
    id_paiement INT AUTO_INCREMENT PRIMARY KEY,
    id_etudiant INT NOT NULL,
    id_inscription INT,
    montant DECIMAL(10,2) NOT NULL,
    date_paiement DATE NOT NULL,
    methode_paiement VARCHAR(50) NOT NULL,
    type_paiement VARCHAR(50) NOT NULL,
    reference_paiement VARCHAR(100),
    statut VARCHAR(20) NOT NULL DEFAULT 'valide',
    FOREIGN KEY (id_etudiant) REFERENCES etudiant(id_etudiant) ON DELETE CASCADE,
    FOREIGN KEY (id_inscription) REFERENCES inscription(id_inscription) ON DELETE SET NULL
);

-- Table souscription_service (Phase 1)
CREATE TABLE souscription_service (
    id_souscription INT AUTO_INCREMENT PRIMARY KEY,
    id_etudiant INT NOT NULL,
    id_service INT NOT NULL,
    date_souscription DATE NOT NULL,
    statut VARCHAR(20) NOT NULL DEFAULT 'actif',
    FOREIGN KEY (id_etudiant) REFERENCES etudiant(id_etudiant) ON DELETE CASCADE,
    FOREIGN KEY (id_service) REFERENCES service(id_service) ON DELETE CASCADE
);

-- Table scolarite (Phase 1)
CREATE TABLE scolarite (
    id_scolarite INT AUTO_INCREMENT PRIMARY KEY,
    id_etudiant INT NOT NULL,
    annee_academique VARCHAR(20) NOT NULL,
    montant_total DECIMAL(10,2) NOT NULL,
    montant_paye DECIMAL(10,2) DEFAULT 0,
    solde DECIMAL(10,2) GENERATED ALWAYS AS (montant_total - montant_paye) STORED,
    statut VARCHAR(20) NOT NULL DEFAULT 'en_cours',
    date_creation TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_etudiant) REFERENCES etudiant(id_etudiant) ON DELETE CASCADE
);

SELECT '=== TABLES PHASE 1 CRÉÉES ===' as INFO;

-- ================================================================
-- ÉTAPE 5: RECRÉATION DES TABLES PHASE 2
-- ================================================================

SELECT '=== CRÉATION DES TABLES PHASE 2 ===' as INFO;

-- Table enseignant (Phase 2)
CREATE TABLE enseignant (
    id_enseignant INT AUTO_INCREMENT PRIMARY KEY,
    matricule VARCHAR(20) UNIQUE NOT NULL,
    nom VARCHAR(50) NOT NULL,
    prenom VARCHAR(50) NOT NULL,
    sexe VARCHAR(1) NOT NULL CHECK (sexe IN ('M', 'F')),
    date_naissance DATE,
    lieu_naissance VARCHAR(100),
    nationalite VARCHAR(50) DEFAULT 'Ivoirien',
    adresse TEXT,
    telephone VARCHAR(20),
    email VARCHAR(100),
    specialite VARCHAR(100),
    grade VARCHAR(50),
    statut VARCHAR(20) NOT NULL DEFAULT 'actif',
    date_embauche DATE,
    date_creation TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table cours (Phase 2)
CREATE TABLE cours (
    id_cours INT AUTO_INCREMENT PRIMARY KEY,
    nom_cours VARCHAR(100) NOT NULL,
    code_cours VARCHAR(20) UNIQUE NOT NULL,
    id_enseignant INT NOT NULL,
    id_classe INT NOT NULL,
    volume_horaire INT NOT NULL,
    coefficient INT NOT NULL DEFAULT 1,
    type_cours VARCHAR(50) NOT NULL,
    statut VARCHAR(20) NOT NULL DEFAULT 'actif',
    date_creation TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_enseignant) REFERENCES enseignant(id_enseignant) ON DELETE CASCADE,
    FOREIGN KEY (id_classe) REFERENCES classe(id_classe) ON DELETE CASCADE
);

-- Table absence (Phase 2)
CREATE TABLE absence (
    id_absence INT AUTO_INCREMENT PRIMARY KEY,
    id_etudiant INT NOT NULL,
    id_cours INT NOT NULL,
    date_absence DATE NOT NULL,
    heure_debut TIME,
    heure_fin TIME,
    type_absence VARCHAR(50) NOT NULL,
    justifiee BOOLEAN DEFAULT FALSE,
    motif TEXT,
    date_creation TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_etudiant) REFERENCES etudiant(id_etudiant) ON DELETE CASCADE,
    FOREIGN KEY (id_cours) REFERENCES cours(id_cours) ON DELETE CASCADE
);

-- Table honoraire_enseignant (Phase 2)
CREATE TABLE honoraire_enseignant (
    id_honoraire INT AUTO_INCREMENT PRIMARY KEY,
    id_enseignant INT NOT NULL,
    id_cours INT NOT NULL,
    mois VARCHAR(20) NOT NULL,
    annee INT NOT NULL,
    heures_effectuees DECIMAL(5,2) NOT NULL,
    taux_horaire DECIMAL(10,2) NOT NULL,
    montant_total DECIMAL(10,2) GENERATED ALWAYS AS (heures_effectuees * taux_horaire) STORED,
    statut VARCHAR(20) NOT NULL DEFAULT 'en_attente',
    date_creation TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_enseignant) REFERENCES enseignant(id_enseignant) ON DELETE CASCADE,
    FOREIGN KEY (id_cours) REFERENCES cours(id_cours) ON DELETE CASCADE
);

-- Table delegue (Phase 2)
CREATE TABLE delegue (
    id_delegue INT AUTO_INCREMENT PRIMARY KEY,
    id_etudiant INT NOT NULL,
    id_classe INT NOT NULL,
    type_delegue VARCHAR(50) NOT NULL,
    date_nomination DATE NOT NULL,
    date_fin_mandat DATE,
    statut VARCHAR(20) NOT NULL DEFAULT 'actif',
    FOREIGN KEY (id_etudiant) REFERENCES etudiant(id_etudiant) ON DELETE CASCADE,
    FOREIGN KEY (id_classe) REFERENCES classe(id_classe) ON DELETE CASCADE
);

SELECT '=== TABLES PHASE 2 CRÉÉES ===' as INFO;

-- ================================================================
-- ÉTAPE 6: VÉRIFICATION FINALE
-- ================================================================

SELECT '=== VÉRIFICATION FINALE - TABLES CRÉÉES ===' as INFO;
SHOW TABLES;

SELECT '=== SCRIPT TERMINÉ AVEC SUCCÈS ===' as MESSAGE;
SELECT 'Toutes les tables Phase 1 et Phase 2 ont été recréées proprement!' as RESULTAT;
