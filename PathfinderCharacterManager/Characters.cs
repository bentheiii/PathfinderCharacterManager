using System;
using System.Collections.Generic;
using System.Linq;
using Edge.Arrays;
using Edge.Looping;

namespace PathfinderCharacterManager
{
    public enum Gender { Male, Female, Undefined}
    public enum DamageKind { Lethal, NonLethal}
    public enum EffectType
    { };
    public enum ResistanceType
    { };
    [Flags]
    public enum Alignment
    {
        None=0,
        Chaotic = 1, OrderNeutral = 2, Lawful = 3,
        Good = 4, MoralNeutral = 8, Evil = 12
    }
    public class Character
    {
        public string Name { get; }
        public ModifiableStat<Species> Race { get; }
        public ModifiableStat<Gender> Gender { get; }
        public IDictionary<Ability, ModifiableStat<int>> Abilities { get; } = new Dictionary<Ability, ModifiableStat<int>>();
        public IDictionary<SkillType,RankedSkill> Skills { get; } = new Dictionary<SkillType, RankedSkill>();
        public ModifiableStat<int> Hp { get; } = new ModifiableStat<int>(0);
        public IDictionary<DamageKind, int> Damage { get; } = new Dictionary<DamageKind, int>();
        public IDictionary<Class, int> Levels { get; } = new Dictionary<Class, int>();
        public IDictionary<EventType, List<IEventSubscriber>> subscribers { get; } = new Dictionary<EventType, List<IEventSubscriber>>();
        public IDictionary<EffectType, ConfigurableStat<ResistanceType>> Resistances = new Dictionary<EffectType, ConfigurableStat<ResistanceType>>();
        public ConfigurableStat<SizeCatagory> size = new ConfigurableStat<SizeCatagory>(null);
        public virtual void DealDamage(EffectType type, DamageKind kind, int damage)
        {
            Damage.EnsureDefinition(kind);
            Damage[kind] += damage;
        }
        public virtual void LevelUp(DecisionMaker m)
        {
            var cl = m.Choose(new Decision<Class>("Which class to level up", "Choose a class to level up in",
                    this.Notify(new EligableClassRequestEvent(this)).OfType<Class>().Distinct().SelectToArray(a => new Choice<Class>(a.name, $"level up in {a.name}", a)))).Value;
            Levels.EnsureDefinition(cl);
            Levels[cl]++;
            cl.AddLevel(this,m);
            this.Notify(new LevelUpEvent(cl));
        }
        //TODO equipment
        //TODO feats
        //TODO special abilities
        //TODO attacks
        //TODO spells
        //TODO choice log?
    }
    public static class CharacterExtensions
    {
        public static void Subscribe(this Character @this, IEventSubscriber subscriber, EventType typestosub)
        {
            foreach (EventType type in typestosub.EnumFlags())
            {
                @this.subscribers.EnsureDefinition(type, new List<IEventSubscriber>());
                @this.subscribers[type].Add(subscriber);
            }
        }
        public static void UnSubscribe(this Character @this, IEventSubscriber subscriber)
        {
            @this.subscribers.Values.Where(a=>a.Contains(subscriber)).Do(a=>a.Remove(subscriber));
        }
        public static object[] Notify(this Character @this, Event e)
        {
            return e.type.EnumFlags().SelectMany(a => @this.subscribers[a]).Distinct().Select(a=>a.ActivateEvent(e)).ToArray();
        } 
    }
}
