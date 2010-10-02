using System;

namespace LemurLang.Conditions
{
    public abstract class ConditionElement
    {
        public ConditionElement Parent { get; set; }

        public ConditionElement(ConditionElement parent)
        {
            this.Parent = parent;
        }

        public override abstract string ToString();

        public abstract string DisplayTree(int currentLevel);

        public abstract bool Evaluate(Func<string, object> contextGetter);
    }
}
