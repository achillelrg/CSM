# TP CSM - Guide d'installation, d'utilisation et de test

## 1. Objet du document

Ce document explique comment:

- recuperer le projet
- preparer l'environnement de developpement
- comprendre les dependances utiles
- lancer l'application
- tester le capteur de force avec l'application
- tester la connexion robot xArm
- executer les deux modes d'essai (`Mechanical test` et `Force control`)

Le projet est prevu pour le **TP CSM Robot + Force** avec un **xArm6 (6 axes)**.

## 2. Structure du projet

Le depot GitHub contient une solution Visual Studio:

- `csharp_studio.sln`: solution a ouvrir
- `RS232-PC/`: projet principal WinForms
- `RS232-PC/Interop/`: wrapper C# pour l'API xArm
- `RS232-PC/Native/xarm.dll`: DLL native du constructeur, deja embarquee

Le projet principal contient:

- l'IHM
- la gestion de la liaison serie du capteur
- la gestion de la connexion reseau robot
- le jog manuel en repere TOOL
- le mode `Mechanical test`
- le mode `Force control`
- l'export CSV

## 3. Dependances necessaires

### 3.1 Logiciels a installer

Pour utiliser et recompiler le projet, il faut:

1. **Windows**
   - Le projet est un projet WinForms C# prevu pour Windows.

2. **Visual Studio 2022**
   - Edition Community, Professional ou Enterprise.

3. **Charge de travail Visual Studio**
   - Installer `Developpement d'applications de bureau .NET`.

4. **.NET Framework 4.8 Developer Pack / Targeting Pack**
   - Le projet cible `.NET Framework 4.8`.
   - Si Visual Studio affiche une erreur de ciblage a l'ouverture ou a la compilation, il faut installer le pack de developpement `.NET Framework 4.8`.

5. **Git**
   - Necessaire seulement pour cloner / pull / push.

### 3.2 Materiel utile

Selon la phase de test:

- un **robot xArm6**
- un **capteur de force reel** avec son interface serie
- ou un **emulateur capteur** (ESP32 / Arduino UNO suivant votre materiel)
- un PC connecte:
  - au robot via le **reseau**
  - au capteur via le **port serie**

### 3.3 Dependances deja incluses dans le depot

Les dependances suivantes sont **deja dans le depot**:

- le wrapper C# xArm:
  - `RS232-PC/Interop/Robot.cs`
  - `RS232-PC/Interop/XArmAPI.cs`
- la DLL native xArm:
  - `RS232-PC/Native/xarm.dll`

Il n'y a donc **rien a telecharger ni a copier manuellement** pour l'API xArm si vous utilisez le depot tel quel.

Le fichier `xarm.dll` est automatiquement copie dans le dossier de sortie a la compilation.

## 4. Ou placer le projet

Le cours indique de placer le projet sous `C:\WORKSPACE` pour eviter certains problemes lies a l'antivirus.

Recommande:

- cloner dans un chemin du type:
  - `C:\WORKSPACE\CSM`

## 5. Recuperation du projet

### 5.1 Clonage initial

```bash
git clone https://github.com/achillelrg/CSM.git
```

### 5.2 Mise a jour

```bash
git pull
```

## 6. Ouverture du projet

1. Ouvrir `csharp_studio.sln` dans Visual Studio.
2. Choisir la configuration:
   - `Debug | x64`
3. Definir `RS232-PC` comme projet de demarrage si besoin.
4. Compiler la solution.

## 7. Lancement de l'application

L'application se lance depuis Visual Studio ou directement avec:

- `RS232-PC\bin\Debug\x64\RS232-PC.exe`

L'IHM contient cinq onglets:

- `Connexions`
- `Capteur`
- `Robot`
- `xARM Base`
- `Essais`

## 8. Comportement general de l'application

### 8.1 Onglet `Connexions`

Il permet:

- de choisir et ouvrir le port serie du capteur
- de saisir l'adresse IP du robot
- de se connecter au robot
- d'activer ou desactiver le mouvement
- de consulter le journal systeme

### 8.2 Onglet `Capteur`

Il permet:

- de voir le log serie brut
- de voir la derniere mesure interpretee
- d'envoyer les commandes capteur:
  - `M`
  - `C`
  - `Q`
  - `T`
  - `U`

Le parsing reconnait les lignes du type:

```text
Reading: -20.032 Kg
```

