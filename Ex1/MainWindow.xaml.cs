using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ex1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		NORTHWNDEntities db = new NORTHWNDEntities();
		public MainWindow()
		{
			InitializeComponent();
		}
		//Ex1 Customer Names
		private void btnQuery_Click(object sender, RoutedEventArgs e)
		{
			var query = from c in db.Customers
					   select c.CompanyName;

			lbxCustomerEx1.ItemsSource = query.ToList();

		}
		//Ex2 - Customer Objects 
		private void btnQueryEx2_Click(object sender, RoutedEventArgs e)
		{
			var query = from c in db.Customers
						select c;
			dgCustomerEx2.ItemsSource = query.ToList();

		}

		//Ex3 Order Informtiom
		private void btnQueryEx3_Click(object sender, RoutedEventArgs e)
		{
			var query = from o in db.Orders
						where o.Customer.City.Equals("London")
						|| o.Customer.City.Equals("Paris")
						|| o.Customer.Country.Equals("USA")
						orderby o.Customer.CompanyName
						select new
						{
							CustomerName = o.Customer.CompanyName,
							City = o.Customer.City,
							Address = o.ShipAddress
						};

			dgCustomerEx3.ItemsSource = query.ToList().Distinct();

		}
		//Ex4 Product Information
		private void btnQueryEx4_Click(object sender, RoutedEventArgs e)
		{
			ShowProducts(dgCustomerEx4);
		
		
		}
		private void ShowProducts (DataGrid currentGrid)
		{
			var query = from p in db.Products
						where p.Category.CategoryName.Equals("Beverages")
						orderby p.ProductID descending
						select new
						{
							 p.ProductID,
							p.ProductName,
							p.Category.CategoryName,
							p.UnitPrice

						};
			currentGrid.ItemsSource = query.ToList();
		}

		//Ex5 Insert Information
		private void btnQueryEx5_Click(object sender, RoutedEventArgs e)
		{
			Product p = new Product()
			{
				ProductName = "Kickapoo Jungle Joy Juice ",
				UnitPrice = 12.49m,
				CategoryID = 1
			};

			db.Products.Add(p);
			db.SaveChanges();

			ShowProducts(dgCustomerEx5);
		}

		//Ex6 Update Product Information
		private void btnQueryEx6_Click(object sender, RoutedEventArgs e)
		{
			Product p1 = (db.Products
				.Where(p => p.ProductName.StartsWith("Kick"))
				.Select(p => p)).First();

			p1.UnitPrice = 100m;
			db.SaveChanges();
			ShowProducts(dgCustomerEx6);

		}

		//Ex7 -Multiply Update 
		private void btnQueryEx7_Click(object sender, RoutedEventArgs e)
		{
			var products = from p in db.Products
						   where p.ProductName.StartsWith("Kick")
						   select p;

			foreach(var item in products)
			{
				item.UnitPrice = 100m;
			}
			db.SaveChanges();
			ShowProducts(dgCustomerEx7);
		}
		//Ex8 Delete
		private void btnQueryEx8_Click(object sender, RoutedEventArgs e)
		{
			var products = from p in db.Products
						   where p.ProductName.StartsWith("Kick")
						   select p;

			db.Products.RemoveRange(products);
			db.SaveChanges();
			ShowProducts(dgCustomerEx8);

		}
		//Ex9 Stored Precedure

		private void btnQueryEx9_Click(object sender, RoutedEventArgs e)
		{
			var query = db.Customers_By_City("London");

			dgCustomerEx9.ItemsSource = query.ToList();

		}
	}
}
