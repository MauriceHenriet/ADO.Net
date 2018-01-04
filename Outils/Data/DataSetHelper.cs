using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Outils.TData
{
	public static class DataSetHelper
	{
		/// <summary>
		/// Convertit une DataTable en liste générique d'éléments de type complexe
		/// Les lignes supprimées sont ignorées
		/// </summary>
		/// <typeparam name="T">type des éléments de la liste</typeparam>
		/// <param name="table">table à convertir</param>
		/// <returns>Liste d'éléments de type T</returns>
		public static List<T> ToList<T>(this DataTable table) where T : new()
		{
			// NB/ La contrainte new spécifie que le type T 
			// doit avoir un constructeur public sans paramètre

			IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
			List<T> result = new List<T>();

			foreach (DataRow row in table.Select("", "", DataViewRowState.CurrentRows))
			{
				var item = CreateItemFromRow<T>(row, properties);
				result.Add(item);
			}

			return result;
		}

		/// <summary>
		/// Convertit une DataTable en liste générique d'éléments de type simple (valeur ou string)
		/// </summary>
		/// <typeparam name="T">type des éléments de la liste</typeparam>
		/// <param name="table">table à convertir</param>
		/// <returns></returns>
		public static List<T> ToSimpleList<T>(this DataTable table) where T : IConvertible
		{
			List<T> result = new List<T>();

			foreach (DataRow row in table.Select("", "", DataViewRowState.CurrentRows))
			{
				var val = row.ItemArray[0];
				result.Add((T)val);
			}

			return result;
		}

		/// <summary>
		/// Créer un élément de liste générique à partir d'une ligne de DataTable
		/// </summary>
		/// <typeparam name="T">type des éléments de la liste</typeparam>
		/// <param name="row">ligne de la DataTable</param>
		/// <param name="properties"></param>
		/// <returns></returns>
		private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
		{
			T item = new T();
			foreach (var property in properties)
			{
				if (row.Table.Columns.Contains(property.Name))
				{
					var val = row[property.Name];

					if (property.PropertyType == typeof(DayOfWeek))
					{
						DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), val.ToString());
						property.SetValue(item, day, null);
					}
					else
					{
						if (val == DBNull.Value)
							property.SetValue(item, null, null);
						else
							property.SetValue(item, val, null);
					}
				}

				// Si le nom de la colonne de la DataTable ne correspond à aucun
				// nom de propriété de la classe qui stockera les données, on ne fait rien
			}

			return item;
		}
	}

}
