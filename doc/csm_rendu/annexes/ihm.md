# Annexe - IHM

Cette annexe presente l'etat actuel des interfaces graphiques disponibles dans le depot de reference. A ce stade du projet, le rendu ne peut pas encore s'appuyer sur une application finale integree unique. Il est donc plus juste de decrire separement les deux interfaces existantes, tout en reservant un emplacement pour la future version fusionnee.

L'interface `RS232-PC` constitue la base de travail pour la partie capteur. Elle permet deja de configurer la liaison serie, d'ouvrir et de fermer le port, d'afficher les messages recus, d'envoyer des commandes simples et d'illustrer une acquisition periodique. Cette interface est `Disponible` et elle sera utile pour documenter la prise en main du capteur, en particulier la lecture des trames et l'export CSV.

L'interface `xARMForm` constitue, de son cote, la base de demonstration pour le pilotage robot. Elle expose les commandes de connexion, l'activation du mouvement, la lecture des positions et des articulations, ainsi que plusieurs ordres de deplacement tels que `MoveBase`, `MoveTool` et `MoveHome`. Elle permet egalement d'observer un premier usage d'un timer cote robot. Cette brique est elle aussi `Disponible`, meme si elle reste separee de la partie capteur.

Dans le rendu final, il sera pertinent d'ajouter des captures d'ecran sobres et lisibles plutot que de multiplier les vues redondantes. Une vue generale de chaque interface, une capture de la zone de configuration serie, une capture de la lecture de pose robot et une illustration de la commande `MoveTool` devraient suffire a documenter l'essentiel. Si une IHM integree est finalisee par la suite, elle devra naturellement remplacer les deux interfaces separees comme support principal du rapport.
