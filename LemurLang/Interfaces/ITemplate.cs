using System;
using System.Collections.Generic;

namespace LemurLang.Interfaces
{
    public interface ITemplate
    {
        List<ITemplate> Children { get; }
        string UsedTag { get; set; }
        ITemplate Parent { get; set; }
        int IndexInTemplate { get; set; }

        string Arguments { get; set; }

        string DisplayTree(int currentLevel);

        TemplateParseResult Parse(string template, ITemplate currentItem, int index, char nextChar);

        void Evaluate(EvaluationContext evaluationContext, Action<string> write);
    }
}
