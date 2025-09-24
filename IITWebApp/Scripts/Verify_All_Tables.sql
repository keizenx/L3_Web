-- VÉRIFICATION COMPLÈTE DES AUTRES TABLES
USE iit_university_db;

-- ================================================================
-- TABLES PHASE 1 RESTANTES
-- ================================================================

SELECT 'STRUCTURE TABLE: inscription' as INFO;
DESCRIBE inscription;

SELECT 'STRUCTURE TABLE: service' as INFO;
DESCRIBE service;

SELECT 'STRUCTURE TABLE: souscription_service' as INFO;
DESCRIBE souscription_service;

SELECT 'STRUCTURE TABLE: paiement' as INFO;
DESCRIBE paiement;

SELECT 'STRUCTURE TABLE: scolarite' as INFO;
DESCRIBE scolarite;

-- ================================================================
-- TABLES PHASE 2 COMPLÈTES
-- ================================================================

SELECT 'STRUCTURE TABLE: enseignant' as INFO;
DESCRIBE enseignant;

SELECT 'STRUCTURE TABLE: cours' as INFO;
DESCRIBE cours;

SELECT 'STRUCTURE TABLE: absence' as INFO;
DESCRIBE absence;

SELECT 'STRUCTURE TABLE: honoraire_enseignant' as INFO;
DESCRIBE honoraire_enseignant;

SELECT 'STRUCTURE TABLE: delegue' as INFO;
DESCRIBE delegue;

-- ================================================================
-- CONTRAINTES ET RELATIONS DE TOUTES LES TABLES
-- ================================================================

SELECT 'TOUTES LES CONTRAINTES FOREIGN KEY:' as INFO;
SELECT 
    TABLE_NAME as 'Table',
    COLUMN_NAME as 'Colonne',
    CONSTRAINT_NAME as 'Contrainte',
    REFERENCED_TABLE_NAME as 'Table_Référencée',
    REFERENCED_COLUMN_NAME as 'Colonne_Référencée'
FROM 
    information_schema.KEY_COLUMN_USAGE 
WHERE 
    REFERENCED_TABLE_SCHEMA = 'iit_university_db'
    AND REFERENCED_TABLE_NAME IS NOT NULL
ORDER BY TABLE_NAME, COLUMN_NAME;

SELECT 'VÉRIFICATION TERMINÉE' as RESULTAT;
