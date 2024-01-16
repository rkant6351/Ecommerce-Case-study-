using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Application.Exceptions_classes
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException() : base("Product Id is invalid")
        {

        }
    }
}
