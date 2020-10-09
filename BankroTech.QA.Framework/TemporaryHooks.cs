using BankroTech.QA.Framework.SqlDriver;
using BoDi;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework
{
    [Binding]
    public class TemporaryHooks
    {
        private readonly IObjectContainer objectContainer;

        public TemporaryHooks(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void Register()
        {
            objectContainer.RegisterTypeAs<PgsqlDriver, ISqlDriver>();
        }
    }
}
