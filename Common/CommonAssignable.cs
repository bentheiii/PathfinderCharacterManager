using System.Collections.Generic;
using PathfinderCharacterManager;

namespace Common
{
    public class CommonAssignable : InflatingAssignable
    {
        private readonly IDictionary<AbilityScore, int?> _baseAbilityScores;
        private readonly Alignment _alignment;
        public CommonAssignable(IDictionary<AbilityScore, int?> baseAbilityScores, Alignment alignment = Alignment.Any)
        {
            _baseAbilityScores = baseAbilityScores;
            _alignment = alignment;
        }
        public override IEnumerable<IAssignable> OnInflateAssign(Character c, DecisionMaker m)
        {
            foreach (var baseAbilityScore in _baseAbilityScores)
            {
                yield return
                    new Modification<int?, NotificationEvent>(baseAbilityScore.Value, ModifierTypes.Base, AbilityModifierKinds.Base,
                        baseAbilityScore.Key, "Base Score", (e, ch) => false);
            }
            yield return new AbilityModSubscriber().ToAssignable();
            yield return new TokenMaster<Alignment,NotificationEvent>(_alignment,AlignmentToken.Token,"Base");
        }
    }
}
