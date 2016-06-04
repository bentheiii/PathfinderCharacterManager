using SubscriberFramework;

namespace PathfinderCharacterManager
{
    public static class DecisionEventTypes
    {
        public static readonly EventCatagory
            Query = new EventCatagory(),
            Request = new EventCatagory(),
            Notification = new EventCatagory();
    }
    /*
    Conventions:
    Queries are not allowed to self remove or change state (Queries can be "hypothetical questions" as far as the subscriber is concerned)
    Notification should return null and never change the event
    Queries and Notifications cannot call the decision maker
    */
    public abstract class DecisionEvent : Event
    {
        protected DecisionEvent(DecisionMaker decisionMaker)
        {
            DecisionMaker = decisionMaker;
        }
        public DecisionMaker DecisionMaker { get; }
    }
    public class NotificationEvent : DecisionEvent
    {
        public NotificationEvent(DecisionMaker decisionMaker) : base(decisionMaker) { }
        public override EventCatagory catagory => DecisionEventTypes.Notification;
    }
    public class RequesEvent : DecisionEvent
    {
        public RequesEvent(DecisionMaker decisionMaker) : base(decisionMaker) { }
        public override EventCatagory catagory => DecisionEventTypes.Request;
    }
    public class QueryEvent : DecisionEvent
    {
        public QueryEvent() : base(null) { }
        public override EventCatagory catagory => DecisionEventTypes.Query;
    }
    public class RoundPassedEvent : NotificationEvent
    {
        public RoundPassedEvent(DecisionMaker decisionMaker) : base(decisionMaker) { }
    }
    public class LevelUpEvent : NotificationEvent
    {
        public LevelUpEvent(Class chosenClass, DecisionMaker maker) : base(maker)
        {
            ChosenClass = chosenClass;
        }
        private Class ChosenClass { get; }
    }
    public class EligableClassRequestEvent : QueryEvent
    {
        public EligableClassRequestEvent(Character c)
        {
            this.c = c;
        }
        public Character c { get; }
    }
    public class IsClassSkillRequestEvent : QueryEvent
    {
        public IsClassSkillRequestEvent(SkillType skill)
        {
            this.skill = skill;
        }
        public SkillType skill { get; }
    }
    public class SkillBonusRequestEvent : QueryEvent
    {
        public SkillBonusRequestEvent(SkillType skill, SkillApplication application)
        {
            this.skill = skill;
            Application = application;
        }
        public SkillType skill { get; }
        public SkillApplication Application { get; }
    }
    public class DamageToDeal : QueryEvent
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
