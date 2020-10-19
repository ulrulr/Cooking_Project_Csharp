-- suppression database du même nom --
drop database `cooking` ;

-- création de la database --
create database cooking;
 
USE `cooking`;

-- création des tables --
CREATE TABLE `cooking`.`Client` (
  `nom_c` VARCHAR(20) NOT NULL,
  `num_tel` INT NULL ,
  `statut` BOOLEAN NULL ,
  `solde_cook` FLOAT NULL,
  PRIMARY KEY (`nom_c`) );
        
CREATE TABLE `cooking`.`Fournisseur` (
  `nom_f` VARCHAR(20) NOT NULL,
  `num_f` INT NULL,
  PRIMARY KEY (`nom_f`) ); 
  
CREATE TABLE `cooking`.`Recettes` (
  `nom_recette` VARCHAR(20) NOT NULL,
  `type` VARCHAR(20) NULL,
  `descriptif` VARCHAR(256)NULL,
  `prix_vente` FLOAT NULL,
  `nom_c` VARCHAR(40)NULL,
  PRIMARY KEY (`nom_recette`) ,
  CONSTRAINT `nom_cdr` FOREIGN KEY (`nom_c`)
		REFERENCES `cooking`.`Client` (`nom_c`)
		ON DELETE CASCADE
		ON UPDATE NO ACTION );
 
CREATE TABLE `cooking`.`Produits` (
  `nom_produit` VARCHAR(20) NOT NULL,
  `categorie` VARCHAR(20) NULL,
  `unite_qte` VARCHAR(20) NULL,
  `stock_actuel` FLOAT NULL,
  `stock_min` FLOAT NULL,
  `stock_max` FLOAT NULL,
  `nom_f` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`nom_produit`));

ALTER TABLE `Produits` ADD FOREIGN KEY (`nom_f`) REFERENCES `Fournisseur`(`nom_f`) ON DELETE CASCADE ;

CREATE TABLE `cooking`.`Commande` (
  `num_commande` INT NOT NULL,
  `contenu` VARCHAR(200) NULL,
  `montant_c` FLOAT NULL,
  `date_c` DATETIME NULL,
  PRIMARY KEY (`num_commande`) );
        
ALTER TABLE `Commande` ADD FOREIGN KEY (`contenu`) REFERENCES `Recettes`(`nom_recette`)ON DELETE CASCADE;
  
CREATE TABLE `cooking`.`Passent` (
  `nom_client` VARCHAR(20) NOT NULL,
  `num_commande` INT NOT NULL,
  PRIMARY KEY (`nom_client`,`num_commande`) );
  
CREATE TABLE `cooking`.`Constituent` (
  `num_commande` INT NOT NULL,
  `nom_recette` VARCHAR(20) NOT NULL,
  `nb_commandes` INT NULL,
  PRIMARY KEY (`num_commande`)  );
  
  ALTER TABLE `Constituent` ADD FOREIGN KEY (`num_commande`) REFERENCES `Commande`(`num_commande`) ON DELETE CASCADE ;
  ALTER TABLE `Constituent` ADD FOREIGN KEY (`nom_recette`) REFERENCES `Recettes`(`nom_recette`) ON DELETE CASCADE ;
  
CREATE TABLE `cooking`.`Composent` (
  `nom_produit` VARCHAR(20) NOT NULL,
  `nom_recette` VARCHAR(20) NOT NULL,
  `quantité` FLOAT NULL,
  PRIMARY KEY (`nom_produit`,`nom_recette`)  );
        
ALTER TABLE `Composent` ADD FOREIGN KEY (`nom_produit`) REFERENCES `Produits`(`nom_produit`) ON DELETE CASCADE ;
ALTER TABLE `Composent` ADD FOREIGN KEY (`nom_recette`) REFERENCES `Recettes`(`nom_recette`) ON DELETE CASCADE ;

-- peuplement de la database --

