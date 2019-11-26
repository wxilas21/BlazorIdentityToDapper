using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorApp1.Models
{
    /*
CREATE TABLE `gm`.`gmclaims` (
`gmId` INT NOT NULL,
`claimType` VARCHAR(256) NOT NULL,
`claimValue` VARCHAR(256) NOT NULL,
  PRIMARY KEY (`gmId`, `ClaimType`, `ClaimValue`))
COMMENT = 'Gm claims';
 */

    public class ApplicationUserClaim
    {
        public int gmId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }
    }
}
