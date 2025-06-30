using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case_Study
{
    public class PropertyUtil
    {
        public static string GetPropString()
        {
            return @"Data Source=KBEAST\SQLEXPRESS;Initial Catalog=E_Commerce_App;Integrated Security=True;Trust Server Certificate=True";
        }
    }

    public class DBConnection
    {
        private static SqlConnection con;

        public static SqlConnection GetConnection()
        {
            if (con == null)
            {
                con = new SqlConnection(PropertyUtil.GetPropString());
            }
            return con;
        }
    }

    // custom Exceptions
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException(string Customer_Name) : base($"The {Customer_Name} Name is  not found") { }
    }

    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(string Product_Name) : base($"The {Product_Name} Not fount") { }
    }

    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException(string Order) : base($"there is no order like {Order}") { }
    }
}
