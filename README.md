RomRight
==============================================

RomRight est un gestionnaire automatisé de bibliothèque de roms qui va vous simplifier la vie ! Il a été développé pour la création d'une borne d'arcade Raspberry Pi sous RetroPie mais peut être utilisé dans bien d'autres cas.

Problématique
--------------------------------------

Lorsque vous téléchargez une rom d'un jeu rétro, vous récupérez bien souvent un fichier .7z qui contient des tonnes de versions différentes du jeu original. Par exemple, si on ouvre le jeu MegaDrive "Sonic The Hedgehog 3.7z", on se retrouve avec la liste suivante :

- Sonic 3 Delta V0.1 (S3 Hack).gen
- Sonic 3 Delta V0.1a (S3 Hack).gen
- Sonic The Hedgehog 3 (E) [!].gen
- Sonic The Hedgehog 3 (J) [!].gen
- Sonic The Hedgehog 3 (U) [!].gen
- Sonic The Hedgehog 3 (U) [b1].gen
- Sonic The Hedgehog 3 (U) [b2].gen
- Sonic The Hedgehog 3 (U) [b3].gen
- Sonic The Hedgehog 3 (U) [b4].gen
- ...

Dans mon cas, je vis en France. Ma borne d'arcade a besoin d'une rom et d'une seule. J'aurais donc tendance à choisir celle correspondant à l'Europe (E) et qui a été vérifiée [!].

Maintenant, imaginez que vous ayez 300 jeux pour la MegaDrive, 300 autres pour la Super Nes, encore 300 pour la GameBoy... vous allez passer des mois à ouvrir vos archives une par une pour extraire la bonne rom.

La solution
---------------------------------------

RomRight est un programme C# (.NET 4.0) s'exécutant dans la console Windows et qui va s'occuper de choisir pour vous la rom qui correspond la mieux à vos attentes, automatiquement.

Comment ça marche ?
---------------------------------------

Le programme va ouvrir les fichiers 7z dans un dossier que vous pouvez spécifier. Il analyse ensuite tous les noms des roms qui s'y trouvent et leur donne une note en fonction de ce qu'il repère. Par exemple, il donne beaucoup de points aux roms qui ont été vérifiées et pénalise celles qui ont été marquées comme mauvaises.

Il priorise également les zones géographiques que vous spécifiez. Vous pouvez par exemple préciser que vous préférez les roms Europe (E) mais que s'il n'y en pas, vous vous accommoderez d'une rom USA (U).

Gestion des variantes
---------------------------------------

Il est également capable de détecter les déclinaisons de jeu à l'intérieur d'un même fichier 7z. Par exemple, la rom MegaDrive "Sonic and Knuckles.7z" contient les jeux "Sonic and Knuckles" mais aussi toutes les variantes du fameux Lock-on qui consistait à insérer d'autres jeux Sonic par dessus la cartouche de S&K. On trouve donc dans le même fichier 7z les variantes avec Sonic 1, 2 et 3.

Pas de soucis ! RomRight gère parfaitement ce cas de figure et va traiter chaque variante de jeu comme un jeu à part entière, avec un système de notation indépendant des uns et des autres.

Utilisation
---------------------------------------

Téléchargez les fichiers exécutables de RomRight en cliquant ici :

[RomRight v1.0](LINK)

Décompressez les fichiers dans un dossier. Vous devriez avoir :

- RomRight.exe
- 7z.dll
- SevenZipSharp.dll

Pour utiliser rapidement RomRight, double cliquez sur RomRight.exe et suivez les instructions à l'écran. Vous pouvez choisir les dossiers où se trouvent les roms, où elles vont être exportées après le tri et les zones à traiter.

Pour une plus grande personnalisation, RomRight s'exécute également en ligne de commande. Vous pouvez ouvrir une console Windows, vous rendre dans le dossier où se trouve RomRight.exe et faire :

	RomRight.exe "chemin/vers/roms/source" "chemin/export/roms" "zones"

- **"chemin/vers/roms/source"** correspond au chemin relatif ou absolu vers le dossier contenant tous vos fichiers 7z.
- **"chemin/export/roms"** correspond au chemin dans lequel RomRight va extraire les roms qu'il aura choisi.
- **"zones"** correspond aux différentes zones géographiques à exporter, sans parenthèses et séparées par des espaces. Vous pouvez également intégrer des patchs de traduction en préfixant le patch d'un +. Par exemple, pour choisir en premier les roms Françaises, puis se rabattre sur les roms patchées en Français puis sur les roms Europe et enfin sur les roms USA et World, vous pouvez entrer "F +Fre E U W".

Par défaut, l'algorithme est réglé sur "F +Fre E UE W U +Eng JU". Il prendra donc dans l'ordre de préférence :

1. Les roms France.
2. Les roms qui ont été traduites en Français, peu importe la langue d'origine.
3. Les roms Europe.
4. Les roms USA/Europe.
6. Les roms World.
7. Les roms USA.
5. Les roms qui ont été traduites en Anglais, peu importe la langue d'origine.
8. Les roms Japon/USA.

Lors de l'exécution du script, si RomRight a un doute sur deux roms, il vous demandera de choisir laquelle vous souhaitez garder.