using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LemurLang.Exceptions;
using System.Collections;

namespace LemurLang
{
    public class EvaluationContext
    {
        public EvaluationContext(IDictionary<string, object> context, EvaluationContext parent)
        {
            this.Context = context;
            this.Parent = parent;

            if (this.Context == null)
                this.Context = new Dictionary<string, object>();
        }

        internal IDictionary<string, object> Context { get; set; }

        internal EvaluationContext Parent { get; set; }


        internal object GetValue(string reference)
        {
            string[] path = reference.Split('.');
            object rootObject = GetRootObject(path[0]);

            if (path.Length == 1)
            {
                return rootObject;
            }
            else
            {
                if (rootObject == null)
                    throw new EvaluationException("NULL reference: " + reference);
                
                return TraverseRootObject(rootObject, path.Skip(1).ToArray());
            }
        }

        internal object GetRootObject(string variableName)
        {
            if (this.Context.ContainsKey(variableName))
            {
                return this.Context[variableName];
            }
            else if (this.Parent != null)
            {
                return this.Parent.GetRootObject(variableName);
            }
            else
            {
                throw new EvaluationException("Variable could not be found: " + variableName);
            }
        }

        private object TraverseRootObject(object currentObject, string[] path)
        {
            if (currentObject == null)
                throw new EvaluationException("NULL reference: " + string.Join(".", path));

            string propertyName = path[0];
            object value = null;
            if (currentObject is IDictionary)
            {
                value = ((IDictionary)currentObject)[propertyName];
            }
            else
            {
                Type type = currentObject.GetType();
                value = type.GetProperty(propertyName).GetValue(currentObject, null);
            }

            if (path.Length == 1)
            {
                return value;
            }
            else
            {
                return TraverseRootObject(value, path.Skip(1).ToArray());
            }
        }
    }
}
