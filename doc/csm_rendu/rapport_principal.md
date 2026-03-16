# Rapport principal - CSM Robot + Force

## Informations generales

Le present rendu concerne le projet d'integration d'un robot xArm et d'un capteur de force dans le cadre du module *Systemes mecatroniques (Exp.)*. Le rapport est redige a partir de l'etat reel du depot de travail actuellement retenu comme reference technique. Les noms des membres de l'equipe et la date precise de remise seront ajoutes dans cette section avant depot final.

Ce document adopte volontairement une posture de rapport final partiel. Il ne cherche pas a presenter comme acheve un developpement qui ne l'est pas encore. Lorsqu'un element est deja visible dans le depot ou dans les documents fournis, il est considere comme `Disponible`. Lorsqu'un element releve d'une integration encore en cours, il est note `A completer`. Enfin, tout ce qui depend encore d'une verification sur le materiel reel est marque `A valider`.

Les details techniques sont developpes dans les annexes suivantes : [IHM](annexes/ihm.md), [capteur et liaison serie](annexes/capteur_serie.md), [robot et reseau](annexes/robot_reseau.md), [integration Robot + Force](annexes/integration_robot_force.md), [tests et validation](annexes/tests_validation.md) et [extraits de code](annexes/extraits_code.md). Les fichiers [fonctions_pertinentes.txt](annexes/fonctions_pertinentes.txt), [exemple_csv.csv](annexes/exemple_csv.csv) et [checklist_rendu.md](checklist_rendu.md) completent ce dossier de rendu.

## 1. Introduction

Le travail demande dans le cadre du module CSM porte sur une cellule robotisee composee d'un robot xArm, d'un capteur de force et d'un poste PC charge d'assurer le pilotage logiciel. L'objectif pedagogique n'est pas seulement de prendre en main deux equipements distincts, mais surtout de construire une logique d'integration dans laquelle une mesure de force acquise en serie peut etre exploitee pour commander un mouvement robotique, enregistrer des essais et preparer une analyse experimentale.

Dans sa forme cible, l'application doit permettre d'etablir une communication reseau avec le robot, de communiquer en serie avec le capteur, de parser correctement la mesure retournee, puis d'utiliser cette information pour calculer un deplacement de l'outil, principalement sur l'axe Z. Le projet doit egalement produire des traces exploitables sous forme de captures, d'extraits de code et de fichiers CSV, afin d'alimenter le bilan technique final.

## 2. Cahier des charges et livrables attendus

Les consignes de `CSM.docx` demandent un dossier de rendu suffisamment complet pour decrire la demarche du groupe, l'interface developpee, la gestion des communications, le couplage Robot / Force, ainsi que les premiers elements d'asservissement et de test mecanique. Le rendu final devra prendre la forme d'un dossier compresse contenant au minimum un rapport PDF, des annexes techniques, des extraits de code commentes et un exemple de fichier CSV issu des essais.

L'attendu ne se limite donc pas a un programme qui compile. Il faut egalement fournir une explication claire de l'architecture retenue, des choix techniques realises pour la liaison serie et la liaison reseau, des fonctions mobilisees pour piloter le robot, ainsi que de la facon dont les mesures de force sont utilisees dans la logique de commande. Le rapport doit enfin rester honnete sur l'etat d'avancement reel du projet.

## 3. Etat actuel du projet

L'etat actuel du depot montre deux briques distinctes qui constituent une base de travail exploitable, mais qui ne forment pas encore l'application finale integree. La premiere brique est la solution `RS232-PC`, qui couvre la communication serie avec le capteur. La seconde est `xARMForm`, qui sert de base de demonstration pour la connexion et les mouvements du robot.

La brique `RS232-PC` est deja pertinente pour la suite du projet car elle permet d'ouvrir un port serie, de recevoir des messages de facon asynchrone, d'afficher le flux recu et d'illustrer un premier mecanisme d'acquisition periodique. Elle fournit egalement un point d'appui utile pour l'export CSV. De son cote, `xARMForm` montre comment etablir une connexion vers le robot, activer le mouvement, lire la pose et les articulations, puis envoyer des commandes telles que `MoveBase`, `MoveTool` ou `MoveHome`.

En revanche, le depot ne contient pas encore, a la date du present rapport, une application unique dans laquelle la mesure de force et la commande robot sont reellement fusionnees dans une meme boucle de traitement. Cette limite est centrale pour la suite du rapport : les briques de base sont `Disponibles`, mais l'integration Robot + Force reste `A completer`.

## 4. Architecture visee

L'architecture cible repose sur un poste PC jouant le role de superviseur logiciel, une liaison serie entre le PC et le capteur de force, et une liaison reseau entre le PC et le robot xArm. Dans cette organisation, le PC concentre la logique applicative : il recueille la mesure du capteur, l'interprete, lit l'etat du robot, calcule la correction necessaire puis envoie la commande de mouvement.

Sur le plan fonctionnel, la chaine de traitement attendue est la suivante. Une mesure est d'abord demandee au capteur, puis la valeur numerique est extraite du message retourne. La position courante du robot est ensuite lue afin de disposer du contexte mecanique de l'essai. Le programme peut alors calculer un `Delta Z`, appliquer une commande `MoveTool` et enregistrer simultanement les donnees de force et de position. Cette architecture est deja clairement decrite dans les supports du cours, mais son implementation complete reste `A completer`.

## 5. Brique capteur

La partie capteur s'appuie sur un ensemble materiel relativement classique pour ce type de TP : un capteur de force en S, un module HX711 et une carte FireBeetle ESP32-E. Le protocole expose dans les documents du cours repose sur des commandes courtes telles que `C`, `Q`, `M`, `T` et `U`. La structure de reponse attendue pour une mesure est du type `Reading: -20.032 Kg`, ce qui permet d'envisager un parsing direct de la valeur et de l'unite.

