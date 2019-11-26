namespace BlazorApp1.Models
{
    /*
  CREATE TABLE `gm`.`gmuser` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `UserName` VARCHAR(128) NOT NULL,
  `NormalizedUserName` VARCHAR(128) NOT NULL,
  `Email` VARCHAR(128) NOT NULL,
  `NormalizedEmail` VARCHAR(128) NOT NULL,
  `EmailConfirmed` TINYINT(4) NOT NULL,
  `PasswordHash` VARCHAR(1024) NOT NULL,
  `PhoneNumber` VARCHAR(45) NOT NULL,
  `PhoneNumberConfirmed` TINYINT(4) NULL,
  `TwoFactorEnabled` TINYINT(4) NULL,
  PRIMARY KEY (`Id`),
  INDEX `NormalizedNameIDX` (`NormalizedUserName` ASC),
  INDEX `NormalizedEmailIDX` (`NormalizedEmail` ASC))
COMMENT = 'Gm Account';
     */

    /*
CREATE TABLE `gm`.`gmuserrole` (
`gmId` INT NOT NULL,
`RoleId` INT NOT NULL,
PRIMARY KEY (`gmId`, `RoleId`))
COMMENT = 'Gm User Role';
     */

    public class ApplicationUser
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }
    }
}
