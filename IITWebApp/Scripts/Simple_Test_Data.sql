-- ================================================================
-- SCRIPT SIMPLE SANS RELATIONS COMPLEXES
-- Test CRUD de base sans contraintes
-- ================================================================

USE iit_university_db;

-- Nettoyer et insérer des données simples
DELETE FROM etudiant WHERE matricule LIKE 'TEST%';

-- Insérer des étudiants simples pour test CRUD
INSERT INTO etudiant (matricule, nom, prenom, date_naissance, lieu_naissance, sexe, nationalite, niveau, statut) VALUES
('TEST2025001', 'KOUAME', 'Jean', '2005-01-15', 'Abidjan', 'M', 'Ivoirien', 'L1', 'actif'),
('TEST2025002', 'DIALLO', 'Marie', '2004-05-22', 'Bouaké', 'F', 'Ivoirien', 'L2', 'actif'),
('TEST2025003', 'TRAORE', 'Paul', '2003-09-10', 'Korhogo', 'M', 'Ivoirien', 'L3', 'actif')
ON DUPLICATE KEY UPDATE nom = VALUES(nom);

-- VÉRIFICATIONS
SELECT 'DONNÉES DE TEST INSÉRÉES' as STATUS;
SELECT * FROM etudiant WHERE matricule LIKE 'TEST%';
SELECT DISTINCT niveau FROM classe ORDER BY niveau;

SELECT 'TESTEZ MAINTENANT : http://localhost:5000/Etudiants' as MESSAGE;
