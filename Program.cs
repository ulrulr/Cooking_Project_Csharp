using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Xml;

namespace projet_final
{
    class Program
    {
        static MySqlConnection Connection()
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=cooking;UID=root;PASSWORD=0171038163Lol!;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            return connection;
        }

        static MySqlDataReader Reader(MySqlConnection connection, string text)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = text;

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            return reader;
        }

        static void ExecuteNonQuery(MySqlCommand command)
        {
            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(" ErreurConnexion : " + e.ToString());
                Console.ReadLine();
                return;
            }
        }

        // fonction qui permet de récupérer le dernier numéro de commande pour que les autres num_commande soient dans la continuité
        static int GetLastCommandNumber(MySqlConnection c)
        {
            MySqlDataReader reader = Reader(c, "SELECT num_commande FROM Commande ORDER BY num_commande DESC LIMIT 1;");

            int num;
            List<string> liste = new List<string>();
            string[] valueString = new string[reader.FieldCount];
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valueString[i] = reader.GetValue(i).ToString();
                    //Console.Write(valueString[i] );
                    liste.Add(valueString[i]);
                }
            }
            num = Convert.ToInt32(liste[0]);
            reader.Close();
            return num;

        }

        static void Main(string[] args)
        {
            MySqlConnection c = Connection();
            c.Open();
            Console.WriteLine("Bienvenue chez Cooking !");
            Console.WriteLine("Que souhaitez-vous faire ?" +
                "\n 1) S'identifier" +
                "\n 2) Créer un compte" +
                "\n 3) Gestionnaire administratif" +
                "\n 4) Mode démo");
            int choix = Convert.ToInt32(Console.ReadLine());
            switch (choix)
            {
                case 1:
                    Console.WriteLine("Saisir votre identifiant : (NOM + ' ' + prénom)");
                    string identifiant = Console.ReadLine();
                    
                    MySqlConnection connection = Connection();
                    connection.Open();
                    
                    MySqlDataReader reader = Reader(connection,"SELECT DISTINCT nom_c FROM Client;");

                    List<string> liste = new List<string>();
                    string[] valueString = new string[reader.FieldCount];
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            valueString[i] = reader.GetValue(i).ToString();
                            //Console.Write(valueString[i] );
                            liste.Add(valueString[i]);
                        }
                        //Console.WriteLine();
                    }
                    Console.WriteLine();
                    for (int i = 0; i < liste.Count; i++)
                    {
                        if (identifiant == liste[i])
                        {
                            Console.WriteLine("Bienvenue dans votre espace client " + identifiant + " !");
                            break;
                        }
                        //else
                        //{
                        //    Console.WriteLine("Désolée, il y a une erreur d'identification...");
                        //}
                    }
                    reader.Close();
                    //command.Dispose();
                    Console.WriteLine();

                    Console.WriteLine("Quelle opération souhaitez-vous effectuer ? " +
                        "\n 1) Passer une commande" +
                        "\n 2) Consulter mes infos personnelles" +
                        "\n 3) Créer une recette" +
                        "\n 4) Visualiser mes recettes");
                    int choix1 = Convert.ToInt32(Console.ReadLine());
                    switch (choix1)
                    {
                        case 1:
                            bool continu = true;
                            int num_commande = GetLastCommandNumber(c);
                            while (continu == true)
                            {


                                //on incrémente directement le compteur de commandes pour éviter les doublons si le client fait plusieurs commandes
                                num_commande++;
                                Console.WriteLine(" Voici le menu : \n");

                                MySqlDataReader reader1 = Reader(connection,"SELECT type, ' : ', nom_recette, ' : ', descriptif,' -> ',prix_vente, 'euros' FROM Recettes;"); 

                                string[] valueString1 = new string[reader1.FieldCount];
                                while (reader1.Read())
                                {
                                    for (int i = 0; i < reader1.FieldCount; i++)
                                    {
                                        valueString1[i] = reader1.GetValue(i).ToString();
                                        Console.Write(valueString1[i]);
                                    }
                                    Console.WriteLine();
                                }
                                Console.WriteLine();

                                reader1.Close();
                                //command1.Dispose();

                                // Ajouter une instance à cooking.Passent
                                MySqlParameter id = new MySqlParameter("@id", MySqlDbType.VarChar);
                                MySqlParameter num = new MySqlParameter("@num", MySqlDbType.Int32);
                                id.Value = identifiant;
                                num.Value = num_commande;

                                string insertTable1 = "INSERT INTO Passent Values (@id,@num); ";
                                MySqlCommand command3 = connection.CreateCommand();
                                command3.Parameters.Add(id);
                                command3.Parameters.Add(num);
                                command3.CommandText = insertTable1;
                                ExecuteNonQuery(command3);

                                command3.Dispose();

                                // Ajouter une instance à cooking.Commande
                                MySqlParameter contenu = new MySqlParameter("@contenu", MySqlDbType.VarChar);
                                MySqlParameter num1 = new MySqlParameter("@num1", MySqlDbType.Int32);
                                MySqlParameter prix1 = new MySqlParameter("@prix1", MySqlDbType.Int32);
                                MySqlParameter date = new MySqlParameter("@date", MySqlDbType.DateTime);
                                Console.WriteLine(" Saisir le nom du plat à commander :");
                                string info = Console.ReadLine();
                                contenu.Value = info;
                                num1.Value = num_commande;
                                date.Value = DateTime.Today;
                                Console.WriteLine(" Veuillez ressaisir son prix :");
                                int vprice = Convert.ToInt32(Console.ReadLine());
                                prix1.Value = vprice;

                                string insertTable3 = "INSERT INTO Commande Values (@num1,@contenu,@prix1,@date); ";
                                MySqlCommand command5 = connection.CreateCommand();
                                command5.Parameters.Add(contenu);
                                command5.Parameters.Add(num1);
                                command5.Parameters.Add(date);
                                command5.Parameters.Add(prix1);
                                command5.CommandText = insertTable3;
                                ExecuteNonQuery(command5);

                                command5.Dispose();


                                //Console.WriteLine(" Saisir le nom du plat à commander : ");
                                //string reponse = Convert.ToString(Console.ReadLine());

                                // Ajouter une instance à cooking.Constituent
                                MySqlParameter plat = new MySqlParameter("@plat", MySqlDbType.VarChar);
                                plat.Value = info;

                                Console.WriteLine(" Combien en voulez-vous ? ");
                                string reponse1 = Convert.ToString(Console.ReadLine());
                                MySqlParameter qte = new MySqlParameter("@qte", MySqlDbType.Int32);
                                qte.Value = reponse1;

                                MySqlParameter num_com = new MySqlParameter("@num_com", MySqlDbType.Int32);
                                num_com.Value = num_commande;

                                string insertTable2 = " INSERT INTO Constituent Values (@num_com,@plat,@qte);";
                                MySqlCommand command4 = connection.CreateCommand();
                                command4.Parameters.Add(num_com);
                                command4.Parameters.Add(plat);
                                command4.Parameters.Add(qte);
                                command4.CommandText = insertTable2;
                                ExecuteNonQuery(command4);

                                command4.Dispose();

                                // actualiser les stocks des produits utilisés pour faire la recette
                                
                                MySqlParameter nplat = new MySqlParameter("@nplat", MySqlDbType.VarChar);
                                nplat.Value = info;
                                MySqlCommand commandbisa = c.CreateCommand();
                                commandbisa.CommandText = "SELECT p.nom_produit, c.quantité FROM Produits p, Composent c WHERE c.nom_produit=p.nom_produit AND c.nom_recette=@nplat;";
                                commandbisa.Parameters.Add(nplat);

                                MySqlDataReader readerbisa = commandbisa.ExecuteReader();

                                int coeff = Convert.ToInt32(reponse1);
                                int quantity;
                                string nproduct;
                                MySqlCommand commande4 = connection.CreateCommand();
                                while (readerbisa.Read())
                                {
                                    nproduct =  readerbisa.GetString(0);
                                    quantity = (int)readerbisa.GetDouble(1);

                                    commande4.CommandText = "UPDATE Produits SET stock_actuel=stock_actuel-" + coeff*quantity + " WHERE nom_produit = \""+nproduct+"\";";
                                    ExecuteNonQuery(commande4);
                                }
                                Console.WriteLine();

                                readerbisa.Close();
                                commandbisa.Dispose();



                                // Update du solde_cook du Client : on lui retire le prix du plat * la quantité qu'il vient de commander 

                                MySqlParameter client = new MySqlParameter("@client", MySqlDbType.VarChar);
                                client.Value = identifiant;
                                MySqlParameter price0 = new MySqlParameter("@price", MySqlDbType.VarChar);
                                price0.Value = vprice;
                                MySqlParameter eff = new MySqlParameter("@eff", MySqlDbType.Float);
                                eff.Value = reponse1;

                                MySqlCommand commandbis0 = connection.CreateCommand();
                                commandbis0.Parameters.Add(client);
                                commandbis0.Parameters.Add(price0);
                                commandbis0.Parameters.Add(eff);
                                commandbis0.CommandText = "UPDATE Client SET solde_cook=solde_cook-(@eff*@price) WHERE nom_c=@client;";
                                ExecuteNonQuery(commandbis0);
                                Console.WriteLine();
                                
                                commandbis0.Dispose();




                                //on vérifie si la commande passée fait que le plat a été commandé plus de 10 fois, si c'est le cas, on augmente son prix de 2 cooks
                                

                                MySqlCommand update = c.CreateCommand();
                                update.CommandText = "UPDATE Recettes SET prix_vente=prix_vente+2 WHERE nom_recette= \"" + info + "\" AND nom_recette IN (select nom_recette from Constituent group by nom_recette having sum(nb_commandes)>10);";
                                ExecuteNonQuery(update);

                                update.Dispose();

                                //on vérifie si la commande passée fait que le plat a été commandé plus de 50 fois, si c'est le cas, on augmente son prix de 5 cooks et on augmente le CdR de 4 cooks

                                

                                //augmenter le prix de la recette de 5 cooks si commandée plus de 50 fois
                                MySqlCommand update1 = c.CreateCommand();
                                update1.CommandText = "UPDATE Recettes SET prix_vente=prix_vente+5 WHERE nom_recette= \"" + info + "\" AND nom_recette IN (select nom_recette from Constituent group by nom_recette having sum(nb_commandes)>50);";
                                ExecuteNonQuery(update1);

                                update1.Dispose();


                                //augmenter le solde du CdR de 4 cooks si sa recette est commandée plus de 50 fois
                                MySqlCommand update2 = c.CreateCommand();
                                update2.CommandText = "UPDATE Client SET solde_cook=solde_cook+4 WHERE nom_c IN (SELECT Client.nom_c FROM Recettes r WHERE r.nom_c=Client.nom_c AND r.nom_recette= \"" + info + "\" AND r.nom_recette IN (select nom_recette from Constituent group by nom_recette having sum(nb_commandes)>50) );";
                                ExecuteNonQuery(update2);

                                update2.Dispose(); 
                                


                                Console.WriteLine("Voulez-vous passer une autre commande (true or false)");
                                continu = Convert.ToBoolean(Console.ReadLine());


                            }

                            break;
                        case 2:
                            // affichage des infos du client (notamment solde_cook)
                            

                            MySqlParameter nom = new MySqlParameter("@nom", MySqlDbType.VarChar);
                            nom.Value = identifiant;
                            MySqlCommand command2 = connection.CreateCommand();
                            command2.CommandText = "SELECT * FROM Client WHERE nom_c=@nom;";
                            command2.Parameters.Add(nom);

                            MySqlDataReader reader2 = command2.ExecuteReader();
                            
                            string[] valueString2 = new string[reader2.FieldCount];
                            string[] intitule = new string[] { "Nom_client", "Tel", "Statut", "Solde_Cook" }; 
                            while (reader2.Read())
                            {
                                for (int i = 0; i < reader2.FieldCount; i++)
                                {
                                    valueString2[i] = reader2.GetValue(i).ToString();
                                    Console.WriteLine(intitule[i]+ " : "+valueString2[i]);
                                }
                                Console.WriteLine();
                            }
                            Console.WriteLine();

                            reader2.Close();
                            command2.Dispose();
                            
                            break;

                        case 3:

                            // insertion d'une recette dans Cooking.Recettes


                            //vérifier qu'il s'agit bien d'un cuisinier 
                            // si oui, il peut créer la recette
                            // sinon, afficher message de refus


                            MySqlParameter naame = new MySqlParameter("@naame", MySqlDbType.VarChar);
                            naame.Value = identifiant;
                            MySqlCommand cmd = c.CreateCommand();
                            cmd.CommandText = "SELECT statut FROM Client WHERE nom_c=@naame;";
                            cmd.Parameters.Add(naame);

                            MySqlDataReader readerbisaa = cmd.ExecuteReader();
                            
                            bool stat;
                            while (readerbisaa.Read())
                            {
                                stat = (bool)readerbisaa.GetValue(0);
                                if (stat is true)  // true si cdr, false sinon
                                {
                                    Console.WriteLine("Vous allez ajouter une recette, veuillez remplir le formulaire ci-dessous :");
                                    Console.WriteLine("Saisir le nom de la recette ");
                                    string nom_recette = Console.ReadLine();
                                    Console.WriteLine("Saisir le type de la recette : ");
                                    string type = Console.ReadLine();
                                    Console.WriteLine("Saisir le descriptif de la recette ");
                                    string descriptif = Console.ReadLine();
                                    Console.WriteLine("Saisir le prix de vente de la recette: (entier)");
                                    float prix = (float)Convert.ToDouble(Console.ReadLine());

                                    MySqlParameter nom_rec = new MySqlParameter("@nom_rec", MySqlDbType.VarChar);
                                    nom_rec.Value = nom_recette;
                                    MySqlParameter type_r = new MySqlParameter("@type_r", MySqlDbType.VarChar);
                                    type_r.Value = type;
                                    MySqlParameter desc = new MySqlParameter("@desc", MySqlDbType.VarChar);
                                    desc.Value = descriptif;
                                    MySqlParameter prix_r = new MySqlParameter("@prix_r", MySqlDbType.VarChar);
                                    prix_r.Value = prix;
                                    MySqlParameter nom_cr = new MySqlParameter("@nom_cr", MySqlDbType.VarChar);
                                    nom_cr.Value = identifiant;


                                    string insertTable20 = " INSERT INTO Recettes  Values (@nom_rec,@type_r,@desc,@prix_r,@nom_cr);";
                                    MySqlCommand command20 = connection.CreateCommand();
                                    command20.Parameters.Add(nom_rec);
                                    command20.Parameters.Add(type_r);
                                    command20.Parameters.Add(desc);
                                    command20.Parameters.Add(prix_r);
                                    command20.Parameters.Add(nom_cr);
                                    command20.CommandText = insertTable20;
                                    ExecuteNonQuery(command20);

                                    command20.Dispose();

                                    bool suite = true;
                                    while (suite == true)
                                    {
                                        Console.WriteLine("Saisir un ingrédient ");
                                        string ingr = Console.ReadLine();
                                        Console.WriteLine("Saisir la quantité nécessaire : ");
                                        float quantite = (float)Convert.ToDouble(Console.ReadLine());

                                        MySqlParameter ingr_ = new MySqlParameter("@ingr_", MySqlDbType.VarChar);
                                        ingr_.Value = ingr;
                                        MySqlParameter quant = new MySqlParameter("@quant", MySqlDbType.VarChar);
                                        quant.Value = quantite;


                                        string insertTable21 = " INSERT INTO Composent  Values (@ingr_,@nom_rec,@quant);";
                                        MySqlCommand command21 = connection.CreateCommand();
                                        command21.Parameters.Add(ingr_);
                                        command21.Parameters.Add(quant);
                                        command21.Parameters.Add(nom_rec);
                                        command21.CommandText = insertTable21;
                                        ExecuteNonQuery(command21);

                                        command21.Dispose();
                                        Console.WriteLine("Ajouter un autre ingédient ? (true or false)");
                                        suite = Convert.ToBoolean(Console.ReadLine());

                                    }


                                    Console.WriteLine();
                                    Console.WriteLine("Votre recette a bien été ajoutée à la base de données Cooking !");
                                }
                                else
                                {
                                    Console.WriteLine("Désolé, vous n'êtes pas cuisinier : vous ne pouvez donc pas créer de recettes !");
                                }
                            }
                            Console.WriteLine();

                            readerbisaa.Close();
                            cmd.Dispose();

                            break;

                        case 4:
                            // vérifier qu'il s'agit bien d'un CdR en amont
                            // affichage de la liste de recettes du CdR connecté

                            MySqlParameter naame_ = new MySqlParameter("@naame_", MySqlDbType.VarChar);
                            naame_.Value = identifiant;
                            MySqlCommand cmd_ = c.CreateCommand();
                            cmd_.CommandText = "SELECT statut FROM Client WHERE nom_c=@naame_;";
                            cmd_.Parameters.Add(naame_);

                            MySqlDataReader readerbisaa_ = cmd_.ExecuteReader();

                            bool stat_;
                            while (readerbisaa_.Read())
                            {
                                stat_ = (bool)readerbisaa_.GetValue(0);
                                if (stat_ is true) // true si cdr, false sinon
                                {
                                    Console.WriteLine("Voici la liste de vos recettes :");
                                    Console.WriteLine();

                                    MySqlParameter name = new MySqlParameter("@name", MySqlDbType.VarChar);
                                    name.Value = identifiant;

                                    MySqlCommand command_1 = connection.CreateCommand();
                                    command_1.CommandText = "SELECT nom_recette,type,descriptif,prix_vente FROM Recettes WHERE nom_c=@name;";
                                    command_1.Parameters.Add(name);

                                    MySqlDataReader reader_1 = command_1.ExecuteReader();

                                    string[] valueString_1 = new string[reader_1.FieldCount];
                                    string[] intitule_1 = new string[] { "Nom_recette", "Type", "Descritpif", "Prix_de_vente" }; 
                                    while (reader_1.Read())
                                    {
                                        for (int i = 0; i < reader_1.FieldCount; i++)
                                        {
                                            valueString_1[i] = reader_1.GetValue(i).ToString();
                                            Console.WriteLine(intitule_1[i] + " : " + valueString_1[i]);
                                        }
                                        Console.WriteLine();
                                    }
                                    Console.WriteLine();

                                    reader_1.Close();
                                    command_1.Dispose();
                                }
                                else
                                {
                                    Console.WriteLine("Désolé, vous n'êtes pas cuisinier : vous n'avez donc pas créé de recettes !");
                                }
                            }
                            Console.WriteLine();

                            readerbisaa_.Close();
                            cmd_.Dispose();

                           
                            break;
                       
                    }
                    connection.Close();

                    break;
                case 2:
                    MySqlConnection connection1 = Connection();
                    connection1.Open();

                    Console.WriteLine("Vous allez créer votre compte Cooking, veuillez remplir le formulaire ci-dessous :");
                    Console.WriteLine("Saisir votre NOM + ' ' + Prénom : ");
                    string nom_prenom = Console.ReadLine();
                    Console.WriteLine("Saisir votre numéro de téléphone : ");
                    int numero = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Souhaitez-vous vous enregistrer en tant que client ou Créateur de Recettes ? (0 :client // 1 :Créateur de Recettes): ");
                    int result = Convert.ToInt32(Console.ReadLine());


                    MySqlParameter nom_pren = new MySqlParameter("@nom_pren", MySqlDbType.VarChar);
                    nom_pren.Value = nom_prenom;
                    MySqlParameter numer = new MySqlParameter("@numer", MySqlDbType.Int32);
                    numer.Value = numero;
                    MySqlParameter res = new MySqlParameter("@res", MySqlDbType.Int32);
                    res.Value = result;
                    string insertTable = " INSERT INTO Client  Values (@nom_pren,@numer,@res,'0');"; //solde_cook à 0 par défaut quand le compte est créé
                    MySqlCommand command10 = connection1.CreateCommand();
                    command10.Parameters.Add(nom_pren);
                    command10.Parameters.Add(numer);
                    command10.Parameters.Add(res);
                    command10.CommandText = insertTable;
                    ExecuteNonQuery(command10);

                    command10.Dispose();

                    Console.WriteLine();
                    Console.WriteLine("Vous avez bien été ajouté au serveur Cooking !");
                    break;

                case 3:

                    Console.WriteLine("Saisir le mot de passe :");
                    string mdp = Console.ReadLine();
                    Console.WriteLine();
                    if (mdp == "cooking2020")
                    {
                        Console.WriteLine("Voici le gestionnaire des tâches, que souhaitez-vous faire ?" +
                            "\n 1) Visualisation de stocks" +
                            "\n 2) Visualisation des infos clients" +
                            "\n 3) Visualisation des infos commandes" +
                            "\n 4) Top 5 des commandes de la semaine" +
                            "\n 5) Top 5 des commandes en général" +
                            "\n 6) Voir le cuisinier de la semaine" +
                            "\n 7) Voir le cuisinier d'or" +
                            "\n 8) Supprimer un client/ cuisinier" +
                            "\n 9) Supprimer une recette" +
                            "\n 10) Vérification et update des stocks inutilisés" +
                            "\n 11) Génération fichier XmL ");
                        Console.WriteLine();

                        int choix2 = Convert.ToInt32(Console.ReadLine());
                        switch (choix2)
                        {
                            case 1:
                                Console.WriteLine(" Voici l'état du stock actuel : \n");
                                MySqlCommand command1 = c.CreateCommand();
                                command1.CommandText = "SELECT * FROM Produits;";

                                MySqlDataReader reader11;
                                reader11 = command1.ExecuteReader();

                                /* exemple de manipulation du resultat */
                                string[] valueString6 = new string[reader11.FieldCount];
                                string[] intitule1 = new string[] { "Nom_produit", "Catégorie", "Unité_qté", "Stock_actuel", "Stock_min", "Stock_max", "Nom_fournisseur" };
                                while (reader11.Read())
                                {
                                    for (int i = 0; i < reader11.FieldCount; i++)
                                    {
                                        valueString6[i] = reader11.GetValue(i).ToString();
                                        Console.WriteLine(intitule1[i] + " : " + valueString6[i]);
                                        Console.WriteLine();
                                    }
                                    Console.WriteLine();
                                }
                                Console.WriteLine();

                                reader11.Close();
                                command1.Dispose();
                                break;
                            case 2:
                                Console.WriteLine(" Voici les informations concernant les clients actuels : \n");
                                MySqlCommand command2 = c.CreateCommand();
                                command2.CommandText = "SELECT * FROM Client;";

                                MySqlDataReader reader4;
                                reader4 = command2.ExecuteReader();

                                /* exemple de manipulation du resultat */
                                string[] valueString3 = new string[reader4.FieldCount];
                                string[] intitule2 = new string[] { "Nom_client", "Num_tel", "Statut", "Solde_cook" };
                                while (reader4.Read())
                                {
                                    for (int i = 0; i < reader4.FieldCount; i++)
                                    {
                                        valueString3[i] = reader4.GetValue(i).ToString();
                                        Console.WriteLine(intitule2[i] + " : " + valueString3[i]);
                                        Console.WriteLine();
                                    }
                                    Console.WriteLine();
                                }
                                Console.WriteLine();

                                reader4.Close();
                                command2.Dispose();
                                break;
                            case 3:
                                Console.WriteLine(" Voici les informations concernant l'état des commandes : \n");
                                MySqlCommand command3 = c.CreateCommand();
                                command3.CommandText = "SELECT * FROM Commande;";

                                MySqlDataReader reader5;
                                reader5 = command3.ExecuteReader();

                                /* exemple de manipulation du resultat */
                                string[] valueString4 = new string[reader5.FieldCount];
                                string[] intitule3 = new string[] { "Num_commande", "Contenu", "Montant", "Date_commande" };
                                while (reader5.Read())
                                {
                                    for (int i = 0; i < reader5.FieldCount; i++)
                                    {
                                        valueString4[i] = reader5.GetValue(i).ToString();
                                        Console.WriteLine(intitule3[i] + " : " + valueString4[i]);
                                        Console.WriteLine();
                                    }
                                    Console.WriteLine();
                                }
                                Console.WriteLine();

                                reader5.Close();
                                command3.Dispose();
                                break;
                            case 4:

                                //top 5 des recettes de la semaine
                                Console.WriteLine(" Voici le top 5 des commandes de la semaine : \n");
                                MySqlCommand command7 = c.CreateCommand();
                                command7.CommandText = "SELECT r.nom_recette, r.nom_c , sum(nb_commandes) as nb FROM Recettes r, Constituent c, Commande co WHERE r.nom_recette = c.nom_recette and c.num_commande = co.num_commande and date_c BETWEEN NOW() -INTERVAL 1 week and NOW() group by r.nom_recette order by nb DESC LIMIT 5; ";

                                MySqlDataReader reader7= command7.ExecuteReader();

                                
                                string[] valueString7 = new string[reader7.FieldCount];
                                string[] intitule7 = new string[] { "Nom_recette", "Nom_cuisinier", "Total_commandes" };
                                while (reader7.Read())
                                {
                                    for (int i = 0; i < reader7.FieldCount; i++) 
                                    {
                                        valueString7[i] = reader7.GetValue(i).ToString();
                                        Console.WriteLine(intitule7[i] + " : " + valueString7[i]);
                                        Console.WriteLine();
                                    }
                                    Console.WriteLine();
                                }
                                Console.WriteLine();

                                reader7.Close();
                                command7.Dispose();
                                break;

                            case 5:

                                //top 5 des recettes en général
                                Console.WriteLine(" Voici le top 5 des commandes en général : \n");
                                MySqlCommand command7_ = c.CreateCommand();
                                command7_.CommandText = "SELECT r.nom_recette, r.nom_c , sum(nb_commandes) as nb FROM Recettes r, Constituent c, Commande co WHERE r.nom_recette = c.nom_recette and c.num_commande = co.num_commande group by r.nom_recette order by nb DESC LIMIT 5; ";

                                MySqlDataReader reader7_ = command7_.ExecuteReader();


                                string[] valueString7_ = new string[reader7_.FieldCount];
                                string[] intitule7_ = new string[] { "Nom_recette", "Nom_cuisinier", "Total_commandes" };
                                while (reader7_.Read())
                                {
                                    for (int i = 0; i < reader7_.FieldCount; i++)
                                    {
                                        valueString7_[i] = reader7_.GetValue(i).ToString();
                                        Console.WriteLine(intitule7_[i] + " : " + valueString7_[i]);
                                        Console.WriteLine();
                                    }
                                    Console.WriteLine();
                                }
                                Console.WriteLine();

                                reader7_.Close();
                                command7_.Dispose();
                                break;

                            case 6:
                                // CdR de la semaine
                                Console.WriteLine(" Voici le créateur de recettes de la semaine : \n");
                                MySqlCommand command8 = c.CreateCommand();
                                command8.CommandText = "SELECT r.nom_c,sum(nb_commandes) FROM Recettes r, Constituent c, Commande co WHERE r.nom_recette = c.nom_recette and c.num_commande = co.num_commande and date_c BETWEEN NOW() -INTERVAL 1 week and NOW() group by r.nom_c ORDER BY sum(nb_commandes) DESC LIMIT 1 ; ";

                                MySqlDataReader reader8;
                                reader8 = command8.ExecuteReader();
                                
                                string[] valueString8 = new string[reader8.FieldCount];
                                string[] intitule8 = new string[] { "Nom_recette", "Total_commandes"};
                                while (reader8.Read())
                                {
                                    for (int i = 0; i < reader8.FieldCount; i++) 
                                    {
                                        valueString8[i] = reader8.GetValue(i).ToString();
                                        Console.WriteLine(intitule8[i] + " : " + valueString8[i]);
                                        Console.WriteLine();
                                    }
                                    Console.WriteLine();
                                }
                                Console.WriteLine();

                                reader8.Close();
                                command8.Dispose();
                                break;

                            case 7:
                                // CdR d'Or
                                Console.WriteLine(" Voici le créateur de recettes d'or : \n");
                                MySqlCommand command8_ = c.CreateCommand();
                                command8_.CommandText = "SELECT r.nom_c,sum(nb_commandes) FROM Recettes r, Constituent c, Commande co WHERE r.nom_recette = c.nom_recette and c.num_commande = co.num_commande group by r.nom_c ORDER BY sum(nb_commandes) DESC LIMIT 1 ; ";

                                MySqlDataReader reader8_;
                                reader8_ = command8_.ExecuteReader();

                                string[] valueString8_ = new string[reader8_.FieldCount];
                                string[] intitule8_ = new string[] { "Nom_recette", "Total_commandes" };
                                string nomcrea;
                                while (reader8_.Read())
                                {
                                    for (int i = 0; i < reader8_.FieldCount; i++)
                                    {
                                        valueString8_[i] = reader8_.GetValue(i).ToString();
                                        Console.WriteLine(intitule8_[i] + " : " + valueString8_[i]);
                                        Console.WriteLine();
                                    }
                                    Console.WriteLine();
                                    nomcrea = valueString8_[0];
                                }
                                Console.WriteLine();
                                nomcrea = valueString8_[0];

                                reader8_.Close();
                                command8_.Dispose();

                                Console.WriteLine(" Voici ses 5 recettes les plus commandées : \n");
                                MySqlCommand command8_0 = c.CreateCommand();
                                MySqlParameter createur=new MySqlParameter("@createur", MySqlDbType.VarChar);
                                createur.Value = nomcrea;
                                command8_0.Parameters.Add(createur);
                                command8_0.CommandText = "SELECT r.nom_recette, sum(nb_commandes) as nb FROM Recettes r, Constituent c, Commande co WHERE r.nom_recette = c.nom_recette and c.num_commande = co.num_commande AND r.nom_c=@createur group by r.nom_recette order by nb DESC LIMIT 5;";
                                MySqlDataReader reader8_0;
                                reader8_0 = command8_0.ExecuteReader();

                                string[] valueString8_0 = new string[reader8_0.FieldCount];
                                string[] intitule8_0 = new string[] { "Nom_recette", "Nb_commandes" };
                                while (reader8_0.Read())
                                {
                                    for (int i = 0; i < reader8_0.FieldCount; i++)
                                    {
                                        valueString8_0[i] = reader8_0.GetValue(i).ToString();
                                        Console.WriteLine(intitule8_0[i] + " : " + valueString8_0[i]);
                                        Console.WriteLine();
                                    }
                                    Console.WriteLine();
                                }
                                Console.WriteLine();

                                reader8_0.Close();
                                command8_0.Dispose();
                                break;

                            case 8:

                                //suppression d'un client
                            
                                Console.WriteLine("Voici la liste actuelle des clients/ cusiniers :");
                                Console.WriteLine();
                                MySqlCommand command_2 = c.CreateCommand();
                                command_2.CommandText = "SELECT * FROM Client ;";

                                MySqlDataReader reader_2;
                                reader_2 = command_2.ExecuteReader();

                                /* exemple de manipulation du resultat */
                                string[] valueString2 = new string[reader_2.FieldCount];
                                string[] intitule = new string[] { "Nom_client", "Tel", "Statut", "Solde_Cook" };
                                while (reader_2.Read())
                                {
                                    for (int i = 0; i < reader_2.FieldCount; i++)
                                    {
                                        valueString2[i] = reader_2.GetValue(i).ToString();
                                        if (valueString2[i] == null)
                                        {
                                            valueString2[i] = "NULL";
                                        }
                                        Console.WriteLine(intitule[i] + " : " + valueString2[i]);
                                    }
                                    Console.WriteLine();
                                }
                                Console.WriteLine();

                                reader_2.Close();
                                command_2.Dispose();


                                Console.WriteLine("Quel client/ cusiniers souhaitez-vous supprimer ?");
                                string answer = Console.ReadLine();

                                MySqlParameter suppr = new MySqlParameter("@supprime", MySqlDbType.VarChar);
                                suppr.Value = answer;

                                string suppress = " DELETE FROM Client WHERE nom_c=@supprime;";
                                MySqlCommand command_0 = c.CreateCommand();
                                command_0.Parameters.Add(suppr);
                                command_0.CommandText = suppress;
                                ExecuteNonQuery(command_0);

                                command_0.Dispose();


                                Console.WriteLine();
                                Console.WriteLine("Voici la liste actualisée des clients/ cusiniers :");
                                Console.WriteLine();
                                MySqlCommand command_3 = c.CreateCommand();
                                command_3.CommandText = "SELECT * FROM Client ;";

                                MySqlDataReader reader_3;
                                reader_3 = command_2.ExecuteReader();

                                /* exemple de manipulation du resultat */
                                string[] valueString_3 = new string[reader_3.FieldCount];
                                string[] intitule_3 = new string[] { "Nom_client", "Tel", "Statut", "Solde_Cook" }; //cas solde cook null ??
                                while (reader_3.Read())
                                {
                                    for (int i = 0; i < reader_3.FieldCount; i++)
                                    {
                                        valueString_3[i] = reader_3.GetValue(i).ToString();
                                        if (valueString_3[i] == null)
                                        {
                                            valueString_3[i] = "NULL";
                                        }
                                        Console.WriteLine(intitule_3[i] + " : " + valueString_3[i]);
                                    }
                                    Console.WriteLine();
                                }
                                Console.WriteLine();

                                reader_3.Close();
                                command_3.Dispose();
                                break;

                            case 9:

                                //suppression d'une recette
                                Console.WriteLine("Voici la liste actuelle des recettes :");
                                Console.WriteLine();
                                MySqlCommand command_4 = c.CreateCommand();
                                command_4.CommandText = "SELECT * FROM Recettes ;";

                                MySqlDataReader reader_4;
                                reader_4 = command_4.ExecuteReader();

                                /* exemple de manipulation du resultat */
                                string[] valueString_4 = new string[reader_4.FieldCount];
                                string[] intitule_4 = new string[] { "Nom_recette", "Type", "Descriptif", "Prix_vente", "Nom_créateur" };
                                while (reader_4.Read())
                                {
                                    for (int i = 0; i < reader_4.FieldCount; i++)
                                    {
                                        valueString_4[i] = reader_4.GetValue(i).ToString();
                                        if (valueString_4[i] == null)
                                        {
                                            valueString_4[i] = "NULL";
                                        }
                                        Console.WriteLine(intitule_4[i] + " : " + valueString_4[i]);
                                    }
                                    Console.WriteLine();
                                }
                                Console.WriteLine();

                                reader_4.Close();
                                command_4.Dispose();


                                Console.WriteLine("Quel recette souhaitez-vous supprimer ?");
                                string answer_0 = Console.ReadLine();

                                MySqlParameter suppri = new MySqlParameter("@supprime_", MySqlDbType.VarChar);
                                suppri.Value = answer_0;

                                string suppression = " DELETE FROM Recettes WHERE nom_recette=@supprime_;";
                                MySqlCommand command_5 = c.CreateCommand();
                                command_5.Parameters.Add(suppri);
                                command_5.CommandText = suppression;
                                ExecuteNonQuery(command_5);

                                command_5.Dispose();


                                Console.WriteLine();
                                Console.WriteLine("Voici la liste actualisée des recettes :");
                                Console.WriteLine();
                                MySqlCommand command_6 = c.CreateCommand();
                                command_6.CommandText = "SELECT * FROM Recettes ;";

                                MySqlDataReader reader_6;
                                reader_6 = command_6.ExecuteReader();

                                /* exemple de manipulation du resultat */
                                string[] valueString_6 = new string[reader_6.FieldCount];
                                string[] intitule_6 = new string[] { "Nom_recette", "Type", "Descriptif", "Prix_vente", "Nom_créateur" };
                                while (reader_6.Read())
                                {
                                    for (int i = 0; i < reader_6.FieldCount; i++)
                                    {
                                        valueString_6[i] = reader_6.GetValue(i).ToString();
                                        if (valueString_6[i] == null)
                                        {
                                            valueString_6[i] = "NULL";
                                        }
                                        Console.WriteLine(intitule_6[i] + " : " + valueString_6[i]);
                                    }
                                    Console.WriteLine();
                                }
                                Console.WriteLine();

                                reader_6.Close();
                                command_6.Dispose();
                                break;
                            
                            case 10:

                                // afficher la lsite des produits non utilisés au cours des 30 derniers jours + leur stock_actuel

                                Console.WriteLine("\n Voici les produits qui n'ont pas été utilisés au cours des 30 derniers jours :");
                                Console.WriteLine();
                                MySqlCommand comm = c.CreateCommand();
                                comm.CommandText = "SELECT nom_produit, stock_actuel FROM Produits WHERE stock_actuel>0 AND nom_produit NOT IN (SELECT p.nom_produit FROM Produits p JOIN Composent c ON p.nom_produit=c.nom_produit JOIN Recettes r ON r.nom_recette = c.nom_recette JOIN Constituent co ON co.nom_recette = r.nom_recette JOIN Commande com ON co.num_commande = com.num_commande WHERE com.date_c BETWEEN NOW() - INTERVAL 30 day and NOW() GROUP BY p.nom_produit) ; ";

                                MySqlDataReader read= comm.ExecuteReader();
                                
                                string[] vString = new string[read.FieldCount];
                                string[] intitul = new string[] { "Nom_produit", "Stock_actuel" };
                                while (read.Read())
                                {
                                    for (int i = 0; i < read.FieldCount; i++)
                                    {
                                        vString[i] = read.GetValue(i).ToString();
                                        Console.WriteLine(intitul[i] + " : " + vString[i]);
                                    }
                                    Console.WriteLine();
                                }
                                Console.WriteLine();

                                read.Close();
                                comm.Dispose();

                                //diviser leur stock_min et stock_max par 2

                                

                                bool follow = true;
                                while(follow==true)
                                {
                                    Console.WriteLine("Saisir le nom du produit dont vous voulez diviser le stock par 2");
                                    string saisie = Console.ReadLine();
                                    MySqlCommand up1 = c.CreateCommand();
                                    MySqlCommand up2 = c.CreateCommand();
                                    up1.CommandText = "UPDATE Produits SET stock_min=stock_min/2 WHERE nom_produit = \"" + saisie + "\";";
                                    ExecuteNonQuery(up1);
                                    up2.CommandText = "UPDATE Produits SET stock_max=stock_max/2 WHERE nom_produit = \"" + saisie + "\";";
                                    ExecuteNonQuery(up2);

                                    Console.WriteLine("Ses stocks min et max ont bien été divisés par 2 !");
                                    Console.WriteLine();

                                    //affichage pour la vérifiaction

                                    MySqlCommand comm1 = c.CreateCommand();
                                    comm1.CommandText = "SELECT nom_produit, stock_actuel, stock_min, stock_max FROM Produits WHERE nom_produit = \"" + saisie + "\";";
                                    MySqlDataReader rdr = comm1.ExecuteReader();

                                    string[] vStringv = new string[rdr.FieldCount];
                                    string[] intitulv = new string[] { "Nom_produit", "Stock_actuel", "Stock_min", "Stock_max" };
                                    while (rdr.Read())
                                    {
                                        for (int i = 0; i < rdr.FieldCount; i++)
                                        {
                                            vStringv[i] = rdr.GetValue(i).ToString();
                                            Console.WriteLine(intitulv[i] + " : " + vStringv[i]);
                                        }
                                        Console.WriteLine();
                                    }
                                    Console.WriteLine();


                                    rdr.Close();
                                    comm1.Dispose();

                                    Console.WriteLine("Voulez-vous diviser d'autres stocks par 2 ? (true or false)");
                                    follow = Convert.ToBoolean(Console.ReadLine());
                                }

                               



                                break;
                            case 11:
                                //générer le fichier xml avec les produits à commander

                                string requete3 = "SELECT nom_f,nom_produit,(stock_max)-(stock_actuel) FROM Produits WHERE stock_actuel <= stock_min;";
                                MySqlCommand comm3 = c.CreateCommand();
                                comm3.CommandText = requete3;
                                Console.WriteLine();
                                MySqlDataReader read3 = comm3.ExecuteReader();

                                
                                XmlDocument listeCommande = new XmlDocument();
                                Console.WriteLine();

                                XmlElement racine = listeCommande.CreateElement("commandes");
                                listeCommande.AppendChild(racine);


                                XmlDeclaration xmldecl = listeCommande.CreateXmlDeclaration("1.0", "UTF-8", "no");
                                listeCommande.InsertBefore(xmldecl, racine);


                                while (read3.Read())
                                {

                                    XmlElement fournisseur = listeCommande.CreateElement("fournisseur");
                                    racine.AppendChild(fournisseur);

                                    XmlElement nom = listeCommande.CreateElement("nom");
                                    nom.InnerText = (string)read3["nom_f"];
                                    fournisseur.AppendChild(nom);

                                    XmlElement produit3 = listeCommande.CreateElement("produit");
                                    produit3.InnerText = (string)read3["nom_produit"];
                                    fournisseur.AppendChild(produit3);

                                    XmlElement stock = listeCommande.CreateElement("stock_à_commander");
                                    double a = (double)read3["(stock_max)-(stock_actuel)"];
                                    stock.InnerText = "" + a;
                                    fournisseur.AppendChild(stock);
                                }


                                listeCommande.Save("listeCommande.xml");
                                Console.WriteLine("fichier listeCommande.xml créé");

                                read3.Close();
                                comm3.Dispose();
                                break;

                        }
                    }
                    else
                    {
                        Console.WriteLine("ERREUR IDENTIFICATION");
                    }
                    break;
                case 4:
                    //MODE DEMO

                    // nombre de clients
                    Console.WriteLine("Bienvenue dans le mode démo !");
                    Console.WriteLine("\n Voici le nombre total de clients à ce jour : ");
                    
                    MySqlCommand command_8 = c.CreateCommand();
                    command_8.CommandText = "SELECT count(*) FROM Client; ";

                    MySqlDataReader reader_8;
                    reader_8 = command_8.ExecuteReader();
                    
                    string[] valueString_8 = new string[reader_8.FieldCount];
                    while (reader_8.Read())
                    {
                        for (int i = 0; i < reader_8.FieldCount; i++)
                        {
                            valueString_8[i] = reader_8.GetValue(i).ToString();
                            Console.Write(valueString_8[i]);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();

                    reader_8.Close();
                    command_8.Dispose();

                    // Nombre de CdR
                    Console.WriteLine("\n Voici le nombre total de CdR : \n");

                    MySqlCommand command_9 = c.CreateCommand();
                    command_9.CommandText = "SELECT count(nom_c) FROM Client WHERE statut=1; ";

                    MySqlDataReader reader_9;
                    reader_9 = command_9.ExecuteReader();

                    string[] valueString_9 = new string[reader_9.FieldCount];
                    while (reader_9.Read())
                    {
                        for (int i = 0; i < reader_9.FieldCount; i++)
                        {
                            valueString_9[i] = reader_9.GetValue(i).ToString();
                            Console.Write(valueString_9[i]);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();

                    reader_9.Close();
                    command_9.Dispose();

                    // Liste des CdR
                    Console.WriteLine("\n Voici la liste des CdR et leurs noms : \n");

                    MySqlCommand command_09 = c.CreateCommand();
                    command_09.CommandText = "SELECT nom_c FROM Client WHERE statut=1; ";

                    MySqlDataReader reader_09;
                    reader_09 = command_09.ExecuteReader();

                    string[] valueString_09 = new string[reader_09.FieldCount];
                    while (reader_09.Read())
                    {
                        for (int i = 0; i < reader_09.FieldCount; i++)
                        {
                            valueString_09[i] = reader_09.GetValue(i).ToString();
                            Console.Write(valueString_09[i]);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();

                    reader_09.Close();
                    command_09.Dispose();

                    // Liste des CdR avec leur nb total de recettes commandées
                    Console.WriteLine("\n Voici la liste des CdR avec leurs recettes commandées : \n");

                    MySqlCommand command_10 = c.CreateCommand();
                    command_10.CommandText = "SELECT c.nom_c, ' : ',r.nom_recette , ' -> ',SUM(co.nb_commandes) FROM Client c, Recettes r, Constituent co WHERE c.statut=1 AND r.nom_c=c.nom_c AND co.nom_recette=r.nom_recette GROUP BY nom_recette; ";

                    MySqlDataReader reader_10;
                    reader_10 = command_10.ExecuteReader();
                    
                    string[] valueString_10 = new string[reader_10.FieldCount];
                    while (reader_10.Read())
                    {
                        for (int i = 0; i < reader_10.FieldCount; i++)
                        {
                            valueString_10[i] = reader_10.GetValue(i).ToString();
                            Console.Write(valueString_10[i]);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();

                    reader_10.Close();
                    command_10.Dispose();


                    // nombre de recettes

                    Console.WriteLine("\n Voici le nombre total de recettes à ce jour : ");
                    MySqlCommand command_11 = c.CreateCommand();
                    command_11.CommandText = "SELECT count(*) FROM Recettes; ";

                    MySqlDataReader reader_11;
                    reader_11 = command_11.ExecuteReader();

                    
                    string[] valueString_11 = new string[reader_11.FieldCount];
                    while (reader_11.Read())
                    {
                        for (int i = 0; i < reader_11.FieldCount; i++)
                        {
                            valueString_11[i] = reader_11.GetValue(i).ToString();
                            Console.Write(valueString_11[i]);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();

                    reader_11.Close();
                    command_11.Dispose();


                    // liste de produits dont le stock actuel est inférieur à 2 fois la quantité minimale

                    Console.WriteLine("\n Voici les produits dont le stock actuel est inférieur à 2 fois la quantité minimale : \n ");
                    MySqlCommand command_12 = c.CreateCommand();
                    command_12.CommandText = "SELECT nom_produit, stock_actuel, stock_min FROM Produits WHERE 2 * stock_actuel <= stock_min;";


                    MySqlDataReader reader_12;
                    reader_12 = command_12.ExecuteReader();

                    string[] valuesString12 = new string[reader_12.FieldCount];
                    string[] intituler12 = new string[] { "Nom_produit", "Stock_actuel", "Stock_min" };
                    while (reader_12.Read())
                    {
                        for (int i = 0; i < reader_12.FieldCount; i++)
                        {
                            valuesString12[i] = reader_12.GetValue(i).ToString();
                            Console.WriteLine(intituler12[i] + " : " + valuesString12[i]);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();

                    reader_12.Close();
                    command_12.Dispose();

                    //saisie au clavier d'un produit de la liste précédente + affichage des recettes qui le contiennent

                    Console.WriteLine("Saisir le produit qui vous intéresse :");
                    string prod = Console.ReadLine();
                    Console.WriteLine("\n Voici les recettes contenant ce produit : \n");
                    MySqlParameter produit = new MySqlParameter("@produit", MySqlDbType.VarChar);
                    produit.Value = prod;
                    MySqlCommand command_13 = c.CreateCommand();
                    command_13.CommandText = "SELECT nom_recette FROM Composent WHERE nom_produit=@produit GROUP BY nom_recette;;";
                    command_13.Parameters.Add(produit);

                    MySqlDataReader reader_13;
                    reader_13 = command_13.ExecuteReader();

                    string[] valueString13 = new string[reader_13.FieldCount];
                    while (reader_13.Read())
                    {
                        for (int i = 0; i < reader_13.FieldCount; i++)
                        {
                            valueString13[i] = reader_13.GetValue(i).ToString();
                            Console.WriteLine(valueString13[i]);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine(" PAS D'AUTRES RECETES");

                    reader_13.Close();
                    command_13.Dispose();


                    break;

            }


            Console.ReadKey();
        }
    }
}
