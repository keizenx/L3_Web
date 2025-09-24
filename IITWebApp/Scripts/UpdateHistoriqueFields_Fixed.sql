-- ================================================================
-- SCRIPT DE MISE À JOUR POUR LES CHAMPS HISTORIQUES
-- Système IIT - Base MySQL (Compatible WampServer)
-- ================================================================

-- Ajouter les colonnes manquantes dans DELEGUE (une par une)
-- Si la colonne existe déjà, l'erreur sera ignorée

-- Ajouter date_debut
SET @sql = IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE table_name = 'DELEGUE' 
               AND column_name = 'date_debut' 
               AND table_schema = 'iit_university_db') > 0,
              'SELECT "Column date_debut already exists"',
              'ALTER TABLE DELEGUE ADD COLUMN date_debut DATE NULL');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Ajouter date_fin
SET @sql = IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE table_name = 'DELEGUE' 
               AND column_name = 'date_fin' 
               AND table_schema = 'iit_university_db') > 0,
              'SELECT "Column date_fin already exists"',
              'ALTER TABLE DELEGUE ADD COLUMN date_fin DATE NULL');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Ajouter date_modification à SOUSCRIPTION_SERVICE
SET @sql = IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE table_name = 'SOUSCRIPTION_SERVICE' 
               AND column_name = 'date_modification' 
               AND table_schema = 'iit_university_db') > 0,
              'SELECT "Column date_modification already exists"',
              'ALTER TABLE SOUSCRIPTION_SERVICE ADD COLUMN date_modification DATETIME NULL');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Ajouter motif_fin à SOUSCRIPTION_SERVICE
SET @sql = IF((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE table_name = 'SOUSCRIPTION_SERVICE' 
               AND column_name = 'motif_fin' 
               AND table_schema = 'iit_university_db') > 0,
              'SELECT "Column motif_fin already exists"',
              'ALTER TABLE SOUSCRIPTION_SERVICE ADD COLUMN motif_fin VARCHAR(200) NULL');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Mettre à jour les enregistrements existants avec des valeurs par défaut
-- Pour DELEGUE : utiliser date_election comme date_debut si elle existe
UPDATE DELEGUE 
SET date_debut = COALESCE(date_election, '2024-09-01')
WHERE date_debut IS NULL;

-- Pour SOUSCRIPTION_SERVICE : initialiser date_modification
UPDATE SOUSCRIPTION_SERVICE 
SET date_modification = date_debut
WHERE date_modification IS NULL AND date_fin IS NOT NULL;

-- Vérifier que toutes les tables existent et sont accessibles
SELECT 'ETUDIANT' as table_name, COUNT(*) as count FROM ETUDIANT
UNION ALL
SELECT 'DELEGUE' as table_name, COUNT(*) as count FROM DELEGUE  
UNION ALL
SELECT 'SOUSCRIPTION_SERVICE' as table_name, COUNT(*) as count FROM SOUSCRIPTION_SERVICE;

-- Afficher la structure des tables modifiées
SHOW COLUMNS FROM DELEGUE;
SHOW COLUMNS FROM SOUSCRIPTION_SERVICE;