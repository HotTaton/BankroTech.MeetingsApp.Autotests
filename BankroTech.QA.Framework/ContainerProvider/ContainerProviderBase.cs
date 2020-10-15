using Autofac;
using BankroTech.QA.Framework.API;
using BankroTech.QA.Framework.Attributes;
using BankroTech.QA.Framework.PageObjects;
using BankroTech.QA.Framework.PageObjects.PageFactory;
using BankroTech.QA.Framework.Proxy;
using BankroTech.QA.Framework.SqlDriver;
using BankroTech.QA.Framework.TemplateResolver;
using BankroTech.QA.Framework.TemplateResolver.Resolvers;
using OpenQA.Selenium;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework.Helpers
{
    public class ContainerProviderBase
    {
        public virtual ContainerBuilder CreateContainerAndRegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ContextHelper>().As<IContextHelper>();

            builder.RegisterType<ParamResolverWrapper>().AsSelf();
            builder.RegisterType<WaitHelper>().As<IWaitHelper>();
            builder.RegisterType<PageFactory>().As<IPageFactory>();
            
            builder.RegisterInstance(ProxyServiceContainer.ProxyHandler)
                .As<IProxyHandlerService>()
                .As<IProxyHttpService>();

            builder.RegisterType<PgsqlDriver>().As<ISqlDriver>();

            builder.RegisterType<DateTemplateResolver>().AsSelf();
            builder.RegisterType<ParamTemplateResolver>().AsSelf();
            builder.RegisterType<RandomTemplateResolver>().AsSelf();
            builder.RegisterType<UrlTemplateResolver>().AsSelf();
            builder.RegisterType<TemplateResolverService>().As<ITemplateResolverService>();
                        
            var bindings = AppDomain
                                    .CurrentDomain
                                    .GetAssemblies()
                                    .SelectMany(assembly => assembly
                                            .GetTypes()
                                            .Where(t => Attribute.IsDefined(t, typeof(BindingAttribute))))
                                    .ToArray();
            builder.RegisterTypes(bindings).SingleInstance();

            var pageObjects = AppDomain
                                    .CurrentDomain
                                    .GetAssemblies()
                                    .SelectMany(assembly => assembly
                                            .GetTypes()
                                            .Where(t => Attribute.IsDefined(t, typeof(PageNameAttribute)) && typeof(BasePageObject).IsAssignableFrom(t)))
                                    .ToArray();
            builder.RegisterTypes(pageObjects).SingleInstance();

            builder.RegisterInstance(WebDriverContainer.WebDriver).As<IWebDriver>();

            builder.RegisterType<RestClientService>().As<IRestClientService>().SingleInstance();

            builder.RegisterType<BrowserNavigationService>().As<IBrowserNavigationService>().SingleInstance();

            return builder;
        }
    }
}
