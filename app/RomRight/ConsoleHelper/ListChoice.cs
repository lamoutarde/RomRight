using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomRight.ConsoleHelper
{
    /// <summary>
    /// Définit un élément de liste pouvant être sélectionné.
    /// </summary>
    public class ListChoice
    {
        /// <summary>
        /// Définit le choix à faire.
        /// </summary>
        public string Choice
        {
            get;
            set;
        }

        /// <summary>
        /// Permet d'afficher un texte "user-friendly" si le choix n'est pas explicite.
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Définit un nouvel élément de liste.
        /// </summary>
        /// <param name="choice">Le choix à faire.</param>
        /// <param name="title">Le texte qui sera affiché à l'utilisateur.</param>
        public ListChoice(string choice, string title)
        {
            this.Choice = choice;
            this.Title = title;
        }

    }
}