INSERT INTO `cooking`.`Client` (`nom_c`,`num_tel`,`statut`,`solde_cook`) VALUES ('DUVAL Jean', '0678910023', '1', '110.0');
INSERT INTO `cooking`.`Client` (`nom_c`,`num_tel`,`statut`,`solde_cook`) VALUES ('LECLERC Pierre', '0733410024', '1', null);
INSERT INTO `cooking`.`Client` (`nom_c`,`num_tel`,`statut`,`solde_cook`) VALUES ('DURAND Marie', '0674410025', '0', null);
INSERT INTO `cooking`.`Client` (`nom_c`,`num_tel`,`statut`,`solde_cook`) VALUES ('ANDRE Laura','0612310026','1','73.0');
INSERT INTO `cooking`.`Client` (`nom_c`,`num_tel`,`statut`,`solde_cook`) VALUES ('LAPEL Carla','0657310027','1','552.0');
INSERT INTO `cooking`.`Client` (`nom_c`,`num_tel`,`statut`,`solde_cook`) VALUES ('DONZEAU Antoine','0711210028','0',null);
INSERT INTO `cooking`.`Client` (`nom_c`,`num_tel`,`statut`,`solde_cook`) VALUES ('LECOQ Paul','0623410029','1','99.99');
INSERT INTO `cooking`.`Client` (`nom_c`,`num_tel`,`statut`,`solde_cook`) VALUES ('MAHDOUB Salma','0649845534','1','120.99');
INSERT INTO `cooking`.`Client` (`nom_c`,`num_tel`,`statut`,`solde_cook`) VALUES ('MAMA Ulrich','0612435029','1','135.99');
INSERT INTO `cooking`.`Client` (`nom_c`,`num_tel`,`statut`,`solde_cook`) VALUES ('COURBIN Pierre','0709345429','1','299.99');

INSERT INTO `cooking`.`Recettes` (`nom_recette`,`type`,`descriptif`,`prix_vente`,`nom_c`) VALUES ('tomates-mozza','entrée','Délicieuse entrée composée de tomates et de sa mozarrella fraichement tranchées','6.00','LECLERC Pierre');
INSERT INTO `cooking`.`Recettes` (`nom_recette`,`type`,`descriptif`,`prix_vente`,`nom_c`) VALUES ('carpaccio','entrée','Fines tranches de filet de boeuf accompagnées de champignons frais','13.00','LAPEL Carla');
INSERT INTO `cooking`.`Recettes` (`nom_recette`,`type`,`descriptif`,`prix_vente`,`nom_c`) VALUES ('bruschetta','entrée',"Ciabatta croustillante avec des tomates marinées dans de l'huile d'olive",'4.00','LECLERC Pierre');
INSERT INTO `cooking`.`Recettes` (`nom_recette`,`type`,`descriptif`,`prix_vente`,`nom_c`) VALUES ('pâtes_au_pesto','plat',"Pâtes fraiches maison accompagnées de leur sauce au pesto",'14.00','MAHDOUB Salma');
INSERT INTO `cooking`.`Recettes` (`nom_recette`,`type`,`descriptif`,`prix_vente`,`nom_c`) VALUES ('pâtes_au_saumon','plat',"Tagliattelles fraiches maison et leur pavé de saumon",'17.00','LECLERC Pierre');
INSERT INTO `cooking`.`Recettes` (`nom_recette`,`type`,`descriptif`,`prix_vente`,`nom_c`) VALUES ('poulet_braisé','plat',"Cuisse de poulet cuite à la braise accompagnée de frites maison",'16.00','ANDRE Laura');
INSERT INTO `cooking`.`Recettes` (`nom_recette`,`type`,`descriptif`,`prix_vente`,`nom_c`) VALUES ('buddha_bowl','plat',"Bowl frais aux saveurs exotiques",'14.00','DONZEAU Antoine');
INSERT INTO `cooking`.`Recettes` (`nom_recette`,`type`,`descriptif`,`prix_vente`,`nom_c`) VALUES ('moelleux_au_chocolat','déssert',"Authentique moelleux, gourmand et fondant",'6.00','ANDRE Laura');
INSERT INTO `cooking`.`Recettes` (`nom_recette`,`type`,`descriptif`,`prix_vente`,`nom_c`) VALUES ('tarte_aux_pommes','déssert',"Tarte aus pommes traditionnelle, comme à la maison !",'6.00','MAMA Ulrich');
INSERT INTO `cooking`.`Recettes` (`nom_recette`,`type`,`descriptif`,`prix_vente`,`nom_c`) VALUES ('salade_de_fruits','déssert',"Salade de fruit de saison coupés minutes",'5.00','MAMA Ulrich');
 
