# RanceConnect

## Description

Ce projet a pour but de créer un logiciel de gestion des stocks pour une grande surface. 

L'objectif est d'avoir une application de bureau qui permet de visualiser et gérer les stocks de produits, en prenant en compte les dates de péremptions, de prévenir les utilisateurs lorsque ces dates approchent, voire de passer automatiquement des commandes quand des produits vont périmer ou que le stock est bas, jusqu'à atteindre un seul donné. Des logs sont également stockés pour toutes les interactions.

## Membres

Les membres du projet sont
- Louis de Lavenne de Choulot de Chabaud-la-Tour (front-end)
- Quentin Lemaire (front-end)
- Maxime Pages (back-end)

## Frameworks & libraries

- .NET 8
- WPF
- LightDB
- PowerSerializer
- Docker

## Fonctionnalités

### Gestion des produits

- Un produit est défini par un EAN, un nom, un prix, etc (voir diagramme UML), mais surtout des règles et des catgories
- Une fenêtre affiche tous les produits en stocks, ainsi que certaines informations importantes
- Une fenêtre dédiée affiche toutes les informations sur un produit, et permet d'accéder aux catégories
    - Recherche de produits (par nom, par prix, etc) dans cette fenêtre
    - filtres par catégorie
- Un produit peut avoir plusieurs catégories, et ces dernières peuvent être modifiées facilement grâce à une fenêtre dédiée
    - affichage de tous les attributs
    - modification des attributs
    - accès aux catégories
- Implémentation des méthodes CRUD pour les produits
- Gestion des dates de péremption
- Gestion des règles sur les produits
    - la page dédiée aux produits permet de définir des règles sur les quotas et les dates de péremptions facilement, grâce à trois champs spécifiques

### Gestion des catégories

- Une catégorie contient un nom et des règles
- La logique des règles des catégories n'est pas implémentée, mais la gestion en mémoire et dans la DB est déjà présente
    - l'implémentation de la logique pourra se faire rapidement en prenant comme modèle la logique des règles sur les produits
- pas de page dédiée aux catégories, on les modifie directement depuis la page "Produit"
- une page spécifique pourra être créée à l'avenir, si le besoin s'en fait sentir, mais le popup actuel est suffisant pour le CRUD

### Gestion des règles

- Il est possible d'ajouter des règles aux produits (et bientôt aux catégories) grâce aux champs dédiés sur la page "Produit"
- Il y a deux types de règles :
    - Péremption : le nombre de jours avant la date de péremption à partir duquel on crée une alerte
    - Quotas : le seuil minimum à partir duquel on crée une alerte (et/ou on passe une commande), ainsi que le seuil maximum à partir duquel on arrête de passer des commandes
- Ces règles ont pour but de créer des alertes automatiquement lorsqu'on atteint un seuil ou une date de péremption

### Gestion des dates de péremption

- La page produit répertorie les différents lots avec leurs dates de péremption pour un produit donné.
- on peut supprimer un lot si tous les produits de ce dernier ont été vendus, ce qui permet de supprimer l'alerte de péremption pour ce lot, le cas échéant.

### Gestion des alertes

- Les alertes sont surtout caractérisées par un nom, un type, et l'EAN du produit concerné
- Elles sont affichées sur la page principale, ainsi que sur leur page dédiée
- un bouton permet de se rendre rapidement ssur la page du produit concerné
- Les alertes sont de deux types :
    - quand un produit a une quantité en stock inférieure à son seuil défini dans ses règles
    - quand la date dépasse le seuil défini dans un produit avant sa date de péremtpion (ou qu'on a dépassé cette date de péremption)

###  Gestion des logs

- Chaque requête au serveur crée un log dans la base de donnée
- un log contient un nom et une date
- à l'avenir, on pourra également ajouter des catégories de logs, avec différents degrés de priorité

## Installation

 ### Serveur

Il suffit de télécharger, dans la branche server du github, le fichier Makefile ainsi que le fichier dockerfile, puis d'exécuter la commande make.

Le script se chargera ensuite de télécharger de lui même le reste des dépendances, et de lancer le docker qui fait tourner le serveur.

### Client

Un fichier .exe est fourni dans l'onglet release, il duffit de le télécharger et de le lancer. Il faut cependant avoir un serveur qui tourne auquel se connecter, car certaines exceptions ne sont pas encore gérées.

## Utilisation

Le programme est plutôt simple d'utilisation. 

La page principale affiche les stocks et les alertes.

La page stocks affiche avec plus de détails tous les produits du stock, et permet de se rendre sur une page d'un produit spécifique.

La page produit permet de créer des règles et de modifier les informations d'un produit spécifique, ainsi que d'accéder au popup catagories. Elle affiche également les différents lots de livraisons de ce produit, avec leurs dates de péremption, pour la gestion des alertes.

Le popup catégories permet de voir toutes les catégories, d'en créer et d'en supprimer, ainsi que de rajouter et retirer des catégories au produit spécifique que l'on manipule.

La page alerte affiche toutes les alertes actuellement en cours, et permet de se rendre rapidement sur la page du produit concerné.

La page logs affiche tous les logs de l'application, avec leur date et leur description.