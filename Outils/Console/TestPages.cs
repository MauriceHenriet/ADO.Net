using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outils.TConsole
{
	public class PageTestAccueil : MenuPage
	{
		public PageTestAccueil() : base("Accueil", false)
		{
			Menu.AddOption("0", "Quitter l'application",
				() => Environment.Exit(0));
			Menu.AddOption("1", "Test Menus",
				() => TestApp.Instance.NavigateTo(typeof(PageTestMenus)));
			Menu.AddOption("2", "Test tableaux",
				() => TestApp.Instance.NavigateTo(typeof(PageTestTableaux)));
		}
	}

	public class PageTestMenus : MenuPage
	{
		public PageTestMenus() : base("Tests des menus")
		{
			Menu.AddOption("1", "Liste des produits", 
					() => Console.WriteLine("Affichage de la liste des produits...\n"));
			Menu.AddOption("2", "Nouveau produit",
					() => Console.WriteLine("Saisie d'un nouveau produit...\n"));
			Menu.AddOption("3", "Supprimer un produit",
					() => Console.WriteLine("Choix du produit à supprimer...\n"));
		}
	}

	public class PageTestTableaux : Page
	{
		public PageTestTableaux() : base("Test des tableaux")
		{
		}

		public override void Display()
		{
			var table = new ConsoleTable("Col1", "Col2", "Col3");
			table.AddRow(1, 2, 3)
				  .AddRow("this line should be longer", "yes it is", "oh");

			Console.WriteLine("Format par défaut (Markdown):\n");
			table.Display();

			Console.WriteLine("Format Alternative:\n");
			table.Display(format: Formats.Alternative);
			Console.WriteLine();

			Console.WriteLine("Format Minimal + compteur d'items:\n");
			table.Display("trucs", Formats.Minimal);
			Console.WriteLine();

			// Création d'un tableau à partir d'une liste d'objets
			var rows = new List<Something>();
			for (int i = 0; i < 10; i++)
			{
				rows.Add(new Something());
			}
			ConsoleTable.From(rows).Display();

			// Création d'un tableau d'éléments de type simple
			string[] tabArbres = { "Chêne", "Etre", "Peuplier", "Platane", "Saule" };
			ConsoleTable.From(tabArbres, "Arbres").Display();

			decimal[] tabPrix = { 9.99m, 12.80m, 15.20m, 19.50m, 25.25m };
			ConsoleTable.From(tabPrix, "Prix (€)").Display();
		}
	}

	public class Something
	{
		private static int cnt;

		public Something()
		{
			Id = Guid.NewGuid().ToString("N");
			Name = "Elément " + (++cnt).ToString();
		}

		public string Id { get; set; }
		[DisplayName("Nom")]
		public string Name { get; set; }
	}
}
