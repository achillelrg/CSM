# Annexe - Robot et reseau

## Objet

Cette annexe decrit la brique robot, les primitives principales observees dans le depot et les contraintes de connexion.

## 1. Brique robot disponible

Statut: `Disponible`

Le projet `demo/csharp_studio/xARMForm` fournit une base C# permettant:

- la creation de la connexion robot
- l'activation du mouvement
- la lecture de la pose courante
- la lecture des articulations courantes
- des deplacements cartesiens et articulaires
- une demonstration avec timer

## 2. Primitives utiles pour le rendu

Statut: `Disponible`

Exemples de primitives deja presentes:

- `Create`
- `EnableMotion`
- `GetCurrentPosition`
- `GetCurrentJoint`
- `MoveTool`
- `MoveBase`
- `MoveHome`

## 3. Contraintes reseau

Statut: `Disponible`

Le sujet rappelle:

- connexion reseau vers le controleur xArm
- obligation d'etre sur le bon reseau
- un seul utilisateur connecte a la fois

## 4. Elements a completer

Statut: `A completer`

- robot reel effectivement utilise par l'equipe
- premiers tests reseau reel
- captures d'ecran de connexion
- remarques de securite observables en seance

## 5. Resultats a inserer plus tard

Statut: `A valider`

- capture de connexion reussie
- capture lecture pose / joints
- capture d'un `MoveTool` sur l'axe voulu
