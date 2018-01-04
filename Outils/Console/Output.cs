using System;

namespace Outils.TConsole
{
    /// <summary>
    /// Classe offrant des méthodes d'affichage en mode console
    /// </summary>
    public static class Output
    {
        /// <summary>
        /// Affiche un ensemble de valeurs sur une ligne, avec la couleur et la chaîne de format spécifiées
        /// </summary>
        /// <param name="color"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WriteLine(ConsoleColor color, string format, params object[] args)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(format, args);
            Console.ResetColor();
        }

        /// <summary>
        /// Affiche une ligne de texte avec une couleur donnée 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="value"></param>
        public static void WriteLine(ConsoleColor color, string value)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        /// <summary>
        /// Affiche un ensemble de valeurs sur une ligne, avec la chaîne de format spécifiée
        /// (Simple encapsulation de Console.Writeline)
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        /// <summary>
        /// Affiche une ligne de texte
        /// (Simple encapsulation de Console.Writeline)
        /// </summary>
        /// <param name="value"></param>
		public static void WriteLine(string value)
		{
			Console.WriteLine(value);
		}

	}
}
