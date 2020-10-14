using BankroTech.QA.Framework.Attributes;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BankroTech.QA.Framework.PageObjects
{
    public abstract class BasePageObject
    {
        public string AbsoluteUrl => $"{_applicationName}#{Action}";
        protected abstract string Action { get; }
        protected IWebDriver WebDriver { get; }

        private Dictionary<string, Func<IWebElement>> _pageElementGetters = new Dictionary<string, Func<IWebElement>>();
        private readonly string _applicationName;

        protected BasePageObject(IWebDriver webDriver, IConfigurationRoot configuration)
        {
            WebDriver = webDriver;

            var applicationName = configuration.GetSection("ApplicationName").Value;
            if (!applicationName.EndsWith("/"))
            {
                applicationName += "/";
            }
            _applicationName = applicationName;            

            InitPropertyDictionary();
        }        

        private void InitPropertyDictionary()
        {
            var properties = GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.GetCustomAttributes(typeof(PageElementAttribute), false).Any(x => x is PageElementAttribute)
                    && typeof(IWebElement).IsAssignableFrom(property.PropertyType))
                {
                    var attr = (PageElementAttribute)property
                                    .GetCustomAttributes(typeof(PageElementAttribute), false)
                                    .FirstOrDefault(x => x is PageElementAttribute);

                    var propExpression = Expression.Property(Expression.Constant(this), property.Name);
                    var propLambda = Expression.Lambda<Func<IWebElement>>(propExpression);
                    var propBody = propLambda.Compile();

                    _pageElementGetters[attr.Name] = propBody;
                }
            }
        }

        public IWebElement this[string elementName] => _pageElementGetters[elementName].Invoke();

        public T Get<T>(string elementName)
        {
            var obj = _pageElementGetters[elementName]?.Invoke();

            if (obj is T)
            {
                return (T)obj;
            }

            return default;
        }

        public void SetInput(string name, string value) => this[name].SendKeys(value);

        public void ClickButton(string name) => this[name].Click();

        public void ClickOnExpansionPanel(string name)
        {
            var elem = this[name];
            new Actions(WebDriver).MoveToElement(elem).Click().Build().Perform();
        }

        public void Submit(string name) => this[name].Submit();
    }
}
