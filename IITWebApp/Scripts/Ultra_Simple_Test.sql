-- ================================================================
-- SCRIPT MINIMAL POUR TEST IMMÉDIAT 
-- Version ultra-simple sans erreurs
-- ================================================================

USE iit_university_db;

-- 1. FILIÈRES ESSENTIELLES
DELETE FROM filiere WHERE filiere_id > 0;
INSERT INTO filiere (nom_filiere, description, duree_annees) VALUES
('Informatique', 'Formation informatique generale', 3),
('Genie Logiciel', 'Formation developpement logiciel', 3),
('Reseaux', 'Formation reseaux et telecoms', 3);

-- 2. CLASSES DE BASE
DELETE FROM classe WHERE classe_id > 0;
INSERT INTO classe (nom_classe, niveau, filiere_id, effectif_max) VALUES
('L1-INFO', 'L1', 1, 30),
('L2-INFO', 'L2', 1, 30),
('L3-INFO', 'L3', 1, 30),
('L1-GL', 'L1', 2, 25),
('L2-GL', 'L2', 2, 25),
('L3-GL', 'L3', 2, 25);

-- 3. SERVICES SIMPLES
DELETE FROM service WHERE service_id > 0;
INSERT INTO service (nom_service, description, tarif_mensuel) VALUES
('Internat', 'Hebergement campus', 25000),
('Transport', 'Transport universitaire', 15000),
('Restauration', 'Cantine universitaire', 20000);

-- 4. SCOLARITÉ DE BASE
DELETE FROM scolarite WHERE scolarite_id > 0;
INSERT INTO scolarite (filiere_id, niveau, montant_scolarite, annee_academique) VALUES
(1, 'L1', 350000, '2024-2025'),
(1, 'L2', 380000, '2024-2025'),
(1, 'L3', 420000, '2024-2025'),
(2, 'L1', 360000, '2024-2025'),
(2, 'L2', 390000, '2024-2025'),
(2, 'L3', 430000, '2024-2025');

-- 5. ENSEIGNANTS SIMPLES
DELETE FROM enseignant WHERE enseignant_id > 0;
INSERT INTO enseignant (matricule, nom, prenom, specialite, telephone, email, statut) VALUES
('ENS001', 'KOUAME', 'Jean', 'Programmation', '01020304', 'j.kouame@iit.ci', 'actif'),
('ENS002', 'DIALLO', 'Marie', 'Base de Donnees', '05060708', 'm.diallo@iit.ci', 'actif'),
('ENS003', 'TRAORE', 'Paul', 'Reseaux', '09101112', 'p.traore@iit.ci', 'actif');

-- VÉRIFICATIONS IMMÉDIATES
SELECT 'DONNEES INSEREES AVEC SUCCES' as STATUS;
SELECT 'FILIERES:' as INFO, COUNT(*) as TOTAL FROM filiere;
SELECT 'CLASSES:' as INFO, COUNT(*) as TOTAL FROM classe;
SELECT 'SERVICES:' as INFO, COUNT(*) as TOTAL FROM service;
SELECT 'ENSEIGNANTS:' as INFO, COUNT(*) as TOTAL FROM enseignant;

-- NIVEAUX DISPONIBLES
SELECT 'NIVEAUX DISPONIBLES:' as INFO;
SELECT DISTINCT niveau FROM classe ORDER BY niveau;

SELECT 'SCRIPT TERMINE - TESTEZ MAINTENANT VOTRE APPLICATION!' as MESSAGE;
