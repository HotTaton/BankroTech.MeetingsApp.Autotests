using System;

namespace BankroTech.QA.Framework.TemplateResolver.Resolvers
{
    public abstract class BaseTemplateResolver
    {
        protected bool EqualArgs(string arg1, string arg2)
        {
            return string.Equals(arg1, arg2, StringComparison.OrdinalIgnoreCase);
        }
    }
}
