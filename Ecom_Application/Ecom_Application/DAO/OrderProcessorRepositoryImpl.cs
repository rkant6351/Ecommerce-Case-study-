using Ecom_Application.Entity;
using Ecom_Application.Exceptions_classes;
using Ecom_Application.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ecom_Application.DAO
{
    public class OrderProcessorRepositoryImpl : OrderProcessorRepository
    {
        static SqlConnection conn;
        SqlCommand cmd;
        public bool createCustomer(Customers customers)
        {
            try
            {
                using (conn = DBConnection.GetConnection())
                {
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    cmd.CommandText = $"insert into customers(Name,email,password) values('{customers.Name}','{customers.Email}','{customers.Password}')";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"select SCOPE_IDENTITY()";
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    Console.WriteLine($"\n\nYou have been allocated a Customer_id Kindly note it down\n\n\nCustomerid={reader[0]}\n\n");
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool createProduct(Products products)
        {
            try
            {
                using (conn = DBConnection.GetConnection())
                {

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    cmd.CommandText = $"insert into products(Name,Price,description,stockQuantity) values('{products.Name}','{products.Price}','{products.Description}',{products.StockQuantity})";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"select SCOPE_IDENTITY()";
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    Console.WriteLine($"\n\nYour Product have been allocated a Product id Kindly note it down\n\n\nproduct id={reader[0]}\n\n");
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public bool addToCart(Customers customer, Products product, int quantity)
        {
            try
            {
                using (conn = DBConnection.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $"insert into cart(customer_id,product_id,quantity) values({customer.Customer_id},{product.Product_id},{quantity})";
                    int addedornot = cmd.ExecuteNonQuery();
                    if (addedornot > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool deleteCustomer(int customer_id)
        {
            try
            {
                using (conn = DBConnection.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    cmd.CommandText = $"Delete from customers where customer_id={customer_id}";

                    int customerdeletedornot = cmd.ExecuteNonQuery();
                    if (customerdeletedornot > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool deleteProduct(int product_id)
        {
            try
            {
                using (conn = DBConnection.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    cmd.CommandText = $"Delete from products where product_id={product_id}";

                    int productdeletedornot = cmd.ExecuteNonQuery();
                    if (productdeletedornot > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public bool removeFromCart(Customers customer, Products product)
        {
            try
            {
                using (conn = DBConnection.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    cmd.CommandText = $"Delete from cart where customer_id={customer.Customer_id} and product_id={product.Product_id}";
                    int removedornot = cmd.ExecuteNonQuery();
                    if (removedornot > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<Products> getAllFromCart(Customers customer)
        {
            try
            {
                using (conn = DBConnection.GetConnection())
                {
                    List<Products> productsincartlist = new List<Products>();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $"select p.product_id,p.Name,p.Price,p.description from products p join Cart c on c.product_id=p.product_id where c.customer_id={customer.Customer_id}";
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            productsincartlist.Add(new Products()
                            {
                                Product_id = (int)reader[0],
                                Name = reader[1].ToString(),
                                Price = (decimal)reader[2],
                                Description = (string)reader[3].ToString()
                            });
                        }
                        return productsincartlist;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        public List<Tuple<Products, int>> getOrdersByCustomer(int Customer_id)
        {
            try
            {
                using (conn = DBConnection.GetConnection())
                {
                    List<Tuple<Products, int>> customerOrders = new List<Tuple<Products, int>>();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $"select  p.product_id, p.name, oi.Quantity,p.price from Orders o join Order_items oi on o.order_id=oi.order_id" +
                        $" join products p  on p.product_id=oi.product_id where o.customer_id={Customer_id}";
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Products products = new Products() { Product_id = (int)reader[0], Name = reader[1].ToString(), Price = ((int)reader[2] * (decimal)reader[3]) };
                            customerOrders.Add(Tuple.Create(products, (int)reader[2]));
                        }
                        return customerOrders;
                    }
                    else
                    {
                        throw new OrderNotFoundException();
                    }
                }
            }
            catch (OrderNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public bool PlaceOrder(Customers customer, List<Tuple<Products, int>> productsAndQuantities, string shippingAddress)
        {
            try
            {
                using (conn = DBConnection.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    decimal totalamount = calculatetotalamount(productsAndQuantities);
                    cmd.CommandText = $"insert into orders(customer_id,order_date,total_price,shipping_address) OUTPUT inserted.order_id values({customer.Customer_id},GETDATE(),{totalamount},'{shippingAddress}')";
                    int order_id = (int)cmd.ExecuteScalar();
                    int placed = 0;
                    foreach (var orderitemandquantity in productsAndQuantities)
                    {
                        cmd.CommandText = $"insert into order_items(order_id,product_id,quantity) values({order_id},{orderitemandquantity.Item1.Product_id},{orderitemandquantity.Item2})";
                        int insertedinorderitems = cmd.ExecuteNonQuery();
                        placed += insertedinorderitems;
                    }
                    if (placed > 0 && order_id > 0)
                    {
                        Console.WriteLine($"You have been allocated a order id: {order_id}");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error placing order: {ex.Message}");
                return false;
            }

        }






        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Some miscelleneous functions
        public bool ValidateCustomerExistance(Customers customer)
        {
            using (conn = DBConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.Text;
                conn.Open();
                cmd.CommandText = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM customers WHERE customer_id = {customer.Customer_id}) THEN 1 ELSE 0 END";
                int customerexistance = (int)cmd.ExecuteScalar();
                if (customerexistance == 1)
                {
                    cmd.CommandText = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM customers WHERE customer_id = {customer.Customer_id} and password COLLATE Latin1_General_CS_AS ='{customer.Password}') THEN 1 ELSE 0 END";
                    int passwordvalidation = (int)cmd.ExecuteScalar();
                    if (passwordvalidation == 1)
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter valid password");
                        string reenter_password = Console.ReadLine();
                        cmd.CommandText = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM customers WHERE customer_id = {customer.Customer_id} and password COLLATE Latin1_General_CS_AS ='{reenter_password}') THEN 1 ELSE 0 END";
                        int passwordrevalidation = (int)cmd.ExecuteScalar();
                        if(passwordrevalidation == 1)
                        {
                            return true;
                        }
                        else
                        {
                            throw new Exception("You have entered wrong password twice");
                        }

                    }
                }
                else
                {
                    conn.Close();
                    throw new CustomerNotFoundException();
                }
            }
        }

        public bool ValidateProductExistance(int product_id)
        {
            using (conn = DBConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.Text;
                conn.Open();
                cmd.CommandText = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM Products WHERE Product_id = {product_id}) THEN 1 ELSE 0 END";
                int productexistance = (int)cmd.ExecuteScalar();
                if (productexistance == 1)
                {
                    return true;
                }
                else
                {
                    throw new ProductNotFoundException();
                }
            }
        }

        static decimal calculatetotalamount(List<Tuple<Products, int>> listofproductandquantity)
        {
            decimal totalamount = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            foreach (var list in listofproductandquantity)
            {
                cmd.CommandText = $"Select price from products where product_id={list.Item1.Product_id}";
                decimal rate = (decimal)cmd.ExecuteScalar();
                totalamount += rate * (list.Item2);
            }
            return totalamount;
        }
    }
}
