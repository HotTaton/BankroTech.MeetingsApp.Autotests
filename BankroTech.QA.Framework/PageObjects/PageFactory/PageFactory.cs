using BankroTech.QA.Framework.Attributes;
using BoDi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankroTech.QA.Framework.PageObjects.PageFactory
{
    /// <summary>
    /// Contains information about all pages
    /// </summary>
    public class PageFactory
    {
        private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();
        private readonly IObjectContainer _objectContainer;

        public PageFactory(IObjectContainer objectContainer)
        {

            var items = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(PageNameAttribute)) && typeof(BasePageObject).IsAssignableFrom(t))).ToArray();
            /*var items = typeof(PageFactory)
                    .Assembly
                    .GetTypes()
                    .Where(t => Attribute.IsDefined(t, typeof(PageNameAttribute)) && typeof(BasePageObject).IsAssignableFrom(t))
                    .ToArray();*/

            foreach (var item in items)
            {
                var attr = item.GetCustomAttributes(typeof(PageNameAttribute), false).FirstOrDefault() as PageNameAttribute;
                _pages[attr.Name] = item;
            }

            _objectContainer = objectContainer;
        }

        /// <summary>
        /// Create page by name
        /// </summary>
        /// <param name="pageObjectName">Alias of the page</param>
        /// <returns>Created page</returns>
        public BasePageObject this[string pageObjectName] => (BasePageObject)_objectContainer.Resolve(_pages[pageObjectName]);

        public T Get<T>(string pageObjectName)
        {
            var obj = _objectContainer.Resolve(_pages[pageObjectName]);
            if (obj is T)
            {
                return (T)obj;
            }
            return default;
        }
    }
}
