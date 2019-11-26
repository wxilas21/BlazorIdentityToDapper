
namespace BlazorApp1.Models
{
    /*
    CREATE TABLE `gm`.`gmrole` (
  `RoleId` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  `NormalizedName` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`RoleId`),
  INDEX `NormalizedName` (`NormalizedName` ASC) VISIBLE);
COMMENT = 'Gm Role';
     */
    public class ApplicationRole
    {
        public int RoleId { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }
    }
}
