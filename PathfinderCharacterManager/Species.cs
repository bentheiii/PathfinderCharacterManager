namespace PathfinderCharacterManager
{
    public interface IAssignable
    {
        void OnAssign(Character c, DecisionMaker m);
    }
    public class SizeCatagory : IAssignable
    {
        public void OnAssign(Character c, DecisionMaker m) { }
    }
    public class Species : IAssignable
    {
        public virtual CreatureType Type { get; }
        public virtual SizeCatagory NativeSize { get; }
        public virtual void OnAssign(Character c, DecisionMaker m)
        {
            Type.OnAssign(c,m);
            NativeSize.OnAssign(c,m);
        }
    }
    public class CreatureType : IAssignable
    {
        public virtual void OnAssign(Character c, DecisionMaker m) { }
    }
}
