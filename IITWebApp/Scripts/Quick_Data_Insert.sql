-- ================================================================
-- SCRIPT RAPIDE - DONNÉES DE BASE IIT UNIVERSITY
-- Insertion simple pour test CRUD immédiat
-- ================================================================

USE iit_university_db;

-- 1. FILIÈRES ESSENTIELLES
INSERT INTO filiere (nom_filiere, description, duree_annees) VALUES
('Informatique', 'Formation en informatique générale', 3),
('Génie Logiciel', 'Formation en développement logiciel', 3),
('Réseaux', 'Formation en réseaux et télécommunications', 3)
ON DUPLICATE KEY UPDATE nom_filiere = VALUES(nom_filiere);

-- 2. CLASSES DE BASE
INSERT INTO classe (nom_classe, niveau, filiere_id, effectif_max) VALUES
('L1-INFO', 'L1', 1, 35),
('L2-INFO', 'L2', 1, 35),
('L3-INFO', 'L3', 1, 35),
('L1-GL', 'L1', 2, 30),
('L2-GL', 'L2', 2, 30),
('L3-GL', 'L3', 2, 30)
ON DUPLICATE KEY UPDATE nom_classe = VALUES(nom_classe);

-- 3. SERVICES DE BASE
INSERT INTO service (nom_service, description, tarif_mensuel) VALUES
('Internat', 'Hébergement campus', 25000.00),
('Transport', 'Transport universitaire', 15000.00),
('Restauration', 'Cantine universitaire', 20000.00)
ON DUPLICATE KEY UPDATE nom_service = VALUES(nom_service);

-- 4. SCOLARITÉ DE BASE
INSERT INTO scolarite (filiere_id, niveau, montant_scolarite, annee_academique) VALUES
(1, 'L1', 350000.00, '2024-2025'),
(1, 'L2', 380000.00, '2024-2025'),
(1, 'L3', 420000.00, '2024-2025'),
(2, 'L1', 360000.00, '2024-2025'),
(2, 'L2', 390000.00, '2024-2025'),
(2, 'L3', 430000.00, '2024-2025')
ON DUPLICATE KEY UPDATE montant_scolarite = VALUES(montant_scolarite);

-- 5. ENSEIGNANTS DE TEST
INSERT INTO enseignant (matricule, nom, prenom, specialite, telephone, email, statut) VALUES
('ENS001', 'KOUAME', 'Jean', 'Programmation', '01-02-03-04', 'j.kouame@iit.ci', 'actif'),
('ENS002', 'DIALLO', 'Marie', 'Base de Données', '05-06-07-08', 'm.diallo@iit.ci', 'actif'),
('ENS003', 'TRAORE', 'Paul', 'Réseaux', '09-10-11-12', 'p.traore@iit.ci', 'actif')
ON DUPLICATE KEY UPDATE nom = VALUES(nom);

-- VÉRIFICATIONS
SELECT 'DONNÉES INSÉRÉES AVEC SUCCÈS' as STATUS;
SELECT 'FILIÈRES', COUNT(*) as TOTAL FROM filiere;
SELECT 'CLASSES', COUNT(*) as TOTAL FROM classe;
SELECT 'SERVICES', COUNT(*) as TOTAL FROM service;
SELECT 'ENSEIGNANTS', COUNT(*) as TOTAL FROM enseignant;

-- Vérifier les niveaux disponibles
SELECT DISTINCT niveau FROM classe ORDER BY niveau;
