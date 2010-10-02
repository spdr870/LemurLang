using LemurLang.Interfaces;

namespace LemurLang.Templates
{
    public class ElseTemplate : ElseIfTemplate
    {
        public override string ToString()
        {
            return "ELSE:";
        }

        protected override bool GetConditionEvaluation(EvaluationContext evaluationContext)
        {
            return true;
        }

        public override TemplateParseResult Parse(string template, ITemplate currentItem, int index, char nextChar)
        {
            currentItem.Children.Add(this);

            return new TemplateParseResult(currentItem, index+1);
        }
    }
}
