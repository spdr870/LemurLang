﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
