using System.Collections.Generic;
using System.Linq;
using Edge.Arrays;
using Edge.Comparison;
using Edge.Fielding;
using Edge.Guard;
using Edge.Looping;
using SubscriberFramework;

namespace PathfinderCharacterManager
{
    public static class ModifierTypes
    {
        public static readonly ModifierType
            Ability = new NonStackingModifierType(),
            Alchemical = new NonStackingModifierType(),
            Armor = new NonStackingModifierType(),
            Circumstance = new StackingModifierType(),
            Competence = new NonStackingModifierType(),
            Deflection = new NonStackingModifierType(),
            Dodge = new StackingModifierType(),
            Enhancment = new NonStackingModifierType(),
            Insight = new NonStackingModifierType(),
            Luck = new NonStackingModifierType(),
            Morale = new NonStackingModifierType(),
            Natural = new NonStackingModifierType(),
            Profane = new NonStackingModifierType(),
            Racial = new NonStackingModifierType(),
            Resistance = new NonStackingModifierType(),
            Sacred = new NonStackingModifierType(),
            Shield = new NonStackingModifierType(),
            Size = new NonStackingModifierType(),
            Base = new StackingModifierType();
    }
    public interface ModifierType
    {
        IEnumerable<Modifier<T>> EffectiveMods<T>(IEnumerable<Modifier<T>> mods);
    }
    public class NonStackingModifierType : ModifierType
    {
        public IEnumerable<Modifier<T>> EffectiveMods<T>(IEnumerable<Modifier<T>> mods)
        {
            IGuard<Modifier<T>> max = new Guard<Modifier<T>>(), min = new Guard<Modifier<T>>();
            mods.HookComp(max: max, min: min, comp: new FunctionComparer<Modifier<T>,T>(a=>a.value)).Do();
            var b = max.value;
            if (b != null)
                yield return b;
            var d = min.value;
            if (d != null)
                yield return d;
        }
    }
    public class StackingModifierType : ModifierType
    {
        public IEnumerable<Modifier<T>> EffectiveMods<T>(IEnumerable<Modifier<T>> mods)
        {
            return mods;
        }
    }
    public class Modifier<T>
    {
        public Modifier(T value, ModifierType type, string source)
        {
            this.value = value;
            Type = type;
            this.source = source;
        }
        public T value { get; }
        public ModifierType Type { get; }
        public string source { get; }
    }
    public class Trait {}
    //Convention: Sum of all non-global kinds of a trait is equal to the global kind of a trait
    public class TraitKind
    {
        public static readonly TraitKind
            Global = new TraitKind();
    }
    public class TraitModQuery : QueryEvent
    {
        public TraitModQuery(Trait trait, IEnumerable<TraitKind> kinds = null)
        {
            Trait = trait;
            Kinds = kinds;
        }
        public IEnumerable<TraitKind> Kinds { get; }
        public Trait Trait { get; }
    }
    public class TraitValue<T>
    {
        public TraitValue(Trait trait, IEnumerable<Modifier<T>> mods)
        {
            this.trait = trait;
            this.mods = mods;
            value = mods.Select(a => a.value).getSum();
        }
        public T value { get; }
        public Trait trait { get; }
        public IEnumerable<Modifier<T>> mods { get; }
    }
    public abstract class TraitQuerySubscriber : AdaptedSubscriber<Character, DecisionEvent, TraitModQuery>
    {
        private readonly Trait _trait;
        protected TraitQuerySubscriber(EventCatagory catagoryToSubscribe, Trait trait) : base(catagoryToSubscribe)
        {
            _trait = trait;
        }
        public override bool isValid(Character sender, TraitModQuery e)
        {
            return e.Trait.Equals(_trait);
        }
    }
}
