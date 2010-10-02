using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
