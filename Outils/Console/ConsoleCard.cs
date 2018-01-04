using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Outils.TConsole
{
    /// <summary>
    /// Classe permettant d'afficher dans la console des données
    /// sous forme de fiche contenant des couples libellé - valeur
    /// </summary>
    public class ConsoleCard
	{
		/// <summary>
		/// Lignes de la fiche (paires libellé - valeur)
		/// </summary>
		private IDictionary<string, string> Rows { get; set; }

		public ConsoleCard()
		{
			Rows = new Dictionary<string, string>();
		}

		/// <summary>
		/// Crée une fiche à partir d'un objet
		/// Chaque ligne de la fiche est composée du libellé et de la valeur d'une propriété de l'objet
		/// Le libellé correspond à la valeur de l'attribut Display ou au nom de la propriété
		/// </summary>
		/// <typeparam name="T">type des de l'objet à afficher</typeparam>
		/// <param name="item">objet à afficher</param>
		/// <returns>La fiche créée</returns>
		public static ConsoleCard From<T>(T item)
		{
			Type t = typeof(T);
			ConsoleCard card = new ConsoleCard();

			// Récupère les indices des propriétés à afficher
			PropertyInfo[] props = t.GetProperties();
			List<int> visiblePropIndex = new List<int>();
			for (int p = 0; p < props.Length; p++)
			{
				var att = props[p].GetCustomAttribute<DisplayAttribute>();
				
				// Si la propriété doit être affichée, on mémorise son indice
				if (att == null || att.ShortName.ToLower() != "none")
					visiblePropIndex.Add(p);
			}

			for (int i = 0; i < visiblePropIndex.Count; i++)
			{
				// Crée un libellé à partir du nom de la propriété correspondante
				//  ou de la valeur de son attribut Display
				// [Display(ShortName="None")] sur une propriété indique de ne pas afficher la propriété
				string label = string.Empty;
				var att = props[visiblePropIndex[i]].GetCustomAttribute<DisplayAttribute>();
				if (att == null)
					label = props[visiblePropIndex[i]].Name;
				else if (att.ShortName.ToLower() != "none")
					label = att.ShortName;

				card.Rows.Add(label, props[visiblePropIndex[i]].GetValue(item)?.ToString());
			}

			return card;
		}

		/// <summary>
		/// Affiche la fiche à l'écran
		/// </summary>
		/// <param name="cardName">Libellé qui sera affiché au-dessus de la fiche</param>
		/// <param name="displayLabels">Indique si on doit afficher les libellés des lignes (faux par défaut)</param>
		public void Display(string cardName = null, bool displayLabels = false)
        {
			// Si un libellé a été défini pour la fiche, on l'affiche
			if (cardName != null)
			{
				Console.WriteLine(cardName);
				Console.WriteLine("---------------------------------------");
			}

			foreach (var row in Rows)
			{
				if (displayLabels)
					Console.WriteLine("{0} : {1}", row.Key, row.Value);
				else if (row.Value != null)
					Console.WriteLine(row.Value);
			}
			Console.WriteLine();
		}
	}
}
