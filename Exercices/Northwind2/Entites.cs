using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2
{


    public class Fournisseur
    {
        public string CompanyName { get; set; }
        public int SupplierId { get; set; }
    }

    public class CommandeClient
    {
        public string CompanyName { get; set; }
        public string CustomerId { get; set; }

        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public decimal Freight { get; set; }
        public int NbArticlesDiff { get; set; }
        public double Montant { get; set; }
    }

    public class Category
    {
        public Guid CategoryID { get; set; }
        public string Name  { get; set; }
        public string Description { get; set; }

    }

    public class Produit
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public Int16 UnitsInStock { get; set; }
 //       [Display(ShortName = "None")]
        public Guid CategoryID { get; set; }
        public int SupplierId { get; set; }
    }

    //class Commande
    //{
    //    public int OrderID { get; set; }
    //    public DateTime OrderDate { get; set; }
    //    public DateTime ShippedDate { get; set; }
    //    public int Freight { get; set; }
    //    public int NbArticlesDiff { get; set; }
    //    public int Montant { get; set; }
    //}
}
