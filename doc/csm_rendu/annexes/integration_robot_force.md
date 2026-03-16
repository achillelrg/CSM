# Annexe - Integration Robot + Force

## Objet

Cette annexe decrit la cible d'integration Robot + Force et l'ecart entre cette cible et le depot actuel.

## 1. Cible fonctionnelle

Statut: `Disponible`

L'integration attendue par le sujet est:

1. demander une mesure de force
2. extraire la valeur numerique
3. lire la position courante du robot
4. calculer un `Delta Z`
5. envoyer une commande `MoveTool`
6. enregistrer les donnees
7. exporter un CSV

## 2. Etat actuel du depot

Statut: `A completer`

Le depot de Martin contient des briques separees:

- `RS232-PC` pour la serie et l'acquisition
- `xARMForm` pour le pilotage robot

Il ne contient pas encore, a ce stade, l'application integree finale unique qui fusionne les deux.

## 3. Travail restant identifie

Statut: `A completer`

- reprendre `Robot.cs` et `XArmAPI.cs` dans la base `RS232-PC`
- raccorder les commandes robot a l'IHM principale
- ajouter les deplacements dans le repere TOOL
- coupler acquisition capteur et lecture robot
- calculer puis appliquer un `Delta Z`
- centraliser les donnees pour l'export CSV

## 4. Positionnement dans le rapport

Statut: `Disponible`

Cette annexe doit rester honnete:

- ne pas pretendre que l'integration finale est deja aboutie
- documenter precisement la cible
- expliquer ce qui manque encore
- preparer le terrain pour les captures et preuves qui seront ajoutees plus tard

## 5. Emplacements a completer plus tard

- `[Schema de flux capteur -> parsing -> robot]`
- `[Capture application integree]`
- `[Description du Delta Z retenu]`
