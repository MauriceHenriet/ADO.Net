using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Outils.TConsole
{
	public enum Formats { MarkDown, Alternative, Minimal }

    /// <summary>
    /// Classe permettant d'afficher dans la console des données sous forme de tableau
    /// </summary>
    public class ConsoleTable
	{
		#region Propriétés
		/// <summary>
		/// Colonnes du tableau
		/// </summary>
		public IList<object> Columns { get; private set; }
		/// <summary>
		/// Lignes du tableau
		/// </summary>
		private IList<object[]> Rows { get; set; }
		#endregion

		/// <summary>
		/// Crée un tableau vide avec les éventuelles colonnes spécifiées
		/// </summary>
		public ConsoleTable(params string[] columns)
		{
			Rows = new List<object[]>();
			Columns = new List<object>(columns);
		}

		/// <summary>
		/// Crée un tableau à partir d'une liste d'items
		/// Si les items sont de type valeur ou string, le libellé de la colonne doit être spécifié
		/// Sinon, les libellés sont déduits des propriétés du type de l'item
		/// (en reprenant leur attribut Display ou leur nom)
		/// </summary>
		/// <typeparam name="T">type des items du tableau</typeparam>
		/// <param name="items">liste des items du tableau</param>
		/// <param name="columnLabel">libellé de colonne utilisé si les items sont de type valeur ou string</param>
		/// <returns>Le tableau créé</returns>
		public static ConsoleTable From<T>(IEnumerable<T> items, string columnLabel = null)
		{
			var table = new ConsoleTable();

			Type t = typeof(T);
			if (t.IsValueType || t == typeof(string))
			{
				if (columnLabel == null)
					throw new ArgumentNullException("Le libellé de la colonne unique doit être fourni");

				table.Columns.Add(columnLabel);
				foreach (var i in items) table.AddRow(i);
			}
			else
			{
				// Crée chaque colonne en définissant son libellé à partir
				// du nom de la propriété correspondante ou de la valeur de son attribut Display
				// [Display(ShortName="None")] sur une propriété indique de ne pas afficher la colonne
				PropertyInfo[] props = t.GetProperties();
				List<int> visibleColumnsIndex = new List<int>();
				for (int p = 0; p < props.Length; p++)
				{
					var att = props[p].GetCustomAttribute<DisplayAttribute>();
					if (att == null)
						table.Columns.Add(props[p].Name);
					else if (att.ShortName.ToLower() != "none")
						table.Columns.Add(att.ShortName);

					// Si la colonne doit être affichée, on mémorise l'indice de la propriété correspondante
					if (att == null || att.ShortName.ToLower() != "none")
						visibleColumnsIndex.Add(p);
				}

				// Ajoute les lignes
				foreach (var item in items)
				{
					object[] vals = new object[visibleColumnsIndex.Count];
					for (int i = 0; i < visibleColumnsIndex.Count; i++)
						vals[i] = props[visibleColumnsIndex[i]].GetValue(item);

					table.AddRow(vals);
				}
			}

			return table;
		}

		/// <summary>
		/// Ajoute une ligne au tableau et retourne le tableau
		/// </summary>
		/// <param name="values">tableau d'objets contenant les valeurs de la ligne</param>
		/// <returns>Le tableau</returns>
		public ConsoleTable AddRow(params object[] values)
		{
			if (values == null)
				throw new ArgumentNullException(nameof(values));

			if (!Columns.Any())
				throw new Exception("Les colonnes du tableau n'ont pas été définies");

			if (Columns.Count != values.Length)
				throw new Exception(
					 $"Le nombre de valeurs fournies ({values.Length}) ne correpsond pas au nombre de colonnes ({Columns.Count})");

			Rows.Add(values);
			return this;
		}

		/// <summary>
		/// Affiche le tableau à l'écran sous le format spécifié
		/// </summary>
		/// <param name="itemsName">Nom des items qui sera affiché au-dessus du tableau après le nombre d'items
		/// Si aucun nom n'est défini, le nombre d'items n'est pas affiché</param>
		/// <param name="format">valeur énumérée pour le format souhaité</param>
		public void Display(string itemsName = null, Formats format = Formats.MarkDown)
		{
			// Si un nom a été défini pour les items, on l'affiche avec le nombre d'items
			if (itemsName != null)
			{
				Console.WriteLine("{0} {1}", Rows.Count, itemsName);
				Console.WriteLine();
			}

			if (itemsName != null && Rows.Count > 0)
			{
				switch (format)
				{
					case Formats.MarkDown:
						Console.WriteLine(ToMarkDownString());
						break;
					case Formats.Alternative:
						Console.WriteLine(ToStringAlternative());
						break;
					case Formats.Minimal:
						Console.WriteLine(ToMinimalString());
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(format), format, null);
				}
			}
		}

		public override string ToString()
		{
			var builder = new StringBuilder();

			// find the longest column by searching each row
			var columnLengths = ColumnLengths();

			// create the string format with padding
			var format = Enumerable.Range(0, Columns.Count)
				 .Select(i => " | {" + i + ",-" + columnLengths[i] + "}")
				 .Aggregate((s, a) => s + a) + " |";

			// find the longest formatted line
			var maxRowLength = Math.Max(0, Rows.Any() ? Rows.Max(row => string.Format(format, row).Length) : 0);
			var columnHeaders = string.Format(format, Columns.ToArray());

			// longest line is greater of formatted columnHeader and longest row
			var longestLine = Math.Max(maxRowLength, columnHeaders.Length);

			// add each row
			var results = Rows.Select(row => string.Format(format, row)).ToList();

			// create the divider
			var divider = " " + string.Join("", Enumerable.Repeat("-", longestLine - 1)) + " ";

			builder.AppendLine(divider);
			builder.AppendLine(columnHeaders);

			foreach (var row in results)
			{
				builder.AppendLine(divider);
				builder.AppendLine(row);
			}

			builder.AppendLine(divider);

			return builder.ToString();
		}

		#region Méthodes privées
		private string ToMarkDownString()
		{
			return ToMarkDownString('|');
		}

		private string ToMarkDownString(char delimiter)
		{
			var builder = new StringBuilder();

			// find the longest column by searching each row
			var columnLengths = ColumnLengths();

			// create the string format with padding
			var format = Format(columnLengths, delimiter);

			// find the longest formatted line
			var columnHeaders = string.Format(format, Columns.ToArray());

			// add each row
			var results = Rows.Select(row => string.Format(format, row)).ToList();

			// create the divider
			var divider = Regex.Replace(columnHeaders, @"[^|]", "-");

			builder.AppendLine(columnHeaders);
			builder.AppendLine(divider);
			results.ForEach(row => builder.AppendLine(row));

			return builder.ToString();
		}

		private string ToMinimalString()
		{
			return ToMarkDownString(char.MinValue);
		}

		private string ToStringAlternative()
		{
			var builder = new StringBuilder();

			// find the longest column by searching each row
			var columnLengths = ColumnLengths();

			// create the string format with padding
			var format = Format(columnLengths);

			// find the longest formatted line
			var columnHeaders = string.Format(format, Columns.ToArray());

			// add each row
			var results = Rows.Select(row => string.Format(format, row)).ToList();

			// create the divider
			var divider = Regex.Replace(columnHeaders, @"[^|]", "-");
			var dividerPlus = divider.Replace("|", "+");

			builder.AppendLine(dividerPlus);
			builder.AppendLine(columnHeaders);

			foreach (var row in results)
			{
				builder.AppendLine(dividerPlus);
				builder.AppendLine(row);
			}
			builder.AppendLine(dividerPlus);

			return builder.ToString();
		}

		private string Format(List<int> columnLengths, char delimiter = '|')
		{
			var delimiterStr = delimiter == char.MinValue ? string.Empty : delimiter.ToString();
			var format = (Enumerable.Range(0, Columns.Count)
				 .Select(i => " " + delimiterStr + " {" + i + ",-" + columnLengths[i] + "}")
				 .Aggregate((s, a) => s + a) + " " + delimiterStr).Trim();
			return format;
		}

		// Renvoie les longueurs maxi des colonnes du tableau
		private List<int> ColumnLengths()
		{
			var columnLengths = Columns
				 .Select((t, i) => Rows.Select(x => x[i])
					  .Union(Columns)
					  .Where(x => x != null)
					  .Select(x => x.ToString().Length).Max())
				 .ToList();
			return columnLengths;
		}
		#endregion
	}
}
