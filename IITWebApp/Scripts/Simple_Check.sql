-- ================================================================
-- VÉRIFICATION SIMPLE DE LA STRUCTURE
-- Script avec les vrais noms de tables (minuscules)
-- ================================================================

USE iit_university_db;

-- Vérification des tables existantes
SELECT 'TABLES EXISTANTES:' as INFO;
SHOW TABLES;

-- Compte des données dans chaque table
SELECT 'NOMBRE DE DONNÉES PAR TABLE:' as INFO;

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

-- Structure de quelques tables importantes
SELECT 'STRUCTURE TABLE: filiere' as INFO;
DESCRIBE filiere;

SELECT 'STRUCTURE TABLE: classe' as INFO;
DESCRIBE classe;

SELECT 'STRUCTURE TABLE: etudiant' as INFO;
DESCRIBE etudiant;

SELECT 'STRUCTURE TABLE: inscription' as INFO;
DESCRIBE inscription;

SELECT 'TERMINÉ' as RESULTAT;
