using Microsoft.VisualStudio.TestTools.UnitTesting;
using Northwind2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2.Tests
{
    [TestClass()]
    public class TestsDataContext
    {
        [TestMethod()]
        public void TestGetPaysFournisseurs()
        {
            List<string> _pays = Contexte.GetPaysFournisseurs() ;
            Assert.AreEqual(16, _pays.Count);
            Assert.AreEqual("USA", _pays[15]);     //  Assert.AreEqual("USA", _pays[_pays.Count-1]);

            //var list = Contexte.GetPaysFournisseurs();
            //int index = list.Count;
            //Assert.AreEqual(16, index);
            //Assert.AreEqual("USA", list[index - 1]);
        }

        [TestMethod()]
        public void TestGetProduit()
        {
            Guid id= Guid.Empty ;                                   //Guid id = Guid.Parse("......")           
            List<Category> listeCatTest = Contexte.GetCategory();   //
            foreach (Category cat in listeCatTest)                  //
            {                                                       //
                if (cat.Name == "Seafood")                          //
                {                                                   //
                    id = cat.CategoryID;                            //
                }                                                   //
            }                                                       //
            List<Produit> listeProdTest = Contexte.GetProduits(id); //(id);

            Assert.AreEqual(13, listeProdTest.Count);
            Assert.AreEqual(40, listeProdTest[6].ProductId);
        }

        [TestMethod()]
        public void TestGetNbProduits()
        {
            Assert.AreEqual(7, Contexte.GetNbProduits("UK"));
        }

        [TestMethod()]
        public void TestGetFournisseurs()
        {
            List<Fournisseur> listFour = Contexte.GetFournisseurs("Japon"); // verifier
            foreach ( Fournisseur F in listFour)
            {
                Assert.AreEqual(6, F.SupplierId);
            }

            var list = Contexte.GetFournisseurs("Japan");


        }

        [TestMethod()]
        public void TestGetClientsCommandes()
        {
            List<CommandeClient> listeTest = Contexte.GetClientsCommandes();

            int i = 0;
            int j = 0;
            foreach ( CommandeClient coCli in listeTest)
            {
                if(coCli.CustomerId == "RANCH")
                {
                    i++;
                }
            }
            Assert.AreEqual(830, listeTest.Count);
            Assert.AreEqual(5, i);
        }

        [TestMethod()]
        public void TestGetCategory()
        {
            List<Category> liste = Contexte.GetCategory();
            Assert.AreEqual(8, liste.Count);
            Assert.AreEqual("Seafood", liste[7].Name);
        }

        [TestMethod()]
        public void TestAjouterModifierProduit()
        {
            Guid c = Guid.Parse("323734f8-a4ac-4d92-b4e5-a4e896fc32a2");
            Produit ProduitAjoutTest = new Produit { CategoryID = c, Name = "Reblochon" , SupplierId = 5, UnitPrice = 10.50m, UnitsInStock = 50};
            Assert.AreEqual(1, Contexte.AjouterModifierProduit(false, ProduitAjoutTest));
            Assert.AreEqual(11, Contexte.GetProduits(c).Count);
        }

        [TestMethod()]
        public void TestSupprimerProduit()
        {
            int idProdMax = Contexte.GetIdProduitMax();
            Contexte.SupprimerProduit(idProdMax);
            Guid idCate = Guid.Parse("323734f8-a4ac-4d92-b4e5-a4e896fc32a2");
            int nbProduits = Contexte.GetProduits(idCate).Count;
            Assert.AreEqual(10, nbProduits);
        }

        // pas d'attribut
        private void Essai()
        {

        }


    }
}