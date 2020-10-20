using Autofac;
using Autofac.Builder;
using BankroTech.QA.Framework.API;
using BankroTech.QA.Framework.Attributes;
using BankroTech.QA.Framework.Helpers.ScreenshotMaker;
using BankroTech.QA.Framework.PageObjects;
using BankroTech.QA.Framework.PageObjects.PageFactory;
using BankroTech.QA.Framework.Proxy;
using BankroTech.QA.Framework.SqlDriver;
using BankroTech.QA.Framework.TemplateResolver;
using BankroTech.QA.Framework.TemplateResolver.Resolvers;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace BankroTech.QA.Framework.Helpers
{
    public class ContainerProviderBase
    {
        private static readonly object locker = new object();

        private static Type[] _pageObjects;
        private static Type[] PageObjects
        {
            get
            {
                if (_pageObjects == null)
                {
                    lock (locker)
                    {
                        if (_pageObjects == null)
                        {
                            _pageObjects = AppDomain
                                    .CurrentDomain
                                    .GetAssemblies()
                                    .SelectMany(assembly => assembly
                                            .GetTypes()
                                            .Where(t => Attribute.IsDefined(t, typeof(PageNameAttribute)) && typeof(BasePageObject).IsAssignableFrom(t)))
                                    .ToArray();
                        }
                    }
                }
                return _pageObjects;
            }
        }

        private static Type[] _bindings;
        private static Type[] Bindings
        {
            get
            {
                if (_bindings == null)
                {
                    lock (locker)
                    {
                        if (_bindings == null)
                        {
                            _bindings = AppDomain
                                    .CurrentDomain
                                    .GetAssemblies()
                                    .SelectMany(assembly => assembly
                                            .GetTypes()
                                            .Where(t => Attribute.IsDefined(t, typeof(BindingAttribute))))
                                    .ToArray();
                        }
                    }
                }
                return _bindings;
            }
        }

        public virtual ContainerBuilder CreateContainerAndRegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ContextHelper>().As<IContextHelper>().As<IInternalContextHelper>();

            builder.RegisterType<ParamResolverWrapper>().AsSelf();
            builder.RegisterType<WaitHelper>().As<IWaitHelper>();
            builder.RegisterType<PageFactory>().As<IPageFactory>().SingleInstance();

            builder.RegisterType<ProxyHandlerService>()
                .As<IProxyCookieService>()
                .As<IProxyHttpService>()
                .As<IAuthService>()
                .SingleInstance();

            builder.RegisterType<PgsqlDriver>().As<ISqlDriver>();

            builder.RegisterType<ScreenshotService>().As<IScreenshotService>().SingleInstance();

            builder.RegisterType<DateTemplateResolver>().AsSelf();
            builder.RegisterType<ParamTemplateResolver>().AsSelf();
            builder.RegisterType<RandomTemplateResolver>().AsSelf();            
            builder.RegisterType<TemplateResolverService>().As<ITemplateResolverService>();
                        
            builder.RegisterTypes(Bindings).SingleInstance();

            builder.RegisterTypes(PageObjects).SingleInstance();

            builder.RegisterType<RestClientService>().As<IRestClientService>().SingleInstance();

            builder.RegisterType<BrowserNavigationService>().As<IBrowserNavigationService>().SingleInstance();

            builder.RegisterType<WebContextInfo>().As<IWebContextInfo>().SingleInstance();            

            builder.Register(componentContext =>
            {
                var webContext = componentContext.Resolve<IWebContextInfo>();
                return webContext.WebDriver;
            }).AsSelf().As<IWebDriver>().SingleInstance();

            return builder;
        }
    }
}
