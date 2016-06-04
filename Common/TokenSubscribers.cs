using System;
using System.Collections.Generic;
using PathfinderCharacterManager;
using SubscriberFramework;

namespace Common
{
    public static class TokenSubscribers
    {
        public class TokenStatSubscriber<T> : AdaptedSubscriber<Character, DecisionEvent, TokenQuery>
        {
            private readonly Token _t;
            public Func<Character, TokenQuery, T> Value;
            private readonly string _source;
            public TokenStatSubscriber(EventCatagory catagoryToSubscribe, Token t, Func<Character, TokenQuery, T> value, string source) : base(catagoryToSubscribe)
            {
                _t = t;
                this.Value = value;
                _source = source;
            }
            public override bool isValid(Character sender, TokenQuery e)
            {
                return e.token == _t;
            }
            public override object Activate(Character sender, TokenQuery e)
            {
                return new TokenModifer<T>(Value(sender,e),_source);
            }
        }
        public class TokenEditSubscriber<T> : AdaptedSubscriber<Character, DecisionEvent, TokenEditEvent<T>>
        {
            private readonly TokenStatSubscriber<T> _guest;
            private readonly object _editToken;
            public TokenEditSubscriber(EventCatagory catagoryToSubscribe, TokenStatSubscriber<T> guest, object editToken) : base(catagoryToSubscribe)
            {
                _guest = guest;
                _editToken = editToken;
            }
            public override bool isValid(Character sender, TokenEditEvent<T> e)
            {
                return e.editToken == this._editToken;
            }
            public override object Activate(Character sender, TokenEditEvent<T> e)
            {
                _guest.Value = e.NewVal;
                return null;
            }
        }
    }
    public class TokenEditEvent<T> : NotificationEvent
    {
        public object editToken { get; }
        public readonly Func<Character, TokenQuery, T> NewVal;
        public TokenEditEvent(DecisionMaker decisionMaker, Func<Character, TokenQuery, T> newVal, object editToken) : base(decisionMaker)
        {
            this.editToken = editToken;
            this.NewVal = newVal;
        }
        public TokenEditEvent(DecisionMaker decisionMaker, T newVal, object editToken) : this(decisionMaker, (character, query) => newVal, editToken) { }
    }
    public class TokenMaster<T, E> : IAssignable where E : NotificationEvent
    {
        public readonly Func<E, Character, bool> ManagerValidator;
        private readonly object _editToken;
        public readonly string Source;
        public readonly Func<Character, TokenQuery, T> Value;
        private readonly Token _trait;
        public TokenMaster(T value, Token trait, string source, Func<E, Character, bool> managerValidator = null, object editToken = null) : this((character, query) => value, trait, source, managerValidator, editToken) { }
        public TokenMaster(Func<Character, TokenQuery, T> value, Token trait, string source, Func<E, Character, bool> managerValidator = null, object editToken = null)
        {
            this.ManagerValidator = managerValidator;
            _editToken = editToken;
            this.Source = source;
            this.Value = value;
            this._trait = trait;
        }
        public IEnumerable<IEventSubscriber<DecisionEvent>> OnAssign(Character c, DecisionMaker m)
        {
            var statSubscriber = new TokenSubscribers.TokenStatSubscriber<T>(DecisionEventTypes.Query, _trait, Value, Source);
            yield return statSubscriber;
            var managed = new List<EventSubscriber<Character, DecisionEvent>> { statSubscriber };
            if (_editToken != null)
            {
                var editSubscriber = new TokenSubscribers.TokenEditSubscriber<T>(DecisionEventTypes.Notification, statSubscriber, _editToken);
                yield return editSubscriber;
                managed.Add(editSubscriber);
            }
            yield return new SubscriberManager<E>(DecisionEventTypes.Notification, managed, ManagerValidator);
        }
    }
}
