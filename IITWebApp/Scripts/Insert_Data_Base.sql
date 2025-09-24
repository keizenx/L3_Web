-- ================================================================
-- INSERTION DES DONNÉES DE BASE IIT UNIVERSITY
-- Données essentielles pour les tests CRUD Phase 1 & 2
-- ================================================================

USE iit_university_db;

-- 1. FILIÈRES (données de base)
INSERT INTO filiere (nom_filiere, description, duree_annees) VALUES
('Informatique et Réseaux', 'Formation en développement logiciel et administration réseaux', 3),
('Génie Logiciel', 'Formation spécialisée en conception et développement de logiciels', 3),
('Système d''Information', 'Formation en gestion et analyse des systèmes d''information', 3),
('Intelligence Artificielle', 'Formation en IA, Machine Learning et Data Science', 3),
('Cybersécurité', 'Formation en sécurité informatique et protection des données', 3)
ON DUPLICATE KEY UPDATE nom_filiere = VALUES(nom_filiere);

-- 2. CLASSES (par niveau et filière)
INSERT INTO classe (nom_classe, niveau, filiere_id, effectif_max) VALUES
-- Licence 1
('L1-INFO-A', 'L1', 1, 35),
('L1-INFO-B', 'L1', 1, 35),
('L1-GL-A', 'L1', 2, 30),
('L1-SI-A', 'L1', 3, 30),

-- Licence 2  
('L2-INFO-A', 'L2', 1, 35),
('L2-INFO-B', 'L2', 1, 35),
('L2-GL-A', 'L2', 2, 30),
('L2-SI-A', 'L2', 3, 30),

-- Licence 3
('L3-INFO-A', 'L3', 1, 35),
('L3-GL-A', 'L3', 2, 30),
('L3-IA-A', 'L3', 4, 25),
('L3-CYBER-A', 'L3', 5, 25)
ON DUPLICATE KEY UPDATE nom_classe = VALUES(nom_classe);

-- 3. SERVICES IIT (internat, transport, etc.)
INSERT INTO service (nom_service, description, tarif_mensuel) VALUES
('Internat', 'Hébergement sur le campus universitaire', 25000.00),
('Transport', 'Service de transport universitaire', 15000.00),
('Restauration', 'Service de restauration - cantine universitaire', 20000.00),
('Bibliothèque Premium', 'Accès étendu aux ressources bibliothèque', 5000.00),
('Sport et Loisirs', 'Accès aux installations sportives', 8000.00),
('Soutien Scolaire', 'Cours de soutien et tutorat', 12000.00)
ON DUPLICATE KEY UPDATE nom_service = VALUES(nom_service);

-- 4. SCOLARITÉ (tarifs par filière et niveau)
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

-- 5. ENSEIGNANTS (Phase 2)
INSERT INTO enseignant (matricule, nom, prenom, specialite, telephone, email, statut) VALUES
('ENS2025001', 'KOUAME', 'Yves', 'Programmation Web', '+225-01-02-03-04', 'y.kouame@iit.edu.ci', 'actif'),
('ENS2025002', 'DIALLO', 'Aminata', 'Base de Données', '+225-05-06-07-08', 'a.diallo@iit.edu.ci', 'actif'),
('ENS2025003', 'TRAORE', 'Mamadou', 'Réseaux Informatiques', '+225-09-10-11-12', 'm.traore@iit.edu.ci', 'actif'),
('ENS2025004', 'KONE', 'Fatou', 'Intelligence Artificielle', '+225-13-14-15-16', 'f.kone@iit.edu.ci', 'actif'),
('ENS2025005', 'OUATTARA', 'Ibrahim', 'Cybersécurité', '+225-17-18-19-20', 'i.ouattara@iit.edu.ci', 'actif'),
('ENS2025006', 'NGUESSAN', 'Marie', 'Génie Logiciel', '+225-21-22-23-24', 'm.nguessan@iit.edu.ci', 'actif')
ON DUPLICATE KEY UPDATE nom = VALUES(nom);

