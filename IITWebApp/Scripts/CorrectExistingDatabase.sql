-- ================================================================
-- CORRECTION DE LA BASE DE DONNÉES EXISTANTE
-- Alignement avec les modèles C# de l'application
-- ================================================================

USE iit_university_db;

-- 1. CORRIGER LA TABLE DELEGUE (problèmes avec les ENUM)
ALTER TABLE delegue MODIFY COLUMN fonction VARCHAR(20) NOT NULL;
ALTER TABLE delegue MODIFY COLUMN statut VARCHAR(20) DEFAULT 'actif';

-- 2. VÉRIFIER ET AJOUTER LA COLONNE type_paiement SI NÉCESSAIRE
-- (Version compatible WampServer/MySQL)
SET @col_exists = (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS 
                   WHERE TABLE_SCHEMA = 'iit_university_db' 
                   AND TABLE_NAME = 'paiement' 
                   AND COLUMN_NAME = 'type_paiement');

SET @sql = IF(@col_exists = 0, 
              'ALTER TABLE paiement ADD COLUMN type_paiement VARCHAR(20) DEFAULT "scolarite"', 
              'SELECT "Colonne type_paiement existe déjà" as MESSAGE');

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- 3. CORRIGER LA TABLE SOUSCRIPTION_SERVICE (colonne statut)
ALTER TABLE souscription_service MODIFY COLUMN statut VARCHAR(20) DEFAULT 'active';

-- 4. VÉRIFIER ET AJOUTER LA COLONNE niveau SI NÉCESSAIRE
SET @col_exists_niveau = (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS 
                          WHERE TABLE_SCHEMA = 'iit_university_db' 
                          AND TABLE_NAME = 'etudiant' 
                          AND COLUMN_NAME = 'niveau');

SET @sql_niveau = IF(@col_exists_niveau = 0, 
                     'ALTER TABLE etudiant ADD COLUMN niveau VARCHAR(2) NULL', 
                     'SELECT "Colonne niveau existe déjà" as MESSAGE');

PREPARE stmt_niveau FROM @sql_niveau;
EXECUTE stmt_niveau;
DEALLOCATE PREPARE stmt_niveau;

-- 5. METTRE À JOUR LES DONNÉES EXISTANTES
-- Convertir les ENUM en VARCHAR pour les délégués
UPDATE delegue SET fonction = 'delegue' WHERE fonction = 'délégué';
UPDATE delegue SET statut = 'actif' WHERE statut IS NULL OR statut = '';

-- 6. CORRIGER LES TYPES DE PAIEMENT (seulement si la colonne existe)
UPDATE paiement SET type_paiement = 'scolarite' WHERE type_paiement IS NULL OR type_paiement = 'scolarité' OR type_paiement = '';

-- 7. STRUCTURE DÉJÀ AJOUTÉE CI-DESSUS

-- Extraire le niveau des matricules existants et les mettre à jour
UPDATE etudiant 
SET niveau = CASE 
    WHEN matricule LIKE '%L1%' THEN 'L1'
    WHEN matricule LIKE '%L2%' THEN 'L2'
    WHEN matricule LIKE '%L3%' THEN 'L3'
    ELSE 'L1'
END
WHERE niveau IS NULL;

-- 8. VÉRIFICATIONS FINALES
SELECT 'VERIFICATION FINALE' as STATUS;

-- Vérifier les délégués
SELECT 'DELEGUES', COUNT(*) as total, 
       SUM(CASE WHEN fonction IN ('delegue', 'adjoint') THEN 1 ELSE 0 END) as valid_fonction,
       SUM(CASE WHEN statut IN ('actif', 'inactif') THEN 1 ELSE 0 END) as valid_statut
FROM delegue;

-- Vérifier les paiements
SELECT 'PAIEMENTS', COUNT(*) as total,
       SUM(CASE WHEN type_paiement IN ('scolarite', 'service', 'autre') THEN 1 ELSE 0 END) as valid_type
FROM paiement;

-- Vérifier les étudiants
SELECT 'ETUDIANTS', COUNT(*) as total,
       SUM(CASE WHEN niveau IN ('L1', 'L2', 'L3', 'M1', 'M2') THEN 1 ELSE 0 END) as valid_niveau
FROM etudiant;