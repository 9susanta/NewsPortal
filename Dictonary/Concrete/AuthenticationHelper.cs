using NewsPortal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace NewsPortal.Concrete
{
    public class AuthenticationHelper
    {
        internal static List<Claim> CreateClaim(UserSessionModel userSessionModel,string UserName,int UserId,params string[] roles)
        {
                var claims = new List<Claim>
                {
                 new Claim(ClaimTypes.NameIdentifier, UserName),
                 new Claim(ClaimTypes.Name, userSessionModel.DisplayName),
                 new Claim(ClaimTypes.PrimarySid, UserId.ToString()),
                 new Claim(Constants.UserSession, userSessionModel.ToJson())
                };
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String, Constants.Issuer));
                }
                return claims;
        }
    }
}