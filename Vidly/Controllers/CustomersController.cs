using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using System.Data.Entity;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
	public class CustomersController : Controller
	{
		// Initializing a Db Connection
		private ApplicationDbContext _context;

		public CustomersController()
		{
			_context = new ApplicationDbContext();
		}

		protected override void Dispose(bool disposing)
		{
			_context.Dispose();
		}

		public ActionResult New()
		{
			var membershipTypes = _context.MembershipType.ToList();
			var viewModel = new NewCustomerViewModel
			{
				MembershipTypes = membershipTypes
			};

			return View(viewModel);
		}

		[HttpPost]
		public ActionResult Create(Customers customer)
		{
			_context.Customers.Add(customer);
			_context.SaveChanges();
			return RedirectToAction("Index", "Customers");
		}

		// GET: Customers
		public ViewResult Index()
		{
			var customers = _context.Customers.Include(c => c.MembershipType).ToList();
			return View(customers);
		}

		public ActionResult Details(int Id)
		{
			var customer = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == Id);
			if (customer == null)
			{
				return HttpNotFound();
			}
			return View(customer);
		}
	}
}