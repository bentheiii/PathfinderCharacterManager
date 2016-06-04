using System;
using PathfinderCharacterManager;
using SubscriberFramework;

namespace Common
{
    public static class AbilityModifierKinds
    {
        public static readonly TraitKind
            Base = new TraitKind(),
            Progression = new TraitKind(),
            Equipment = new TraitKind();
    }
    public class AbilityScore : Trait
    {
        private AbilityScore(Ability ability)
        {
            this.ability = ability;
        }
        public Ability ability { get; }
        public static readonly AbilityScore
            Strength = new AbilityScore(Ability.Strength),
            Dexterity = new AbilityScore(Ability.Dexterity),
            Constitution = new AbilityScore(Ability.Constitution),
            Intelligence = new AbilityScore(Ability.Intelligence),
            Wisdom = new AbilityScore(Ability.Wisdom),
            Charisma = new AbilityScore(Ability.Charisma);
    }
    public class AbilityMod : Trait {
        private AbilityMod(AbilityScore ability)
        {
            this.ability = ability;
        }
        public AbilityScore ability { get; }
        public static readonly AbilityMod
            Strength = new AbilityMod(AbilityScore.Strength),
            Dexterity = new AbilityMod(AbilityScore.Dexterity),
            Constitution = new AbilityMod(AbilityScore.Constitution),
            Intelligence = new AbilityMod(AbilityScore.Intelligence),
            Wisdom = new AbilityMod(AbilityScore.Wisdom),
            Charisma = new AbilityMod(AbilityScore.Charisma);
    }
    public class AbilityModSubscriber : AdaptedSubscriber<Character, DecisionEvent, TraitModQuery>
    {
        public AbilityModSubscriber() : base(DecisionEventTypes.Query) { }
        public override bool isValid(Character sender, TraitModQuery e)
        {
            return e.Trait is AbilityMod;
        }
        public override object Activate(Character sender, TraitModQuery e)
        {
            var score = sender.GetTrait<int?>((e.Trait as AbilityMod).ability).value;
            if (score.HasValue)
                return score.Value / 2 - 5;
            throw new Exception("requesting mod of null ability");
        }
    }
}
