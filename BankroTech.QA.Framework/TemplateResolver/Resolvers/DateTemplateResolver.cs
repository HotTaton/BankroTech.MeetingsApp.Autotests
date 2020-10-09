namespace BankroTech.QA.Framework.TemplateResolver.Resolvers
{
    public class DateTemplateResolver : BaseDateTemplateResolver, ITemplateResolver
    {
        protected override string StringFormat => "dd.MM.yyyy";
    }
}
