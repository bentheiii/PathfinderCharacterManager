using System;

namespace PathfinderCharacterManager
{
    public interface IEventSubscriber
    {
        object ActivateEvent(Event e, out Event forwardEvent);
        EventType typesToSubscribe { get; }
    }
    /*
    Conventions:
    Requests are not allowed to self remove or change state (Requests can be "hypothetical questions" as far as the subscriber is concerned)
    Notification should return null and never change the event
    Requests and Notifications cannot call the decision maker
    */
    [Flags] public enum EventType { None=0 , Progression = 1, Combat = 2, Request = 4, Notification = 8, All = ~0}
    public abstract class Event
    {
        protected Event(DecisionMaker decisionMaker)
        {
            DecisionMaker = decisionMaker;
        }
        public abstract EventType type { get; }
        public DecisionMaker DecisionMaker { get; }
    }
    public class ProgressionEvent : Event
    {
        public ProgressionEvent(DecisionMaker decisionMaker) : base(decisionMaker) { }
        public override EventType type => EventType.Progression;
    }
    public class RequestEvent : Event
    {
        public RequestEvent() : base(null) { }
        public override EventType type => EventType.Request;
    }
    public class CombatEvent : Event
    {
        public CombatEvent(DecisionMaker decisionMaker) : base(decisionMaker) { }
        public override EventType type => EventType.Combat;
    }
    public class RoundPassedEvent : CombatEvent
    {
        public RoundPassedEvent(DecisionMaker decisionMaker) : base(decisionMaker) { }
    }
    public class LevelUpEvent : ProgressionEvent
    {
        public LevelUpEvent(Class chosenClass, DecisionMaker maker) : base(maker)
        {
            ChosenClass = chosenClass;
        }
        private Class ChosenClass { get; }
    }
    public class EligableClassRequestEvent : RequestEvent
    {
        public EligableClassRequestEvent(Character c)
        {
            this.c = c;
        }
        public Character c { get; }
    }
    public class IsClassSkillRequestEvent : RequestEvent
    {
        public IsClassSkillRequestEvent(SkillType skill)
        {
            this.skill = skill;
        }
        public SkillType skill { get; }
    }
    public class SkillBonusRequestEvent : RequestEvent
    {
        public SkillBonusRequestEvent(SkillType skill, SkillApplication application)
        {
            this.skill = skill;
            Application = application;
        }
        public SkillType skill { get; }
        public SkillApplication Application { get; }
    }
    public class DamageToDeal : RequestEvent
    {
        public DamageToDeal(int damageCount, DamageKind kind, EffectType effectType)
        {
            this.damageCount = damageCount;
            this.kind = kind;
            this.effectType = effectType;
        }
        public DamageKind kind { get; }
        public EffectType effectType { get; set; }
        public int damageCount { get; }
    }
}