-- 6. COURS (Phase 2 - Planification des enseignements)
INSERT INTO cours (nom_cours, code_cours, enseignant_id, classe_id, credit, volume_horaire, jour_semaine, heure_debut, heure_fin, salle) VALUES
-- Cours L1-INFO-A
('Introduction à la Programmation', 'PROG101', 1, 1, 6, 45, 'Lundi', '08:00:00', '10:30:00', 'Salle A1'),
('Mathématiques Discrètes', 'MATH101', 2, 1, 4, 30, 'Mardi', '10:45:00', '12:15:00', 'Salle B2'),
('Algorithmique et Structures de Données', 'ALGO101', 1, 1, 6, 45, 'Mercredi', '14:00:00', '16:30:00', 'Labo1'),

-- Cours L2-INFO-A  
('Programmation Orientée Objet', 'POO201', 1, 5, 6, 45, 'Jeudi', '08:00:00', '10:30:00', 'Labo2'),
('Base de Données', 'BDD201', 2, 5, 6, 45, 'Vendredi', '14:00:00', '16:30:00', 'Salle C3'),
('Réseaux et Communication', 'RES201', 3, 5, 4, 30, 'Lundi', '10:45:00', '12:15:00', 'Labo3'),

-- Cours L3-INFO-A
('Développement Web Avancé', 'WEB301', 1, 9, 6, 45, 'Mardi', '08:00:00', '10:30:00', 'Labo4'),
('Intelligence Artificielle', 'IA301', 4, 11, 6, 45, 'Mercredi', '08:00:00', '10:30:00', 'Salle D4'),
('Sécurité Informatique', 'SEC301', 5, 12, 4, 30, 'Jeudi', '14:00:00', '15:30:00', 'Salle E5')
ON DUPLICATE KEY UPDATE nom_cours = VALUES(nom_cours);

-- 7. QUELQUES ÉTUDIANTS DE TEST (avec matricules automatiques)
INSERT INTO etudiant (matricule, nom, prenom, date_naissance, lieu_naissance, sexe, telephone, email, adresse, nationalite, niveau, classe_id, statut) VALUES
('IIT2025L1001', 'ASSOU', 'Koffi', '2005-03-15', 'Abidjan', 'M', '+225-01-23-45-67', 'k.assou@student.iit.ci', 'Cocody, Abidjan', 'Ivoirien', 'L1', 1, 'actif'),
('IIT2025L1002', 'BA', 'Aissatou', '2004-07-22', 'Bouaké', 'F', '+225-02-34-56-78', 'a.ba@student.iit.ci', 'Bouaké Centre', 'Ivoirien', 'L1', 1, 'actif'),
('IIT2025L2001', 'COULIBALY', 'Seydou', '2003-11-08', 'Korhogo', 'M', '+225-03-45-67-89', 's.coulibaly@student.iit.ci', 'Korhogo', 'Ivoirien', 'L2', 5, 'actif'),
('IIT2025L2002', 'DEMBELE', 'Mariam', '2003-09-14', 'Man', 'F', '+225-04-56-78-90', 'm.dembele@student.iit.ci', 'Man Centre', 'Ivoirien', 'L2', 5, 'actif'),
('IIT2025L3001', 'FOFANA', 'Ousmane', '2002-05-30', 'San-Pédro', 'M', '+225-05-67-89-01', 'o.fofana@student.iit.ci', 'San-Pédro', 'Ivoirien', 'L3', 9, 'actif')
ON DUPLICATE KEY UPDATE nom = VALUES(nom);

-- 8. INSCRIPTIONS CORRESPONDANTES
INSERT INTO inscription (etudiant_id, filiere_id, classe_id, annee_academique, date_inscription, statut) VALUES
(1, 1, 1, '2024-2025', '2024-09-01', 'confirmee'),
(2, 1, 1, '2024-2025', '2024-09-02', 'confirmee'),
(3, 1, 5, '2024-2025', '2024-09-01', 'confirmee'),
(4, 1, 5, '2024-2025', '2024-09-03', 'confirmee'),
(5, 1, 9, '2024-2025', '2024-09-01', 'confirmee')
ON DUPLICATE KEY UPDATE statut = VALUES(statut);

-- 9. QUELQUES PAIEMENTS DE TEST
INSERT INTO paiement (etudiant_id, montant, date_paiement, mode_paiement, type_paiement, statut) VALUES
(1, 175000.00, '2024-09-01', 'virement', 'scolarite', 'valide'),
(2, 350000.00, '2024-09-02', 'especes', 'scolarite', 'valide'),
(3, 190000.00, '2024-09-01', 'cheque', 'scolarite', 'valide'),
(4, 185000.00, '2024-09-03', 'virement', 'scolarite', 'valide'),
(5, 210000.00, '2024-09-01', 'virement', 'scolarite', 'valide')
ON DUPLICATE KEY UPDATE montant = VALUES(montant);

