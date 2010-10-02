using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace LemurLang.Interfaces
{
    public interface ITemplate
    {
        List<ITemplate> Children { get; }
        string UsedTag { get; set; }
        ITemplate Parent { get; set; }
        int IndexInTemplate { get; set; }
        bool NeedsToBeEnded { get; set; }

        string Arguments { get; set; }

        string DisplayTree(int currentLevel);

        TemplateParseResult Parse(string template, ITemplate currentItem, int index, char nextChar);

        string Evaluate(EvaluationContext evaluationContext);
    }
}
