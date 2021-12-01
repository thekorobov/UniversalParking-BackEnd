using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParking.BLL.DTO;

namespace UniversalParking.BLL.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(int userID, string role);
        bool ValidateCurrentToken(string token);
        string GetClaim(string token, string claimType);
    }
}
