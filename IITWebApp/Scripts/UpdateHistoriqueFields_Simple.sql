-- ================================================================
-- SCRIPT SIMPLE - MISE À JOUR CHAMPS HISTORIQUES
-- Compatible avec WampServer MySQL
-- ================================================================

-- 1. Ajouter les colonnes à DELEGUE
ALTER TABLE DELEGUE ADD COLUMN date_debut DATE NULL;
ALTER TABLE DELEGUE ADD COLUMN date_fin DATE NULL;

-- 2. Ajouter les colonnes à SOUSCRIPTION_SERVICE  
ALTER TABLE SOUSCRIPTION_SERVICE ADD COLUMN date_modification DATETIME NULL;
ALTER TABLE SOUSCRIPTION_SERVICE ADD COLUMN motif_fin VARCHAR(200) NULL;

-- 3. Initialiser les données existantes
UPDATE DELEGUE 
SET date_debut = COALESCE(date_election, '2024-09-01')
WHERE date_debut IS NULL;

UPDATE SOUSCRIPTION_SERVICE 
SET date_modification = date_debut
WHERE date_modification IS NULL AND date_fin IS NOT NULL;

-- 4. Vérification des tables
SELECT 'ETUDIANT' as table_name, COUNT(*) as count FROM ETUDIANT
UNION ALL
SELECT 'DELEGUE' as table_name, COUNT(*) as count FROM DELEGUE  
UNION ALL
SELECT 'SOUSCRIPTION_SERVICE' as table_name, COUNT(*) as count FROM SOUSCRIPTION_SERVICE;