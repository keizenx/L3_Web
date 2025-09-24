    -- ================================================================
    -- VÉRIFICATION DE LA STRUCTURE EXISTANTE
    -- Script pour analyser les tables Phase 1 et Phase 2
    -- ================================================================

    USE iit_university_db;

    -- ================================================================
    -- SECTION 1: VÉRIFICATION DES TABLES EXISTANTES
    -- ================================================================

    SELECT 'TABLES EXISTANTES DANS LA BASE:' as INFO;
    SHOW TABLES;

    -- ================================================================
    -- SECTION 2: STRUCTURE DES TABLES PHASE 1
    -- ================================================================

    SELECT '==================== PHASE 1 ====================' as INFO;

SELECT 'STRUCTURE TABLE: FILIERE' as INFO;
DESCRIBE FILIERE;

SELECT 'STRUCTURE TABLE: CLASSE' as INFO;
DESCRIBE CLASSE;

SELECT 'STRUCTURE TABLE: ETUDIANT' as INFO;
DESCRIBE ETUDIANT;

SELECT 'STRUCTURE TABLE: INSCRIPTION' as INFO;
DESCRIBE INSCRIPTION;

SELECT 'STRUCTURE TABLE: SERVICE' as INFO;
DESCRIBE SERVICE;

SELECT 'STRUCTURE TABLE: SOUSCRIPTION_SERVICE' as INFO;
DESCRIBE SOUSCRIPTION_SERVICE;

SELECT 'STRUCTURE TABLE: PAIEMENT' as INFO;
DESCRIBE PAIEMENT;

SELECT 'STRUCTURE TABLE: SCOLARITE' as INFO;
DESCRIBE SCOLARITE;    -- ================================================================
    -- SECTION 3: STRUCTURE DES TABLES PHASE 2
    -- ================================================================

    SELECT '==================== PHASE 2 ====================' as INFO;

SELECT 'STRUCTURE TABLE: ENSEIGNANT' as INFO;
DESCRIBE ENSEIGNANT;

SELECT 'STRUCTURE TABLE: COURS' as INFO;
DESCRIBE COURS;

SELECT 'STRUCTURE TABLE: ABSENCE' as INFO;
DESCRIBE ABSENCE;

SELECT 'STRUCTURE TABLE: HONORAIRE_ENSEIGNANT' as INFO;
DESCRIBE HONORAIRE_ENSEIGNANT;

SELECT 'STRUCTURE TABLE: DELEGUE' as INFO;
DESCRIBE DELEGUE;    -- ================================================================
    -- SECTION 4: CONTRAINTES ET RELATIONS
    -- ================================================================

    SELECT 'CONTRAINTES FOREIGN KEY:' as INFO;
    SELECT 
        TABLE_NAME,
        COLUMN_NAME,
        CONSTRAINT_NAME,
        REFERENCED_TABLE_NAME,
        REFERENCED_COLUMN_NAME
    FROM 
        information_schema.KEY_COLUMN_USAGE 
    WHERE 
        REFERENCED_TABLE_SCHEMA = 'iit_university_db'
        AND REFERENCED_TABLE_NAME IS NOT NULL
    ORDER BY TABLE_NAME, COLUMN_NAME;

    -- ================================================================
    -- SECTION 5: INDEX ET CLÉS
    -- ================================================================

    SELECT 'INDEX ET CLÉS:' as INFO;
    SELECT 
        TABLE_NAME,
        COLUMN_NAME,
        INDEX_NAME,
        NON_UNIQUE,
        INDEX_TYPE
    FROM 
        information_schema.STATISTICS 
    WHERE 
        TABLE_SCHEMA = 'iit_university_db'
    ORDER BY TABLE_NAME, SEQ_IN_INDEX;

    -- ================================================================
    -- SECTION 6: DONNÉES EXISTANTES (COMPTE)
    -- ================================================================

    SELECT 'NOMBRE DE DONNÉES PAR TABLE:' as INFO;

-- Vérifions d'abord si les tables existent avec différentes casses
SELECT 'Vérification existence tables:' as INFO;
SHOW TABLES LIKE '%filiere%';
SHOW TABLES LIKE '%FILIERE%';

-- Comptons avec les noms corrects de tables (probablement en majuscules)
SELECT 'FILIERE' as table_name, COUNT(*) as count FROM FILIERE
UNION ALL
SELECT 'CLASSE', COUNT(*) FROM CLASSE
UNION ALL
SELECT 'ETUDIANT', COUNT(*) FROM ETUDIANT
UNION ALL
SELECT 'INSCRIPTION', COUNT(*) FROM INSCRIPTION
UNION ALL
SELECT 'SERVICE', COUNT(*) FROM SERVICE
UNION ALL
SELECT 'SOUSCRIPTION_SERVICE', COUNT(*) FROM SOUSCRIPTION_SERVICE
UNION ALL
SELECT 'PAIEMENT', COUNT(*) FROM PAIEMENT
UNION ALL
SELECT 'SCOLARITE', COUNT(*) FROM SCOLARITE
UNION ALL
SELECT 'ENSEIGNANT', COUNT(*) FROM ENSEIGNANT
UNION ALL
SELECT 'COURS', COUNT(*) FROM COURS
UNION ALL
SELECT 'ABSENCE', COUNT(*) FROM ABSENCE
UNION ALL
SELECT 'HONORAIRE_ENSEIGNANT', COUNT(*) FROM HONORAIRE_ENSEIGNANT
UNION ALL
SELECT 'DELEGUE', COUNT(*) FROM DELEGUE;    SELECT 'VÉRIFICATION TERMINÉE - PRÊT POUR ADAPTATION C#' as RESULTAT;
