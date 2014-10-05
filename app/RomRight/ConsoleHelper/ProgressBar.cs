using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomRight.ConsoleHelper
{
    public class ProgressBar
    {

        #region Champs

        private int cursorPosition = 0;
        private float percentage = 0;
        private int barLength = 0;

        #endregion


        #region Propriétés

        /// <summary>
        /// Permet de régler le pourcentage de progression de la barre (entre 0 et 1).
        /// </summary>
        public float Percentage
        {
            get { return percentage; }
            set
            {
                percentage = value;

                if (percentage < 0)
                    percentage = 0;

                if (percentage > 1)
                    percentage = 1;

                Display();
            }
        }

        #endregion


        #region Constructeurs

        /// <summary>
        /// Définit une nouvelle barre de progression à la ligne actuelle et l'affiche.
        /// </summary>
        /// <param name="barLength">Longueur voulue de la barre de téléchargements en nombre de caractères.</param>
        /// <param name="percentage">Pourcentage actuel de progression</param>
        public ProgressBar(int barLength = 40, int percentage = 0)
        {
            this.barLength = barLength;
            this.percentage = percentage;
            this.cursorPosition = Console.CursorTop;

            // On affiche la barre
            Display();

            // L'affichage replace le curseur sur sa position d'origine.
            // Il faut donc sauter manuellement une ligne ou l'utilisateur écrira par dessus.
            Console.CursorTop++;
        }

        #endregion


        #region Méthodes

        /// <summary>
        /// Permet d'afficher le texte nécessaire au rendu de l'élément dans la console.
        /// </summary>
        private void Display()
        {
            // On sauvegarde l'état de la console
            Utils.SaveConsoleConfig();

            // On replace temporairement le curseur là où se trouve la barre
            Console.CursorTop = cursorPosition;

            Console.Write("[");

            for (int i = 0; i < (int)Math.Floor(percentage * barLength); i++)
                Console.Write("#");

            for (int i = 0; i < barLength - (int)Math.Floor(percentage * barLength); i++)
                Console.Write("-");

            Console.WriteLine("] " + (int)Math.Floor(percentage * 100) + "%");

            // On restaure l'état de la console
            Utils.RestoreConsoleConfig();
        }

        #endregion

    }
}
