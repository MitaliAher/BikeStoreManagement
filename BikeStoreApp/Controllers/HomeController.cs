using Microsoft.AspNetCore.Mvc;
using BikeStoreApp.Models;
using System.Linq;
using BikeStoreApp.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;



namespace BikeStoreApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly BikeApplicationContext _context;
        private readonly IEmailService _emailService;

        public HomeController(BikeApplicationContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password, string loginAs)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                if (loginAs == "admin" && user.Isadmin)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username) ,
                new Claim("IsAdmin", "true")
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("AdminDashboard");
                }
                else if (loginAs == "staff" && user.Isstaff)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("IsStaff", "true")
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("StaffDashboard");
                }
            }

            ViewBag.ErrorMessage = "Please enter valid credentials";
            return View("Index");
        }


        public IActionResult AdminDashboard()
        {
            return View();
        }

        public IActionResult StaffDashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddMember()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMember(User user)
        {
            if (ModelState.IsValid)
            {
                user.Isadmin = bool.Parse(Request.Form["IsAdmin"]);
                user.Isstaff = bool.Parse(Request.Form["IsStaff"]);

                _context.Users.Add(user);
                _context.SaveChanges();

                return Json(new { success = true, message = "Member added successfully!" });
            }

            var errorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                 .Select(e => e.ErrorMessage)
                                                 .ToList();
            return Json(new { success = false, errors = errorMessages });
        }

        //  Products
        public IActionResult ManageProduct()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();

                return RedirectToAction("ManageProduct");
            }
            return View(product);
        }

        // public IActionResult EditProduct(int id)
        // {
        //     var product = _context.Products.Find(id);
        //     if (product == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(product);
        // }

        // [HttpPost]
        // public IActionResult EditProduct(Product product)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _context.Products.Update(product);
        //         _context.SaveChanges();

        //         return RedirectToAction("ManageProduct");
        //     }
        //     return View(product);
        // }

           public IActionResult EditProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

      [HttpPost]
public IActionResult EditProduct(Product product)
{
    if (ModelState.IsValid)
    {
        var existingProduct = _context.Products.Find(product.Productid);
        if (existingProduct == null)
        {
            return NotFound();
        }

        // Reload non-edited fields from the database
        _context.Entry(existingProduct).Reload();

        // Update only the fields you want to modify
        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;

        // Update the entity in the database
        _context.Products.Update(existingProduct);
        _context.SaveChanges();

        return RedirectToAction("ManageProduct");
    }
    return View(product);
}

        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("ManageProduct");
        }

        public IActionResult ManageStaffProduct()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
        [HttpGet]
        public IActionResult AddStaffProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStaffProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();

                return RedirectToAction("ManageStaffProduct");
            }
            return View(product);
        }

                  public IActionResult EditStaffProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

      [HttpPost]
