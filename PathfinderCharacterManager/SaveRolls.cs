using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PathfinderCharacterManager
{
    public class SaveType
    {
        public static readonly SaveType
            Reflex = new SaveType("Reflex", Ability.Dexterity),
            Will = new SaveType("Will",Ability.Wisdom),
            Fortitude = new SaveType("Fortitude",Ability.Constitution);
        private SaveType(string name, Ability ability)
        {
            this.Name = name;
            Ability = ability;
        }
        public string Name { get; }
        public Ability Ability { get; }
    }
}