INSERT INTO `cooking`.`Fournisseur` (`nom_f`,`num_f`) VALUES ('BioFrais','0123098876');
INSERT INTO `cooking`.`Fournisseur` (`nom_f`,`num_f`) VALUES ('Picard','0123367676');
INSERT INTO `cooking`.`Fournisseur` (`nom_f`,`num_f`) VALUES ('Ciqual','0178665498');
INSERT INTO `cooking`.`Fournisseur` (`nom_f`,`num_f`) VALUES ('BioAgri','0134009867');
INSERT INTO `cooking`.`Fournisseur` (`nom_f`,`num_f`) VALUES ('ElPescador','0166789987');
INSERT INTO `cooking`.`Fournisseur` (`nom_f`,`num_f`) VALUES ('LeBonBoucher','0177342576');
INSERT INTO `cooking`.`Fournisseur` (`nom_f`,`num_f`) VALUES ('Amora','0122567789');
INSERT INTO `cooking`.`Fournisseur` (`nom_f`,`num_f`) VALUES ('Patisseo','0112456003');

INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('carottes','legume','1 piece',42,15,80,'Ciqual');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('brocolis','legume','100g',20,10,80,'Picard');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('chou_fleur','legume','100g',15,10,80,'Picard');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('champignons','legume','100g',42,15,80,'BioFrais');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('tomates','legume','100g',45,20,90,'BioFrais');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('epinards','legume','100g',34,20,90,'BioFrais');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('haricots_verts','legume','100g',39,20,90,'BioFrais');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('pommes','fruit','100g',33,20,50,'BioFrais');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('poires','fruit','100g',5,20,50,'BioFrais');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('fraises','fruit','100g',64,40,80,'BioFrais');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('framboises','fruit','100g',39,20,90,'BioFrais');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('mozzarella','fromage','100g',20,50,100,'BioFrais');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('riz_basmati','feculent','100g',25,50,100,'BioAgri');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('riz_complet','feculent','100g',11,50,100,'BioAgri');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('riz_brun','feculent','100g',33,30,100,'BioAgri');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('pomme_de_terre','feculent','100g',62,60,120,'BioAgri');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('tagliattelles','feculent','100g',67,50,110,'BioAgri');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('spaghetti','feculent','100g',74,50,100,'BioAgri');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('coquillettes','feculent','100g',22,50,100,'BioAgri');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('pain','feculent','2 tranches',84,100,1000,'BioAgri');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('colin','poisson','100g',60,30,80,'ElPescador');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('saumon','poisson','100g',83,50,100,'ElPescador');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('crevettes','poisson','100g',78,50,100,'ElPescador');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('St_Jacques','poisson','100g',33,20,60,'ElPescador');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('boeuf','viande','100g',12,50,150,'LeBonBoucher');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('dinde','viande','100g',60,50,150,'LeBonBoucher');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('poulet','viande','100g',77,50,150,'LeBonBoucher');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('porc','viande','100g',125,50,150,'LeBonBoucher');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('mayonnaise','sauce','10g',120,100,500,'Amora');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('ketchup','sauce','10g',130,100,500,'Amora');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('pesto','sauce','10g',149,100,500,'Amora');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('sauce_poivre','sauce','10g',123,100,500,'Amora');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('huile','sauce','10g',132,100,500,'Amora');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('cacao_en_poudre','dessert','50g',1,4,10,'Patisseo');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('farine','dessert','100g',66,50,100,'Patisseo');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('sucre_en_poudre','dessert','10g',132,100,500,'Patisseo');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('sucre_glace','dessert','10g',120,50,150,'Patisseo');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('levure','dessert','10g',176,100,600,'Patisseo');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('pepite_de_chocolat','dessert','10g',156,100,1000,'Patisseo');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('caramel','dessert','10g',24,20,100,'Patisseo');
INSERT INTO `cooking`.`Produits` (`nom_produit`,`categorie`,`unite_qte`,`stock_actuel`,`stock_min`,`stock_max`,`nom_f`) VALUES ('oeuf','dessert','1 piece',198,100,600,'Patisseo');