public IActionResult EditStaffProduct(Product product)
{
    if (ModelState.IsValid)
    {
        var existingProduct = _context.Products.Find(product.Productid);
        if (existingProduct == null)
        {
            return NotFound();
        }

        // Reload non-edited fields from the database
        _context.Entry(existingProduct).Reload();

        // Update only the fields you want to modify
        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;

        // Update the entity in the database
        _context.Products.Update(existingProduct);
        _context.SaveChanges();

        return RedirectToAction("ManageStaffProduct");
    }
    return View(product);
}
        public IActionResult DeleteStaffProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("ManageStaffProduct");
        }


        // AddCategory
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();

                return RedirectToAction("ManageCategory");
            }
            return View(category);
        }

        // EditCategory
        public IActionResult EditCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();

                return RedirectToAction("ManageCategory");
            }
            return View(category);
        }

        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            var associatedProducts = _context.Products.Where(p => p.Categoryid == id).ToList();

            foreach (var product in associatedProducts)
            {
                _context.Products.Remove(product);
            }

            try
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("ManageCategory");
        }


        // ManageCategory
        public IActionResult ManageCategory()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult AddStaffCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStaffCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();

                return RedirectToAction("ManageStaffCategory");
            }
            return View(category);
        }

        // EditCategory
        public IActionResult EditStaffCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult EditStaffCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();

                return RedirectToAction("ManageStaffCategory");
            }
            return View(category);
        }

        // DeleteCategory
        public IActionResult DeleteStaffCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("ManageStaffCategory");
        }

        // ManageCategory
        public IActionResult ManageStaffCategory()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }


        //  Brands
        public IActionResult ManageBrands()
        {
            var brands = _context.Brands.ToList();
            return View(brands);
        }
        [HttpGet]
        public IActionResult AddBrand()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBrand(Brand brand)
        {
            if (ModelState.IsValid)
            {
                _context.Brands.Add(brand);
                _context.SaveChanges();

                return RedirectToAction("ManageBrands");
            }
            return View(brand);
        }
        public IActionResult EditBrand(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost]
        public IActionResult EditBrand(Brand brand)
        {
            if (ModelState.IsValid)
            {
                _context.Brands.Update(brand);
                _context.SaveChanges();

                return RedirectToAction("ManageBrands");
            }
            return View(brand);
        }

        public IActionResult DeleteBrand(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand == null)
            {
                return NotFound();
            }

            _context.Brands.Remove(brand);
            _context.SaveChanges();

            return RedirectToAction("ManageBrands");
        }

        public IActionResult ManageStaffBrands()
        {
            var brands = _context.Brands.ToList();
            return View(brands);
        }
        [HttpGet]
        public IActionResult AddStaffBrand()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStaffBrand(Brand brand)
        {
            if (ModelState.IsValid)
            {
                _context.Brands.Add(brand);
                _context.SaveChanges();

                return RedirectToAction("ManageStaffBrands");
            }
            return View(brand);
        }
        public IActionResult EditStaffBrand(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost]
        public IActionResult EditStaffBrand(Brand brand)
        {
            if (ModelState.IsValid)
            {
                _context.Brands.Update(brand);
                _context.SaveChanges();

                return RedirectToAction("ManageStaffBrands");
            }
            return View(brand);
        }

        public IActionResult DeleteStaffBrand(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand == null)
            {
                return NotFound();
            }

            _context.Brands.Remove(brand);
            _context.SaveChanges();

            return RedirectToAction("ManageStaffBrands");
        }


        // GET: Order/Customer
        public IActionResult Customer()
        {
            var orders = _context.Orders.Include(o => o.Product).ToList();
            return View(orders);
        }

        [HttpGet]
        public IActionResult EditCustomer(int id)
        {
            var order = _context.Orders.Include(o => o.Product).FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        public IActionResult EditCustomer(Order model)
        {
            if (ModelState.IsValid)
            {
                var originalOrder = _context.Orders.FirstOrDefault(o => o.OrderId == model.OrderId);

                if (originalOrder == null)
                {
                    return NotFound();
                }

                originalOrder.CustomerName = model.CustomerName;
                originalOrder.Contact = model.Contact;
                originalOrder.Address = model.Address;

                _context.SaveChanges();

                return RedirectToAction("Customer");
            }
            return View(model);
        }

        public IActionResult DeleteCustomer(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            try
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
                return RedirectToAction("Customer");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]
        public IActionResult DeleteOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();

            return Ok("Order deleted successfully");
        }


        //store
        public IActionResult Store()
        {
            var stores = _context.Stores.ToList();
            return View(stores);
        }


        public IActionResult StoreView()
        {
            var stores = _context.Stores.ToList();
            return View(stores);
        }
        [HttpGet]
        public IActionResult AddStore()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStore(Store store)
        {
            if (ModelState.IsValid)
            {
                _context.Stores.Add(store);
                _context.SaveChanges();

                return RedirectToAction("StoreView");
            }
            return View(store);
        }

        [HttpGet("EditStore/{id}")]
        public IActionResult EditStore(int id)
        {
            var store = _context.Stores.Find(id);
            if (store == null)
            {
                return NotFound();
            }
            return View("EditStore", store);
        }

        [HttpPost("EditStore/{id}")]
        public IActionResult EditStore(int id, Store store)
        {
            if (ModelState.IsValid)
            {
                var existingStore = _context.Stores.Find(id);
                if (existingStore == null)
                {
                    return NotFound();
                }

                existingStore.Name = store.Name;
                existingStore.Location = store.Location;

                _context.Stores.Update(existingStore);
                _context.SaveChanges();

                return RedirectToAction("StoreView");
            }
            return View("EditStore", store);
        }


        public IActionResult DeleteStore(int id)
        {
            var store = _context.Stores.Find(id);
            if (store == null)
            {
                return NotFound();
            }
            _context.Stores.Remove(store);
            _context.SaveChanges();

            return RedirectToAction("StoreView");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("Email", "Please provide a valid email address.");
                return View();
            }
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email address not found.");
                return View();
            }
            
            var existingResetToken = _context.Passwordresets.FirstOrDefault(pr => pr.Userid == user.Userid);
            if (existingResetToken != null && existingResetToken.Expirationtime > DateTime.UtcNow)
            {
                ViewBag.ErrorMessage = "A password reset email has already been sent. Please check your inbox.";
                return View("Error");
            }
            string resetToken = Guid.NewGuid().ToString();
            var passwordReset = new Passwordreset
            {
                Userid = user.Userid,
                Resettoken = resetToken,
                Expirationtime = DateTime.UtcNow.AddHours(1)
            };

            _context.Passwordresets.Add(passwordReset);
            _context.SaveChanges();
            string resetUrl = Url.Action("ResetPassword", "Home", new { token = resetToken }, Request.Scheme);
            string message = $"Click <a href='{resetUrl}'>Click here</a> to reset your password.";
            _emailService.SendEmailAsync(user.Email, "Password Reset", message).Wait();

            ViewBag.SuccessMessage = "Reset password instructions sent to your email.";
            return View("ForgotPasswordConfirmation");
        }



        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            var passwordReset = _context.Passwordresets.FirstOrDefault(pr => pr.Resettoken == token && pr.Expirationtime > DateTime.UtcNow);

            if (passwordReset == null)
            {
                ViewBag.ErrorMessage = "Invalid or expired reset token.";
                return View("Error");
            }
            ViewBag.ResetToken = token;
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(string token, string newPassword, string confirmPassword)
        {
            var passwordReset = _context.Passwordresets.FirstOrDefault(pr => pr.Resettoken == token && pr.Expirationtime > DateTime.UtcNow);

            if (passwordReset == null)
            {
                ViewBag.ErrorMessage = "Invalid or expired reset token.";
                return View("Error");
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match.";
                return View();
            }

            var user = _context.Users.FirstOrDefault(u => u.Userid == passwordReset.Userid);

            if (user == null)
            {
                ViewBag.ErrorMessage = "User not found.";
                return View("Error");
            }
            user.Password = newPassword;
            _context.Passwordresets.Remove(passwordReset);
            _context.SaveChanges();

            ViewBag.SuccessMessage = "Password reset successfully.";
            return View("ResetPasswordConfirmation");
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        public IActionResult StoreDetails(int id)
        {
            var store = _context.Stores.Include(s => s.Products)
                                        .ThenInclude(p => p.Brand)
                                        .Include(s => s.Products)
                                        .ThenInclude(p => p.Category)
                                        .FirstOrDefault(s => s.Storeid == id);

            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }
        public IActionResult StoreStaffDetails(int id)
        {
            var store = _context.Stores.Include(s => s.Products)
                                        .ThenInclude(p => p.Brand)
                                        .Include(s => s.Products)
                                        .ThenInclude(p => p.Category)
                                        .FirstOrDefault(s => s.Storeid == id);

            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }
        public IActionResult Search(string searchTerm)
        {
            var lowerSearchTerm = searchTerm.ToLower();

            var searchResults = _context.Products
                .Include(p => p.Store)
                .ThenInclude(s => s.Products)
                .ThenInclude(p => p.Brand)
                .Include(p => p.Store)
                .ThenInclude(s => s.Products)
                .ThenInclude(p => p.Category)
                .Where(p => p.Name.ToLower().Contains(lowerSearchTerm) ||
                            p.Store.Location.ToLower().Contains(lowerSearchTerm) ||
                            p.Store.Name.ToLower().Contains(lowerSearchTerm))
                .ToList();


            if (searchResults == null || searchResults.Count == 0)
            {
                ViewBag.NoRecordsFound = true;
            }
            else
            {
                ViewBag.NoRecordsFound = false;
            }

            return View(searchResults);
        }

        public IActionResult StaffDetails()
        {
            var staffDetails = _context.Users.Where(u => u.Isstaff).ToList();

            return View(staffDetails);
        }

        // GET: /Order/PlaceOrder/{productId}
        public IActionResult PlaceOrder(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult PlaceOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                var product = _context.Products.Find(order.ProductId);
                if (product == null)
                {
                    return NotFound();
                }

                decimal? gstAmount = (product.Price ?? 0) * 0.12m;
                order.FinalAmount = (product.Price ?? 0) + gstAmount;

                _context.Orders.Add(order);
                _context.SaveChanges();
                return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
            }
            return View(order);
        }



        // GET: /Order/OrderConfirmation/{orderId}
        public IActionResult OrderConfirmation(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        public IActionResult OrderDetail()
        {
            var orders = _context.Orders.ToList();

            return View(orders);
        }
        public IActionResult OrderStaffDetail()
        {
            var orders = _context.Orders.ToList();

            return View(orders);
        }


        public IActionResult SearchOrder(string searchQuery)
        {
            var orders = _context.Orders.ToList();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();

                orders = orders.Where(o =>
                    o.OrderId.ToString().ToLower().Contains(searchQuery) ||
                    o.CustomerName.ToLower().Contains(searchQuery)
                ).ToList();
            }

            if (orders.Count == 0)
            {
                ViewBag.Message = "No records found.";
            }

            return View("SearchResults", orders);
        }

        public IActionResult SearchStaffOrder(string searchQuery)
        {
            var orders = _context.Orders.ToList();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();

                orders = orders.Where(o =>
                    o.OrderId.ToString().ToLower().Contains(searchQuery) ||
                    o.CustomerName.ToLower().Contains(searchQuery)
                ).ToList();
            }

            if (orders.Count == 0)
            {
                ViewBag.Message = "No records found.";
            }

            return View("SearchStaffResults", orders);
        }
    }
}


