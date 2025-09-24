-- ================================================================
-- ANALYSE COMPLÈTE DE LA STRUCTURE EXISTANTE
-- Toutes les tables Phase 1 et Phase 2 avec détails complets
-- ================================================================

USE iit_university_db;

-- ================================================================
-- SECTION 1: STRUCTURE COMPLÈTE DES TABLES PHASE 1
-- ================================================================

SELECT '==================== PHASE 1 - DÉTAILS COMPLETS ====================' as INFO;

SELECT 'TABLE: filiere' as INFO;
DESCRIBE filiere;

SELECT 'TABLE: classe' as INFO;
DESCRIBE classe;

SELECT 'TABLE: etudiant' as INFO;
DESCRIBE etudiant;

SELECT 'TABLE: inscription' as INFO;
DESCRIBE inscription;

SELECT 'TABLE: service' as INFO;
DESCRIBE service;

SELECT 'TABLE: souscription_service' as INFO;
DESCRIBE souscription_service;

SELECT 'TABLE: paiement' as INFO;
DESCRIBE paiement;

SELECT 'TABLE: scolarite' as INFO;
DESCRIBE scolarite;

-- ================================================================
-- SECTION 2: STRUCTURE COMPLÈTE DES TABLES PHASE 2
-- ================================================================

SELECT '==================== PHASE 2 - DÉTAILS COMPLETS ====================' as INFO;

SELECT 'TABLE: enseignant' as INFO;
DESCRIBE enseignant;

SELECT 'TABLE: cours' as INFO;
DESCRIBE cours;

SELECT 'TABLE: absence' as INFO;
DESCRIBE absence;

SELECT 'TABLE: honoraire_enseignant' as INFO;
DESCRIBE honoraire_enseignant;

SELECT 'TABLE: delegue' as INFO;
DESCRIBE delegue;

-- ================================================================
-- SECTION 3: VUES (VIEWS) EXISTANTES
-- ================================================================

SELECT '==================== VUES EXISTANTES ====================' as INFO;

SELECT 'VIEW: v_delegues_actuels' as INFO;
SHOW CREATE VIEW v_delegues_actuels;

SELECT 'VIEW: v_etudiants_complets' as INFO;
SHOW CREATE VIEW v_etudiants_complets;

SELECT 'VIEW: v_paiements_detaille' as INFO;
SHOW CREATE VIEW v_paiements_detaille;

-- ================================================================
-- SECTION 4: CONTRAINTES ET RELATIONS
-- ================================================================

SELECT '==================== CONTRAINTES FOREIGN KEY ====================' as INFO;
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

-- ================================================================
-- SECTION 5: INDEX ET CLÉS
-- ================================================================

SELECT '==================== INDEX ET CLÉS ====================' as INFO;
SELECT 
    TABLE_NAME as 'Table',
    COLUMN_NAME as 'Colonne',
    INDEX_NAME as 'Index',
    CASE WHEN NON_UNIQUE = 0 THEN 'UNIQUE' ELSE 'NON_UNIQUE' END as 'Type',
    INDEX_TYPE as 'Type_Index'
FROM 
    information_schema.STATISTICS 
WHERE 
    TABLE_SCHEMA = 'iit_university_db'
    AND TABLE_NAME NOT LIKE 'v_%'  -- Exclure les vues
ORDER BY TABLE_NAME, SEQ_IN_INDEX;

-- ================================================================
-- SECTION 6: DONNÉES EXISTANTES
-- ================================================================

SELECT '==================== NOMBRE DE DONNÉES ====================' as INFO;

SELECT 'filiere' as table_name, COUNT(*) as count FROM filiere
UNION ALL
SELECT 'classe', COUNT(*) FROM classe
UNION ALL
SELECT 'etudiant', COUNT(*) FROM etudiant
UNION ALL
SELECT 'inscription', COUNT(*) FROM inscription
UNION ALL
SELECT 'service', COUNT(*) FROM service
UNION ALL
SELECT 'souscription_service', COUNT(*) FROM souscription_service
UNION ALL
SELECT 'paiement', COUNT(*) FROM paiement
UNION ALL
SELECT 'scolarite', COUNT(*) FROM scolarite
UNION ALL
SELECT 'enseignant', COUNT(*) FROM enseignant
UNION ALL
SELECT 'cours', COUNT(*) FROM cours
UNION ALL
SELECT 'absence', COUNT(*) FROM absence
UNION ALL
SELECT 'honoraire_enseignant', COUNT(*) FROM honoraire_enseignant
UNION ALL
SELECT 'delegue', COUNT(*) FROM delegue;

-- ================================================================
-- SECTION 7: EXEMPLES DE DONNÉES (5 premiers enregistrements)
-- ================================================================

SELECT '==================== EXEMPLES DE DONNÉES ====================' as INFO;

SELECT 'FILIÈRES (5 premières):' as INFO;
SELECT * FROM filiere LIMIT 5;

SELECT 'CLASSES (5 premières):' as INFO;
SELECT * FROM classe LIMIT 5;

SELECT 'ÉTUDIANTS (5 premiers):' as INFO;
SELECT * FROM etudiant LIMIT 5;

SELECT 'INSCRIPTIONS (5 premières):' as INFO;
SELECT * FROM inscription LIMIT 5;

SELECT 'SERVICES (5 premiers):' as INFO;
SELECT * FROM service LIMIT 5;

SELECT 'ENSEIGNANTS (5 premiers):' as INFO;
SELECT * FROM enseignant LIMIT 5;

SELECT 'COURS (5 premiers):' as INFO;
SELECT * FROM cours LIMIT 5;

SELECT '==================== ANALYSE TERMINÉE ====================' as RESULTAT;
SELECT 'STRUCTURE COMPLÈTE OBTENUE - PRÊT POUR ADAPTATION C#' as MESSAGE;
