using System;
using System.Collections.Generic;
using System.Linq;

namespace Outils.TConsole
{
    /// <summary>
    /// Modélise une page d'application console
    /// </summary>
    public abstract class Page
	{
        /// <summary>
        /// Titre de la page
        /// </summary>
        public string Title { get; private set; }

		// Constructeur
		public Page(string title)
		{
			Title = title;
		}

		/// <summary>
		/// Affiche la page
		/// </summary>
		public abstract void Display();
	}

	/// <summary>
	/// Modélise une page avec menu intégré
	/// </summary>
	public abstract class MenuPage : Page
	{
		/// <summary>
		/// Obtient la liste des options de menu à ajouter sur toutes les pages
		/// </summary>
		public static List<Option> DefaultOptions { get; }

		/// <summary>
		/// Optient le menu associé à la page
		/// </summary>
		public Menu Menu { get; }

		static MenuPage()
		{
			DefaultOptions = new List<Option>();
		}

		/// <summary>
		/// Créer une page avec un menu
		/// </summary>
		/// <param name="title">Titre de la page</param>
		/// <param name="addDefaultOptions">Indique si les entrées de menu par défaut
		/// définies par la propriété DefaultOptions doivent être ajoutées</param>
		public MenuPage(string title, bool addDefaultOptions=true) : base(title)
		{
			Menu = new Menu();
			if (addDefaultOptions)
			{
				foreach (var opt in DefaultOptions)
					Menu.AddOption(opt);
			}
		}

		/// <summary>
		/// Affiche la page
		/// </summary>
		public override void Display()
		{
			// Affichage du menu
			Menu.Prompt();
		}
	}
}
