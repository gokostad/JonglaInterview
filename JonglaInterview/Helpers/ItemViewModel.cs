using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonglaInterview.Helpers
{
    class ItemViewModel
    {
        public ItemViewModel(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
