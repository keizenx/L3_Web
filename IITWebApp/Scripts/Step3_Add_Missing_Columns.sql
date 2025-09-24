-- ================================================================
-- ÉTAPE 3: AJOUT DES COLONNES MANQUANTES
-- Copier et exécuter cette section dans phpMyAdmin
-- ================================================================

USE iit_university_db;

SELECT '=== AJOUT DES COLONNES MANQUANTES ===' as INFO;

-- Ajouter id_classe dans etudiant si nécessaire
ALTER TABLE etudiant 
ADD COLUMN IF NOT EXISTS id_classe INT NULL AFTER prenom,
ADD COLUMN IF NOT EXISTS id_filiere INT NULL AFTER id_classe,
ADD COLUMN IF NOT EXISTS niveau VARCHAR(20) DEFAULT 'L1' AFTER id_filiere;

-- Ajouter les contraintes de clés étrangères si elles n'existent pas
ALTER TABLE etudiant 
ADD CONSTRAINT IF NOT EXISTS fk_etudiant_classe 
FOREIGN KEY (id_classe) REFERENCES classe(id_classe) ON DELETE SET NULL;

ALTER TABLE etudiant 
ADD CONSTRAINT IF NOT EXISTS fk_etudiant_filiere 
FOREIGN KEY (id_filiere) REFERENCES filiere(id_filiere) ON DELETE SET NULL;

SELECT 'Colonnes ajoutées - Vérification structure:' as INFO;
DESCRIBE etudiant;