INSERT INTO `cooking`.`Commande` (`num_commande`,`contenu`,`montant_c`,`date_c`) VALUES ('007','tomates-mozza','12','2020-01-04');
INSERT INTO `cooking`.`Commande` (`num_commande`,`contenu`,`montant_c`,`date_c`) VALUES ('008','carpaccio','52','2020-01-06');
INSERT INTO `cooking`.`Commande` (`num_commande`,`contenu`,`montant_c`,`date_c`) VALUES ('009','buddha_bowl','420','2020-01-06');
INSERT INTO `cooking`.`Commande` (`num_commande`,`contenu`,`montant_c`,`date_c`) VALUES ('010','poulet_braisé','32','2020-03-06');
INSERT INTO `cooking`.`Commande` (`num_commande`,`contenu`,`montant_c`,`date_c`) VALUES ('011','bruschetta','44','2020-03-06');
INSERT INTO `cooking`.`Commande` (`num_commande`,`contenu`,`montant_c`,`date_c`) VALUES ('012','tarte_aux_pommes','180','2020-03-06');
INSERT INTO `cooking`.`Commande` (`num_commande`,`contenu`,`montant_c`,`date_c`) VALUES ('013','tarte_aux_pommes','240','2020-05-04');
INSERT INTO `cooking`.`Commande` (`num_commande`,`contenu`,`montant_c`,`date_c`) VALUES ('014','pâtes_au_pesto','756','2020-05-06');
INSERT INTO `cooking`.`Commande` (`num_commande`,`contenu`,`montant_c`,`date_c`) VALUES ('015','carpaccio','26','2020-05-06');
INSERT INTO `cooking`.`Commande` (`num_commande`,`contenu`,`montant_c`,`date_c`) VALUES ('016','moelleux_au_chocolat','360','2020-05-07');
INSERT INTO `cooking`.`Commande` (`num_commande`,`contenu`,`montant_c`,`date_c`) VALUES ('017','tomates-mozza','36','2020-05-08');
INSERT INTO `cooking`.`Commande` (`num_commande`,`contenu`,`montant_c`,`date_c`) VALUES ('018','tomates-mozza','60','2020-05-06');

INSERT INTO `cooking`.`Passent` (`nom_client`,`num_commande`) VALUES ('LAPEL Carla','007');
INSERT INTO `cooking`.`Passent` (`nom_client`,`num_commande`) VALUES ('LAPEL Carla','008');
INSERT INTO `cooking`.`Passent` (`nom_client`,`num_commande`) VALUES ('MAHDOUB Salma','009');
INSERT INTO `cooking`.`Passent` (`nom_client`,`num_commande`) VALUES ('MAMA Ulrich','010');
INSERT INTO `cooking`.`Passent` (`nom_client`,`num_commande`) VALUES ('DURAND Marie','011');
INSERT INTO `cooking`.`Passent` (`nom_client`,`num_commande`) VALUES ('DURAND Marie','012');
INSERT INTO `cooking`.`Passent` (`nom_client`,`num_commande`) VALUES ('DONZEAU Antoine','013');
INSERT INTO `cooking`.`Passent` (`nom_client`,`num_commande`) VALUES ('LAPEL Carla','014');
INSERT INTO `cooking`.`Passent` (`nom_client`,`num_commande`) VALUES ('MAHDOUB Salma','015');
INSERT INTO `cooking`.`Passent` (`nom_client`,`num_commande`) VALUES ('MAMA Ulrich','016');
INSERT INTO `cooking`.`Passent` (`nom_client`,`num_commande`) VALUES ('MAHDOUB Salma','017');
INSERT INTO `cooking`.`Passent` (`nom_client`,`num_commande`) VALUES ('DURAND Marie','018');

INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('tomates','tomates-mozza','3');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('mozzarella','tomates-mozza','3');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('boeuf','carpaccio','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('champignons','carpaccio','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('pain','bruschetta','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('tomates','bruschetta','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('huile','bruschetta','2');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('spaghetti','pâtes_au_pesto','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('pesto','pâtes_au_pesto','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('tagliattelles','pâtes_au_saumon','2');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('saumon','pâtes_au_saumon','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('poulet','poulet_braisé','2');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('pomme_de_terre','poulet_braisé','4');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('mayonnaise','poulet_braisé','3');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('chou_fleur','buddha_bowl','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('carottes','buddha_bowl','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('riz_brun','buddha_bowl','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('sauce_poivre','buddha_bowl','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('brocolis','buddha_bowl','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('saumon','buddha_bowl','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('cacao_en_poudre','moelleux_au_chocolat','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('farine','moelleux_au_chocolat','2');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('oeuf','moelleux_au_chocolat','3');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('levure','moelleux_au_chocolat','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('farine','tarte_aux_pommes','3');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('oeuf','tarte_aux_pommes','3');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('levure','tarte_aux_pommes','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('pommes','tarte_aux_pommes','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('pommes','salade_de_fruits','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('poires','salade_de_fruits','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('fraises','salade_de_fruits','1');
INSERT INTO `cooking`.`Composent` (`nom_produit`,`nom_recette`,`quantité`) VALUES ('framboises','salade_de_fruits','1');

INSERT INTO `cooking`.`Constituent` (`num_commande`,`nom_recette`,`nb_commandes`) VALUES ('007','tomates-mozza','2');
INSERT INTO `cooking`.`Constituent` (`num_commande`,`nom_recette`,`nb_commandes`) VALUES ('008','carpaccio','4');
INSERT INTO `cooking`.`Constituent` (`num_commande`,`nom_recette`,`nb_commandes`) VALUES ('009','buddha_bowl','30');
INSERT INTO `cooking`.`Constituent` (`num_commande`,`nom_recette`,`nb_commandes`) VALUES ('010','poulet_braisé','2');
INSERT INTO `cooking`.`Constituent` (`num_commande`,`nom_recette`,`nb_commandes`) VALUES ('011','bruschetta','11');
INSERT INTO `cooking`.`Constituent` (`num_commande`,`nom_recette`,`nb_commandes`) VALUES ('012','tarte_aux_pommes','30');
INSERT INTO `cooking`.`Constituent` (`num_commande`,`nom_recette`,`nb_commandes`) VALUES ('013','tarte_aux_pommes','40');
INSERT INTO `cooking`.`Constituent` (`num_commande`,`nom_recette`,`nb_commandes`) VALUES ('014','pâtes_au_pesto','54');
INSERT INTO `cooking`.`Constituent` (`num_commande`,`nom_recette`,`nb_commandes`) VALUES ('015','carpaccio','2');
INSERT INTO `cooking`.`Constituent` (`num_commande`,`nom_recette`,`nb_commandes`) VALUES ('016','moelleux_au_chocolat','60');
INSERT INTO `cooking`.`Constituent` (`num_commande`,`nom_recette`,`nb_commandes`) VALUES ('017','tomates-mozza','6');
INSERT INTO `cooking`.`Constituent` (`num_commande`,`nom_recette`,`nb_commandes`) VALUES ('018','tomates-mozza','10');