using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService
{
    /// <summary>
    /// Филиал
    /// </summary>
    public class Branch
    {
        // Имя филиала
        private int name;

        // Очередь посылок в филиале
        public List<Package> Packages;

        public Branch(int N)
        {
            name = N;
            Packages = new List<Package>();
        }

    }
}
