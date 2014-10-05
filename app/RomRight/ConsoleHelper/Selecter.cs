using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomRight.ConsoleHelper
{
    class Selecter
    {
        private ListChoice[] choices;
        private int selectedElement = 0;
        private int cursorPosition = 0;
        private bool enabled = false;


        public Selecter(ListChoice[] choices)
        {
            this.choices = choices;
            cursorPosition = Console.CursorTop;

            // On dessine une première fois le contrôle
            Display();
            // Le curseur doit être déplacé à la fin vu que c'est la première fois qu'on dessine
            Console.CursorTop += choices.Count();
        }

        public ListChoice Choose()
        {
            // On garde en mémoire la visibilité du curseur et on l'efface le temps d'afficher la liste
            bool savedCursorVisibility = Console.CursorVisible;
            Console.CursorVisible = false;
            
            // On indique que le contrôle est actif
            enabled = true;

            ConsoleKey keyPressed = ConsoleKey.NoName;

            // Tant que l'utilisateur n'appuit pas sur Entrée
            while (keyPressed != ConsoleKey.Enter)
            {
                // Si l'utilisateur appuit sur la touche bas
                if (keyPressed == ConsoleKey.DownArrow)
                {
                    // S'il est possible de descendre plus bas, on incrément la position
                    if (selectedElement < choices.Count() - 1)
                        selectedElement++;
                }
                // Si l'utilisateur appuit sur la touche haut
                else if (keyPressed == ConsoleKey.UpArrow)
                {
                    // Si on peut aller plus haut, on le fait
                    if (selectedElement > 0)
                        selectedElement--;
                }

                Display();

                // On attend que l'utilisateur appuit sur une touche
                keyPressed = Console.ReadKey().Key;
            }

            // On affiche une dernière fois les choix en masquant légèrement ceux qui n'ont pas été sélectionnés
            enabled = false;
            Display();

            // On rétablit la visibilité du curseur à son origine
            Console.CursorVisible = savedCursorVisibility;

            // On retourne le choix sélectionné par l'utilisateur
            return choices[selectedElement];
        }


        private void Display()
        {
            Utils.SaveConsoleConfig();

            // On se replace au début de la liste pour la redessiner
            Console.SetCursorPosition(0, cursorPosition);

            // On parcourt tous les choix possibles
            for (int i = 0; i < choices.Count(); i++)
            {
                // Si l'élément actuel est l'élément sélectionné, il faut le mettre en avant par rapport aux autres
                if (i == selectedElement)
                {
                    Console.ForegroundColor = Config.COLOR_SELECTED_TEXT;
                    Console.Write(enabled ? " * " : "   ");
                }
                else
                {
                    Console.ForegroundColor = enabled ? Config.COLOR_MAIN_TEXT : Config.COLOR_DISABLED_TEXT;
                    Console.Write("   ");
                }

                Console.WriteLine(choices[i].Title);
            }

            Utils.RestoreConsoleConfig();
        }

        
    }
}
