# Annexe - Capteur et liaison serie

Cette annexe regroupe les informations utiles a la comprehension de la brique capteur et de la communication serie entre le PC et l'electronique de mesure. L'objectif n'est pas de surcharger le rendu avec un inventaire materiel, mais de rappeler clairement les elements necessaires a la lecture du projet.

Le montage decrit dans les documents du cours repose sur un capteur de force en S, un module HX711 et une carte FireBeetle ESP32-E. Le dialogue serie associe a cet ensemble est volontairement simple. Il s'appuie sur des commandes courtes telles que `C` pour la calibration, `Q` pour la fin de calibration, `M` pour la mesure, `T` pour la tare et `U` pour le changement d'unite. Le format de reponse attendu pour une mesure est du type `Reading: -20.032 Kg`, ce qui rend possible une extraction directe de la valeur numerique et de l'unite.

Dans le depot actuel, la base `RS232-PC` montre deja les points importants de cette brique. La liaison serie est configurable, le debit de `115200` bauds est prevu, la lecture des donnees est geree de facon asynchrone et les messages recus peuvent etre affiches puis journalises. Une logique d'acquisition periodique est egalement deja visible, ce qui rend cette partie `Disponible` comme base de travail.

Les elements qui restent `A completer` concernent surtout la validation sur le capteur reel. Il faudra verifier en seance le comportement exact du flux serie, la robustesse du parsing, la stabilite de la mesure apres tare et la gestion effective des unites utilisees pendant les essais. Le rendu final gagnera en credibilite si cette annexe est completee par une ou deux captures representative du flux brut, plutot que par une accumulation de sorties serie peu lisibles.