### 8.3 Onglet `Robot`

Il permet:

- de lire la pose courante
- de lire les articulations courantes
- de faire du jog manuel en repere TOOL:
  - `+X / -X`
  - `+Y / -Y`
  - `+Z / -Z`
- d'envoyer:
  - `Move home`
  - `Reset`
  - `Emergency stop`

### 8.4 Onglet `xARM Base`

Cet onglet reprend les fonctions principales de l'application xARM d'origine, afin d'eviter une regression fonctionnelle cote robot.

Il permet:

- de lancer `Create xARM`
- d'activer ou couper `Motion xARM`
- de lancer `Reset xARM`
- de lire:
  - `Get Joint`
  - `Get Position`
  - `Get Base`
  - `Get TCP`
- de lancer:
  - `Move Home`
  - `Move Base`
  - `Move TCP`
  - `Move Angle`
- de gerer:
  - `Get Collision Sensitivity`
  - `Set Collision Sensitivity`
  - `Self Collision Detection`
- de tester le timer xARM historique

### 8.5 Onglet `Essais`

Il permet:

- de regler le timer
- de regler les seuils de securite
- de choisir le fichier CSV
- de lancer:
  - `Mechanical test`
  - `Force control`
- de visualiser les echantillons en tableau

## 9. Procedure de test du capteur avec l'application

### 9.1 Test capteur seul avec emulateur

Objectif:

- verifier la liaison serie
- verifier le parsing de la mesure
- verifier que l'IHM affiche correctement la valeur

Procedure:

1. Brancher l'emulateur capteur au PC.
2. Identifier le port `COM` dans le gestionnaire de peripheriques Windows.
3. Lancer l'application.
4. Aller dans `Connexions`.
5. Selectionner le bon port serie.
6. Cliquer sur `Open`.
7. Aller dans `Capteur`.
8. Verifier que le boot apparait dans `Raw serial log`.
9. Cliquer sur `Measure (M)`.
10. Verifier:
    - qu'une ligne `Reading: ... Kg` apparait dans le log
    - que la valeur `Force` est mise a jour
    - que `Raw line` est mise a jour

Resultat attendu:

- la mesure apparait sans erreur
- l'application ne plante pas
- l'unite affichee est cohérente (`Kg` dans le cas de l'emulateur)

### 9.2 Test capteur reel

Objectif:

- verifier le capteur reel
- verifier les commandes de service

Procedure:

1. Brancher le capteur reel.
2. Ouvrir le bon port serie dans `Connexions`.
3. Aller dans `Capteur`.
4. Tester successivement:
   - `Measure (M)`
   - `Tare (T)` si necessaire
   - `Change unit (U)` si necessaire
5. Verifier a chaque fois le retour dans `Raw serial log`.

Remarque:

- `C` et `Q` sont surtout utiles pour la calibration selon le firmware utilise.
- En exploitation normale, la commande principale est `M`.

## 10. Procedure de test du robot

### 10.1 Preconditions

- etre connecte au **bon reseau du robot**
- connaitre la **bonne adresse IP du robot**
- verifier qu'**un seul utilisateur** pilote le robot a ce moment-la
- s'assurer que la zone robot est libre et securisee

Ne pas publier dans le depot public:

- mots de passe Wi-Fi
- informations reseau sensibles de la salle

Ces informations doivent etre fournies en seance ou via les documents internes du cours.

### 10.2 Test de connexion robot

Procedure:

1. Aller dans `Connexions`.
2. Saisir l'IP du robot.
3. Cliquer sur `Connect`.
4. Verifier que l'etat passe a `Connected`.
5. Cliquer sur `Enable motion`.

Resultat attendu:

- le robot est connecte
- le mouvement est autorise
- l'application peut lire la pose et les joints

### 10.3 Test jog TOOL

Procedure:

1. Aller dans `Robot`.
2. Cliquer sur `Refresh state`.
3. Regler un pas faible si necessaire.
4. Tester:
   - `+X`
   - `-X`
   - `+Y`
   - `-Y`
   - `+Z`
   - `-Z`

Resultat attendu:

- le robot bouge dans le repere TOOL
- la pose courante est mise a jour
- les articulations sont mises a jour

## 11. Procedure de test `Mechanical test`

Objectif:

- faire un essai simple `force / position Z`
- enregistrer les echantillons
- exporter un CSV exploitable

Procedure:

