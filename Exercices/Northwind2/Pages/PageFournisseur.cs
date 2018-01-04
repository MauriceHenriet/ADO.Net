using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2.Pages
{
    class PageFournisseur : MenuPage
    {
        public PageFournisseur() : base("Fournisseurs", false)
        {
            Menu.AddOption("1", "Liste des pays.", AfficherPays);
            Menu.AddOption("2", "Liste des fournisseurs.", AfficherFournisseurs);
            Menu.AddOption("3", "Nombre de Produits de Tous les Fournisseurs d'un Pays.", AfficherNombreProduits);
            Menu.AddOption("4", "Afficher les Clients et leurs Commandes.", AfficherClientsCommandes);

        }

        private void AfficherClientsCommandes()
        {
            List<CommandeClient> Comm = Contexte.GetClientsCommandes();
            ConsoleTable.From(Comm, "Comm").Display("Comm");
        }
        private void AfficherNombreProduits()
        {
            string payssaisie = null;
            Console.WriteLine("Choisissez un Pays pour afficher son nombre de Produits :");
            payssaisie = Console.ReadLine();
            int NProduits = Contexte.GetNbProduits(payssaisie);
            Console.WriteLine("Dans le Pays {0}, il y a {1} produit(s).",payssaisie, NProduits);
        }
        private void AfficherPays()
        {
            List<string> Pays = Contexte.GetPaysFournisseurs();
                ConsoleTable.From(Pays, "Pays").Display("pays");
        }
        private void AfficherFournisseurs()
        {
            // string pays = Input.read<string> voir correction

            string PaysSaisie = null;
            Console.WriteLine("choisissez un pays");
            PaysSaisie = Console.ReadLine();
            List<Fournisseur> Fournisseurs= Contexte.GetFournisseurs(PaysSaisie);
            ConsoleTable.From(Fournisseurs, "Fournisseurs").Display("Fournisseurs");
        }
    }
}
