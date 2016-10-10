using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABM
{
    public class Product
    {
        public int License;
        private int ne;
        private object ProductIDer;

        public int Value;
        public int ID;

        public Product(int ne, object ProductIDer)
        {
            // TODO: Complete member initialization
            this.ne = ne;
            this.ProductIDer = ProductIDer;
        }
    }
}
