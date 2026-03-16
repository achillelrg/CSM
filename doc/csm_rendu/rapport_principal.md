# Rapport principal - CSM Robot + Force

## Informations generales

- Titre du projet: Robot xArm + capteur de force
- Module: Systemes mecatroniques (Exp.)
- Equipe: [A completer]
- Date: [A completer]
- Depot de reference: version Martin (`7a74f4b`)

## Statut de lecture

- `Disponible`: element present dans le depot ou directement observable dans les aides de cours
- `A completer`: element prevu mais pas encore finalise dans le depot
- `A valider`: element qui necessite encore une verification experimentale

## Liens vers les annexes

- [Annexe IHM](annexes/ihm.md)
- [Annexe capteur et liaison serie](annexes/capteur_serie.md)
- [Annexe robot et reseau](annexes/robot_reseau.md)
- [Annexe integration Robot + Force](annexes/integration_robot_force.md)
- [Annexe tests et validation](annexes/tests_validation.md)
- [Annexe extraits de code a capturer](annexes/extraits_code.md)
- [Fonctions pertinentes](annexes/fonctions_pertinentes.txt)
- [Exemple CSV placeholder](annexes/exemple_csv.csv)
- [Checklist de rendu](checklist_rendu.md)

## 1. Introduction

Le module CSM vise la conception et l'integration de cellules robotisees integrant des capteurs d'environnement. Dans notre cas, le projet porte sur la prise en main d'un robot xArm, d'un capteur de force communiquant en serie, puis sur la preparation d'une application C# capable d'assurer une logique d'integration Robot + Force.

Le besoin cible du sujet est le suivant:

- piloter le robot xArm depuis un PC via le reseau
- piloter le capteur de force depuis le PC via une liaison serie
- extraire la valeur de force a partir du message de retour
- calculer un deplacement de l'outil, principalement sur l'axe Z
- enregistrer les essais dans un fichier CSV

Le present document est volontairement redige comme un rapport final partiel. Il structure l'ensemble du rendu final des maintenant, tout en distinguant clairement les elements deja disponibles, ceux qui restent a integrer et ceux qui devront etre valides en seance.

## 2. Cahier des charges et livrables attendus

D'apres `CSM.docx`, le rendu attendu est un ZIP contenant au minimum:

- un PDF de bilan du travail
- une description rapide de l'IHM
- le couplage Robot / Force
- l'asservissement en force
- une application de test mecanique
- des captures d'ecran de code commentees
- la gestion du capteur et de la liaison serie
- la gestion du bras robotique et du reseau
- les fonctions pertinentes dans des fichiers `.txt` si necessaire
- un exemple de fichier de test au format CSV

Le PDF devra egalement contenir la composition de l'equipe en debut de document.

## 3. Etat actuel du projet

### 3.1 Vue d'ensemble

Le depot actuellement retenu comme source de verite est celui de Martin. Il contient des briques de base et de demonstration, mais pas encore une application integree finale Robot + Force.

### 3.2 Brique capteur - `RS232-PC`

Statut: `Disponible`

La solution `demo/csharp_studio/RS232-PC` montre une base WinForms pour:

- configurer et ouvrir un port serie
- afficher les messages recus
- lancer une acquisition periodique
- parser des mesures de force dans les messages recus
- sauvegarder une acquisition dans un CSV

Cette brique constitue une base credible pour la partie capteur, mais elle n'integre pas encore le pilotage du robot.

### 3.3 Brique robot - `xARMForm`

Statut: `Disponible`

La solution `demo/csharp_studio/xARMForm` montre une base WinForms pour:

- creer une connexion vers le robot
- activer ou desactiver le mouvement
- lire les articulations et la pose
- lancer des deplacements `MoveBase`, `MoveTool` et `MoveHome`
- gerer certains reglages de collision
- executer un timer de demonstration

Cette brique constitue une base credible pour la partie robot, mais elle n'est pas fusionnee avec la brique capteur.

### 3.4 Integration Robot + Force

Statut: `A completer`

Les aides de cours (`HowTo_CSV.txt`, `HowTo_Timer.txt`, `rs232-vers-xarm.txt`, `xarm-vers-rs232.txt`) decrivent clairement la cible d'integration, mais cette integration n'est pas encore finalisee dans le depot actuel.

## 4. Architecture visee

L'architecture cible du projet repose sur les liens suivants:

- PC superviseur
- liaison serie entre PC et capteur de force
- liaison reseau entre PC et robot xArm

La logique fonctionnelle visee est:

1. demander une mesure au capteur
2. recevoir et parser la force
3. lire la position courante du robot
4. calculer un `Delta Z`
5. envoyer un `MoveTool`
6. journaliser les donnees dans une liste
7. exporter les donnees en CSV

Statut: `A completer`

## 5. Brique capteur

