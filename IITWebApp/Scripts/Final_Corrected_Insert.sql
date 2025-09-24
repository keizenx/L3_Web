-- ================================================================
-- SCRIPT FINAL CORRIGÉ - STRUCTURE RÉELLE
-- Adapté aux vraies colonnes de votre base de données
-- ================================================================

USE iit_university_db;

-- 1. FILIÈRES (colonnes réelles: nom_filiere, description, duree_etudes, statut)
INSERT INTO filiere (nom_filiere, description, duree_etudes, statut) VALUES
('Informatique et Réseaux', 'Formation en développement logiciel et administration réseaux', 3, 'active'),
('Génie Logiciel', 'Formation spécialisée en conception et développement de logiciels', 3, 'active'),
('Système d''Information', 'Formation en gestion et analyse des systèmes d''information', 3, 'active'),
('Intelligence Artificielle', 'Formation en IA, Machine Learning et Data Science', 3, 'active'),
('Cybersécurité', 'Formation en sécurité informatique et protection des données', 3, 'active')
ON DUPLICATE KEY UPDATE nom_filiere = VALUES(nom_filiere);

-- 2. CLASSES (colonnes réelles: nom_classe, niveau, section, annee_academique, effectif_max, statut)
INSERT INTO classe (nom_classe, niveau, section, annee_academique, effectif_max, statut) VALUES
-- Licence 1
('L1-INFO-A', 'L1', 'A', '2024-2025', 35, 'ouverte'),
('L1-INFO-B', 'L1', 'B', '2024-2025', 35, 'ouverte'),
('L1-GL-A', 'L1', 'A', '2024-2025', 30, 'ouverte'),
('L1-SI-A', 'L1', 'A', '2024-2025', 30, 'ouverte'),

-- Licence 2  
('L2-INFO-A', 'L2', 'A', '2024-2025', 35, 'ouverte'),
('L2-INFO-B', 'L2', 'B', '2024-2025', 35, 'ouverte'),
('L2-GL-A', 'L2', 'A', '2024-2025', 30, 'ouverte'),
('L2-SI-A', 'L2', 'A', '2024-2025', 30, 'ouverte'),

-- Licence 3
('L3-INFO-A', 'L3', 'A', '2024-2025', 35, 'ouverte'),
('L3-GL-A', 'L3', 'A', '2024-2025', 30, 'ouverte'),
('L3-IA-A', 'L3', 'A', '2024-2025', 25, 'ouverte'),
('L3-CYBER-A', 'L3', 'A', '2024-2025', 25, 'ouverte')
ON DUPLICATE KEY UPDATE nom_classe = VALUES(nom_classe);

-- 3. SERVICES (colonnes réelles: nom_service, description, montant_mensuel, statut)
INSERT INTO service (nom_service, description, montant_mensuel, statut) VALUES
('Internat', 'Hébergement sur le campus universitaire', 25000.00, 'disponible'),
('Transport', 'Service de transport universitaire', 15000.00, 'disponible'),
('Restauration', 'Service de restauration - cantine universitaire', 20000.00, 'disponible'),
('Bibliothèque Premium', 'Accès étendu aux ressources bibliothèque', 5000.00, 'disponible'),
('Sport et Loisirs', 'Accès aux installations sportives', 8000.00, 'disponible'),
('Soutien Scolaire', 'Cours de soutien et tutorat', 12000.00, 'disponible')
ON DUPLICATE KEY UPDATE nom_service = VALUES(nom_service);

-- 4. VÉRIFICATIONS RAPIDES
SELECT 'INSERTION TERMINÉE AVEC SUCCÈS' as STATUS;

SELECT 'FILIÈRES' as TABLE_NAME, COUNT(*) as TOTAL FROM filiere;
SELECT 'CLASSES' as TABLE_NAME, COUNT(*) as TOTAL FROM classe;
SELECT 'SERVICES' as TABLE_NAME, COUNT(*) as TOTAL FROM service;

-- Vérifier les niveaux d'études disponibles pour le dropdown
SELECT 'NIVEAUX DISPONIBLES:' as INFO;
SELECT DISTINCT niveau FROM classe ORDER BY niveau;

-- Vérifier les filières actives
SELECT 'FILIÈRES ACTIVES:' as INFO;
SELECT nom_filiere, duree_etudes FROM filiere WHERE statut = 'active';

SELECT 'SCRIPT TERMINÉ - TESTEZ MAINTENANT /Etudiants/Create' as MESSAGE;
