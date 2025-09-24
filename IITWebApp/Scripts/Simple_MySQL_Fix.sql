-- ================================================================
-- SCRIPT SIMPLE POUR WAMPSERVER MYSQL
-- Correction rapide des incompatibilités
-- ================================================================

USE iit_university_db;

-- 1. CORRIGER LES ENUM EN VARCHAR
ALTER TABLE delegue MODIFY COLUMN fonction VARCHAR(20) NOT NULL;
ALTER TABLE delegue MODIFY COLUMN statut VARCHAR(20) DEFAULT 'actif';

-- 2. AJOUTER LA COLONNE niveau AUX ÉTUDIANTS (ignorer l'erreur si elle existe déjà)
ALTER TABLE etudiant ADD COLUMN niveau VARCHAR(2) NULL;

-- 3. AJOUTER LA COLONNE type_paiement (ignorer l'erreur si elle existe déjà)  
ALTER TABLE paiement ADD COLUMN type_paiement VARCHAR(20) DEFAULT 'scolarite';

-- 4. NETTOYER LES DONNÉES
UPDATE delegue SET fonction = 'delegue' WHERE fonction = 'délégué';
UPDATE delegue SET statut = 'actif' WHERE statut IS NULL OR statut = '';

-- 5. EXTRAIRE LES NIVEAUX DES MATRICULES EXISTANTS
UPDATE etudiant 
SET niveau = CASE 
    WHEN matricule LIKE '%L1%' THEN 'L1'
    WHEN matricule LIKE '%L2%' THEN 'L2'
    WHEN matricule LIKE '%L3%' THEN 'L3'
    WHEN matricule LIKE '%M1%' THEN 'M1'
    WHEN matricule LIKE '%M2%' THEN 'M2'
    ELSE 'L1'
END
WHERE niveau IS NULL;

-- 6. CORRIGER LES TYPES DE PAIEMENT
UPDATE paiement SET type_paiement = 'scolarite' 
WHERE type_paiement IS NULL OR type_paiement = '';

-- 7. VÉRIFICATIONS FINALES
SELECT 'VERIFICATION TERMINÉE - RÉSULTATS :' as STATUS;

SELECT 'ETUDIANTS' as TABLE_NAME, COUNT(*) as TOTAL,
       SUM(CASE WHEN niveau IN ('L1', 'L2', 'L3', 'M1', 'M2') THEN 1 ELSE 0 END) as NIVEAUX_OK
FROM etudiant;

SELECT 'DELEGUES' as TABLE_NAME, COUNT(*) as TOTAL,
       SUM(CASE WHEN fonction IN ('delegue', 'adjoint') THEN 1 ELSE 0 END) as FONCTIONS_OK
FROM delegue;

SELECT 'PAIEMENTS' as TABLE_NAME, COUNT(*) as TOTAL,
       SUM(CASE WHEN type_paiement IS NOT NULL THEN 1 ELSE 0 END) as TYPES_OK
FROM paiement;

-- AFFICHER LA STRUCTURE DES TABLES IMPORTANTES
SHOW COLUMNS FROM etudiant;
SHOW COLUMNS FROM delegue;
SHOW COLUMNS FROM paiement;
