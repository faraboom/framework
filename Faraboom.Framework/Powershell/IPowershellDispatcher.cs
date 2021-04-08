using System.Threading.Tasks;

namespace Faraboom.Framework.Powershell
{
    public interface IPowershellDispatcher
    {
        Task Dispatch(PowershellContext context);
    }
}
