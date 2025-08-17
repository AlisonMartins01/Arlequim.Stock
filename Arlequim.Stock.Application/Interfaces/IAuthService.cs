using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arlequim.Stock.Application.Interfaces
{
    public interface IAuthService
    {      
        Task<string> RegisterAsync(string name, string email, string password, string role, CancellationToken ct = default);
        Task<string> LoginAsync(string email, string password, CancellationToken ct = default);

    }
}
