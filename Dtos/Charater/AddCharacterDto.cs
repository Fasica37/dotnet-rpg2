using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg2.Dtos.Charater
{
    public class AddCharacterDto//AddCharacterRequestDto
    {
        public string Name { get; set; } = "Abebe";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defence { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;
    }
}