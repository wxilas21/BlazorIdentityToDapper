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