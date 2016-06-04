using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfinderCharacterManager
{
    public class Ability
    {
        public static readonly Ability Strength = new Ability("strength", "str"),
                                       Dexterity = new Ability("dexterity", "dex"),
                                       Constitution = new Ability("constitution", "con"),
                                       Intelligence = new Ability("intelligence", "int"),
                                       Wisdom = new Ability("wisdom", "wis"),
                                       Charisma = new Ability("charisma", "cha");
        private Ability(string name, string shorthand)
        {
            this.name = name;
            this.shorthand = shorthand;
        }
        public string name { get; }
        //in lowercase
        public string shorthand { get; }
    }
}
