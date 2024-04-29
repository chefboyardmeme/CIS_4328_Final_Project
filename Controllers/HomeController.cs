using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
//
using UNFBusShuttle.Models;
//
using UNFBusShuttle.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UNFBusShuttle.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext db = new ApplicationDbContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Get Bus Schedules 
            var busSchedules = (from bs in db.BusSchedules
                                select bs).ToList();
            ViewBag.BusSchedules = busSchedules;

            int? userPk = HttpContext.Session.GetInt32("UserPk");
            var driver = (from d in db.Drivers
                          where d.UserId == userPk
                          select d).FirstOrDefault();

            if (userPk != null)
            {
                if (driver != null)
                {
                    // Set Driver Name
                    ViewBag.DriverName = HttpContext.Session.GetString("UserName");

                    // Set Bus Number
                    ViewBag.BusNumber = driver.BusNumber;

                }                
            }            

            return View();
        }

        public IActionResult Home()
        {
            //int? userPk = HttpContext.Session.GetInt32("UserPk");

            // Get Bus Schedules 
            var busSchedules = (from bs in db.BusSchedules
                                select bs).ToList();
            ViewBag.BusSchedules = busSchedules;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region JSON Methods

        [HttpPost]
        //[Route("InsertDriverLocation")]
        public JsonResult InsertDriverLocation(string latitude, string longitude)
        {            
            string response = string.Empty;
                       
            // Call to Database Object using ApplicationDbContext to Save Driver's Location            
            try
            {
                int? userPk = HttpContext.Session.GetInt32("UserPk");
                //string? firstName = string.IsNullOrEmpty(HttpContext.Session.GetString("FirstName")) ? HttpContext.Session.GetString("UserName") : string.Empty;
                //string? lastName = !string.IsNullOrEmpty(HttpContext.Session.GetString("LastName")) ? HttpContext.Session.GetString("LastName") : string.Empty;

                // Get Driver by User-PK
                var driver = (from d in db.Drivers
                              .Where(x => x.UserId == userPk)
                              select d).FirstOrDefault();

                if (driver == null)
                {
                    response = "Driver details are not avaialble. Please contact with the System Administrator.";
                }
                else
                {
                    using (var db = new ApplicationDbContext())
                    {
                        db.Database.EnsureCreated();

                        var driverLocaction = new DriverLocation();
                        driverLocaction.DriverId = driver.Id;
                        //driverLocaction.UserId;
                        driverLocaction.Latitude = latitude;
                        driverLocaction.Longitude = longitude;

                        db.DriverLocations.Add(driverLocaction);
                        db.SaveChanges();
                        //
                        response = "success";
                    }
                }
            }
            catch (Exception ex)
            {
                response = "An error has occurred. Please contact with the System Administrator. Details are: " + ex.ToString();
            }            
            //
            return Json(response);
        }

        [HttpPost]
        //[Route("InsertRiderLocation")]
        public JsonResult InsertRiderLocation(string latitude, string longitude)
        {
            string response = string.Empty;

            // Call to Database Object using ApplicationDbContext to Save Driver's Location            
            try
            {
                //int? userPk = HttpContext.Session.GetInt32("UserPk");
                //string? firstName = string.IsNullOrEmpty(HttpContext.Session.GetString("FirstName")) ? HttpContext.Session.GetString("UserName") : string.Empty;
                //string? lastName = !string.IsNullOrEmpty(HttpContext.Session.GetString("LastName")) ? HttpContext.Session.GetString("LastName") : string.Empty;

                using (var db = new ApplicationDbContext())
                {
                    db.Database.EnsureCreated();

                    var driverLocaction = new DriverLocation();
                    //driverLocaction.DriverId;
                    //driverLocaction.UserId;
                    driverLocaction.Latitude = latitude;
                    driverLocaction.Longitude = longitude;

                    db.DriverLocations.Add(driverLocaction);
                    db.SaveChanges();
                    //
                    response = "success";
                }
            }
            catch (Exception ex)
            {
                response = "An error has occurred. Please contact with the System Administrator. Details are: " + ex.ToString();
            }
            //
            return Json(response);
        }

        [HttpPost]
        //[Route("DeleteDriverLocation")]
        public JsonResult DeleteDriverLocation()
        {
            string response = string.Empty;

            // Call to Database Object using ApplicationDbContext to Delete Driver's Location(s) -- Clock Out
            try
            {
                int? userPk = HttpContext.Session.GetInt32("UserPk");
                //string? firstName = string.IsNullOrEmpty(HttpContext.Session.GetString("FirstName")) ? HttpContext.Session.GetString("UserName") : string.Empty;
                //string? lastName = !string.IsNullOrEmpty(HttpContext.Session.GetString("LastName")) ? HttpContext.Session.GetString("LastName") : string.Empty;

                // Get Driver by User-PK
                var driver = (from d in db.Drivers
                              .Where(x => x.UserId == userPk)
                              select d).FirstOrDefault();

                if (driver == null)
                {
                    response = "Driver details are not avaialble. Please contact with the System Administrator.";
                }
                else
                {
                    using (var db = new ApplicationDbContext())
                    {
                        db.Database.EnsureCreated();

                        // Get Driver Locations
                        var driverLocations = (from d in db.DriverLocations
                                               .Where(x => x.DriverId == driver.Id)
                                               select d).ToList();
                        
                        if (driverLocations != null && driverLocations.Count() > 0)
                        {
                            foreach(var location in driverLocations)
                            {
                                db.DriverLocations.Attach(location);
                                db.Entry(location).State = EntityState.Deleted;                                
                            }
                            //
                            db.SaveChanges();   // Finally Call to Database to Delete Driver's Location(s)
                        }
                        //
                        response = "success";
                    }
                }
            }
            catch (Exception ex)
            {
                response = "An error has occurred. Please contact with the System Administrator. Details are: " + ex.ToString();
            }
            //
            return Json(response);
        }

        [HttpPost]        
        public JsonResult GetDriversLocation()
        {
            string response = string.Empty;

            // Call to Database using ApplicationDbContext to Get Driver's Location            
            try
            {
                var driverLocations = (from d in db.DriverLocations 
                                       where d.DriverId != null
                            select d).ToList();

                //var list = new List<DriverLocation>();
                var list = new List<string>();
                foreach(var item in driverLocations)
                {
                    // Get Driver Details by Driver-Pk
                    var driver = (from d in db.Drivers
                              .Where(x => x.Id == item.DriverId)
                                  select d).FirstOrDefault();

                    //list.Add(new DriverLocation(item.Latitude, item.Longitude));
                    list.Add(item.Latitude + "$" + item.Longitude + "$" + (driver.FirstName + " " + driver.LastName));                    
                }

                return Json(list);
                //return Json(new { Id = 123, drivers = "Hero" });
            }
            catch (Exception ex)
            {
                response = "An error has occurred. Please contact with the System Administrator. Details are: " + ex.ToString();
            }
            //
            return Json(response);
        }

        [HttpPost]
        public JsonResult GetRidersLocation()
        {
            string response = string.Empty;

            // Call to Database using ApplicationDbContext to Get Rider's Location            
            try
            {
                var driverLocations = (from d in db.DriverLocations
                                       where d.DriverId == null
                                       select d).ToList();

                //var list = new List<DriverLocation>();
                var list = new List<string>();
                foreach (var item in driverLocations)
                {                                       
                    list.Add(item.Latitude + "$" + item.Longitude);
                }

                return Json(list);
                
            }
            catch (Exception ex)
            {
                response = "An error has occurred. Please contact with the System Administrator. Details are: " + ex.ToString();
            }
            //
            return Json(response);
        }

        [HttpPost]
        //[Route("InsertRiderRequest")]
        public JsonResult InsertRiderRequest(int busNumber, int waitTime)
        {
            string response = string.Empty;

            // Call to Database Object using ApplicationDbContext to Save Rider's Request
            try
            {                
                using (var db = new ApplicationDbContext())
                {
                    db.Database.EnsureCreated();

                    var riderRequest = new RiderRequest();
                    riderRequest.BusNumber = busNumber;
                    riderRequest.WaitTime = waitTime;
                    riderRequest.RequestStatus = false;
                    riderRequest.ActionTaken = false;

                    db.RiderRequests.Add(riderRequest);
                    db.SaveChanges();
                    //
                    response = "success";
                }
            }
            catch (Exception ex)
            {
                response = "An error has occurred. Please contact with the System Administrator. Details are: " + ex.ToString();
            }
            //
            return Json(response);
        }

        [HttpPost]
        //[Route("UpdateRiderRequest")]
        public JsonResult UpdateRiderRequestStatus(int busNumber, string status)
        {
            string response = string.Empty;

            // Call to Database Object using ApplicationDbContext to Update Rider's Request
            try
            {
                int? userPk = HttpContext.Session.GetInt32("UserPk");

                using (var db = new ApplicationDbContext())
                {
                    db.Database.EnsureCreated();

                    var riderRequest = (from r in db.RiderRequests
                                        .Where(x => x.BusNumber == busNumber)
                                        select r).OrderByDescending(o => o.Id).FirstOrDefault();

                    if (riderRequest != null)
                    {
                        riderRequest.ActionTaken = true;
                        riderRequest.RequestStatus = status.ToLower() == "accepted" ? true : false;
                        riderRequest.DriverRemarks = status.ToLower() == "accepted" ? "Request Accepted" : "Request Rejected";
                        riderRequest.DateUpdated = DateTime.Now;
                        riderRequest.UpdatedBy = userPk;

                        db.RiderRequests.Attach(riderRequest);
                        db.Entry(riderRequest).State = EntityState.Modified;
                        db.SaveChanges();
                        //
                        response = "success";
                    }
                    else
                    {
                        response = "fail";
                    }

                }
            }
            catch (Exception ex)
            {
                response = "An error has occurred. Please contact with the System Administrator. Details are: " + ex.ToString();
            }
            //
            return Json(response);
        }

        [HttpPost]
        public JsonResult GetRiderRequestStatus(int busNumber)
        {
            string response = string.Empty;

            // Call to Database using ApplicationDbContext to Get Rider's Request Status
            try
            {
                var riderRequest = (from r in db.RiderRequests
                                    where r.BusNumber == busNumber
                                    select r).OrderByDescending(o => o.Id).FirstOrDefault();
                                
                if (riderRequest != null)
                {
                    return Json(riderRequest);
                }
                
                return Json(null);

            }
            catch (Exception ex)
            {
                response = "An error has occurred. Please contact with the System Administrator. Details are: " + ex.ToString();
            }
            //
            return Json(response);
        }

        #endregion

    }
  
}