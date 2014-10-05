RomRight
==============================================

RomRight est un gestionnaire automatisé de bibliothèque de roms qui va vous simplifier la vie ! Il a été développé pour la création d'un borne d'arcade Raspberry Pi sous RetroPie mais peut être utilisé dans bien d'autres cas.

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

RomRight est un programme C# (.NET 4.0) s'éxecutant dans la console Windows et qui va s'occuper de choisir pour vous la rom qui correspond la mieux à vos attentes, automatiquement.

Comment ça marche ?
---------------------------------------

Le programme va ouvrir les fichiers 7z dans un dossier que vous pouvez spécifier. Il analyse ensuite tous les noms des roms qui s'y trouvent et leur donne une note en fonction de ce qu'il repère. Par exemple, il donne beaucoup de points aux roms qui ont été vérifiées et pénalise celles qui ont été marquées comme mauvaises.

Il priorétise également les zones géographiques que vous spécifiez. Vous pouvez par exemple préciser que vous préférez les roms Europe (E) mais que s'il n'y en pas, vous vous accomoderez d'une rom USA (U).

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

