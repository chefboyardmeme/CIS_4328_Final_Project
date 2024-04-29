using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
//
using UNFBusShuttle.Models;
//
using UNFBusShuttle.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UNFBusShuttle.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private ApplicationDbContext db = new ApplicationDbContext();

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index()
        {            
            try
            {
                // Check User Details (If Admin?)               
                int? userPk = HttpContext.Session.GetInt32("UserPk");               

                // Get User by User-PK
                var user = (from u in db.Users
                              .Where(x => x.Id == userPk && x.UserType.ToLower() == "admin")
                              select u).FirstOrDefault();

                if (user == null)
                {
                    return View("UnAuthorized");
                    //ViewBag.Error = "An error has occurred. Please contact with the System Administrator.";                    
                }

                return View();

            }
            catch (Exception)
            {
                //throw ex;
                ViewBag.Error = "An error has occurred. Please contact with the System Administrator.";
                return View();
            }
            
        }        

        public ActionResult ManageUsers()
        {
            try
            {
                // Check User Details (If Admin?)  
                int? userPk = HttpContext.Session.GetInt32("UserPk");
                
                // Get User by User-PK
                var user = (from u in db.Users
                              .Where(x => x.Id == userPk && x.UserType.ToLower() == "admin")
                            select u).FirstOrDefault();

                if (user == null)
                    return View("UnAuthorized");

                // Get Users Details
                var users = (from u in db.Users
                             select u).ToList();

                return View(users);

            }
            catch (Exception)
            {
                //throw ex;
                ViewBag.Error = "An error has occurred. Please contact with the System Administrator.";
                return View();
            }
            
        }

        public ActionResult UpSertUser(int id = 0)
        {
            try
            {
                // Check User Details (If Admin?)  
                int? userPk = HttpContext.Session.GetInt32("UserPk");

                // Get User by User-PK
                var user = (from u in db.Users
                              .Where(x => x.Id == userPk && x.UserType.ToLower() == "admin")
                            select u).FirstOrDefault();

                if (user == null)
                    return View("UnAuthorized");

               if (id > 0)
               {
                    // Get User Details
                    var userForEdit = (from u in db.Users
                              .Where(x => x.Id == id)
                            select u).FirstOrDefault();

                    return View(userForEdit);
                }

                return View();

            }
            catch (Exception)
            {
                //throw ex;
                ViewBag.Error = "An error has occurred. Please contact with the System Administrator.";
                return View();
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpSertUser(User obj)
        {
            try
            {
                // Check User Details
                int? userPk = HttpContext.Session.GetInt32("UserPk");

                if (obj.Id == 0)
                {
                    // Add User
                    db.Database.EnsureCreated();

                    obj.IsActive = obj.UserStatus.ToLower() == "active" ? true : false;
                    obj.IsAdmin = false; //obj.UserType.ToLower() == "admin" ? true : false;
                    obj.DateAdded = DateTime.Now;
                    obj.AddedBy = userPk;

                    db.Users.Add(obj);
                    db.SaveChanges();
                    //
                    TempData["Success"] = "New User Added Successfully.";

                }
                else
                {
                    // Update User                  
                    db.Database.EnsureCreated();

                    obj.IsActive = obj.UserStatus.ToLower() == "active" ? true : false;
                    obj.IsAdmin = false; //obj.UserType.ToLower() == "admin" ? true : false;
                    obj.DateUpdated = DateTime.Now;
                    obj.UpdatedBy = userPk;

                    db.Users.Attach(obj);
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    //
                    TempData["Success"] = "User Details Updated Successfully.";
                }
               
                return RedirectToAction("ManageUsers");

            }
            catch (Exception)
            {
                //throw ex;
                ViewBag.Error = "An error has occurred. Please contact with the System Administrator.";
                return View();
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                // Check User Details
                int? userPk = HttpContext.Session.GetInt32("UserPk");

                // Get User by User-PK
                var user = (from u in db.Users
                              .Where(x => x.Id == userPk && x.UserType.ToLower() == "admin")
                              select u).FirstOrDefault();

                if (user == null)
                    return View("UnAuthorized");


                // Delete User                  
                db.Database.EnsureCreated();

                // Get User Details
                var userForDelete = (from u in db.Users
                          .Where(x => x.Id == id)
                                   select u).FirstOrDefault();

                db.Users.Attach(userForDelete);
                db.Entry(userForDelete).State = EntityState.Deleted;
                db.SaveChanges();
                //
                TempData["Success"] = "User Deleted Successfully.";

                return RedirectToAction("ManageUsers");

            }
            catch (Exception)
            {
                //throw ex;
                ViewBag.Error = "An error has occurred. Please contact with the System Administrator.";
                return View();
            }
        }

        public ActionResult ManageDrivers()
        {
            try
            {
                // Check User Details (If Admin?)  
                int? userPk = HttpContext.Session.GetInt32("UserPk");

                // Get User by User-PK
                var user = (from u in db.Users
                              .Where(x => x.Id == userPk && x.UserType.ToLower() == "admin")
                            select u).FirstOrDefault();

                if (user == null)
                    return View("UnAuthorized");

                // Get Drivers Details
                var drivers = (from d in db.Drivers
                             select d).ToList();

                return View(drivers);

            }
            catch (Exception)
            {
                //throw ex;
                ViewBag.Error = "An error has occurred. Please contact with the System Administrator.";
                return View();
            }
        }

        public ActionResult UpSertDriver(int id = 0)
        {
            try
            {
                // Check User Details (If Admin?)  
                int? userPk = HttpContext.Session.GetInt32("UserPk");

                // Get User by User-PK
                var user = (from u in db.Users
                            .Where(x => x.Id == userPk && x.UserType.ToLower() == "admin")
                            select u).FirstOrDefault();

                if (user == null)
                    return View("UnAuthorized");

                var users = (from u in db.Users
                            .Where(x => x.IsActive == true)
                             select u).ToList();

                if (id > 0)
                {
                    // Get Driver Details
                    var driver = (from d in db.Drivers
                                  .Where(x => x.Id == id)
                                  select d).FirstOrDefault();

                    ViewBag.Users = new SelectList(users, "Id", "UserName", driver.UserId);

                    return View(driver);
                }

                ViewBag.Users = new SelectList(users, "Id", "UserName");

                return View();

            }
            catch (Exception)
            {
                //throw ex;
                ViewBag.Error = "An error has occurred. Please contact with the System Administrator.";
                return View();
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpSertDriver(Driver obj)
        {
            try
            {
                // Check User Details
                int? userPk = HttpContext.Session.GetInt32("UserPk");

                if (obj.Id == 0)
                {
                    // Add User
                    db.Database.EnsureCreated();

                    obj.IsActive = obj.DriverStatus.ToLower() == "active" ? true : false;                    
                    obj.DateAdded = DateTime.Now;
                    obj.AddedBy = userPk;

                    db.Drivers.Add(obj);
                    db.SaveChanges();
                    //
                    TempData["Success"] = "New Driver Added Successfully.";

                }
                else
                {
                    // Update User                  
                    db.Database.EnsureCreated();

                    obj.IsActive = obj.DriverStatus.ToLower() == "active" ? true : false;                    
                    obj.DateUpdated = DateTime.Now;
                    obj.UpdatedBy = userPk;

                    db.Drivers.Attach(obj);
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    //
                    TempData["Success"] = "Driver Details Updated Successfully.";
                }

                return RedirectToAction("ManageDrivers");

            }
            catch (Exception)
            {
                //throw ex;
                ViewBag.Error = "An error has occurred. Please contact with the System Administrator.";
                return View();
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteDriver(int id)
        {
            try
            {
                // Check User Details
                int? userPk = HttpContext.Session.GetInt32("UserPk");

                // Get User by User-PK
                var user = (from u in db.Users
                            .Where(x => x.Id == userPk && x.UserType.ToLower() == "admin")
                            select u).FirstOrDefault();

                if (user == null)
                    return View("UnAuthorized");


                // Delete User                  
                db.Database.EnsureCreated();

                // Get Driver Details
                var driver = (from d in db.Drivers
                              .Where(x => x.Id == id)
                              select d).FirstOrDefault();

                db.Drivers.Attach(driver);
                db.Entry(driver).State = EntityState.Deleted;
                db.SaveChanges();
                //
                TempData["Success"] = "Driver Deleted Successfully.";

                return RedirectToAction("ManageUsers");

            }
            catch (Exception)
            {
                //throw ex;
                ViewBag.Error = "An error has occurred. Please contact with the System Administrator.";
                return View();
            }
        }

        public ActionResult ManageBusSchedules()
        {
            try
            {
                // Check User Details (If Admin?)  
                int? userPk = HttpContext.Session.GetInt32("UserPk");

                // Get User by User-PK
                var user = (from u in db.Users
                              .Where(x => x.Id == userPk && x.UserType.ToLower() == "admin")
                            select u).FirstOrDefault();

                if (user == null)
                    return View("UnAuthorized");

                // Get Bus Schedules Details
                var busSchedules = (from bs in db.BusSchedules
                               select bs).ToList();

                return View(busSchedules);

            }
            catch (Exception)
            {
                //throw ex;
                ViewBag.Error = "An error has occurred. Please contact with the System Administrator.";
                return View();
            }
        }

        public ActionResult UpSertBusSchedule(int id = 0)
        {
            try
            {
                // Check User Details
                int? userPk = HttpContext.Session.GetInt32("UserPk");

                // Get User by User-PK
                var user = (from u in db.Users
                            .Where(x => x.Id == userPk && x.UserType.ToLower() == "admin")
                            select u).FirstOrDefault();

                if (user == null)
                    return View("UnAuthorized");
               
                if (id > 0)
                {
                    // Get Bus Schedule Details
                    var busSchedule = (from bs in db.BusSchedules
                                  .Where(x => x.Id == id)
                                  select bs).FirstOrDefault();
                    
                    return View(busSchedule);
                }
                
                return View();

            }
            catch (Exception)
            {
                //throw ex;
                ViewBag.Error = "An error has occurred. Please contact with the System Administrator.";
                return View();
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpSertBusSchedule(BusSchedule obj)
        {
            try
            {
                // Check User Details
                int? userPk = HttpContext.Session.GetInt32("UserPk");

                if (obj.Id == 0)
                {
                    // Add User
                    db.Database.EnsureCreated();
                    
                    obj.DateAdded = DateTime.Now;
                    obj.AddedBy = userPk;

                    db.BusSchedules.Add(obj);
                    db.SaveChanges();
                    //
                    TempData["Success"] = "New Bus Schedule Added Successfully.";

                }
                else
                {
                    // Update User                  
                    db.Database.EnsureCreated();
                    
                    obj.DateUpdated = DateTime.Now;
                    obj.UpdatedBy = userPk;

                    db.BusSchedules.Attach(obj);
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    //
                    TempData["Success"] = "Bus Schedule Details Updated Successfully.";
                }

                return RedirectToAction("ManageBusSchedules");

            }
            catch (Exception)
            {
                //throw ex;
                ViewBag.Error = "An error has occurred. Please contact with the System Administrator.";
                return View();
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteBusSchedule(int id)
        {
            try
            {
                // Check User Details (If Admin?)  
                int? userPk = HttpContext.Session.GetInt32("UserPk");

                // Get User by User-PK
                var user = (from u in db.Users
                            .Where(x => x.Id == userPk && x.UserType.ToLower() == "admin")
                            select u).FirstOrDefault();

                if (user == null)
                    return View("UnAuthorized");


                // Delete User                  
                db.Database.EnsureCreated();
                
                // Get Bus Schedule Details
                var busSchedule = (from bs in db.BusSchedules
                              .Where(x => x.Id == id)
                                   select bs).FirstOrDefault();

                db.BusSchedules.Attach(busSchedule);
                db.Entry(busSchedule).State = EntityState.Deleted;
                db.SaveChanges();
                //
                TempData["Success"] = "Bus Schedule Deleted Successfully.";

                return RedirectToAction("ManageBusSchedules");

            }
            catch (Exception)
            {
                //throw ex;
                ViewBag.Error = "An error has occurred. Please contact with the System Administrator.";
                return View();
            }
        }

    }

}
