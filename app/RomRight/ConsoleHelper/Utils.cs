using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RomRight.ConsoleHelper
{
    /// <summary>
    /// Fonctions utilitaires pour la gestion de la console.
    /// </summary>
    public static class Utils
    {

        #region Champs

        /// <summary>
        /// Dernière couleur du texte sauvegardée
        /// </summary>
        public static ConsoleColor SAVED_TEXT_COLOR;
        
        /// <summary>
        /// Dernière position du curseur sauvegardée
        /// </summary>
        public static int SAVED_CURSOR_POSITION;
        
        /// <summary>
        /// Texte de log
        /// </summary>
        public static string LOG = "";

        #endregion


        #region Méthodes

        /// <summary>
        /// Permet d'ouvrir une boite de sélection de répertoire. Attention, en mode console la fonction main doit être marqué comme [STAThread].
        /// </summary>
        /// <returns>Le chemin du choix de l'utilisateur (ou null s'il a annulé).</returns>
        public static string GetDirectoryWithDialog()
        {
            string result = null;

            // On va créer et ouvrir la boite de dialog
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.RootFolder = Environment.SpecialFolder.Desktop;
            folderDialog.ShowNewFolderButton = true;

            // Par défaut, on se place dans le répertoire où se trouve l'application
            folderDialog.SelectedPath = Path.GetDirectoryName(Application.ExecutablePath);

            // Si l'utilisateur a sélectionné un dossier et a appuyé sur OK
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                result = folderDialog.SelectedPath;
            }

            return result;
        }

        /// <summary>
        /// Cette fonction permet d'écrire une ligne à la fois dans la console et dans les logs.
        /// </summary>
        /// <param name="line">La ligne de texte à écrire.</param>
        /// <param name="showInConsole">Indique si on doit l'afficher dans la console ou pas.</param>
        public static void WriteLineInLog(string line, bool showInConsole = true)
        {
            // On écrit la ligne dans les logs
            LOG += line + "\n";

            if (showInConsole)
                Console.WriteLine(line);
        }

        public static void SaveConsoleConfig()
        {
            SAVED_TEXT_COLOR = Console.ForegroundColor;
            SAVED_CURSOR_POSITION = Console.CursorTop;
        }

        public static void RestoreConsoleConfig()
        {
            Console.ForegroundColor = SAVED_TEXT_COLOR;
            Console.CursorTop = SAVED_CURSOR_POSITION;
        }

        #endregion

    }
}
