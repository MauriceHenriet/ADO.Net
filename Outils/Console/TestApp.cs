using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outils.TConsole
{
	public class TestApp : ConsoleApplication
	{
		private static TestApp _instance;

		/// <summary>
		/// Obtient l'instance unique de l'application
		/// </summary>
		public static TestApp Instance
		{
			get
			{
				if (_instance == null)
					_instance = new TestApp();

				return _instance;
			}
		}

		// Constructeur
		public TestApp()
		{
			// Définition des options de menu à ajouter dans tous les menus de pages
			MenuPage.DefaultOptions.Add(
				new Option("a", "Accueil", () => _instance.NavigateHome()));

			//MenuPage.DefaultOptions.Add(
			//	new Option("p", "Page précédente", () => _instance.NavigateBack()));
		}

	}
}
