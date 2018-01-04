using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2
{
    public class Contexte
    {
        public static List<string> GetPaysFournisseurs()
        {
            var list = new List<string>();
            var cmd = new SqlCommand();
            cmd.CommandText = @"select distinct A.Country from supplier S
                                inner join Address A on (A.AddressId = S.AddressId)
                                order by A.Country";

            using (var cnx = new SqlConnection(Settings.Default.NorthwindConnectionString))
            {

                cmd.Connection = cnx;
                cnx.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        list.Add((string)reader["Country"]);
                    }
                }
            }
            return list;
        }

        public static Produit ChargerProduit(int ProdID)
        {
            Produit Pproduit = new Produit();
            var cmd = new SqlCommand();
            cmd.CommandText = @"select P.Name, P.UnitPrice, P.UnitsInStock, P.CategoryId, P.SupplierId
                                from Product P
                                where P.ProductId = @ProdID";

            var param = new SqlParameter
            {
                SqlDbType = SqlDbType.Int,
                ParameterName = "@ProdID",
                Value = ProdID
            };

            cmd.Parameters.Add(param);

            using (var cnx = new SqlConnection(Settings.Default.NorthwindConnectionString))
            {
                cmd.Connection = cnx;
                cnx.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();

                    Pproduit.Name = (string)reader["Name"];
                    Pproduit.UnitPrice = (decimal)reader["UnitPrice"];
                    Pproduit.UnitsInStock = (Int16)reader["UnitsInStock"];
                    Pproduit.CategoryID = (Guid)reader["CategoryId"];
                    Pproduit.SupplierId = (int)reader["SupplierId"];

                }
            }
            return Pproduit;
        }

        public static List<Produit> GetProduits(Guid Cat)
        {
            var list = new List<Produit>();
            var cmd = new SqlCommand();
            cmd.CommandText = @"select P.ProductId, P.Name, P.UnitPrice, P.UnitsInStock, P.CategoryId, P.SupplierId
                                from Product P
                                where P.CategoryId = @Cat
                                order by 1";

            var param = new SqlParameter
            {
                SqlDbType = SqlDbType.UniqueIdentifier,
                ParameterName = "@Cat",
                Value = Cat
            };

            cmd.Parameters.Add(param);

            using (var cnx = new SqlConnection(Settings.Default.NorthwindConnectionString))
            {

                cmd.Connection = cnx;
                cnx.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var Pproduit = new Produit();
                        Pproduit.ProductId = (int)reader["ProductId"];
                        Pproduit.Name = (string)reader["Name"];
                        Pproduit.UnitPrice = (decimal)reader["UnitPrice"];
                        Pproduit.UnitsInStock = (Int16)reader["UnitsInStock"];
                        Pproduit.CategoryID = (Guid)reader["CategoryId"];
                        Pproduit.SupplierId = (int)reader["SupplierId"];
                        list.Add(Pproduit);
                    }
                }
            }
            return list;
        }

        public static int GetIdProduitMax()
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"select max(ProductID) from Product order by 1";

            using (var cnx = new SqlConnection(Settings.Default.NorthwindConnectionString))
            {
                cmd.Connection = cnx;
                cnx.Open();
                return(int)cmd.ExecuteScalar();
            }
        }

        public static int AjouterModifierProduit(bool operation, Produit Prod)
        //Guid CategoryID, string Name, int SupplierId, decimal UnitPrice, Int16 UnitsInStock)
        {
            var cmd = new SqlCommand();
            if (operation == false)
            {
                cmd.CommandText = @"insert Product (CategoryId, SupplierId, Name, UnitPrice, UnitsInStock) 
				                values (@Cate, @Fourni, @Nom, @PU, @Stock)";
            }
            else
            {
                cmd.CommandText = @"update Product set Name = @Nom, CategoryId = @Cate,
								SupplierId = @Fourni, UnitPrice = @PU, UnitsInStock = @Stock
								where ProductId = @Id";
                cmd.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@Id", Value = Prod.ProductId });
                Console.WriteLine("ok");
            }


        
            cmd.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@Nom", Value = Prod.Name });
            cmd.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@Cate", Value = Prod.CategoryID });
            cmd.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@Fourni", Value = Prod.SupplierId });
            cmd.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.Decimal, ParameterName = "@PU", Value = Prod.UnitPrice });
            cmd.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.SmallInt, ParameterName = "@Stock", Value = Prod.UnitsInStock });


            using (var cnx = new SqlConnection(Settings.Default.NorthwindConnectionString))
            {
                cmd.Connection = cnx;
                cnx.Open();

                return cmd.ExecuteNonQuery();
            }
        }

        public static int GetNbProduits(string Pays)
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"select dbo.ufn_GetNbProduits(@pays)";

            var param = new SqlParameter
            {
                SqlDbType = SqlDbType.NVarChar,
                ParameterName = "@pays",
                Value = Pays
            };

            cmd.Parameters.Add(param);

            using (var cnx = new SqlConnection(Settings.Default.NorthwindConnectionString))
            {
                cmd.Connection = cnx;
                cnx.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public static List<Fournisseur> GetFournisseurs(string Pays)
        {
            var list = new List<Fournisseur>();
            var cmd = new SqlCommand();
            cmd.CommandText = @"select S.CompanyName,S.SupplierId
                                from supplier S 
                                inner join Address A on (A.AddressId = S.AddressId) 
                                where A.Country = @pays
                                order by S.CompanyName";

            // Création d'un paramètre (on précise son type, son nom et sa valeur) 
            var param = new SqlParameter
            {
                SqlDbType = SqlDbType.NVarChar,
                ParameterName = "@pays",
                Value = Pays
            };
            // Ajout à la collection des paramètres de la commande
            cmd.Parameters.Add(param);

            using (var cnx = new SqlConnection(Settings.Default.NorthwindConnectionString))
            {

                cmd.Connection = cnx;
                cnx.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {

                        var Foufournisseur = new Fournisseur();
                        Foufournisseur.SupplierId = (int)reader["SupplierId"];
                        Foufournisseur.CompanyName = (string)reader["CompanyName"];

                        list.Add(Foufournisseur);
                    }
                }
            }
            return list;
        }

        #region 
        //public static int GetNbProduits(string Pays)
        //{
        //    var cmd = new SqlCommand();
        //    cmd.CommandText = @"select count(P.ProductId) Nb_Produits
        //                        from supplier S
        //                        inner join Address A on(A.AddressId = S.AddressId)
        //                        inner join Product P on(P.SupplierId = S.SupplierId)
        //                        where A.Country = @pays
        //                        group by A.Country";

        //    var param = new SqlParameter
        //    {
        //        SqlDbType = SqlDbType.NVarChar,
        //        ParameterName = "@pays",
        //        Value = Pays
        //    };

        //    cmd.Parameters.Add(param);

        //    using (var cnx = new SqlConnection(Settings.Default.NorthwindConnectionString))
        //    {
        //        cmd.Connection = cnx;
        //        cnx.Open();
        //        return (int)cmd.ExecuteScalar();
        //    }
        //}
        #endregion
        public static List<CommandeClient> GetClientsCommandes()
        {
            var list = new List<CommandeClient>();
            var cmd = new SqlCommand();
            cmd.CommandText = @"select C.CustomerId, C.CompanyName, O.OrderId, O.OrderDate, O.ShippedDate, 
		                        sum(O.Freight) FraisLivraison, count(distinct OD.ProductId) NbArticlesDiff,
		                        sum(OD.Quantity * OD.UnitPrice*(1- OD.discount)) Montant
                                from Customer C
                                inner join Orders O on (O.CustomerId = C.CustomerId)
                                inner join OrderDetail OD on (OD.OrderId = O.OrderId)
                                group by C.CustomerId,  C.CompanyName, O.OrderId, O.OrderDate, O.ShippedDate, O.Freight
                                order by 1";

            using (var cnx = new SqlConnection(Settings.Default.NorthwindConnectionString))
            {

                cmd.Connection = cnx;
                cnx.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var CocommandeClient = new CommandeClient();
                        CocommandeClient.CustomerId = (string)reader["CustomerId"];
                        CocommandeClient.CompanyName = (string)reader["CompanyName"];
                        CocommandeClient.OrderID = (int)reader["OrderId"];
                        CocommandeClient.OrderDate = (DateTime)reader["OrderDate"];

                        if (reader["ShippedDate"] != DBNull.Value)
                            CocommandeClient.ShippedDate = (DateTime)reader["ShippedDate"];

                        CocommandeClient.Freight = (decimal)reader["FraisLivraison"];
                        CocommandeClient.NbArticlesDiff = (int)reader["NbArticlesDiff"];
                        CocommandeClient.Montant = (double)reader["Montant"];

                        list.Add(CocommandeClient);
                    }
                }
            }
            return list;
        }

        public static List<Category> GetCategory()
        {
            var list = new List<Category>();
            var cmd = new SqlCommand();
            cmd.CommandText = @"select C.CategoryId, C.Name, C.Description
                                from Category C
                                order by 2";

            using (var cnx = new SqlConnection(Settings.Default.NorthwindConnectionString))
            {

                cmd.Connection = cnx;
                cnx.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var Cacategory = new Category();
                        Cacategory.CategoryID = (Guid)reader["CategoryId"];
                        Cacategory.Name = (string)reader["Name"];
                        Cacategory.Description = (string)reader["Description"];
                        list.Add(Cacategory);
                    }
                }
            }
            return list;
        }

        // Requête delete - suppression d'un produit
        // Si le produit est référencé par une commande, la requête lève une
        // SqlException avec le N°547, qu'on intercepte dans le code appelant
        public static void SupprimerProduit(int id)
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"delete from Product where ProductId = @id";
            cmd.Parameters.Add(new SqlParameter
            {
                SqlDbType = SqlDbType.Int,
                ParameterName = "@id",
                Value = id
            });

            using (var conn = new SqlConnection(Settings.Default.NorthwindConnectionString))
            {
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
