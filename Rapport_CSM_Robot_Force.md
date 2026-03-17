# Conception des systèmes mécatroniques
**Équipe :** Harambat Martin / Orhategaray Eneko / Larregle Achille / Kam Melvyn

## Introduction et Bilan du travail

Le projet « CSM Robot + Force » vise à intégrer un capteur de force extéroceptif et un bras robotique industriel xArm6 au sein d'une seule et même application C# basée sur la technologie WinForms. L'objectif de cette intégration est de synchroniser l'acquisition des données d'effort avec le déplacement de l'outil, afin de réaliser des essais mécaniques reproductibles tout en offrant une interface de supervision simple et intuitive. 

Ce projet aboutit à la livraison d'une application unique et fonctionnelle. L'architecture logicielle s'est révélée stable, sécurisée par des vérifications d'état continues, et gère efficacement la coexistence des communications réseau et série. La réussite principale de notre travail réside dans la mise en place d'une boucle de couplage réactive cadencée à 300 millisecondes. En associant la mesure d'effort à un calcul de déplacement, le robot est devenu capable de réagir à son environnement, le tout complété par un export rigoureux des données sous format CSV.

## Description rapide de notre IHM

L'interface principale est construite autour d'un formulaire unique découpé en plusieurs zones logiques bien distinctes, ce qui rassemble tout le nécessaire à l'expérimentation dans un seul espace. Le bloc dédié au robot permet de gérer l'adresse IP, la connexion, l'activation des moteurs ainsi que les paramètres de sécurité liés aux collisions. Juste à côté, un panneau d'état agit comme un tableau de bord affichant en temps réel la position spatiale, les angles articulaires individuels et les offsets appliqués.

Concernant le pilotage manuel, des blocs de mouvement offrent la possibilité de réaliser des déplacements cartésiens relatifs de l’outil, tandis qu'un autre bloc gère les déplacements articulaires pas à pas. Pour l'acquisition de données, un panneau dédié au capteur permet de configurer le port série, d'envoyer des commandes manuelles et d'afficher la force en temps réel. La supervision des essais est regroupée dans une section permettant de sélectionner le fichier CSV de destination et de lancer ou stopper le test. L'interface intègre enfin un journal d'événements affichant l'historique complet de l'activité du système.

## Gestion du capteur et de la liaison série

Le capteur de force est piloté de manière asynchrone via un port série. Sa configuration est modifiable depuis l’interface en ajustant dynamiquement le port COM, le baudrate, la parité et les bits de données. Pour garantir une bonne réactivité, la vitesse de transmission est réglée par défaut à 115200 bauds. 

Le processus d'acquisition fonctionne par requêtes. Pour lire une mesure, l'application envoie la commande « M » au capteur via la liaison série. En réponse, ce dernier renvoie une chaîne de caractères. Afin d'exploiter numériquement cette donnée, le programme utilise une expression régulière (Regex) qui agit comme un filtre pour extraire uniquement la valeur numérique de la force avec précision. Une fois isolée, cette mesure est actualisée sur l'écran et mise en mémoire si un essai est en cours.

## Gestion du bras robotique et du réseau

Le pilotage du robot xArm6 s'effectue par le réseau local et non via une liaison série. Pour transmettre les commandes de mouvement, notre programme intègre la bibliothèque officielle du constructeur directement dans le code C#. 

La routine de démarrage s'effectue en plusieurs étapes successives. Le logiciel valide d'abord la connexion à l’adresse IP saisie par l’utilisateur, instancie l'objet robot, puis active le mouvement tout en désactivant les freins matériels pour préparer le robot à se déplacer. Les mouvements manuels et automatisés utilisent une méthode de cinématique de l'outil : le programme génère un vecteur de coordonnées contenant un déplacement applicable uniquement sur l'axe souhaité, dont le pas en millimètres est réglable par l'opérateur.

## Couplage Robot / Force et Asservissement en force

La véritable intégration du projet réside dans son système de synchronisation, dicté par un timer de 300 millisecondes. À chaque cycle de ce chronomètre, le programme valide d'abord une série de verrous de sécurité en s'assurant que le robot ne s'est pas mis en défaut et que la liaison série n'a pas été interrompue. Le principe de ce couplage est de rendre le robot physiquement réactif à son environnement.

Concrètement, l'application demande la valeur actuelle de l'effort au capteur. Si ce dernier détecte une poussée, le logiciel calcule instantanément un déplacement de compensation sur l'axe Z. La particularité de ce calcul est d'être strictement proportionnel : la distance du déplacement demandé au robot est directement corrélée à l'intensité de la force mesurée. Ainsi, si le capteur subit une contrainte, le robot va naturellement se déplacer dans le sens de cette poussée pour l'accompagner et dissiper l'effort. Ce mode agit comme une annulation d'effort réactive pour protéger le système, offrant un contrôle d'effort direct et efficace.

## Application de test mécanique et Sauvegarde CSV

Le mode d'acquisition développé permet de générer un déplacement régulier couplé à une mesure continue. Cela permet d'obtenir in fine une courbe précise de la force en fonction du déplacement, une fonctionnalité essentielle pour caractériser la déformation d'un matériau ou valider la rigidité d'un assemblage.

