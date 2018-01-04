using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2.Pages
{
    class PageProduits : MenuPage

    {
        private List<Category> Categories;
        private List<Produit> Produits;

        public PageProduits() : base("Produits", false)
        {
            Menu.AddOption("1", "Liste des produits", () => AfficherProduits());
            Menu.AddOption("2", "Créer un nouveau produit", CreerProduit);
            Menu.AddOption("3", "Modifier un nouveau produit", ModifierProduit);

        }

        private void ModifierProduit()
        {
            bool modif = true;
            Guid Categoryid =  AfficherProduits();
            Produit Produitsaisie = new Produit();
            Produitsaisie.CategoryID = Categoryid;
            Produitsaisie.ProductId = Input.Read<int>("Choisissez l'ID du Produit à modifier");

            Produitsaisie =  Produits.Where(p => p.ProductId == Produitsaisie.ProductId).FirstOrDefault();

            // Produitsaisie = Contexte.ChargerProduit(Produitsaisie.ProductId);


            //nom, id de la catégorie, id du fournisseur, prix unitaire, unités en stock



            Produitsaisie.Name = Input.Read<string>("Modifiez le Nom du produit", Produitsaisie.Name);
            Produitsaisie.CategoryID = Input.Read<Guid>("Modifiez la Catégorie du produit", Produitsaisie.CategoryID);
            Produitsaisie.SupplierId = Input.Read<int>("Modifiez l' ID du fournisseur du produit", Produitsaisie.SupplierId);
            Produitsaisie.UnitPrice = Input.Read<decimal>("Modifiez le Prix du produit", Produitsaisie.UnitPrice);
            Produitsaisie.UnitsInStock = Input.Read<Int16>("Modifiez les unités en stock", Produitsaisie.UnitsInStock);

            if (Contexte.AjouterModifierProduit(modif, Produitsaisie) != 0)
            {
                Console.WriteLine("Produit modifié avec succès");
            }
            else
            {
                Console.WriteLine("echec");
            }

        }

        private void CreerProduit()
        {
            bool creer = false;
            //List<Category> Categories = Contexte.GetCategory();
            if (Categories == null)
                Categories = Contexte.GetCategory();
            ConsoleTable.From(Categories, "Categories").Display("Categories");
            Produit Produitsaisie = new Produit();
            Produitsaisie.CategoryID = Input.Read<Guid>("Saisissez la Catégorie du produit");
            Produitsaisie.Name = Input.Read<string>("Saisissez le Nom du produit");
            Produitsaisie.SupplierId = Input.Read<int>("Saisissez l' ID du fournisseur du produit");
            Produitsaisie.UnitPrice = Input.Read<decimal>("Saisissez le Prix du produit");
            Produitsaisie.UnitsInStock = Input.Read<Int16>("Saisissez les unités en stock");

            if ( Contexte.AjouterModifierProduit(creer, Produitsaisie)!=0)
            {
                Console.WriteLine("Produit créé avec succès");
            }
        }

        private Guid AfficherProduits()
        {
            //List<Category> Categories = Contexte.GetCategory();
            if (Categories == null)
                Categories = Contexte.GetCategory();
            ConsoleTable.From(Categories, "Categories").Display("Categories");
            Guid Cat = Input.Read<Guid>("Choisissez une catégorie ID");
            //List<Produit> Produits = Contexte.GetProduits(Cat);
            if (Produits == null)
                Produits = Contexte.GetProduits(Cat);
            ConsoleTable.From(Produits, "Produits").Display("Produits");
           return Cat;
        }
    }
}
