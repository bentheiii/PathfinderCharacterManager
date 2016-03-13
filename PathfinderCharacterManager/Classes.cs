using System.Collections.Generic;
using Edge.Dice;

namespace PathfinderCharacterManager
{
    public interface IBaseAttackBonusProgression
    {
        int AtLevel(int level);
    }
    public interface ISaveBonusProgression
    {
        int AtLevel(int level);
    }
    public interface ISkillRankProgression
    {
        int PerLevel(int level);
    }
    public interface IClassSkillList
    {
        bool IsClassSkill(int level, SkillType skill);
    }
    public class Class
    {
        public Class(string name, string abbreviation, Die<int> hitDie, IBaseAttackBonusProgression baB, IDictionary<SaveType, ISaveBonusProgression> saveProgression, ISkillRankProgression skillRankProgression, IClassSkillList classSkillList)
        {
            this.name = name;
            HitDie = hitDie;
            BaB = baB;
            SaveProgression = saveProgression;
            SkillRankProgression = skillRankProgression;
            ClassSkillList = classSkillList;
            this.abbreviation = abbreviation;
        }
        public string name { get; }
        public string abbreviation { get; }
        public Die<int> HitDie { get; }
        public IBaseAttackBonusProgression BaB { get; }
        public IDictionary<SaveType, ISaveBonusProgression> SaveProgression { get; }
        public ISkillRankProgression SkillRankProgression { get; }
        //TODO spells
        public IClassSkillList ClassSkillList { get; }
        public virtual void AddLevel(Character c, DecisionMaker decisionMaker) { }
    }
}