-- 10. SOUSCRIPTIONS À DES SERVICES
INSERT INTO souscription_service (etudiant_id, service_id, date_souscription, statut) VALUES
(1, 1, '2024-09-01', 'active'), -- Internat
(1, 2, '2024-09-01', 'active'), -- Transport
(2, 2, '2024-09-02', 'active'), -- Transport
(3, 1, '2024-09-01', 'active'), -- Internat
(3, 3, '2024-09-01', 'active'), -- Restauration
(4, 2, '2024-09-03', 'active'), -- Transport
(5, 4, '2024-09-01', 'active')  -- Bibliothèque Premium
ON DUPLICATE KEY UPDATE statut = VALUES(statut);

-- 11. DÉLÉGUÉS DE CLASSE
INSERT INTO delegue (etudiant_id, classe_id, fonction, annee_mandat, statut) VALUES
(1, 1, 'delegue', '2024-2025', 'actif'),
(2, 1, 'adjoint', '2024-2025', 'actif'),
(3, 5, 'delegue', '2024-2025', 'actif')
ON DUPLICATE KEY UPDATE fonction = VALUES(fonction);

-- 12. QUELQUES ABSENCES (Phase 2)
INSERT INTO absence (etudiant_id, cours_id, date_absence, justifiee, motif) VALUES
(1, 1, '2024-09-10', false, NULL),
(2, 1, '2024-09-10', true, 'Maladie'),
(3, 4, '2024-09-12', false, NULL),
(1, 2, '2024-09-11', true, 'Rendez-vous médical')
ON DUPLICATE KEY UPDATE justifiee = VALUES(justifiee);

-- 13. HONORAIRES ENSEIGNANTS (Phase 2)
INSERT INTO honoraire_enseignant (enseignant_id, mois, annee, montant_base, heures_supplementaires, montant_total, statut_paiement) VALUES
(1, 9, 2024, 180000.00, 12, 198000.00, 'paye'),
(2, 9, 2024, 175000.00, 8, 187000.00, 'paye'),
(3, 9, 2024, 170000.00, 15, 192500.00, 'en_attente'),
(4, 9, 2024, 185000.00, 10, 200000.00, 'paye'),
(5, 9, 2024, 190000.00, 6, 199000.00, 'en_attente')
ON DUPLICATE KEY UPDATE montant_total = VALUES(montant_total);

-- ================================================================
-- VÉRIFICATIONS FINALES
-- ================================================================

SELECT 'INSERTION TERMINÉE - RÉSULTATS :' as STATUS;

SELECT 'FILIÈRES' as TABLE_NAME, COUNT(*) as TOTAL FROM filiere;
SELECT 'CLASSES' as TABLE_NAME, COUNT(*) as TOTAL FROM classe;
SELECT 'SERVICES' as TABLE_NAME, COUNT(*) as TOTAL FROM service;
SELECT 'ÉTUDIANTS' as TABLE_NAME, COUNT(*) as TOTAL FROM etudiant;
SELECT 'ENSEIGNANTS' as TABLE_NAME, COUNT(*) as TOTAL FROM enseignant;
SELECT 'COURS' as TABLE_NAME, COUNT(*) as TOTAL FROM cours;
SELECT 'INSCRIPTIONS' as TABLE_NAME, COUNT(*) as TOTAL FROM inscription;
SELECT 'PAIEMENTS' as TABLE_NAME, COUNT(*) as TOTAL FROM paiement;
SELECT 'SOUSCRIPTIONS' as TABLE_NAME, COUNT(*) as TOTAL FROM souscription_service;
SELECT 'ABSENCES' as TABLE_NAME, COUNT(*) as TOTAL FROM absence;
SELECT 'HONORAIRES' as TABLE_NAME, COUNT(*) as TOTAL FROM honoraire_enseignant;

-- Vérifier les niveaux d'études disponibles
SELECT DISTINCT niveau FROM classe ORDER BY niveau;

-- Vérifier les étudiants par niveau
SELECT niveau, COUNT(*) as nb_etudiants FROM etudiant GROUP BY niveau ORDER BY niveau;
