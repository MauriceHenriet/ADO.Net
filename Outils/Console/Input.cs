using System;
using System.Collections;

namespace Outils.TConsole
{
    /// <summary>
    /// Classe offrant des méthodes de saisie en mode console
    /// </summary>
    public static class Input
	{
		/// <summary>
		/// Demande la saisie d'un entier compris entre des valeurs min et max
		/// </summary>
		/// <param name="prompt">Message d'invite</param>
		/// <param name="min">Valeur min</param>
		/// <param name="max">Valeur max</param>
		/// <returns></returns>
		public static int ReadIntBetween(string prompt, int min, int max)
		{
			bool correct = false;
			int value = 0;
			while (!correct)
			{
				value = Read<int>(prompt);
				if (value < min || value > max)
					Output.WriteLine("Vous devez saisir un entier compris entre {0} et {1}", min, max);
				else
					correct = true;
			}

			return value;
		}

		/// <summary>
		/// Demande la saisie d'une donnée du type T, en affichant éventuellement une valeur par défaut
		/// Si la chaîne saisie ne peut pas être convertie dans ce type, un message d'erreur est affiché
		/// </summary>
		/// <param name="prompt">Message d'invite</param>
		/// <param name="defaultValue">Objet représentant la valeur par défaut. Affiché avec ToString()</param>
		/// <returns>Donnée dans le type demandé</returns>
		public static T Read<T>(string prompt, object defaultValue = null)
		{
			string input;
			object value = null;
			bool correct = false;

			while (!correct)
			{
				Console.Write(prompt + " ");

				// Si une valeur par défaut est définie, on l'envoie à l'appli
				// Elle s'affichera juste après l'invite et sera donc renvoyée par Readline après appui sur Entrée 
				if (defaultValue != null)
					System.Windows.Forms.SendKeys.SendWait(defaultValue.ToString());
				input = Console.ReadLine();

				try
				{
					Type t = typeof(T);
					if (t == typeof(Guid))
						value = Guid.Parse(input);
					else
						value = Convert.ChangeType(input, t);

					correct = true;
				}
				catch (Exception)
				{
					TypeCode tc = Type.GetTypeCode(typeof(T));
					switch (tc)
					{
						case TypeCode.Char:
							Output.WriteLine("Vous devez saisir un caractère");
							break;
						case TypeCode.Int16:
						case TypeCode.Int32:
						case TypeCode.Int64:
							Output.WriteLine("Vous devez saisir un entier");
							break;
						case TypeCode.UInt16:
						case TypeCode.UInt32:
						case TypeCode.UInt64:
							Output.WriteLine("Vous devez saisir un entier positif");
							break;
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							Output.WriteLine("Vous devez saisir un nombre");
							break;
						case TypeCode.DateTime:
							Output.WriteLine("Vous devez saisir une date");
							break;
						default:
							Output.WriteLine("Format de la saisie non valide");
							break;
					}
				}
			}

			return (T)value;
		}

		/// <summary>
		///  Demande l'appui sur une touche
		/// </summary>
		/// <param name="prompt">Message d'invite</param>
		/// <returns></returns>
		public static ConsoleKeyInfo ReadKey(string prompt)
		{
			Output.WriteLine(prompt);
			return Console.ReadKey();
		}
	}
}