Pour permettre l'analyse a posteriori de ces essais, un module d'exportation de données a été intégré. Les données accumulées dans la mémoire tampon sont écrites ligne par ligne dans un fichier CSV préalablement sélectionné par l'utilisateur. Le formatage rigoureux inclut l'horodatage précis de l'action, la force appliquée en Newtons, et la position absolue en Z du robot. Afin d'assurer une sécurité maximale, le logiciel intègre une vérification : si le fichier cible est verrouillé par un autre programme, l'application génère automatiquement un fichier de secours pour éviter toute perte d'informations.

---

## Extraits de codes commentés de nos applications

### 1. Gestion du capteur et de la liaison série

L'acquisition série accumule les données dans une mémoire tampon pour éviter les coupures, puis isole la valeur de la force à l'aide d'une expression régulière.

```csharp
// Traitement asynchrone des données reçues sur le port série
private void ProcessSerialChunk(string chunk)
{
    if (InvokeRequired)
    {
        BeginInvoke(new Action<string>(ProcessSerialChunk), chunk);
        return;
    }

    serialBuffer.Append(chunk);

    while (true)
    {
        string buffer = serialBuffer.ToString();
        int lineBreakIndex = buffer.IndexOf('\n');
        if (lineBreakIndex < 0)
            break;

        // Nettoyage de la ligne reçue
        string line = buffer.Substring(0, lineBreakIndex).Trim('\r', '\n', '\t', ' ');
        serialBuffer.Remove(0, lineBreakIndex + 1);

        if (line.Length == 0)
            continue;

        // Appel de la fonction de parsing (Regex) et d'enregistrement
        HandleSensorLine(line); 
    }
}
```

### 2. Gestion du bras robotique et du réseau

La communication réseau instancie l'objet robot et gère les déplacements relatifs dans le repère cartésien de l'outil.

```csharp
// Fonction de connexion réseau et d'initialisation du robot
private void ButtonConnectRobot_Click(object sender, EventArgs e)
{
    if (xARM.Create(textBoxRobotIp.Text.Trim()))
    {
        ApplyMotionSpeedProfile();
        UpdateRobotStatus(true);
        AppendSensorLog("Robot connected");
        RefreshRobotTelemetry();
        SetSelfCollisionCheck(xARM.GetSelfCollision());
    }
    else
    {
        UpdateRobotStatus(false);
        MessageBox.Show("Unable to connect to the robot.", "Robot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
    SyncUiState();
}

// Fonction de déplacement relatif dans le repère de l'outil (TOOL)
public int MoveToolRelative(float dx, float dy, float dz, bool wait = true, float timeout = NO_TIMEOUT)
{
    // Création du vecteur de déplacement cartésien
    float[] pose = { dx, dy, dz, 0.0F, 0.0F, 0.0F };
    return MoveTool(pose, wait, timeout);
}

// Exemple d'appel pour un déplacement manuel de 10mm sur l'axe X
private void ButtonToolXPlus_Click(object sender, EventArgs e)
{
    MoveToolByStep(GetToolStep(), 0.0F, 0.0F, "Move Tool +X");
}
```

### 3. Couplage Robot/Force (La boucle d'acquisition à 300 ms)

Le chronomètre déclenche la séquence logicielle de manière séquentielle : vérification de la sécurité, mouvement du robot, puis requête envoyée au capteur.

```csharp
// Boucle principale d'automatisation (Exécution toutes les 300 ms)
private void timerCMD_Tick(object sender, EventArgs e)
{
    // Verrous de sécurité avant mouvement
    if (!IsRobotReady() || !IsSerialReady())
    {
        StopAcquisition(true);
        MessageBox.Show("Robot motion or sensor serial connection is not ready.", "Acquisition", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    // Déplacement du robot sur l'axe Z (vers la zone de contrainte)
    if (!ExecuteRobotCommand(() => xARM.MoveToolRelative(0.0F, 0.0F, GetToolStep(), true), "Move Tool +Z"))
    {
        StopAcquisition(true); // Arrêt immédiat si le mouvement échoue (ex: collision)
        return; 
    }

    // Mémorisation de la position Z résultante et demande de mesure au capteur
    pendingPositionZ = xARM.GetCurrentPosition()[2];
    awaitingForceSample = true;
    SendSensorCommand("M"); 
}
```

### 4. Application de test mécanique et sauvegarde CSV

Une fois le test mécanique terminé, les données accumulées dans la mémoire vive de l'application sont exportées et formatées pour une utilisation dans un tableur.

```csharp
// Exportation des couples Force/Position dans un fichier CSV
private void WriteMeasures(string path)
{
    // Ouverture du flux d'écriture sécurisé (encodage UTF8)
    using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
    {
        // Création de l'en-tête des colonnes
        writer.WriteLine("Timestamp,Force,PositionZ");
        
        // Formatage InvariantCulture pour garantir le point comme séparateur décimal
        foreach (Measure measure in measures)
        {
            writer.WriteLine(
                measure.Timestamp.ToString("O", CultureInfo.InvariantCulture) + "," +
                measure.Force.ToString("G9", CultureInfo.InvariantCulture) + "," +
                measure.PositionZ.ToString("G9", CultureInfo.InvariantCulture));
        }
    }
}
```