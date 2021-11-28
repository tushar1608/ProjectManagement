using System.Threading.Tasks;
using ProjectManagement.Web.Models;

namespace ProjectManagement.Web.Interfaces
{
    public interface IJwtAuthenticationManagerService
    {
        Task<string> Authenticate(LoginDetails loginDetails);
    }
}
