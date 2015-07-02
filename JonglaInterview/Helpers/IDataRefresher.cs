using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonglaInterview.Helpers
{
    public interface IDataRefresher
    {
        void Initialize(IDataService dataLoader, object param = null);
        void Start();
        void Stop();
    }
}
