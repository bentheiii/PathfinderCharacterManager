using System.Collections.Generic;
using System.Linq;
using SubscriberFramework;

namespace PathfinderCharacterManager
{
    public interface IAssignable
    {
        IEnumerable<IEventSubscriber<DecisionEvent>> OnAssign(Character c, DecisionMaker m);
    }
    public abstract class InflatingAssignable : IAssignable
    {
        public IEnumerable<IEventSubscriber<DecisionEvent>> OnAssign(Character c, DecisionMaker m)
        {
            return OnInflateAssign(c, m).SelectMany(a => a.OnAssign(c, m));
        }
        public abstract IEnumerable<IAssignable> OnInflateAssign(Character c, DecisionMaker m);
    }
    public static class AssignableExtensions
    {
        public class AssignableSubscriberAdapter : IAssignable
        {
            private readonly IEventSubscriber<DecisionEvent> _ev;
            public AssignableSubscriberAdapter(IEventSubscriber<DecisionEvent> ev)
            {
                _ev = ev;
            }
            public IEnumerable<IEventSubscriber<DecisionEvent>> OnAssign(Character c, DecisionMaker m)
            {
                yield return _ev;
            }
        }
        public static IAssignable ToAssignable(this IEventSubscriber<DecisionEvent> @this)
        {
            return new AssignableSubscriberAdapter(@this);
        }
    }
}