1. Ouvrir la liaison serie capteur.
2. Connecter le robot.
3. Activer le mouvement robot.
4. Aller dans `Essais`.
5. Regler:
   - `Timer (ms)`
   - `Delta Z / tick`
   - `Force threshold`
   - `Z safety window`
6. Cliquer sur `Select file` et choisir un CSV de sortie.
7. Cliquer sur `Start mechanical test`.
8. Observer:
   - le tableau des echantillons
   - la variation de `PositionZ`
   - la variation de `Force`
9. Cliquer sur `Stop run` ou laisser l'arret automatique se produire.
10. Cliquer sur `Export CSV`.

Resultat attendu:

- un echantillon est ajoute a chaque tick
- la force et la position Z sont journalisees
- le test s'arrete si un seuil est depasse

## 12. Procedure de test `Force control`

Objectif:

- approcher un asservissement externe en effort sur `Z`
- utiliser la mesure du capteur pour calculer `Delta Z`

### 12.1 Principe

L'application **n'implemente pas un modele inverse externe du robot**.
Elle fonctionne avec une logique d'asservissement externe approchee:

- lecture de la force
- filtrage
- calcul de l'erreur a la consigne
- calcul d'un `Delta Z`
- envoi d'un `MoveTool` relatif sur `Z`

### 12.2 Procedure

1. Ouvrir la liaison serie capteur.
2. Connecter le robot.
3. Activer le mouvement robot.
4. Aller dans `Essais`.
5. Regler:
   - `Setpoint`
   - `Kp`
   - `Ki`
   - `Kd`
   - `Filter alpha`
   - `Max Delta Z`
   - `Force threshold`
   - `Z safety window`
6. Cocher `Invert Z command` si la reaction du robot est dans le mauvais sens.
7. Cliquer sur `Start force control`.
8. Observer:
   - la convergence de la force
   - la variation de `Delta Z`
   - les arrets eventuels sur seuil

### 12.3 Precautions

- commencer avec de petits gains
- commencer avec un `Max Delta Z` faible
- verifier qu'un arret manuel est toujours possible
- utiliser `Emergency stop` en cas de doute

## 13. Format du CSV

Le CSV exporte reprend directement les colonnes du modele `MeasurementSample`:

- `Timestamp`
- `Mode`
- `Force`
- `Unit`
- `FilteredForce`
- `Setpoint`
- `Error`
- `DeltaZ`
- `PositionZ`
- `RawLine`

Ce format est prevu pour:

- l'analyse experimentale
- les graphes force / position
- le rendu de TP

## 14. Cas d'erreur frequents

### 14.1 Le port serie ne s'ouvre pas

Verifier:

- que le bon `COM` est selectionne
- qu'un autre logiciel n'utilise pas deja le port
- que le pilote USB / serie du microcontroleur est bien installe

### 14.2 La mesure ne remonte pas

Verifier:

- que le firmware du capteur repond bien a `M`
- que le log brut contient une ligne `Reading: ...`
- que le capteur reel est correctement alimente

### 14.3 Le robot ne se connecte pas

Verifier:

- l'adresse IP
- le reseau Wi-Fi / Ethernet actif
- l'absence d'un autre utilisateur deja connecte

### 14.4 Le robot ne bouge pas

Verifier:

- que `Connect` a reussi
- que `Enable motion` a ete fait
- que l'application n'est pas dans un etat d'erreur

### 14.5 Erreur de compilation Visual Studio

Verifier:

- que Visual Studio a bien la charge de travail `.NET desktop`
- que le pack `.NET Framework 4.8 Developer Pack` est installe

## 15. Conseils de validation avant seance

Avant d'aller sur le robot reel, valider dans cet ordre:

1. compilation du projet
2. ouverture de l'application
3. test du capteur avec l'emulateur
4. test du capteur reel
5. connexion robot seule
6. jog manuel TOOL
7. test mecanique
8. asservissement en force

## 16. Fichiers importants a connaitre

- `README.md`
- `TP_Utilisation_CSM.md`
- `RS232-PC/MainForm.cs`
- `RS232-PC/MainForm.UI.cs`
- `RS232-PC/MainForm.Actions.cs`
- `RS232-PC/Services/SerialForceSensorService.cs`
- `RS232-PC/Services/XArmRobotService.cs`
- `RS232-PC/Services/ForceControlLoop.cs`
- `RS232-PC/Models/MeasurementSample.cs`
