-- ================================================================
-- SCRIPT CORRIGÉ - ADAPTÉ À VOTRE STRUCTURE EXISTANTE
-- Insertion des données avec les vraies colonnes
-- ================================================================

USE iit_university_db;

-- 1. FILIÈRES (selon votre structure existante)
INSERT INTO filiere (nom_filiere, description) VALUES
('Informatique et Réseaux', 'Formation en développement logiciel et administration réseaux'),
('Génie Logiciel', 'Formation spécialisée en conception et développement de logiciels'),
('Système d''Information', 'Formation en gestion et analyse des systèmes d''information'),
('Intelligence Artificielle', 'Formation en IA, Machine Learning et Data Science'),
('Cybersécurité', 'Formation en sécurité informatique et protection des données')
ON DUPLICATE KEY UPDATE nom_filiere = VALUES(nom_filiere);

-- 2. CLASSES (selon votre structure)
INSERT INTO classe (nom_classe, niveau, filiere_id) VALUES
-- Licence 1
('L1-INFO-A', 'L1', 1),
('L1-INFO-B', 'L1', 1),
('L1-GL-A', 'L1', 2),
('L1-SI-A', 'L1', 3),

-- Licence 2  
('L2-INFO-A', 'L2', 1),
('L2-INFO-B', 'L2', 1),
('L2-GL-A', 'L2', 2),
('L2-SI-A', 'L2', 3),

-- Licence 3
('L3-INFO-A', 'L3', 1),
('L3-GL-A', 'L3', 2),
('L3-IA-A', 'L3', 4),
('L3-CYBER-A', 'L3', 5)
ON DUPLICATE KEY UPDATE nom_classe = VALUES(nom_classe);

-- 3. SERVICES IIT
INSERT INTO service (nom_service, description, tarif) VALUES
('Internat', 'Hébergement sur le campus universitaire', 25000.00),
('Transport', 'Service de transport universitaire', 15000.00),
('Restauration', 'Service de restauration - cantine universitaire', 20000.00),
('Bibliothèque Premium', 'Accès étendu aux ressources bibliothèque', 5000.00),
('Sport et Loisirs', 'Accès aux installations sportives', 8000.00),
('Soutien Scolaire', 'Cours de soutien et tutorat', 12000.00)
ON DUPLICATE KEY UPDATE nom_service = VALUES(nom_service);

-- 4. SCOLARITÉ (selon votre structure)
INSERT INTO scolarite (filiere_id, niveau, montant_scolarite, annee_academique) VALUES
-- Informatique et Réseaux
(1, 'L1', 350000.00, '2024-2025'),
(1, 'L2', 380000.00, '2024-2025'),
(1, 'L3', 420000.00, '2024-2025'),

-- Génie Logiciel
(2, 'L1', 360000.00, '2024-2025'),
(2, 'L2', 390000.00, '2024-2025'),
(2, 'L3', 430000.00, '2024-2025'),

-- Système d'Information
(3, 'L1', 340000.00, '2024-2025'),
(3, 'L2', 370000.00, '2024-2025'),
(3, 'L3', 410000.00, '2024-2025'),

-- Intelligence Artificielle
(4, 'L3', 450000.00, '2024-2025'),

-- Cybersécurité
(5, 'L3', 460000.00, '2024-2025')
ON DUPLICATE KEY UPDATE montant_scolarite = VALUES(montant_scolarite);

-- 5. ENSEIGNANTS
INSERT INTO enseignant (matricule, nom, prenom, specialite, telephone, email, statut) VALUES
('ENS2025001', 'KOUAME', 'Yves', 'Programmation Web', '+225-01-02-03-04', 'y.kouame@iit.edu.ci', 'actif'),
('ENS2025002', 'DIALLO', 'Aminata', 'Base de Données', '+225-05-06-07-08', 'a.diallo@iit.edu.ci', 'actif'),
('ENS2025003', 'TRAORE', 'Mamadou', 'Réseaux Informatiques', '+225-09-10-11-12', 'm.traore@iit.edu.ci', 'actif'),
('ENS2025004', 'KONE', 'Fatou', 'Intelligence Artificielle', '+225-13-14-15-16', 'f.kone@iit.edu.ci', 'actif'),
('ENS2025005', 'OUATTARA', 'Ibrahim', 'Cybersécurité', '+225-17-18-19-20', 'i.ouattara@iit.edu.ci', 'actif'),
('ENS2025006', 'NGUESSAN', 'Marie', 'Génie Logiciel', '+225-21-22-23-24', 'm.nguessan@iit.edu.ci', 'actif')
ON DUPLICATE KEY UPDATE nom = VALUES(nom);

-- VÉRIFICATIONS FINALES
SELECT 'INSERTION TERMINÉE - RÉSULTATS :' as STATUS;

SELECT 'FILIÈRES' as TABLE_NAME, COUNT(*) as TOTAL FROM filiere;
SELECT 'CLASSES' as TABLE_NAME, COUNT(*) as TOTAL FROM classe;
SELECT 'SERVICES' as TABLE_NAME, COUNT(*) as TOTAL FROM service;
SELECT 'ENSEIGNANTS' as TABLE_NAME, COUNT(*) as TOTAL FROM enseignant;

-- Vérifier les niveaux d'études disponibles
SELECT DISTINCT niveau FROM classe ORDER BY niveau;

SELECT 'SCRIPT TERMINÉ - TESTEZ VOTRE APPLICATION!' as MESSAGE;
