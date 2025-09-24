-- ================================================================
-- ÉTAPE 2: SUPPRESSION DE TOUTES LES DONNÉES
-- Copier et exécuter cette section dans phpMyAdmin
-- ================================================================

USE iit_university_db;

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
SELECT 'Filières:', COUNT(*) FROM filiere;
