-- ================================================================
-- CORRECTION DES RELATIONS - VERSION COMPATIBLE MYSQL
-- Script corrigé pour WampServer/phpMyAdmin
-- ================================================================

USE iit_university_db;

-- 1. VÉRIFIER ET AJOUTER LA COLONNE id_filiere DANS classe
SET @col_exists = (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS 
                   WHERE TABLE_SCHEMA = 'iit_university_db' 
                   AND TABLE_NAME = 'classe' 
                   AND COLUMN_NAME = 'id_filiere');

SET @sql = IF(@col_exists = 0, 
              'ALTER TABLE classe ADD COLUMN id_filiere INT AFTER niveau', 
              'SELECT "Colonne id_filiere existe déjà dans classe" as MESSAGE');

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- 2. VÉRIFIER ET AJOUTER LA COLONNE id_classe DANS etudiant
SET @col_exists2 = (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = 'iit_university_db' 
                    AND TABLE_NAME = 'etudiant' 
                    AND COLUMN_NAME = 'id_classe');

SET @sql2 = IF(@col_exists2 = 0, 
               'ALTER TABLE etudiant ADD COLUMN id_classe INT AFTER niveau', 
               'SELECT "Colonne id_classe existe déjà dans etudiant" as MESSAGE');

PREPARE stmt2 FROM @sql2;
EXECUTE stmt2;
DEALLOCATE PREPARE stmt2;

-- 3. METTRE À JOUR LES RELATIONS (ignorer les erreurs si les colonnes n'existent pas)
UPDATE classe SET id_filiere = 1 WHERE nom_classe LIKE '%INFO%' AND id_filiere IS NOT NULL;
UPDATE classe SET id_filiere = 2 WHERE nom_classe LIKE '%GL%' AND id_filiere IS NOT NULL;
UPDATE classe SET id_filiere = 3 WHERE nom_classe LIKE '%SI%' AND id_filiere IS NOT NULL;

-- 4. INSÉRER QUELQUES ÉTUDIANTS DE TEST
INSERT INTO etudiant (matricule, nom, prenom, date_naissance, lieu_naissance, sexe, telephone, email, adresse, nationalite, niveau, statut) VALUES
('IIT2025L1001', 'ASSOU', 'Koffi', '2005-03-15', 'Abidjan', 'M', '+225-01-23-45-67', 'k.assou@student.iit.ci', 'Cocody, Abidjan', 'Ivoirien', 'L1', 'actif'),
('IIT2025L1002', 'BA', 'Aissatou', '2004-07-22', 'Bouaké', 'F', '+225-02-34-56-78', 'a.ba@student.iit.ci', 'Bouaké Centre', 'Ivoirien', 'L1', 'actif'),
('IIT2025L2001', 'COULIBALY', 'Seydou', '2003-11-08', 'Korhogo', 'M', '+225-03-45-67-89', 's.coulibaly@student.iit.ci', 'Korhogo', 'Ivoirien', 'L2', 'actif'),
('IIT2025L3001', 'FOFANA', 'Ousmane', '2002-05-30', 'San-Pédro', 'M', '+225-05-67-89-01', 'o.fofana@student.iit.ci', 'San-Pédro', 'Ivoirien', 'L3', 'actif')
ON DUPLICATE KEY UPDATE nom = VALUES(nom);

-- 5. LIER LES ÉTUDIANTS AUX CLASSES (si la colonne existe)
UPDATE etudiant SET id_classe = 1 WHERE niveau = 'L1' AND id_classe IS NOT NULL;
UPDATE etudiant SET id_classe = 5 WHERE niveau = 'L2' AND id_classe IS NOT NULL;
UPDATE etudiant SET id_classe = 9 WHERE niveau = 'L3' AND id_classe IS NOT NULL;

-- 6. VÉRIFICATIONS FINALES
SELECT 'CORRECTIONS APPLIQUÉES' as STATUS;

-- Voir la structure des tables
SHOW COLUMNS FROM classe;
SHOW COLUMNS FROM etudiant;

-- Vérifier les données
SELECT COUNT(*) as FILIERES FROM filiere;
SELECT COUNT(*) as CLASSES FROM classe;
SELECT COUNT(*) as ETUDIANTS FROM etudiant;

-- Vérifier les relations si elles existent
SELECT c.nom_classe, c.niveau, f.nom_filiere
FROM classe c
LEFT JOIN filiere f ON c.id_filiere = f.id_filiere
WHERE c.id_filiere IS NOT NULL
ORDER BY c.niveau, c.nom_classe;

SELECT 'SCRIPT TERMINÉ - RECOMPILEZ ET TESTEZ' as MESSAGE;
