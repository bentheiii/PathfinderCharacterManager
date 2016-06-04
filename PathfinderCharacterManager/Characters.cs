using System;
using System.Collections.Generic;
using System.Linq;
using Edge.Arrays;
using SubscriberFramework;

namespace PathfinderCharacterManager
{
    public enum Gender { Male, Female, Undefined}
    public enum DamageKind { Lethal, NonLethal}
    public enum EffectType
    { };
    public enum ResistanceType
    { };
    public class Character : Notifier<DecisionEvent>
    {
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
            var cl = m.Choose(new Decision<Class>("Which class to level up", "Choose a class to level up in",DecisionInterfaceType.List,
                    getEligableClasses(m).SelectToArray(a => new Choice<Class>(a.name, $"level up in {a.name}", a)))).Value;
            this.Notify(new LevelUpEvent(cl, m));
        }
        public TraitValue<T> GetTrait<T>(Trait trait)
        {
            var effectivemods = this.Notify<Modifier<T>>(new TraitModQuery(trait)).ToLookup(a => a.Type).SelectMany(a => a.Key.EffectiveMods(a));
            return new TraitValue<T>(trait, effectivemods);
        }
        public TokenValue<T> GetTokens<T>(Token token)
        {
            var tok = this.Notify<TokenModifer<T>>(new TokenQuery(token));
            return new TokenValue<T>(tok);
        }
        public EffectOutcome CastEffect(Effect effect, DecisionMaker maker)
        {
            var qpos = this.Notify<EffectPossibilityReply>(new EffectPossibilityQuery(effect));
            var qmods = this.Notify<EffectModifier>(new EffectModificationQuery(effect));
            EffectOutcome outcome;
            if (!effect.Proceed(qmods, qpos, out outcome))
                return outcome;
            var rpos = this.Notify<EffectPossibilityReply>(new EffectPossibilityRequest(effect, maker));
            var rmods = this.Notify<EffectModifier>(new EffectPossibilityRequest(effect, maker));
            var mods = qmods.Concat(rmods).ToArray();
            var pos = qpos.Concat(rpos).ToArray();
            if (!effect.Proceed(mods, pos, out outcome))
                return outcome;
            outcome = effect.Affect(this, mods, pos);
            var notification = new EffectAppliedNotification(maker,effect,this,outcome);
            this.Notify(notification);
            return outcome;
        }
        //TODO equipment
        //TODO feats
        //TODO special abilities
        //TODO attacks
        //TODO spells
        //TODO choice log?
    }
}
