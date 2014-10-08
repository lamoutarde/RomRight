using RomRight.ConsoleHelper;
using SevenZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RomRight
{
    class Program
    {

        private static string TITLE_ASCII_ART =
            "  _____                 _____  _       _     _   " + "\n" +
            " |  __ \\               |  __ \\(_)     | |   | |  " + "\n" +
            " | |__) |___  _ __ ___ | |__) |_  __ _| |__ | |_ " + "\n" +
            " |  _  // _ \\| '_ ` _ \\|  _  /| |/ _` | '_ \\| __|" + "\n" +
            " | | \\ \\ (_) | | | | | | | \\ \\| | (_| | | | | |_ " + "\n" +
            " |_|  \\_\\___/|_| |_| |_|_|  \\_\\_|\\__, |_| |_|\\__|" + "\n" +
            "                                  __/ |          " + "\n" +
            "                                 |___/   v" + Assembly.GetCallingAssembly().GetName().Version.ToString();
        
        private const string DEFAULT_ZONES = "F E UE W U";

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                // On éxecute le programme principal
                ExecRomRight(args);
            }
            catch (Exception e)
            {
                Utils.WriteLineInLog("\nUne erreur s'est produite pendant l'exécution de RomRight :");
                Utils.WriteLineInLog(e.Message);
            }
            // Quoi qu'il arrive, on enregistre un fichier de logs
            finally
            {
                File.WriteAllText("log.txt", Utils.LOG);
            }

        }

        /// <summary>
        /// Execute le script principal de RomRight.
        /// </summary>
        /// <param name="args">Arguments à passer</param>
        private static void ExecRomRight(string[] args)
        {
            Console.Title = "RomRight";
            Console.WindowHeight = 40;
            Console.WindowWidth = 100;

            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;

            Console.BufferHeight = 800;

            // Chargement de la dll 7zip
            SevenZipExtractor.SetLibraryPath(@"7z.dll");

            // On écrit le header
            Console.WriteLine(TITLE_ASCII_ART + "\n");
            Console.WriteLine("-----------------------------------------------------------\n");
            Console.WriteLine("Développé par Francois BERTRAND (lamoutarde.fr), RomRight permet de créer automatiquement une bibliothèque de roms localisée à partir de fichiers 7z.\n");
            Console.WriteLine("-----------------------------------------------------------\n");

            string romsDirectory = null;
            string exportDirectory = null;

            ////////////////////////////////////////////////////////
            #region Choix du dossier source
            ////////////////////////////////////////////////////////

            // Si l'utilisateur n'a pas passé d'arg[0], il va falloir lui demander de choisir un dossier où se trouvent les roms
            if(args.Count() == 0)
            {
                Console.WriteLine("\nOù se trouve le répertoire contenant vos roms ?");
                Selecter selectDirectory = new Selecter(new ListChoice[] {
                    new ListChoice("7z", "Répertoire 7z dans le dossier de l'application"),
                    new ListChoice("dialog", "Choisir le dossier dans l'explorateur Windows")
                });

                // Tant qu'on a pas de répertoire sur lequel travailler, on boucle sur le choix de l'utilisateur
                // Y U NO SELECT SOMETHING CORRECT
                while (romsDirectory == null)
                {
                    string choice = selectDirectory.Choose().Choice;

                    // Si l'utilisateur souhaite utiliser le dossier 7z à la racine du projet
                    if (choice == "7z")
                    {
                        romsDirectory = "7z";
                    }
                    // Si l'utilisateur veut sélectionner le dossier des roms via une boite de dialog
                    else if (choice == "dialog")
                    {
                        romsDirectory = Utils.GetDirectoryWithDialog();
                    }
                }

            }
            // Si l'utilisateur a saisi un arg[0], on considère qu'il s'agit du répertoire des roms
            else
            {
                romsDirectory = args[0];
            }

            #endregion

            ////////////////////////////////////////////////////////
            #region Choix du dossier d'export
            ////////////////////////////////////////////////////////

            // Si l'utilisateur n'a pas passé d'arg[1], il va falloir lui demander de choisir un dossier pour l'export
            if (args.Count() < 1)
            {
                Console.WriteLine("\nOù voulez vous exportez les fichiers finaux ?");
                Selecter selectDirectory = new Selecter(new ListChoice[] {
                    new ListChoice("export", "Répertoire export dans le dossier de l'application"),
                    new ListChoice("dialog", "Choisir le dossier dans l'explorateur Windows")
                });

                // Tant qu'on a pas de répertoire sur lequel travailler, on boucle sur le choix de l'utilisateur
                // Y U NO SELECT SOMETHING CORRECT
                while (exportDirectory == null)
                {
                    string choice = selectDirectory.Choose().Choice;

                    // Si l'utilisateur souhaite utiliser le dossier 7z à la racine du projet
                    if (choice == "export")
                    {
                        exportDirectory = "export";

                        // On vide le dossier d'export
                        if (Directory.Exists("export"))
                            Directory.Delete("export", true);

                        Directory.CreateDirectory("export");
                    }
                    // Si l'utilisateur veut sélectionner le dossier des roms via une boite de dialog
                    else if (choice == "dialog")
                    {
                        exportDirectory = Utils.GetDirectoryWithDialog();
                    }
                }
            }
            // Si l'utilisateur a saisi un arg[1], on considère qu'il s'agit du répertoire des roms
            else
            {
                exportDirectory = args[1];
            }

            #endregion

            ////////////////////////////////////////////////////////
            #region Choix des zones géographiques
            ////////////////////////////////////////////////////////

            List<string> worldZones = new List<string>();
            string selectedZones = "";

            // Si l'utilisateur n'a pas passé d'args[2], on va lui demander s'il veut saisir les codes pays
            if (args.Count() < 2)
            {
                // Tant que l'utilisateur n'a rien sélectionné ou rien tappé, on boucle
                while (selectedZones == "")
                {
                    Console.WriteLine("\nQuelles zones doivent-être exportées ?");

                    Selecter selectZones = new Selecter(new ListChoice[] {
                        new ListChoice("default", "Garder la sélection par défaut pour la France ("+DEFAULT_ZONES+")"),
                        new ListChoice("manual", "Entrer manuellement les zones à exporter"),
                        });

                    // S'il souhaite garder le choix par défaut
                    if (selectZones.Choose().Choice == "default")
                    {
                        selectedZones = DEFAULT_ZONES;
                    }
                    // S'il veut entrer manuellement les zones
                    else
                    {
                        Console.WriteLine("\nTappez les codes de pays, séparés par des espaces et sans parenthèse (ex : U E) :");
                        selectedZones = Console.ReadLine();
                    }
                }
            }
            // Si l'utilisateur a saisi un args[2], on l'utilise pour les zones
            else
            {
                selectedZones = args[2];
            }

            // On va entourer les zones saisies de parenthèses et les ajouter à la liste des zones définitives
            foreach (string zoneToAdd in selectedZones.Trim().Split(new char[] { ' ' }))
            {
                worldZones.Add("(" + zoneToAdd.Trim() + ")");
            }
            
            #endregion


            // On lance l'algorithme qui va s'occuper des roms
            BrowseRoms(romsDirectory, exportDirectory, worldZones.ToArray());

            Console.Title = "RomRight";
            Console.Write("\nFin du traitement. Appuyez sur une touche pour quitter le programme... ");
            Console.ReadKey();
        }
               
        /// <summary>
        /// Script parcourant toutes les roms d'un répertoire.
        /// </summary>
        /// <param name="romsDirectory">Répertoire contenant toutes les roms.</param>
        /// <param name="exportDirectory">Répertoire vers lequelle les roms validées seront exportées.</param>
        /// <param name="worldZones">Zones géographiques à exporter (ex : {"(U)", "(E)"}).</param>
        private static void BrowseRoms(string romsDirectory, string exportDirectory, string[] worldZones)
        {
            // Si le répertoire que l'utilisateur a spécifié n'existe pas, on balance une exception et on l'insulte.
            if (!Directory.Exists(romsDirectory))
                throw new Exception("Le répertoire que vous avez spécifié comme source pour les roms n'existe pas. Vous êtes mauvais.");

            // Si le répertoire que l'utilisateur a spécifié n'existe pas, on balance une exception et on l'insulte un peu plus fort.
            if (!Directory.Exists(romsDirectory))
                throw new Exception("Le répertoire que vous avez spécifié pour l'export n'existe pas. Vous êtes très mauvais.");
            
            // On va parcourir le dossier censé contenir les roms
            string[] files = Directory.GetFiles(romsDirectory);

            for (int i = 0; i < files.Count(); i++)
            {
                try
                {
                    Console.Title = "RomRight : " + Math.Round(((i / (double)files.Count()) * 100)) + "%";

                    string romArchive = files[i];

                    if (Path.GetExtension(romArchive) == ".7z")
                    {
                        // Seuil de note en dessous duquel les roms ne seront pas prises
                        int threshold = 100;

                        // Pour éviter une sortie du buffer (fixé à 800) lors des déplacements de curseur
                        // On clear la console de temps en temps
                        if (Console.CursorTop > 700)
                            Console.Clear();

                        Utils.WriteLineInLog("\n======= Parcourt de " + romArchive);

                        string[] romFiles;

                        // On récupère tous les fichiers de roms
                        using (var tmp = new SevenZipExtractor(romArchive))
                        {
                            romFiles = tmp.ArchiveFileNames.ToArray();
                        }

                        // On va créer une liste des variations de roms, regroupées par leur nom commun
                        // Par exemple, "Sonic 1 [!]" et "Sonic 1 (W) [b1]" seront tous deux rangés sous l'id "Sonic 1"
                        Dictionary<string, List<RatedRom>> sortedRomsFiles = new Dictionary<string, List<RatedRom>>();

                        // On va trier les fichiers qu'on a récupéré
                        foreach (string romFile in romFiles)
                        {
                            // Expressions régulières correspondantes aux tags () et []
                            Regex regexParenthesis = new Regex("\\(.*?\\)");
                            Regex regexBracket = new Regex("\\[.*?\\]");

                            // On va récupérer les noms des roms sans les variantes () et []
                            string uid = Path.GetFileNameWithoutExtension(romFile);
                            uid = regexParenthesis.Replace(uid, "");
                            uid = regexBracket.Replace(uid, "");
                            uid = uid.Trim();

                            // Si on a encore jamais croisé cet uid, il faut créer la liste qui va contenir les noms des roms
                            if (!sortedRomsFiles.ContainsKey(uid))
                            {
                                sortedRomsFiles[uid] = new List<RatedRom>();
                            }

                            // On ajoute le fichier de rom à la liste
                            sortedRomsFiles[uid].Add(new RatedRom(Path.GetFileName(romFile), 0));
                        }

                        // On va lancer le système de notation des roms qui va permettre de choisir automatiquement
                        // la plus pertinente pour la collection.
                        foreach (string uid in sortedRomsFiles.Keys)
                        {
                            foreach (RatedRom ratedRom in sortedRomsFiles[uid])
                            {
                                ratedRom.Rate(worldZones);

                                // On calcule le seuil de la rom (le plancher de centaines le plus proche de sa note)
                                // Ex : pour une rom avec une note de 345, on aura 300.
                                int floorMark = (int)Math.Floor(ratedRom.Mark / 100.0) * 100;

                                // Si le seuil est plus bas, on le réhausse
                                if (threshold < floorMark)
                                    threshold = floorMark;
                            }
                        }

                        List<string> finalRomList = new List<string>();

                        // On va chercher les roms les plus pertinentes (celles qui ont eu la meilleure note)
                        foreach (string uid in sortedRomsFiles.Keys)
                        {
                            List<RatedRom> topRoms = new List<RatedRom>();
                            int maxMark = 0;

                            foreach (RatedRom ratedRom in sortedRomsFiles[uid])
                            {
                                Utils.WriteLineInLog("--- " + ratedRom.Mark + " : " + ratedRom.RomFileName);

                                // On ignore les roms qui ont une note inférieur au seuil
                                if (ratedRom.Mark >= threshold)
                                {
                                    // Si la rom actuelle a une note strictement supérieure au maximum trouvé jusqu'à maintenant
                                    if (ratedRom.Mark > maxMark)
                                    {
                                        maxMark = ratedRom.Mark;

                                        // On efface toutes les roms de la top list car on a trouvé mieux
                                        topRoms.Clear();
                                        topRoms.Add(ratedRom);
                                    }
                                    // Si la rom actuelle a une note identique à celle max trouvée, on ajoute simplement la rom à la liste
                                    else if (ratedRom.Mark == maxMark)
                                    {
                                        topRoms.Add(ratedRom);
                                    }
                                }
                            }

                            // Si l'algorithme n'a pas réussi à départager plusieurs roms, il faut demander à l'utilisateur de choisir manuellement
                            if (topRoms.Count() > 1)
                            {
                                // On construit le selecteur pour que l'utilisateur choisisse
                                ListChoice[] romChoices = new ListChoice[topRoms.Count()];

                                for (int j = 0; j < topRoms.Count(); j++)
                                {
                                    romChoices[j] = new ListChoice(topRoms[j].RomFileName, topRoms[j].RomFileName);
                                }

                                Selecter selectBestRom = new Selecter(romChoices);

                                // On récupère la rom choisie par l'utilisateur et on l'ajoute dans la liste finale
                                ListChoice choosenRom = selectBestRom.Choose();
                                finalRomList.Add(choosenRom.Choice);
                            }
                            // Si on a qu'une seule rom dans le top, on la prend
                            else if (topRoms.Count() == 1)
                            {
                                finalRomList.Add(topRoms[0].RomFileName);
                            }

                        }

                        // On exporte les roms qui ont été sélectionnées comme les plus pertinentes
                        using (var tmp = new SevenZipExtractor(romArchive))
                        {
                            tmp.ExtractFiles(exportDirectory, finalRomList.ToArray());
                        }

                        // On affiche tous les fichiers de roms
                        foreach (string romFileName in finalRomList)
                        {
                            Utils.WriteLineInLog("*** " + romFileName);
                        }
                    }
                }
                catch(Exception e)
                {
                    Utils.WriteLineInLog("Erreur lors du traitement de la rom : " + e.Message);
                }
            }
        }
    }
}
