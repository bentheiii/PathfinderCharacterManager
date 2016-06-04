using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathfinderCharacterManager;
using SubscriberFramework;

namespace Common
{
    public class SubscriberManager<E> : AdaptedSubscriber<Character, DecisionEvent, E> where E : NotificationEvent
    {
        private readonly IEnumerable<EventSubscriber<Character, DecisionEvent>> _guest;
        private readonly Func<E, Character, bool> _validator;
        public SubscriberManager(EventCatagory catagoryToSubscribe, IEnumerable<EventSubscriber<Character, DecisionEvent>> guest, Func<E, Character, bool> validator = null) : base(catagoryToSubscribe)
        {
            _guest = guest;
            _validator = validator ?? ((e, character) => true);
        }
        public override bool isValid(Character sender, E e)
        {
            return _validator(e, sender);
        }
        public override object Activate(Character sender, E e)
        {
            foreach (var g in _guest)
            {
                sender.UnSubscribe(g);
            }
            sender.UnSubscribe(this);
            return null;
        }
    }
}
