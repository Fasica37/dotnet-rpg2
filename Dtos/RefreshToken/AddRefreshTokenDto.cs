using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg2.Dtos.RefreshToken
{
    public class AddRefreshTokenDto
    {
        public required string Token { get; set; }
    }
}