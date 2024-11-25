using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Persistence.IProviders
{
    public interface ISeedProvider
    {
        Task InitProduction();
        Task InitDevelopment();
    }
}
