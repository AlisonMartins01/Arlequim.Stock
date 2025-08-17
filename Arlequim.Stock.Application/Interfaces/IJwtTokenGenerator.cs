using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlequim.Stock.Domain.Entities;

namespace Arlequim.Stock.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string Create(User user);
    }
}
