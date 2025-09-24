-- ================================================================
-- DIAGNOSTIC ET CORRECTION FINALE DU CRUD
-- Résolution de l'erreur HTTP 400
-- ================================================================

USE iit_university_db;

-- 1. VÉRIFIER LES DONNÉES DE L'ÉTUDIANT ID=4
SELECT 'DONNÉES ÉTUDIANT ID=4:' as INFO;
SELECT * FROM etudiant WHERE id_etudiant = 4;

-- 2. VÉRIFIER S'IL Y A DES VALEURS NULL PROBLÉMATIQUES
SELECT 'ÉTUDIANTS AVEC VALEURS NULL:' as INFO;
SELECT id_etudiant, matricule, nom, prenom, 
       CASE WHEN date_naissance IS NULL THEN 'NULL' ELSE 'OK' END as date_naissance,
       CASE WHEN sexe IS NULL THEN 'NULL' ELSE 'OK' END as sexe,
       CASE WHEN nom IS NULL THEN 'NULL' ELSE 'OK' END as nom,
       CASE WHEN prenom IS NULL THEN 'NULL' ELSE 'OK' END as prenom
FROM etudiant 
WHERE date_naissance IS NULL 
   OR sexe IS NULL 
   OR nom IS NULL 
   OR prenom IS NULL;

-- 3. CORRIGER LES DONNÉES MANQUANTES OU INCORRECTES
UPDATE etudiant 
SET date_naissance = '2000-01-01' 
WHERE date_naissance IS NULL;

UPDATE etudiant 
SET sexe = 'M' 
WHERE sexe IS NULL OR sexe = '';

UPDATE etudiant 
SET lieu_naissance = 'Non spécifié' 
WHERE lieu_naissance IS NULL OR lieu_naissance = '';

UPDATE etudiant 
SET nationalite = 'Ivoirien' 
WHERE nationalite IS NULL OR nationalite = '';

-- 4. VÉRIFIER ET CORRIGER LES MATRICULES DUPLIQUÉS
SELECT 'MATRICULES DUPLIQUÉS:' as INFO;
SELECT matricule, COUNT(*) as count 
FROM etudiant 
GROUP BY matricule 
HAVING COUNT(*) > 1;

-- 5. METTRE À JOUR LES MATRICULES DUPLIQUÉS
UPDATE etudiant 
SET matricule = CONCAT('UNI', LPAD(id_etudiant, 6, '0'))
WHERE matricule IS NULL 
   OR matricule = '' 
   OR matricule IN (
       SELECT temp.matricule 
       FROM (
           SELECT matricule 
           FROM etudiant 
           GROUP BY matricule 
           HAVING COUNT(*) > 1
       ) as temp
   );

-- 6. S'ASSURER QUE TOUS LES CHAMPS REQUIS SONT REMPLIS
UPDATE etudiant 
SET 
    nom = COALESCE(NULLIF(nom, ''), 'Nom_Inconnu'),
    prenom = COALESCE(NULLIF(prenom, ''), 'Prenom_Inconnu'),
    statut = COALESCE(NULLIF(statut, ''), 'actif'),
    niveau = COALESCE(NULLIF(niveau, ''), 'L1')
WHERE id_etudiant > 0;

-- 7. VÉRIFICATIONS FINALES
SELECT 'VÉRIFICATION FINALE - ÉTUDIANT ID=4:' as INFO;
SELECT id_etudiant, matricule, nom, prenom, date_naissance, sexe, niveau, statut 
FROM etudiant 
WHERE id_etudiant = 4;

SELECT 'TOUS LES ÉTUDIANTS:' as INFO;
SELECT id_etudiant, matricule, nom, prenom, date_naissance, sexe, niveau, statut 
FROM etudiant 
ORDER BY id_etudiant;

SELECT 'CORRECTION TERMINÉE - TESTEZ /Etudiants/Edit/4' as MESSAGE;
