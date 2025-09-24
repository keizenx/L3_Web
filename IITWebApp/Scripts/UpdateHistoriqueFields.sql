-- ================================================================
-- SCRIPT DE MISE À JOUR POUR LES CHAMPS HISTORIQUES
-- Système IIT - Base MySQL
-- ================================================================

-- Vérifier et ajouter les colonnes manquantes dans DELEGUE
ALTER TABLE DELEGUE 
ADD COLUMN IF NOT EXISTS date_debut DATE NULL,
ADD COLUMN IF NOT EXISTS date_fin DATE NULL;

-- Vérifier et ajouter les colonnes manquantes dans SOUSCRIPTION_SERVICE  
ALTER TABLE SOUSCRIPTION_SERVICE 
ADD COLUMN IF NOT EXISTS date_modification DATETIME NULL,
ADD COLUMN IF NOT EXISTS motif_fin VARCHAR(200) NULL;

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