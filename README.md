# L3 Web App - IIT School Management System (Phase 1)

Un système de gestion scolaire complet développé pour l'Institut International de Technologie (IIT).

## Vue d'ensemble

Ce projet représente la **Phase 1** d'un système de gestion scolaire complet incluant :
- Gestion des étudiants et des inscriptions
- Système de promotion automatique basé sur les moyennes
- Gestion des paiements et suivi des arriérés
- Services universitaires (cantine, transport, etc.)
- Tableau de bord avec statistiques en temps réel

## Prérequis

### Logiciels requis
- **.NET 8.0 SDK** ou version supérieure
- **MySQL Server** 8.0 ou version supérieure
- **Git** pour le contrôle de version

### Configuration système minimale
- Windows 10/11, Linux ou macOS
- 4 GB de RAM minimum
- 2 GB d'espace disque disponible

## Installation

### 1. Clonage du repository
```bash
git clone https://github.com/keizenx/L3_Web.git
cd L3_Web
```

### 2. Configuration de la base de données

#### Création de la base de données
```sql
CREATE DATABASE dbgjessetschoolman CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

#### Importation du schéma
```bash
mysql -u votre_utilisateur -p dbgjessetschoolman < IITWebApp/Scripts/create_database_phase1.sql
```

### 3. Configuration de l'application

#### Modifier le fichier de configuration
Éditez le fichier `IITWebApp/appsettings.json` :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=dbgjessetschoolman;User=root;Password=votre_mot_de_passe;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### 4. Installation des dépendances
```bash
cd IITWebApp
dotnet restore
```

### 5. Compilation et exécution
```bash
dotnet build
dotnet run
```

L'application sera accessible à l'adresse : `http://localhost:5000`

## Structure du projet

```
L3-Web-App/
├── IITWebApp/                 # Application ASP.NET Core MVC
│   ├── Controllers/          # Contrôleurs MVC
│   ├── Models/              # Modèles Entity Framework
│   ├── Views/               # Vues Razor
│   ├── Services/            # Services métier
│   ├── Data/               # Contexte de base de données
│   └── wwwroot/            # Ressources statiques (CSS, JS)
├── Scripts/                # Scripts SQL
├── Documentation/          # Documentation technique
└── README.md              # Ce fichier
```

## Fonctionnalités principales

### Gestion des Étudiants
- Inscription et gestion complète des profils étudiants
- Génération automatique de matricules
- Validation des données

### Système d'Inscriptions
- Inscription annuelle automatique
- Réinscription d'une année à l'autre
- Gestion des classes et filières

### Promotion Automatique
- Promotion L1 → L2 → L3 basée sur les moyennes
- Calcul automatique des seuils de passage
- Historique des promotions

### Gestion des Paiements
- Suivi des règlements et arriérés
- Calcul automatique des montants dus
- Alertes pour les paiements en retard

### Services Universitaires
- Gestion des services additionnels
- Calcul des revenus mensuels
- Suivi des souscriptions actives

## Utilisation

### Accès au système
1. Ouvrez votre navigateur web
2. Allez à l'adresse : `http://localhost:5000`
3. Utilisez le tableau de bord pour naviguer

### Comptes utilisateurs
- **Administrateur** : Accès complet à toutes les fonctionnalités
- **Gestionnaire** : Gestion des étudiants et inscriptions
- **Comptable** : Gestion des paiements uniquement

## Base de données

### Tables principales
- `etudiant` - Informations des étudiants
- `inscription` - Inscriptions annuelles
- `paiement` - Paiements et règlements
- `service` - Services universitaires
- `filiere` - Filières d'études
- `classe` - Classes et niveaux

### Données de test
Le système inclut des données de test pour faciliter les tests :
- 72 étudiants enregistrés
- 61 inscriptions pour l'année 2024-2025
- 16 arriérés de paiement à traiter

## Développement

### Structure des contrôleurs
- `HomeController` - Tableau de bord principal
- `EtudiantsController` - Gestion des étudiants
- `InscriptionsController` - Inscriptions et promotions
- `PaiementsController` - Gestion des paiements
- `ServicesController` - Services universitaires

### Services métier
- `MatriculeService` - Génération automatique des matricules
- `ApplicationDbContext` - Contexte Entity Framework

## Déploiement

### Production
1. Publier l'application : `dotnet publish -c Release`
2. Configurer le serveur web (IIS, Apache, Nginx)
3. Configurer la base de données de production
4. Définir les variables d'environnement

### Configuration recommandée pour la production
- Utiliser une base de données MySQL dédiée
- Configurer HTTPS avec un certificat SSL
- Mettre en place des sauvegardes régulières
- Configurer la journalisation appropriée

## Support et maintenance

### Logs
Les logs de l'application sont disponibles dans :
- Console (mode développement)
- Fichiers (mode production)

### Sauvegarde
Il est recommandé de sauvegarder régulièrement :
- La base de données MySQL
- Les fichiers de configuration
- Les logs d'application

## Licence

Ce projet est développé pour l'Institut International de Technologie (IIT).

## Contact

Pour toute question ou support technique :
- Équipe de développement IIT
- Support technique : support@iit.ci

---

**Version :** 1.0.0 (Phase 1)
**Dernière mise à jour :** Septembre 2025
**Développé avec :** ASP.NET Core, Entity Framework Core, MySQL
