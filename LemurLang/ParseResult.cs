﻿using LemurLang.Interfaces;

namespace LemurLang
{
    public class TemplateParseResult
    {
        public TemplateParseResult(ITemplate current, int index)
        {
            this.CurrentTemplate = current;
            this.Index = index;
        }
        
        public ITemplate CurrentTemplate { get; private set; }
        public int Index { get; private set; }
    }
}
