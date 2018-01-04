using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2.Pages
{
    class PageAccueil : MenuPage
    {
        public PageAccueil() : base("Accueil", false)
        {
  
            Menu.AddOption("0", "Quitter l'application",
                    () => Environment.Exit(0));
            Menu.AddOption("1", "Page Fournisseurs",
                    () =>  Northwind2App.Instance.NavigateTo(typeof(PageFournisseur)));
            Menu.AddOption("2", "Page Commandes",
                    () => Northwind2App.Instance.NavigateTo(typeof(PageCommandes)));
            Menu.AddOption("3", "Page Produits",
                    () => Northwind2App.Instance.NavigateTo(typeof(PageProduits)));

        }
        
    }
}
