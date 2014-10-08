using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RomRight
{
    /// <summary>
    /// RatedRom permet d'associer le nom du fichier d'une rom avec la note associée.
    /// Un algorithme de notation par défaut est disponible via la méthode Rate.
    /// </summary>
    public class RatedRom
    {

        #region Propriétés

        /// <summary>
        /// Nom du fichier de la rom
        /// </summary>
        public string RomFileName { get; set; }

        /// <summary>
        /// Note attribuée à la rom.
        /// </summary>
        public int Mark { get; set; }

        #endregion


        #region Constructeurs

        /// <summary>
        /// Instancie une nouvelle rom pouvant être notée.
        /// </summary>
        /// <param name="romFileName">Le nom du fichier de la rom.</param>
        /// <param name="mark">La note de départ.</param>
        public RatedRom(string romFileName, int mark)
        {
            this.RomFileName = romFileName;
            this.Mark = mark;
        }

        #endregion


        #region Méthodes

        public void Rate(string[] worldZones)
        {
            // On donne quelques points par défaut pour éviter que les petites erreurs sur la rom qui enlèvent peu de points
            // ne la fasse passer en dessous de 0.
            Mark = 25;
            
            //////////////////////////////////////////////////////
            #region Note des patchs de traduction
            //////////////////////////////////////////////////////

            bool patchFound = false;

            // On va chercher des roms traduites (patch de traduction), avec l'ordre d'importance qu'on a spécifié.
            for (int i = 0; i < worldZones.Count(); i++)
            {
                // Si la worldzone commence par +, ce n'est pas une zone normale, c'est un patch de traduction
                if (worldZones[i].Substring(0, 1) == "+")
                {
                    // On cherche les traductions les plus récentes ([T+xxx])
                    Regex regexTranslation = new Regex("\\[T\\+" + worldZones[i].Substring(1) + ".*?\\]");

                    if (regexTranslation.Match(RomFileName).Captures.Count > 0)
                    {
                        // On va donner 100 points par rapprochement géographique.
                        // Si le nom de la rom contient la première zone de traduction déclarée dans la liste, elle aura 100 * le nombre total
                        // de zones. Si elle contient la dernière zone, elle aura 100 points. Si elle n'en contient aucune, elle
                        // ne gagne aucun point.
                        Mark += (worldZones.Count() - i) * 100;

                        // Une rom traduite est forcémment considérée comme "parfaite". Elles sont vérifiées et testées par les
                        // traducteurs. On va donc donner plus de points qu'une rom vérifiée.
                        Mark += 30;

                        patchFound = true;
                    }
                }
            }

            // Si aucun patch n'a été trouvé, on va pénaliser fortemment la présence d'un patch de traduction non spécifié dans les zones
            // (traduction dans une langue qu'on ne souhaite absolument pas).
            if (!patchFound)
            {
                // On cherche les traductions [T+xxx] et [T-xxx]
                Regex regexTranslation = new Regex("\\[T[\\+-].*?\\]");
                if (regexTranslation.Match(RomFileName).Captures.Count > 0)
                    Mark -= 25;
            }

            #endregion

            //////////////////////////////////////////////////////
            #region Note par zone géographique
            //////////////////////////////////////////////////////

            // Si aucun patch n'a encore été trouvé, on va chercher une zone d'origine
            if (!patchFound)
            {
                // On va chercher une correspondance avec la zone.
                for (int i = 0; i < worldZones.Count(); i++)
                {
                    // Si la worldzone ne commence pas par +, c'est une zone normale (entre parenthèses)
                    if (worldZones[i].Substring(0, 1) != "+")
                    {
                        if (RomFileName.Contains("(" + worldZones[i] + ")"))
                        {
                            // On va donner 100 points par rapprochement géographique.
                            // Si le nom de la rom contient la première zone déclarée dans la liste, elle aura 100 * le nombre total
                            // de zones. Si elle contient la dernière zone, elle aura 100 points. Si elle n'en contient aucune, elle
                            // ne gagne aucun point.
                            Mark += (worldZones.Count() - i) * 100;
                        }
                    }
                }
            }

            #endregion
            
            //////////////////////////////////////////////////////
            #region Valorisation des roms vérifiées
            //////////////////////////////////////////////////////

            // On valorise fortemment les rom qui ont été vérifiées et sont certifiées valides (tag [!])
            if (RomFileName.Contains("[!]"))
                Mark += 20;

            #endregion

            //////////////////////////////////////////////////////
            #region Valorisation des révisions
            //////////////////////////////////////////////////////

            // On va attribuer des points aux révisions de roms
            Regex regexRevision = new Regex("\\(REV(.*?)\\)");
            MatchCollection matchesRevision = regexRevision.Matches(RomFileName);
            if (matchesRevision.Count > 0)
            {
                // On récupère ce qu'il y a après REV (supposé être un chiffre)
                string revNumber = matchesRevision[0].Groups[1].Captures[0].Value;

                try
                {
                    // On essaye de transformer le numéro de révision en chiffre et on l'ajoute à la note si on y arrive
                    Mark += int.Parse(revNumber);
                }
                // Si le numéro de révision n'est pas un nombre, on l'ignore simplement
                catch (Exception) { }
            }

            #endregion

            //////////////////////////////////////////////////////
            #region Pénalisation des roms pirates
            //////////////////////////////////////////////////////

            // Une rom pirate (tag [p1], [p2]...) est légèrement pénalisée
            Regex regexPirate = new Regex("\\[p[1-9]*?\\]");
            if (regexPirate.Match(RomFileName).Captures.Count > 0)
                Mark -= 5;

            #endregion

            //////////////////////////////////////////////////////
            #region Pénalisation des roms mal dumpées
            //////////////////////////////////////////////////////

            // Une bad rom (tag [b1], [b2]...) est fortemment pénalisée
            Regex regexBadDump = new Regex("\\[b[1-9]*?\\]");
            if (regexBadDump.Match(RomFileName).Captures.Count > 0)
                Mark -= 25;

            #endregion

            //////////////////////////////////////////////////////
            #region Pénalisation des hacks
            //////////////////////////////////////////////////////

            // Une rom hackée (tag [h1c], [h]...) est fortemment pénalisée.
            Regex regexHack = new Regex("\\[h.*?\\]");
            if (regexHack.Match(RomFileName).Captures.Count > 0)
                Mark -= 15;

            #endregion

            //////////////////////////////////////////////////////
            #region Pénalisation des roms avec des fixs
            //////////////////////////////////////////////////////

            // Une rom fixée (tag [f1], [f2]...) est faiblement pénalisée. On privilégie en effet les roms non modifiées.
            Regex regexFixed = new Regex("\\[f.*?\\]");
            if (regexFixed.Match(RomFileName).Captures.Count > 0)
                Mark -= 1;

            #endregion

            //////////////////////////////////////////////////////
            #region Pénalisation des bundles MegaDrive
            //////////////////////////////////////////////////////

            // Une rom fixée (tag [f1], [f2]...) est faiblement pénalisée. On privilégie en effet les roms non modifiées.
            Regex regexMdBundle = new Regex("\\(MD Bundle\\)");
            if (regexMdBundle.Match(RomFileName).Captures.Count > 0)
                Mark -= 1;

            #endregion

            //////////////////////////////////////////////////////
            #region Pénalisation des roms alternate
            //////////////////////////////////////////////////////

            // Une rom alternate (tag [a1], [a2]...) est faiblement pénalisée. On privilégie en effet les roms non modifiées.
            Regex regexAlternate = new Regex("\\[a.*?\\]");
            if (regexAlternate.Match(RomFileName).Captures.Count > 0)
                Mark -= 1;

            #endregion

            //////////////////////////////////////////////////////
            #region Pénalisation des roms overdumped
            //////////////////////////////////////////////////////

            // Une rom overdumped (tag [o]) est pénalisée.
            Regex regexOverdumped = new Regex("\\[o.*?\\]");
            if (regexOverdumped.Match(RomFileName).Captures.Count > 0)
                Mark -= 5;

            #endregion

            //////////////////////////////////////////////////////
            #region Pénalisation des roms prototype
            //////////////////////////////////////////////////////

            // Une rom prototype (tag (Prototype)) est pénalisée.
            Regex regexPrototype = new Regex("\\(Prototype\\)");
            if (regexPrototype.Match(RomFileName).Captures.Count > 0)
                Mark -= 10;

            #endregion
        }

        #endregion

    }
}
