using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outils.TConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			TestApp app = TestApp.Instance;
			app.Title = "Appli de test";

			// Ajout des pages
			Page accueil = new PageTestAccueil();
			app.AddPage(accueil);
			app.AddPage(new PageTestMenus());
			app.AddPage(new PageTestTableaux());

			// Affichage de la page d'accueil
			app.NavigateTo(accueil);
			app.Run();
		}
	}
}
