using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABM
{
    public class Product
    {
        public int ID;
        public int Value;

        public int License;

        //product heeft een origin, wie is het werk begonnen
        //de origin verand.Nexterd niet, elke license verplicht dat je de originator credit. 
        public int OriginProductID;
        public int OriginAgent;
        public String NameOfOriginAgent;

        public List<int> Contributors;

        public Product(int _l, int _id)
        {
            // TODO: Complete member initialization
            License = _l;
            Value = 500;
            ID = _id;

            Contributors = new List<int>();
        }
    }
}
