using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RomRight
{
    public class RatedRom
    {

        public string RomFileName { get; set; }

        public int Mark { get; set; }

        public RatedRom(string romFileName, int mark)
        {
            this.RomFileName = romFileName;
            this.Mark = mark;
        }

        public void Rate(string[] worldZones)
        {
            //////////////////////////////////////////////////////
            // NOTE PAR ZONE GEOGRAPHIQUE
            //////////////////////////////////////////////////////

            // On va chercher une correspondance avec la zone.
            // On va donner 100 points par rapprochement géographique.
            // Si le nom de la rom contient la première zone déclarée dans la liste, elle aura 100 * le nombre total
            // de zones. Si elle contient la dernière zone, elle aura 100 points. Si elle n'en contient aucune, elle
            // ne gagne aucun point.
            for (int i = 0; i < worldZones.Count(); i++)
            {
                if (RomFileName.Contains(worldZones[i]))
                {
                    Mark += (worldZones.Count() - i) * 100;
                }
            }

            //////////////////////////////////////////////////////
            // VALORISATION DES ROMS VERIFIEES
            //////////////////////////////////////////////////////

            // On valorise fortemment les rom qui ont été vérifiées et sont certifiées valides (tag [!])
            if (RomFileName.Contains("[!]"))
                Mark += 50;

            //////////////////////////////////////////////////////
            // VALORISATION DES REVISIONS
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

            //////////////////////////////////////////////////////
            // PENALISATION DES ROMS PIRATES
            //////////////////////////////////////////////////////

            // Une rom pirate (tag [p1], [p2]...) est légèrement pénalisée
            Regex regexPirate = new Regex("\\[p[1-9]*\\]");
            if (regexPirate.Match(RomFileName).Captures.Count > 0)
                Mark -= 5;

            //////////////////////////////////////////////////////
            // PENALISATION DES ROMS MAL DUMPEES
            //////////////////////////////////////////////////////

            // Une bad rom (tag [b1], [b2]...) est fortemment pénalisée
            Regex regexBadDump = new Regex("\\[b[1-9]*\\]");
            if (regexBadDump.Match(RomFileName).Captures.Count > 0)
                Mark -= 50;

            //////////////////////////////////////////////////////
            // PENALISATION DES ROMS AVEC DES FIXS
            //////////////////////////////////////////////////////

            // Une rom fixée (tag [f1], [f2]...) est faiblement pénalisée. On privilégie en effet les roms non modifiées.
            Regex regexFixed = new Regex("\\[f.*\\]");
            if (regexFixed.Match(RomFileName).Captures.Count > 0)
                Mark -= 1;

            //////////////////////////////////////////////////////
            // PENALISATION DES BUNDLES MEGADRIVE
            //////////////////////////////////////////////////////

            // Une rom fixée (tag [f1], [f2]...) est faiblement pénalisée. On privilégie en effet les roms non modifiées.
            Regex regexMdBundle = new Regex("\\(MD Bundle\\)");
            if (regexMdBundle.Match(RomFileName).Captures.Count > 0)
                Mark -= 1;
        }

    }
}
