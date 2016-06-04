using System;
using Edge.SystemExtensions;
using PathfinderCharacterManager;
using SubscriberFramework;
using System.Linq;

namespace Common
{
    [Flags] public enum Alignment { Good = 1, NeutralMoral = 0, Evil = 3, Lawful = 4, NeutralOrder = 0, Chaotic = 12, AnyMoral = 2, AnyOrder = 8,
        Any = AnyMoral | AnyOrder
    }
    public class AlignmentToken
    {
        public static readonly Token Token = new Token();
    }
    public static class AlignmentExtentions
    {
        public static Tuple<int, int> toMorOrd(this Alignment @this)
        {
            int t = (int)@this;
            return new Tuple<int, int>(t%4,t/4);
        }
        public static int Distance(this Alignment @this, Alignment other)
        {
            var thisMo = @this.toMorOrd();
            var mo = other.toMorOrd();
            return Distance(thisMo.Item1, mo.Item1) + Distance(thisMo.Item2, mo.Item2);
        }
        private static int Distance(int @this, int other)
        {
            if (@this == 2 || other == 2 || @this==other)
                return 0;
            return (@this - other).abs();
        }
    }
    public class AlignmentDistanceQuery : QueryEvent
    {
        public AlignmentDistanceQuery(Alignment origin)
        {
            Origin = origin;
        }
        public Alignment Origin { get; }
    }
    public class AlignmentDistanceSubscriber : AdaptedSubscriber<Character, DecisionEvent, AlignmentDistanceQuery> {
        public AlignmentDistanceSubscriber(EventCatagory catagoryToSubscribe, Alignment alignment) : base(catagoryToSubscribe)
        {}
        public override bool isValid(Character sender, AlignmentDistanceQuery e)
        {
            return true;
        }
        public override object Activate(Character sender, AlignmentDistanceQuery e)
        {
            var alignments = sender.GetTokens<Alignment>(AlignmentToken.Token);
            return alignments.Select(a=>a.value.Distance(e.Origin)).Min();
        }
    }
}
