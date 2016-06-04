using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfinderCharacterManager
{
    public class Choice<T>
    {
        public Choice(string title, string text, T value)
        {
            Title = title;
            Text = text;
            Value = value;
        }
        public string Title { get; }
        public string Text { get; }
        public T Value { get; }
    }
    public class DecisionInterfaceType
    {
        public static readonly
            DecisionInterfaceType List = new DecisionInterfaceType(), DialogRadio = new DecisionInterfaceType();
    }
    public class Decision<T>
    {
        public Decision(string title, string text, DecisionInterfaceType iType, params Choice<T>[] choices)
        {
            Choices = choices;
            Title = title;
            Text = text;
        }
        public string Title { get; }
        public string Text { get; }
        public Choice<T>[] Choices { get; }
    }
    public interface DecisionMaker
    {
        Choice<T> Choose<T>(Decision<T> decision);
        IEnumerable<Choice<T>> Choose<T>(Decision<T> decision, Func<IEnumerable<T>, bool> canContinue);
    }
}