Du point de vue logiciel, la base `RS232-PC` montre deja les elements essentiels pour une integration future. Le port serie est configurable, le debit de `115200` bauds est prevu, la reception est geree de facon asynchrone et l'acquisition periodique est deja amorcee. Cette base est donc `Disponible` pour la prise en main du capteur. En revanche, plusieurs points restent `A completer`, notamment la validation finale du parsing sur le flux reel, la gestion explicite des unites, ainsi que la production de preuves experimentales a inserer dans les annexes.

## 6. Brique robot

La partie robot s'appuie sur le projet `xARMForm`, qui constitue une base de demonstration pour la communication avec le robot xArm. Cette base montre comment creer une connexion, activer le mouvement, interroger la pose courante, lire les articulations et lancer des deplacements cartesiens ou articulaires. Elle couvre donc deja l'essentiel des primitives qui seront necessaires dans l'application finale.

Les documents de cours rappellent toutefois que cette brique doit etre utilisee dans un contexte contraint. Le robot est pilote par reseau, il faut donc disposer du bon acces en salle, et un seul utilisateur peut etre connecte au controleur a la fois. Ces contraintes pratiques ne sont pas accessoires : elles conditionnent la possibilite meme de valider la partie experimentale. Ainsi, la brique robot est `Disponible` comme socle technique, mais sa validation complete en situation reelle reste `A valider`.

## 7. Integration Robot + Force

L'integration attendue par le sujet consiste a faire dialoguer les deux briques precedentes dans une meme application. Le principe est simple dans son enonce : demander une mesure au capteur, extraire la force, lire la position robot, calculer une correction puis l'envoyer au robot. En pratique, cette integration suppose de reprendre les elements utiles du wrapper robot dans la base serie, de structurer une boucle de traitement commune et de centraliser les donnees utiles pour les essais.

A ce stade, le depot ne contient pas encore cette application fusionnee. Il convient donc de presenter cette section comme une cible de developpement et non comme une fonctionnalite deja stabilisee. Le travail restant porte notamment sur la migration des classes robot vers l'application principale, l'ajout des commandes dans le repere TOOL, l'implementation du calcul de `Delta Z`, la synchronisation des acquisitions et la preparation de l'export CSV. Toute validation de cette integration devra etre documentee apres essais sur le materiel reel.

## 8. Test mecanique et asservissement

Le sujet attend egalement une logique de test mecanique et, a terme, un comportement d'asservissement en force. Dans le premier cas, l'idee est de faire varier la position de l'outil, de mesurer la force obtenue et d'enregistrer les couples force / position afin d'obtenir une base d'analyse experimentale. Les fichiers d'aide du cours, en particulier `HowTo_Timer.txt` et `HowTo_CSV.txt`, fournissent deja une methode de travail claire pour cette partie.

L'asservissement en force, quant a lui, doit etre decrit avec prudence dans le rapport. Le principe attendu est connu, mais la version finale ne peut etre consideree comme acquise tant que l'application integree, les essais reels et la validation du comportement n'ont pas ete menes. Dans la redaction, il faudra donc distinguer le principe vise, les choix retenus par l'equipe, puis les resultats effectivement observes en seance. Cette section est donc aujourd'hui en partie `Disponible` sur le plan conceptuel, mais reste `A completer` et `A valider` sur le plan experimental.

## 9. Validation experimentale

La validation experimentale constitue la partie la plus dependante du materiel et de l'avancement du developpement collectif. Le rapport devra faire apparaitre, de maniere structuree, les tests realises sur le capteur seul, sur le robot seul, puis sur l'application integree. Il devra egalement mentionner les preuves disponibles, par exemple les captures d'ecran, les extraits de code selectionnes et les fichiers CSV issus des essais.

Pour l'instant, cette partie doit surtout poser un cadre de validation coherent. Il est preferable de documenter clairement ce qui est deja verifie et ce qui ne l'est pas encore, plutot que de laisser croire a une validation complete. Le detail du suivi de tests est prepare dans [l'annexe de validation](annexes/tests_validation.md), qui pourra etre completee au fil des seances.

## 10. Limites et suites

La principale limite du depot actuel tient au fait qu'il repose encore sur des briques separees. Cette situation n'empeche pas de comprendre l'architecture ni de preparer la structure du rendu, mais elle ne permet pas encore de conclure sur une application finale completement operationnelle. D'autres limites sont egalement a rappeler, notamment la dependance au materiel reel, a la disponibilite du robot et a la possibilite de realiser des essais repetables en salle.

Les suites de travail sont donc relativement claires. Il faudra finaliser l'application integree, effectuer les essais reels, completer les annexes avec des captures et des commentaires de code, puis remplacer les emplacements reserves par des resultats experimentaux effectifs. Le present rapport a surtout pour fonction de fournir un cadre de redaction propre et reutilisable a mesure que le projet avance.

## 11. Conclusion

Le depot actuel fournit une base technique serieuse pour engager le projet. La communication serie avec le capteur et le pilotage reseau du robot sont deja representes par des briques de travail distinctes, suffisamment riches pour servir de point de depart a l'integration. En revanche, le couplage final Robot + Force, le test mecanique complet et la validation d'un comportement d'asservissement restent encore a produire.

Dans cet etat, le dossier de rendu ne pretend pas a un achevement qui n'existe pas encore. Il offre plutot une structure professionnelle, exploitable et honnete, qui permettra d'integrer progressivement les preuves experimentales et les resultats de developpement a mesure que l'equipe terminera le projet.
