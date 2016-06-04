using System;
using System.Collections.Generic;
using System.Linq;
using PathfinderCharacterManager;
using SubscriberFramework;

namespace Common
{
    public static class ModificationSubscribers
    {
        public class ModificationStatSubscriber<T> : AdaptedSubscriber<Character,DecisionEvent,TraitModQuery>
        {
            private readonly Trait _t;
            public Func<Character, TraitModQuery, T> Value;
            private readonly ModifierType _type;
            private readonly string _source;
            private readonly TraitKind _kind;
            public ModificationStatSubscriber(EventCatagory catagoryToSubscribe, Trait t, Func<Character, TraitModQuery, T> value, ModifierType type, string source, TraitKind kind) : base(catagoryToSubscribe)
            {
                _t = t;
                Value = value;
                _type = type;
                _source = source;
                _kind = kind;
            }
            public override bool isValid(Character sender, TraitModQuery e)
            {
                return e.Trait == _t && (e.Kinds == null || e.Kinds.Contains(_kind));
            }
            public override object Activate(Character sender, TraitModQuery e)
            {
                return new Modifier<T>(Value(sender,e),_type,_source);
            }
        }
        public class ModificationEditSubscriber<T> : AdaptedSubscriber<Character,DecisionEvent,ModificationEditEvent<T>>
        {
            private readonly ModificationStatSubscriber<T> _guest;
            private readonly object _editToken;
            public ModificationEditSubscriber(EventCatagory catagoryToSubscribe, ModificationStatSubscriber<T> guest, object editToken) : base(catagoryToSubscribe)
            {
                _guest = guest;
                _editToken = editToken;
            }
            public override bool isValid(Character sender, ModificationEditEvent<T> e)
            {
                return e.editToken == this._editToken;
            }
            public override object Activate(Character sender, ModificationEditEvent<T> e)
            {
                _guest.Value = e.NewVal;
                return null;
            }
        }
        
    }
    public class ModificationEditEvent<T> : NotificationEvent
    {
        public object editToken { get; }
        public readonly Func<Character, TraitModQuery, T> NewVal;
        public ModificationEditEvent(DecisionMaker decisionMaker, Func<Character, TraitModQuery, T> newVal, object editToken) : base(decisionMaker)
        {
            this.editToken = editToken;
            this.NewVal = newVal;
        }
        public ModificationEditEvent(DecisionMaker decisionMaker, T newVal, object editToken) : this(decisionMaker, (character, query) => newVal, editToken) { }
    }
    public class Modification<T,E> : IAssignable where E : NotificationEvent
    {
        public readonly Func<E, Character, bool> ManagerValidator;
        private readonly object _editToken;
        public readonly string Source;
        public readonly Func<Character,TraitModQuery,T> Value;
        public readonly ModifierType Type;
        private readonly TraitKind _kind;
        private readonly Trait _trait;
        public Modification(T value, ModifierType type, TraitKind kind, Trait trait, string source, Func<E, Character, bool> managerValidator = null, object editToken = null) : this((character, query) => value, type, kind, trait, source, managerValidator, editToken) { }
        public Modification(Func<Character, TraitModQuery, T> value, ModifierType type, TraitKind kind, Trait trait, string source, Func<E, Character, bool> managerValidator = null, object editToken = null)
        {
            this.ManagerValidator = managerValidator;
            _editToken = editToken;
            this.Source = source;
            this.Value = value;
            Type = type;
            _kind = kind;
            this._trait = trait;
        }
        public IEnumerable<IEventSubscriber<DecisionEvent>> OnAssign(Character c, DecisionMaker m)
        {
            var statSubscriber = new ModificationSubscribers.ModificationStatSubscriber<T>(DecisionEventTypes.Query, _trait, Value,Type,Source, _kind);
            yield return statSubscriber;
            var managed = new List<EventSubscriber<Character, DecisionEvent>> {statSubscriber};
            if (_editToken != null)
            {
                var editSubscriber = new ModificationSubscribers.ModificationEditSubscriber<T>(DecisionEventTypes.Notification,statSubscriber,_editToken);
                yield return editSubscriber;
                managed.Add(editSubscriber);
            }
            yield return new SubscriberManager<E>(DecisionEventTypes.Notification,managed,ManagerValidator);
        }
    }
}
