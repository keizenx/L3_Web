-- ================================================================
-- VÉRIFICATION STRUCTURE DES TABLES EXISTANTES
-- Script pour voir les colonnes réelles de chaque table
-- ================================================================

USE iit_university_db;

-- Voir la structure de toutes les tables importantes
SHOW COLUMNS FROM filiere;
SHOW COLUMNS FROM classe; 
SHOW COLUMNS FROM etudiant;
SHOW COLUMNS FROM service;
SHOW COLUMNS FROM scolarite;
SHOW COLUMNS FROM enseignant;
SHOW COLUMNS FROM cours;
SHOW COLUMNS FROM inscription;
SHOW COLUMNS FROM paiement;
SHOW COLUMNS FROM absence;
SHOW COLUMNS FROM delegue;
SHOW COLUMNS FROM souscription_service;
SHOW COLUMNS FROM honoraire_enseignant;
