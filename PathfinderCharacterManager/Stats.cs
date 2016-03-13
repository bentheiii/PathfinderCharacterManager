using System;
using System.Collections.Generic;
using System.Linq;
using Edge.Arrays;
using Edge.Comparison;
using Edge.Fielding;
using Edge.Looping;

namespace PathfinderCharacterManager
{
    public interface IStat<out T>
    {
        T value { get; }
    }
    public class Stat<T> : IStat<T>
    {
        public virtual T value { get; }
        public Stat(T realbase)
        {
            value = realbase;
        }
    }
    //odds stack, evens don't
    public enum ModifierType
    {
        Ability = 0,
        Alchemical = 2,
        Armor = 4,
        Circumstance = 1,
        Competence = 6,
        Deflection = 8,
        Dodge = 3,
        Enhancment = 10,
        Insight = 12,
        Luck = 14,
        Morale = 16,
        Natural = 18,
        Profane = 20,
        Racial = 22,
        Resistance = 24,
        Sacred = 26,
        Shield = 28,
        Size = 30
    }
    public enum ModifierKind
    {
        Buff = 0,
        Penalty = 1
    }
    public class StatModifier<T>
    {
        public StatModifier(T value, ModifierType type, object origin = null)
        {
            this.value = value;
            this.type = type;
            this.origin = origin ?? "";
        }
        public virtual T value { get; }
        public ModifierType type { get; }
        public object origin { get; }
        public virtual ModifierKind Kind()
        {
            return value.ToFieldWrapper().IsNegative ? ModifierKind.Penalty : ModifierKind.Buff;
        }
        public override int GetHashCode()
        {
            return origin.GetHashCode();
        }
    }
    public class AbilityStatModifier : StatModifier<int>
    {
        public double Factor { get; }
        public AbilityStatModifier(int value, ModifierType type, double factor, object origin = null) : base(value, type, origin)
        {
            Factor = factor;
        }
        public override int value
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
    public class ConfigurableStat<T> : Stat<T>
    {
        public ConfigurableStat() : this(Fields.getField<T>().zero) { }
        public ConfigurableStat(T realbase) : base(realbase) { }
        private readonly LinkedList<StatModifier<T>> _bases = new LinkedList<StatModifier<T>>();
        public override T value
        {
            get
            {
                return _bases.Select(a=>a.value).FirstOrDefault(base.value);
            }
        }
        public void AddBase(StatModifier<T> newbase)
        {
            _bases.AddFirst(newbase);
        }
        public void RemoveBase(StatModifier<T> toremove)
        {
            _bases.Remove(toremove);
        }
        public void ResetBase()
        {
            _bases.Clear();
        }
    }
    public class ModifiableStat<T> : ConfigurableStat<T>
    {
        public ModifiableStat(T realbase) : base(realbase) {}
        private IDictionary<ModifierKind, IDictionary<ModifierType, ISet<StatModifier<T>>>> mods = new Dictionary<ModifierKind, IDictionary<ModifierType, ISet<StatModifier<T>>>>();
        public void AddMod(StatModifier<T> mod)
        {
            mods.EnsureDefinition(mod.Kind(), new Dictionary<ModifierType, ISet<StatModifier<T>>>());
            mods[mod.Kind()].EnsureDefinition(mod.type, new HashSet<StatModifier<T>>());
            mods[mod.Kind()][mod.type].Add(mod);
        }
        public void RemoveMod(StatModifier<T> mod)
        {
            if (!mods.ContainsKey(mod.Kind()) || !mods[mod.Kind()].ContainsKey(mod.type))
                return;
            mods[mod.Kind()][mod.type].Remove(mod);
        }
        private T modSum()
        {
            return EffectiveMods().Select(a => a.value).getSum();
        }
        public IEnumerable<StatModifier<T>> EffectiveMods()
        {
            return mods.Values.Select(
                a =>
                    a.Select(
                        x =>
                            (int)x.Key % 2 == 0
                                ? x.Value.getMax(new FunctionComparer<StatModifier<T>, T>(m => m.value.ToFieldWrapper().abs().val,
                                    Fields.getField<T>())).Enumerate() : x.Value)
                     .Concat())
                       .Concat();
        } 
        public override T value
        {
            get
            {
                return base.value.ToFieldWrapper()+modSum();
            }
        }
        public void ResetMods()
        {
            mods.Values.Do(a=>a.Values.Do(x=>x.Clear()));
        }
    }
}
