using Ecom_Application.DAO;
using Ecom_Application.Entity;
using Ecom_Application.Exceptions_classes;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Ecom_Application
{
    internal class Ecom_App
    {
        static void Main(string[] args)
        {
            try
            {
                OrderProcessorRepositoryImpl validation = new OrderProcessorRepositoryImpl();

                int flag = 0;
                do
                {
                    try
                    {
                        Console.WriteLine("Welcome to Ecom Application");
                        Console.WriteLine("Enter 1 To Register Customer");
                        Console.WriteLine("Enter 2 To Create Product");
                        Console.WriteLine("Enter 3 To Delete Product");
                        Console.WriteLine("Enter 4 To Add to cart");
                        Console.WriteLine("Enter 5 to Remove from Cart");
                        Console.WriteLine("Enter 6 To View Cart");
                        Console.WriteLine("Enter 7 To Place Order");
                        Console.WriteLine("Enter 8 To View Customer order");
                        Console.WriteLine("Enter 9 To EXIT\n");
                        Console.WriteLine("Enter Your choice");
                        int x = int.Parse(Console.ReadLine());
                        switch (x)
                        {
                            case 1:
                                try
                                {
                                    Customers regcustomers = new Customers();
                                    Console.WriteLine("\nWelcome to Customers Registration Portal\n");
                                    Console.WriteLine("Enter Customer name");
                                    regcustomers.Name = Console.ReadLine();
                                    Console.WriteLine("Enter Customer Email");
                                    regcustomers.Email = Console.ReadLine();
                                    Console.WriteLine("Enter Password");
                                    regcustomers.Password = Console.ReadLine();
                                    OrderProcessorRepository customerregistration = new OrderProcessorRepositoryImpl();
                                    bool registration = customerregistration.createCustomer(regcustomers);
                                    if (registration == true)
                                    {
                                        Console.WriteLine("Customer Registered\n");
                                        Console.ReadLine();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Customer Cannot be registered\n");
                                        Console.ReadLine();
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                    Console.ReadLine();
                                }

                                break;

                            case 2:
                                try
                                {
                                    Products addproduct = new Products();
                                    Console.WriteLine("Welcome to product entry portal");
                                    Console.WriteLine("Enter Product name");
                                    addproduct.Name = Console.ReadLine();
                                    Console.WriteLine("Enter Product price");
                                    addproduct.Price = decimal.Parse(Console.ReadLine());
                                    Console.WriteLine("Enter Product Description");
                                    addproduct.Description = Console.ReadLine();
                                    Console.WriteLine("Enter Stock Quantity");
                                    addproduct.StockQuantity = int.Parse(Console.ReadLine());
                                    OrderProcessorRepository productaddition = new OrderProcessorRepositoryImpl();
                                    bool product_addition = productaddition.createProduct(addproduct);
                                    if (product_addition == true)
                                    {
                                        Console.WriteLine("Product Added\n");
                                        Console.ReadLine();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Product Already Exist\n");
                                        Console.ReadLine();
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                    Console.ReadLine();
                                }
                                break;


                            case 3:
                                try
                                {
                                    Products deleteproduct = new Products();
                                    Console.WriteLine("Enter product id of the product that you want to Delete");
                                    deleteproduct.Product_id = int.Parse(Console.ReadLine());
                                    validation.ValidateProductExistance(deleteproduct.Product_id);
                                    OrderProcessorRepository productdeletion = new OrderProcessorRepositoryImpl();
                                    bool productdeleted = productdeletion.deleteProduct(deleteproduct.Product_id);
                                    if (productdeleted == true)
                                    {
                                        Console.WriteLine("Product Deleted\n");
                                        Console.ReadLine();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Please enter a valid Product id\n");
                                        Console.ReadLine();
                                    }
                                }
                                catch (ProductNotFoundException pn)
                                {
                                    Console.WriteLine(pn.Message);
                                    Console.WriteLine("To try again enter 1 \n To go to main menu enter any other integer");
                                    if (int.Parse(Console.ReadLine()) == 1)
                                    {
                                        goto case 3;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message+" Redirectoring to main menu");
                                }
                                break;

                            case 4:
                                try
                                {
                                    Customers addcartcustomer = new Customers();
                                    Console.WriteLine("Enter Customer id");
                                    addcartcustomer.Customer_id = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Enter Your Password");
                                    addcartcustomer.Password = Console.ReadLine();
                                    validation.ValidateCustomerExistance(addcartcustomer);
                                    Products addcartproduct = new Products();
                                    Console.WriteLine("Enter Product id");
                                    addcartproduct.Product_id = int.Parse(Console.ReadLine());
                                    validation.ValidateProductExistance(addcartproduct.Product_id);
                                    Console.WriteLine("Enter Quantity");
                                    int cartquantity = int.Parse(Console.ReadLine());
                                    OrderProcessorRepository addtocart = new OrderProcessorRepositoryImpl();
                                    bool addedtocart = addtocart.addToCart(addcartcustomer, addcartproduct, cartquantity);
                                    if (addedtocart == true)
                                    {
                                        Console.WriteLine("Added to cart\n");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Cant add the things to your cart\n");
                                        Console.ReadLine();
                                    }
                                }
                                catch (CustomerNotFoundException ce)
                                {
                                    Console.WriteLine(ce.Message);
                                    goto case 4;
                                }
                                catch (ProductNotFoundException pe)
                                {
                                    Console.WriteLine(pe.Message);
                                    Console.WriteLine("To try again enter 1 \n To go to main menu enter any other integer");
                                    if (int.Parse(Console.ReadLine()) == 1)
                                    {
                                        goto case 4;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message + "\nTo goto main menu enter 1 or to try enter any other integer");
                                    int userexceptionchoice = int.Parse(Console.ReadLine());
                                    if (userexceptionchoice == 1)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        goto case 4;
                                    }
                                }
                                break;



                            case 5:
                                try
                                {
                                    Customers removecartforcustomer = new Customers();
                                    Console.WriteLine("Enter Customer id");
                                    removecartforcustomer.Customer_id = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Enter Your Passwoed");
                                    removecartforcustomer.Password = Console.ReadLine();
                                    validation.ValidateCustomerExistance(removecartforcustomer);
                                    Products removeprouctfromcart = new Products();
                                    Console.WriteLine("Enter product id of the product that you want to Delete");
                                    removeprouctfromcart.Product_id = int.Parse(Console.ReadLine());
                                    validation.ValidateProductExistance(removeprouctfromcart.Product_id);
                                    OrderProcessorRepository removefromcart = new OrderProcessorRepositoryImpl();
                                    bool productremoved = removefromcart.removeFromCart(removecartforcustomer, removeprouctfromcart);
                                    if (productremoved == true)
                                    {
                                        Console.WriteLine("Product removed from cart\n");
                                        Console.ReadLine();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Product was not in the cart\n");
                                        Console.ReadLine();
                                    }
                                }
                                catch (CustomerNotFoundException ce)
                                {
                                    Console.WriteLine(ce.Message);
                                    goto case 5;
                                }
                                catch (ProductNotFoundException pn)
                                {
                                    Console.WriteLine(pn.Message + "\nTo goto main menu enter 1 or to try again enter any other integer");
                                    int userexceptionchoice = int.Parse(Console.ReadLine());
                                    if (userexceptionchoice == 1)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        goto case 5;
                                    }

                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message + "\nTo goto main menu enter 1 or to try again enter any other integer");
                                    int userexceptionchoice = int.Parse(Console.ReadLine());
                                    if (userexceptionchoice == 1)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        goto case 5;
                                    }
                                }
                                break;



                            case 6:
                                try
                                {
                                    Customers viewcustomerscart = new Customers();
                                    Console.WriteLine("Enter Customer id");
                                    viewcustomerscart.Customer_id = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Enter Your Password");
                                    viewcustomerscart.Password = Console.ReadLine();
                                    validation.ValidateCustomerExistance(viewcustomerscart);
                                    OrderProcessorRepository viewcart = new OrderProcessorRepositoryImpl();
                                    List<Products> products = viewcart.getAllFromCart(viewcustomerscart);
                                    if (products != null)
                                    {
                                        foreach (Products p in products)
                                        {
                                            Console.WriteLine($"\nProduct id={p.Product_id}\nName={p.Name}\nPrice={p.Price}\nDescription={p.Description}\n");

                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("User Doestnot have any item in his cart");
                                        Console.ReadLine();
                                    }
                                }
                                catch(CustomerNotFoundException cnf)
                                {
                                    Console.WriteLine(cnf.Message);
                                    goto case 6;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message + "\nTo goto main menu enter 1 or to try again enter any other integer");
                                    int userexceptionchoice = int.Parse(Console.ReadLine());
                                    if (userexceptionchoice == 1)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        goto case 6;
                                    }
                                }
                                break;


                            case 7:
                                try
                                {
                                    OrderProcessorRepository placeorder = new OrderProcessorRepositoryImpl();
                                    int flagplaceorder = 0;
                                    int quantity;
                                    Console.WriteLine("\nWelcome to Order placing portal\n");
                                    List<Tuple<Products, int>> placeorderlist = new List<Tuple<Products, int>>();
                                    Customers customer = new Customers();
                                    Console.WriteLine("Enter customer id");
                                    customer.Customer_id = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Enter Your password");
                                    customer.Password = Console.ReadLine();
                                    validation.ValidateCustomerExistance(customer);
                                    do
                                    {
                                        Products products = new Products();
                                        Console.WriteLine("Enter Product id of the product that you want to order");
                                        products.Product_id = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            validation.ValidateProductExistance(products.Product_id);
                                            Console.WriteLine("Enter Quantity of this product");
                                            quantity = int.Parse(Console.ReadLine());
                                            placeorderlist.Add(Tuple.Create(products, quantity));
                                        }
                                        catch (ProductNotFoundException pe)
                                        {
                                            Console.WriteLine(pe.Message);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }
                                        Console.WriteLine("Enter 1 to add more item to the order list or enter any other number to proceed without adding more");
                                        int addmore = int.Parse(Console.ReadLine());
                                        if (addmore == 1)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            flagplaceorder = 1;
                                        }
                                    } while (flagplaceorder != 1);

                                    if (placeorderlist.Count() > 0)
                                    {
                                        Console.WriteLine("Enter Shipping address");
                                        string shippingaddress = Console.ReadLine();
                                        bool orderplacedornot = placeorder.PlaceOrder(customer, placeorderlist, shippingaddress);
                                        if (orderplacedornot == true)
                                        {
                                            Console.WriteLine("Order Placed");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Order cannot be placed");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("You haven't added anything to your order list");
                                    }

                                }
                                catch (CustomerNotFoundException cnf)
                                {
                                    Console.WriteLine(cnf.Message);
                                    goto case 7;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message + "\nTo goto main menu enter 1 or to try again enter any other integer");
                                    int userexceptionchoice = int.Parse(Console.ReadLine());
                                    if (userexceptionchoice == 1)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        goto case 5;
                                    }
                                }
                                break;

                            case 8:
                                try
                                {
                                    Customers getallorder = new Customers();
                                    List<Tuple<Products, int>> listcustomerOrders = new List<Tuple<Products, int>>();
                                    Console.WriteLine("Enter customer id");
                                    getallorder.Customer_id = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Enter Your password");
                                    getallorder.Password = Console.ReadLine();
                                    validation.ValidateCustomerExistance(getallorder);
                                    OrderProcessorRepository Customerorders = new OrderProcessorRepositoryImpl();
                                    listcustomerOrders = Customerorders.getOrdersByCustomer(getallorder.Customer_id);
                                    if (listcustomerOrders != null)
                                    {
                                        Console.WriteLine($"\nOrder Placed by Cutomer having customer id: {getallorder.Customer_id} =======>\n");
                                        foreach (var order in listcustomerOrders)
                                        {

                                            Console.WriteLine($"Product id: {order.Item1.Product_id} Product Name: {order.Item1.Name}, Quantity: {order.Item2} , Total Price= {order.Item1.Price}\n");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("User haven't ordered anything\n");
                                    }
                                }
                                catch (CustomerNotFoundException ce)
                                {
                                    Console.WriteLine(ce.Message);
                                    goto case 8;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message + "\nTo goto main menu enter 1 or to try again enter any other integer");
                                    int userexceptionchoice = int.Parse(Console.ReadLine());
                                    if (userexceptionchoice == 1)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        goto case 8;
                                    }
                                }
                                break;


                            case 9:
                                flag = 1;
                                break;


                            default:
                                Console.WriteLine("Please Enter a valid Choice");
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + " We are redirecting you to main menu");
                    }


                    Console.ReadKey();
                } while (flag != 1);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}
