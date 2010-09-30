using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace LemurLang.Interfaces
{
    public interface IExpression
    {
        List<IExpression> Children { get; }
        string UsedTag { get; set; }
        IExpression Parent { get; set; }
        int IndexInTemplate { get; set; }
        bool NeedsToBeEnded { get; set; }

        string DisplayTree(int currentLevel);

        string Evaluate(EvaluationContext evaluationContext);
    }
}
