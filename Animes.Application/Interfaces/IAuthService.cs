using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animes.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> Authenticate(string username, string password);

    }

}
