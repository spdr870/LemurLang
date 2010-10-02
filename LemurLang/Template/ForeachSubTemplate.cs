using LemurLang.Interfaces;

namespace LemurLang.Templates
{
    public class ForeachSubTemplate : BaseTemplate
    {
        public override TemplateParseResult Parse(string template, ITemplate currentItem, int index, char nextChar)
        {
            currentItem.Children.Add(this);

            return new TemplateParseResult(currentItem, index+1);
        }
    }
}
