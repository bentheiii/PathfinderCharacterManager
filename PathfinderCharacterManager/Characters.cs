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
        public IDictionary<EventType, List<IEventSubscriber>> subscribers { get; } = new Dictionary<EventType, List<IEventSubscriber>>();
        public virtual void DealDamage(EffectType type, DamageKind kind, int damage, DecisionMaker maker)
        {
            damage = this.Notify<int>(new DamageToDeal(damage, kind, type)).LastOrDefault(damage);
            //TODO deal damage
        }
        public virtual IEnumerable<Class> getEligableClasses(DecisionMaker maker)
        {
            return this.Notify<Class>(new EligableClassRequestEvent(this)).Distinct();
        }
        public virtual void LevelUp(DecisionMaker m)
        {
            var cl = m.Choose(new Decision<Class>("Which class to level up", "Choose a class to level up in",DecisionInterfaceType.StandardList,
                    getEligableClasses(m).SelectToArray(a => new Choice<Class>(a.name, $"level up in {a.name}", a)))).Value;
            this.Notify(new LevelUpEvent(cl, m));
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
        public static void Subscribe(this Character @this, IEventSubscriber subscriber)
        {
            foreach (EventType type in subscriber.typesToSubscribe.EnumFlags())
            {
                @this.subscribers.EnsureDefinition(type, new List<IEventSubscriber>());
                @this.subscribers[type].Add(subscriber);
            }
        }
        public static void UnSubscribe(this Character @this, IEventSubscriber subscriber)
        {
            @this.subscribers.Values.Where(a=>a.Contains(subscriber)).Do(a=>a.Remove(subscriber));
        }
        private static IEnumerable<T> CascadeEvent<T>(this IEnumerable<IEventSubscriber> subs, Event e)
        {
            foreach (var sub in subs)
            {
                if (e == null)
                    yield break;
                var ret = sub.ActivateEvent(e, out e);
                if (ret is T)
                    yield return (T)ret;
            }
        }
        public static T[] Notify<T>(this Character @this, Event e)
        {
            return e.type.EnumFlags().SelectMany(a => @this.subscribers[a]).Distinct().CascadeEvent<T>(e).ToArray();
        }
        public static void Notify(this Character @this, Event e)
        {
            e.type.EnumFlags().SelectMany(a => @this.subscribers[a]).Distinct().CascadeEvent<object>(e).Do();
        }
    }
}
