using System.Threading.Tasks;
using Titanium.Web.Proxy.EventArguments;

namespace BankroTech.QA.Framework.Proxy
{
    internal interface IProxyHandlerService
    {           
        Task OnRequest(object sender, SessionEventArgs eventArgs);
        Task OnResponse(object sender, SessionEventArgs eventArgs);
    }
}
