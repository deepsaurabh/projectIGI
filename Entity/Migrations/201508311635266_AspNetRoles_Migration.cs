namespace Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AspNetRoles_Migration : DbMigration
    {
        public override void Up()
        {

            Sql(@"CREATE TABLE `aspnetusers` (
              `Id` varchar(128) CHARACTER SET utf8 NOT NULL,
              `Email` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
              `EmailConfirmed` tinyint(1) NOT NULL,
              `PasswordHash` longtext,
              `SecurityStamp` longtext,
              `PhoneNumber` longtext,
              `PhoneNumberConfirmed` tinyint(1) NOT NULL,
              `TwoFactorEnabled` tinyint(1) NOT NULL,
              `LockoutEndDateUtc` datetime DEFAULT NULL,
              `LockoutEnabled` tinyint(1) NOT NULL,
              `AccessFailedCount` int(11) NOT NULL,
              `UserName` varchar(256) CHARACTER SET utf8 NOT NULL,
              PRIMARY KEY (`Id`)
            ) ENGINE=InnoDB DEFAULT CHARSET=latin1;");

            Sql(@"CREATE TABLE `aspnetroles` (
              `Id` varchar(128) CHARACTER SET utf8 NOT NULL,
              `Name` varchar(256) CHARACTER SET utf8 NOT NULL,
              PRIMARY KEY (`Id`)
            ) ENGINE=InnoDB DEFAULT CHARSET=latin1;");

            Sql(@"CREATE TABLE `aspnetuserclaims` (
              `Id` int(11) NOT NULL AUTO_INCREMENT,
              `UserId` varchar(128) CHARACTER SET utf8 NOT NULL,
              `ClaimType` longtext,
              `ClaimValue` longtext,
              PRIMARY KEY (`Id`),
              UNIQUE KEY `Id` (`Id`),
              KEY `UserId` (`UserId`),
              CONSTRAINT `ApplicationUser_Claims` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
            ) ENGINE=InnoDB DEFAULT CHARSET=latin1;");

            Sql(@"CREATE TABLE `aspnetuserlogins` (
                  `LoginProvider` varchar(128) CHARACTER SET utf8 NOT NULL,
                  `ProviderKey` varchar(128) CHARACTER SET utf8 NOT NULL,
                  `UserId` varchar(128) CHARACTER SET utf8 NOT NULL,
                  PRIMARY KEY (`LoginProvider`,`ProviderKey`,`UserId`),
                  KEY `ApplicationUser_Logins` (`UserId`),
                  CONSTRAINT `ApplicationUser_Logins` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
                ) ENGINE=InnoDB DEFAULT CHARSET=latin1;");

            Sql(@"CREATE TABLE `aspnetuserroles` (
                  `UserId` varchar(128) CHARACTER SET utf8 NOT NULL,
                  `RoleId` varchar(128) CHARACTER SET utf8 NOT NULL,
                  PRIMARY KEY (`UserId`,`RoleId`),
                  KEY `IdentityRole_Users` (`RoleId`),
                  CONSTRAINT `ApplicationUser_Roles` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION,
                  CONSTRAINT `IdentityRole_Users` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
                ) ENGINE=InnoDB DEFAULT CHARSET=latin1;");            
        }
        
        public override void Down()
        {
        }
    }
}
