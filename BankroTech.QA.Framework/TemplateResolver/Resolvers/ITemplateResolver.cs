using System.Collections.Generic;

namespace BankroTech.QA.Framework.TemplateResolver.Resolvers
{
    public interface ITemplateResolver
    {
        string GetData(IEnumerable<string> args);
    }
}
