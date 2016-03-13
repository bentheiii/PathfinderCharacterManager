using System;

namespace PathfinderCharacterManager
{
    public interface IEventSubscriber
    {
        object ActivateEvent(Event e);
    }
    [Flags] public enum EventType { None=0 , Progression = 1, Combat = 2, Request = 4, All = ~0}
    public interface Event
    {
        EventType type { get; }
    }
    public class ProgressionEvent : Event
    {
        public EventType type => EventType.Progression;
    }
    public class RoundPassedEvent : Event
    {
        public EventType type => EventType.Combat;
    }
    public class LevelUpEvent : ProgressionEvent
    {
        public LevelUpEvent(Class chosenClass)
        {
            ChosenClass = chosenClass;
        }
        private Class ChosenClass { get; }
    }
    public class EligableClassRequestEvent : Event
    {
        public EligableClassRequestEvent(Character c)
        {
            this.c = c;
        }
        public Character c { get; }
        public EventType type
        {
            get
            {
                return EventType.Request;
            }
        }
    }
}
