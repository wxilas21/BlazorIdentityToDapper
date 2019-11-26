CREATE TABLE `gm`.`gmclaims` (
`gmId` INT NOT NULL,
`claimType` VARCHAR(256) NOT NULL,
`claimValue` VARCHAR(256) NOT NULL,
  PRIMARY KEY (`gmId`, `ClaimType`, `ClaimValue`))
COMMENT = 'Gm claims';