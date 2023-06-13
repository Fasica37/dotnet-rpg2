using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg2.Dtos.Charater
{
    public class AddCharacterSkillDto
    {
        public int CharaterId { get; set; }
        public int SkillId { get; set; }
    }
}