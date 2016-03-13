using System;

namespace PathfinderCharacterManager
{
    public static class SkillTypes
    {
        // ReSharper disable InconsistentNaming
        public static readonly SkillType
            Acrobatics = new SkillType("acrobatics", Ability.Dexterity),
            Appraise = new SkillType("Appraise", Ability.Intelligence),
            Art_Act = new SkillType("Artistry-Act", Ability.Intelligence),
            Art_Comedy = new SkillType("Artistry-Comedy", Ability.Intelligence),
            Art_Dance = new SkillType("Artistry-Dance",Ability.Intelligence),
            Art_Keyboard = new SkillType("Artistry-Keyboard",Ability.Intelligence),
            Art_Oratory = new SkillType("Artistry-Oratory", Ability.Intelligence),
            Art_Percussion = new SkillType("Artistry-Percussion", Ability.Intelligence),
            Art_String = new SkillType("Artistry-String",Ability.Intelligence),
            Art_WindInstrument = new SkillType("Artistry-Wind Instrument",Ability.Intelligence),
            Art_Sing = new SkillType("Artistry-Sing",Ability.Intelligence),
            Autohypnosis = new SkillType("Auto-hypnosis",Ability.Wisdom),
            Bluff = new SkillType("Bluff",Ability.Charisma),
            Climb = new SkillType("Climb",Ability.Strength),
            Craft_Alchemy = new SkillType("Craft-Alchemy",Ability.Intelligence),
            Craft_Armor = new SkillType("Craft-Armor", Ability.Intelligence),
            Craft_Bows = new SkillType("Craft-Bows", Ability.Intelligence),
            Craft_Traps = new SkillType("Craft-Traps", Ability.Intelligence),
            Craft_Weapons = new SkillType("Craft-Weapons",Ability.Intelligence),
            Diplomacy = new SkillType("Diplomacy", Ability.Charisma),
            DisableDevice = new SkillType("Disable Device",Ability.Dexterity),
            Disguise = new SkillType("Disguise",Ability.Charisma),
            EscapeArtist = new SkillType("Escape Artist", Ability.Charisma),
            Fly = new SkillType("Fly", Ability.Dexterity),
            HandleAnimal = new SkillType("Handle Animal", Ability.Charisma),
            Heal = new SkillType("Heal", Ability.Wisdom),
            Intimidate = new SkillType("Intimidate",Ability.Charisma),
            Knowledge_Arcana = new SkillType("Knowledge_Arcana", Ability.Intelligence),
            Knowledge_Dungeoneering = new SkillType("Knowledge_Doungeoneering", Ability.Intelligence),
            Knowledge_Engineering = new SkillType("Knowledge Engineering", Ability.Intelligence),
            Knowledge_Geography = new SkillType("Knowledge Geography",Ability.Intelligence),
            Knowledge_History = new SkillType("Knowledge History",Ability.Intelligence),
            Knowledge_Local = new SkillType("Knowledge Local",Ability.Intelligence),
            Knowledge_Nature = new SkillType("Knowledge Nature",Ability.Intelligence),
            Knowledge_Nobility = new SkillType("Knowledge Nobility",Ability.Intelligence),
            Knowledge_Planes = new SkillType("Knowledge Planes",Ability.Intelligence),
            Knowledge_Religion = new SkillType("Knowledge Religion",Ability.Intelligence),
            Linguistics = new SkillType("Linguistics",Ability.Intelligence),
            Perception = new SkillType("Perception",Ability.Wisdom),
            Perform_Act = new SkillType("Perform-Act", Ability.Charisma),
            Perform_Comedy = new SkillType("Perform-Comedy", Ability.Charisma),
            Perform_Dance = new SkillType("Perform-Dance",Ability.Charisma),
            Perform_Keyboard = new SkillType("Perform-Keyboard",Ability.Charisma),
            Perform_Oratory = new SkillType("Perform-Oratory", Ability.Charisma),
            Perform_Percussion = new SkillType("Perform-Percussion", Ability.Charisma),
            Perform_String = new SkillType("Perform-String",Ability.Charisma),
            Perform_WindInstrument = new SkillType("Perform-Wind Instrument",Ability.Charisma),
            Perform_Sing = new SkillType("Perform-Sing",Ability.Charisma),
            Ride = new SkillType("Ride",Ability.Dexterity),
            SenseMotive = new SkillType("Sense Motive",Ability.Wisdom),
            SleightOfHand = new SkillType("Sleight of Hand", Ability.Dexterity),
            Spellcraft = new SkillType("Spellcraft",Ability.Intelligence),
            Stealth = new SkillType("Stealth",Ability.Dexterity),
            Survival = new SkillType("Survival",Ability.Wisdom),
            Swim = new SkillType("Swim",Ability.Strength),
            UseMagicDevice = new SkillType("Use Magic Device", Ability.Charisma);
        // ReSharper restore InconsistentNaming
    }
    public class SkillType
    {
        public SkillType(string name, Ability ability)
        {
            this.name = name;
            this.ability = ability;
        }
        public string name { get; }
        public Ability ability { get; }
        public bool armorPenalty
        {
            get
            {
                return ability == Ability.Strength || ability == Ability.Dexterity;
            }
        }
        public virtual int Bonus()
        {
            throw new NotImplementedException();
        }
    }
    public class RankedSkill
    {
        public RankedSkill(SkillType type, Character owner)
        {
            this.type = type;
            this.owner = owner;
            Ranks = 0;
        }
        public SkillType type { get; }
        public Character owner { get; }
        public int Ranks { get; set; }
        public ConfigurableStat<int> Modifiers { get; } = new ConfigurableStat<int>(0);
        public bool IsClassSkill(Character c)
        {
            throw new NotImplementedException();
        }
        public int Bonus(Character c)
        {
            throw new NotImplementedException();
        }
    }
}
