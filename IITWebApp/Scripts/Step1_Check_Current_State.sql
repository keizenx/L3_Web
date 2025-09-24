-- ================================================================
-- ÉTAPE 1: VÉRIFICATION DE L'ÉTAT ACTUEL
-- Copier et exécuter cette section dans phpMyAdmin
-- ================================================================

USE iit_university_db;

SELECT '=== ÉTAT ACTUEL DES TABLES ===' as INFO;

SELECT 'Nombre d''étudiants:' as INFO, COUNT(*) as count FROM etudiant;
SELECT 'Nombre de classes:' as INFO, COUNT(*) as count FROM classe;
SELECT 'Nombre de filières:' as INFO, COUNT(*) as count FROM filiere;

-- Vérifier structure des tables principales
DESCRIBE etudiant;
