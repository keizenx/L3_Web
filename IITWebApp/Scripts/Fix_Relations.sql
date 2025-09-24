-- ================================================================
-- CORRECTION DES RELATIONS ET CONTRAINTES
-- Ajout des colonnes manquantes et correction des mappings
-- ================================================================

USE iit_university_db;

-- 1. VÉRIFIER ET AJOUTER LES COLONNES MANQUANTES

-- Classe doit avoir une référence à filiere
ALTER TABLE classe ADD COLUMN IF NOT EXISTS id_filiere INT AFTER niveau;

-- Étudiant doit avoir une référence à classe  
ALTER TABLE etudiant ADD COLUMN IF NOT EXISTS id_classe INT AFTER niveau;

-- 2. METTRE À JOUR LES RELATIONS EXISTANTES

-- Lier les classes aux filières
UPDATE classe SET id_filiere = 1 WHERE nom_classe LIKE '%INFO%';
UPDATE classe SET id_filiere = 2 WHERE nom_classe LIKE '%GL%';
UPDATE classe SET id_filiere = 3 WHERE nom_classe LIKE '%SI%';

-- 3. CORRIGER LA TABLE SCOLARITE SI ELLE EXISTE
-- Vérifier si la table scolarite existe et la structure
SHOW TABLES LIKE 'scolarite';

-- 4. INSÉRER QUELQUES ÉTUDIANTS DE TEST AVEC LES BONNES RELATIONS
INSERT INTO etudiant (matricule, nom, prenom, date_naissance, lieu_naissance, sexe, telephone, email, adresse, nationalite, niveau, id_classe, statut) VALUES
('IIT2025L1001', 'ASSOU', 'Koffi', '2005-03-15', 'Abidjan', 'M', '+225-01-23-45-67', 'k.assou@student.iit.ci', 'Cocody, Abidjan', 'Ivoirien', 'L1', 1, 'actif'),
('IIT2025L1002', 'BA', 'Aissatou', '2004-07-22', 'Bouaké', 'F', '+225-02-34-56-78', 'a.ba@student.iit.ci', 'Bouaké Centre', 'Ivoirien', 'L1', 1, 'actif'),
('IIT2025L2001', 'COULIBALY', 'Seydou', '2003-11-08', 'Korhogo', 'M', '+225-03-45-67-89', 's.coulibaly@student.iit.ci', 'Korhogo', 'Ivoirien', 'L2', 5, 'actif'),
('IIT2025L3001', 'FOFANA', 'Ousmane', '2002-05-30', 'San-Pédro', 'M', '+225-05-67-89-01', 'o.fofana@student.iit.ci', 'San-Pédro', 'Ivoirien', 'L3', 9, 'actif')
ON DUPLICATE KEY UPDATE nom = VALUES(nom);

-- 5. INSÉRER DES INSCRIPTIONS SI LA TABLE EXISTE
INSERT IGNORE INTO inscription (id_etudiant, id_filiere, id_classe, annee_academique, date_inscription, statut) VALUES
(1, 1, 1, '2024-2025', '2024-09-01', 'confirmee'),
(2, 1, 1, '2024-2025', '2024-09-02', 'confirmee'),
(3, 1, 5, '2024-2025', '2024-09-01', 'confirmee'),
(4, 1, 9, '2024-2025', '2024-09-01', 'confirmee');

-- 6. VÉRIFICATIONS FINALES
SELECT 'CORRECTIONS APPLIQUÉES' as STATUS;

-- Vérifier les étudiants avec leurs classes
SELECT e.matricule, e.nom, e.prenom, e.niveau, c.nom_classe, f.nom_filiere
FROM etudiant e
LEFT JOIN classe c ON e.id_classe = c.id_classe  
LEFT JOIN filiere f ON c.id_filiere = f.id_filiere
ORDER BY e.matricule;

-- Vérifier les classes avec leurs filières
SELECT c.nom_classe, c.niveau, f.nom_filiere
FROM classe c
LEFT JOIN filiere f ON c.id_filiere = f.id_filiere
ORDER BY c.niveau, c.nom_classe;

SELECT 'RELATIONS CORRIGÉES - TESTEZ MAINTENANT LE CRUD' as MESSAGE;
