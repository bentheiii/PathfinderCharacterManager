using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfinderCharacterManager
{
    public class EffectOutcome { }
    public class EffectModifier { }
    public class EffectPossibilityReply { }
    public abstract class Effect
    {
        public abstract bool Proceed(EffectModifier[] mods, EffectPossibilityReply[] possibilityReplies, out EffectOutcome ifCancelledOutcome);
        public abstract EffectOutcome Affect(Character target, EffectModifier[] mods, EffectPossibilityReply[] possibilityReplies);
    }
    public class EffectPossibilityQuery : QueryEvent
    {
        public EffectPossibilityQuery(Effect effect)
        {
            this.effect = effect;
        }
        public Effect effect { get; }
    }
    public class EffectModificationQuery : QueryEvent
    {
        public EffectModificationQuery(Effect effect)
        {
            this.effect = effect;
        }
        public Effect effect { get; }
    }
    public class EffectPossibilityRequest : RequesEvent
    {
        public EffectPossibilityRequest(Effect effect, DecisionMaker maker) : base(maker)
        {
            this.effect = effect;
        }
        public Effect effect { get; }
    }
    public class EffectModificationRequest : RequesEvent
    {
        public EffectModificationRequest(Effect effect, DecisionMaker maker) : base(maker)
        {
            this.effect = effect;
        }
        public Effect effect { get; }
    }
    public class EffectAppliedNotification : NotificationEvent
    {
        public EffectAppliedNotification(DecisionMaker decisionMaker, Effect effect, Character target, EffectOutcome outcome) : base(decisionMaker)
        {
            this.effect = effect;
            this.target = target;
            this.outcome = outcome;
        }
        public Effect effect { get; }
        public Character target { get; }
        public EffectOutcome outcome { get; }
    }
}