### 5.1 Materiel et protocole

Statut: `Disponible`

Le sujet mentionne:

- un capteur en S de 100 kg
- un module HX711
- une carte FireBeetle ESP32-E

Le protocole serie attendu est simple et base sur des commandes mono-caractere:

- `C`: debut de calibration
- `Q`: fin de calibration
- `M`: mesure
- `T`: tare
- `U`: changement d'unite

Le format de retour attendu pour une mesure est du type:

```text
Reading: -20.032 Kg
```

### 5.2 Ce qui est observable dans le depot

Statut: `Disponible`

Le projet `RS232-PC` contient deja:

- la gestion des ports serie
- la lecture asynchrone des donnees
- l'affichage du flux recu
- une logique d'acquisition periodique
- un export CSV

### 5.3 Ce qui reste a completer

Statut: `A completer`

- verrouiller le parsing final retenu pour le rendu
- definir la gestion du cas unite `Kg` / `N`
- preparer les captures d'ecran utiles pour l'annexe IHM
- valider les essais avec le capteur reel

## 6. Brique robot

### 6.1 Pilotage xArm

Statut: `Disponible`

Le projet `xARMForm` fournit deja:

- la creation de la connexion
- l'activation du mouvement
- la lecture de la pose et des articulations
- les deplacements principaux
- un premier usage de `MoveTool`

### 6.2 Contraintes de mise en oeuvre

Statut: `Disponible`

Le sujet rappelle plusieurs contraintes:

- le pilotage du robot se fait par reseau
- un seul utilisateur peut etre connecte au controleur a la fois
- il faut etre connecte au bon reseau de la salle

### 6.3 Ce qui reste a completer

Statut: `A completer`

- decrire la cellule et le robot reel utilises par le groupe
- documenter les premiers essais reels
- integrer la brique robot dans l'application finale unique

## 7. Integration Robot + Force

### 7.1 Principe cible

Statut: `Disponible`

Les documents du cours decrivent une cible claire:

- acquisition d'une force
- lecture de la position robot
- calcul d'un deplacement sur Z
- application de la commande
- enregistrement du couple force / position

### 7.2 Etat actuel

Statut: `A completer`

A la date du present rapport, le depot contient des briques separées mais pas encore l'application fusionnee finale. Le travail d'integration reste donc un objectif de developpement, pas un resultat final deja etabli.

### 7.3 Travail restant

Statut: `A completer`

- migration de `Robot.cs` et `XArmAPI.cs` dans la base `RS232-PC`
- ajout des commandes de deplacement dans le repere TOOL
- mise en place d'un timer commun
- enregistrement combine force / position
- validation experimentale du `Delta Z`

## 8. Test mecanique et asservissement

### 8.1 Test mecanique

Statut: `Disponible` pour le principe, `A completer` pour la validation finale

Le sujet demande un mode de test mecanique base sur un timer, une mesure de force, un enregistrement de `PositionZ`, puis un export CSV. Les aides `HowTo_Timer.txt` et `HowTo_CSV.txt` fournissent deja un squelette methodologique exploitable.

### 8.2 Asservissement en force

Statut: `A completer`

Le besoin final vise un pilotage en effort sur Z. A ce stade, cette logique doit etre decrite dans le rapport comme cible technique, sans pretendre qu'elle est deja finalisee si le depot ne la contient pas encore.

### 8.3 Formulation retenue pour le rapport

Pour rester honnete et academique, les formulations suivantes sont a privilegier:

- "La logique visee est..."
- "L'integration finale sera validee apres fusion..."
- "A ce stade, la base disponible permet deja..."

## 9. Validation experimentale

Statut: `A valider`

La section de validation du rapport principal devra comporter:

- les tests deja realises sur les briques separees
- les tests d'integration a realiser plus tard
- les captures d'ecran finales
- les CSV d'essais
- les remarques sur les limites et la robustesse

Le tableau detaille est prepare dans l'annexe `tests_validation.md`.

## 10. Limites et suites

Statut: `Disponible`

Les limites actuelles sont les suivantes:

- depot de travail encore base sur des briques separees
- integration finale non terminee
- validation dependante du materiel reel et de l'acces robot
- preuves experimentales finales non encore produites

Les suites attendues sont:

- finaliser l'application integree
- executer les essais reels
- inserer les captures et resultats dans les annexes
- exporter ensuite le PDF final du rendu

## 11. Conclusion

Le depot actuel fournit une base technique exploitable pour les deux briques necessaires au projet: la communication serie avec le capteur et le pilotage reseau du robot xArm. En revanche, l'application integree Robot + Force, le test mecanique complet et l'asservissement en effort restent encore a finaliser. Le present dossier Markdown permet donc de commencer des maintenant le rendu de groupe, tout en laissant des emplacements clairs pour les preuves experimentales et les compléments de developpement attendus.
