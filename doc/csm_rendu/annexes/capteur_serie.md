# Annexe - Capteur et liaison serie

## Objet

Cette annexe rassemble les informations utiles a la brique capteur et a la communication serie PC <-> capteur.

## 1. Materiel

Statut: `Disponible`

Le sujet mentionne:

- capteur de force en S
- module HX711
- carte FireBeetle ESP32-E

## 2. Protocole

Statut: `Disponible`

Commandes mentionnees dans les documents du cours:

- `C`: calibration start
- `Q`: calibration stop
- `M`: measurement
- `T`: tare
- `U`: unit toggle

Format de reponse attendu:

```text
Reading: -20.032 Kg
```

## 3. Base logicielle actuelle

Statut: `Disponible`

Dans `demo/csharp_studio/RS232-PC`:

- la liaison serie est configurable
- le baudrate attendu est `115200`
- la lecture des donnees est asynchrone
- les messages recus peuvent etre affiches et journalises
- une acquisition periodique est deja presente

## 4. Points a documenter plus tard

Statut: `A completer`

- configuration exacte du capteur reel utilise
- port COM retenu en seance
- comportement en `Kg` et en `N`
- robustesse du parsing sur le flux reel

## 5. Resultats a inserer

Statut: `A valider`

- capture d'un message de boot
- capture d'une sequence `M`
- capture d'une tare `T`
- description du comportement avec emulateur puis capteur reel
