# ms2d_projet_napaj
# Projet de Gestion de Pâtisserie avec ASP.NET Core

Ce projet vise à créer un système de gestion de pâtisserie en utilisant ASP.NET Core. Il permet d'authentifier les utilisateurs, importer les prix d'achat des ingrédients depuis un fichier CSV, gérer les informations sur les ingrédients, et exporter les prix des ingrédients au format CSV.

## Table des matières

1. [Installation](#installation)
2. [Utilisation](#utilisation)
3. [Modèle de Données](#modèle-de-données)
4. [Contribuer](#contribuer)
5. [Licence](#licence)

## Installation

1. Clônez le dépôt : `git clone https://github.com/votreutilisateur/projet-patisserie-aspnetcore.git`
2. Accédez au répertoire du projet : `cd projet-patisserie-aspnetcore`
3. Configurez votre base de données dans le fichier `appsettings.json`.
4. Ouvrez une ligne de commande dans le répertoire du projet et exécutez la commande : `dotnet restore`

## Utilisation

1. Lancez l'application : `dotnet run`
2. Accédez à l'application dans votre navigateur à l'adresse [http://localhost:5000](http://localhost:5000)

## Modèle de Données

Le projet utilise une base de données avec les tables suivantes :
- `Utilisateurs` : Pour gérer l'authentification.
- `Prix d'Achat` : Pour importer et enregistrer les prix d'achat des ingrédients.
- `Ingrédients` : Pour stocker les informations sur les ingrédients.
- `Recettes` (à ajouter ultérieurement) : Pour gérer les recettes.
- `Détail de la Recette` (à ajouter ultérieurement) : Pour définir les ingrédients et quantités dans les recettes.
- `Export au format CSV` : Pour enregistrer les exports des prix des ingrédients au format CSV.

## Contribuer

Les contributions sont les bienvenues ! Si vous souhaitez contribuer au projet, veuillez suivre ces étapes :
1. Fork du dépôt
2. Créer une branche pour votre contribution : `git checkout -b fonctionnalite-nouvelle`
3. Effectuez vos modifications
4. Commitez et poussez vos changements : `git commit -m "Ajout d'une nouvelle fonctionnalité"`
5. Soumettez une demande de tirage (Pull Request) vers le dépôt d'origine

## Licence

Ce projet est sous licence MIT. Voir le fichier [LICENSE](LICENSE) pour plus de détails.
