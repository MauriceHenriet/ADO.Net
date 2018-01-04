using Northwind2;
using Northwind2.Pages;
using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercices
{
    class Program
    {
        static void Main(string[] args)
        {
            Northwind2App app = Northwind2App.Instance;
            app.Title = "Northwind2";

            // Ajout des pages
            Page accueil = new PageAccueil();
            app.AddPage(accueil);
            Page fournisseurs = new PageFournisseur();
            app.AddPage(fournisseurs);
            Page produits = new PageProduits();
            app.AddPage(produits);



            // Affichage de la page d'accueil
            app.NavigateTo(accueil);
            app.Run();

        }
    }
}
