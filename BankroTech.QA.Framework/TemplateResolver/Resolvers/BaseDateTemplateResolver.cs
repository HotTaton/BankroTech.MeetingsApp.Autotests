using System;
using System.Collections.Generic;

namespace BankroTech.QA.Framework.TemplateResolver.Resolvers
{
    public abstract class BaseDateTemplateResolver : BaseTemplateResolver, ITemplateResolver
    {
        protected abstract string StringFormat { get; }

        public string GetData(IEnumerable<string> args)
        {
            foreach (var arg in args)
            {
                if (EqualArgs(arg, "завтра"))
                {
                    return DateTime.Now.AddDays(1).ToString(StringFormat);
                }
                if (EqualArgs(arg, "послезавтра"))
                {
                    return DateTime.Now.AddDays(2).ToString(StringFormat);
                }
            }
            return string.Empty;
        }
    }
}
