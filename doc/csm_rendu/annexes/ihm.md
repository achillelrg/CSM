# Annexe - IHM

## Objet

Cette annexe decrit les interfaces graphiques actuellement disponibles dans le depot de Martin et liste les captures a produire plus tard pour le rendu final.

## Etat actuel

### 1. Interface `RS232-PC`

Statut: `Disponible`

Fonctions visibles dans la base actuelle:

- ouverture et fermeture du port serie
- configuration des parametres de liaison
- affichage des messages recus
- envoi de commandes manuelles
- acquisition periodique
- export CSV

Captures a prevoir plus tard:

- vue generale de la fenetre
- zone de configuration serie
- zone d'affichage des messages recus
- sequence d'acquisition
- export CSV

### 2. Interface `xARMForm`

Statut: `Disponible`

Fonctions visibles dans la base actuelle:

- creation de la connexion robot
- activation du mouvement
- lecture des articulations
- lecture de la pose
- commandes `MoveBase`, `MoveTool`, `MoveHome`
- timer de demonstration
- collision sensitivity

Captures a prevoir plus tard:

- vue generale de la fenetre
- lecture pose / joints
- commande `MoveTool`
- timer

## Etat cible

Statut: `A completer`

Le rendu final devra idealement montrer une application integree unique. A ce stade, l'annexe decrit donc les deux briques separees et reserve un emplacement pour la future IHM fusionnee.

## Emplacements a completer plus tard

- `[Capture RS232-PC]`
- `[Capture xARMForm]`
- `[Capture application integree finale si disponible]`
