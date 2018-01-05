using DropBoxApplication.App_Start;
using DropBoxApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace DropBoxApplication.Controllers
{
    public class EverGreenDailyActivityController : BaseController
    {
        // GET: EverGreenDailyActivity
        public ActionResult Index()
        {
            ViewBag.LoginID = Session["LoginID"].ToString();
            ViewBag.Username = Session["Username"].ToString();
            ViewBag.Message = "Your application Daily Activity page.";
            return View();
        }
        public async Task<ActionResult> GetEverGreenOrders(int OrderStatusId)
        {
            List<CustomerOrderModel> olist = new List<CustomerOrderModel>();
            //if (ModelState.IsValid)
            //{
                ViewBag.LoginID = Session["LoginID"].ToString();
                ViewBag.Username = Session["Username"].ToString();
                ViewBag.StoreId = Session["StoreId"].ToString();
                ViewBag.Message = "Your application Daily Activity page.";
                string url = GetUrl(2);
                url = url + "UserLogin/MyAllOrderListByStatusId?StatusId=" + OrderStatusId + "&StoreId=" + ViewBag.StoreId + "";
                //CustomerOrderModel order = new CustomerOrderModel();
                OrderRootObject obj = new OrderRootObject();

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage CustomerresponseMessage = await client.GetAsync(url);
                    if (CustomerresponseMessage.IsSuccessStatusCode)
                    {
                        var response = CustomerresponseMessage.Content.ReadAsStringAsync().Result;
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        obj = JsonConvert.DeserializeObject<OrderRootObject>(response, settings);
                        olist = obj.data;
                        ViewBag.TransactionList = olist;
                    }
                }
            //return PartialView("_Storeslist", olist);
            //return View(olist);
            // }
            return PartialView("_OrdersByStatusList", olist);

        }
        [HttpGet]
        public async Task<ViewResult> OrderDetails(int OrderId)
        {
            OrderRootSingleObject orderDetails = new OrderRootSingleObject();
            CustomerOrderModel custModel = new CustomerOrderModel();
            if (ModelState.IsValid)
            {
                ViewBag.LoginID = Session["LoginID"].ToString();
                ViewBag.Username = Session["Username"].ToString();               
                ViewBag.StoreId = Session["StoreId"].ToString();
                //ViewBag.OrderId = OrderId;
                string strurl = GetUrl(2);
                strurl = strurl + "UserLogin/GetOrderByOrderId?orderid=" + OrderId + "&storeid=" + ViewBag.StoreId + "";
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage CustomerresponseMessage = await client.GetAsync(strurl);
                    if (CustomerresponseMessage.IsSuccessStatusCode)
                    {
                        var response = CustomerresponseMessage.Content.ReadAsStringAsync().Result;
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        orderDetails = JsonConvert.DeserializeObject<OrderRootSingleObject>(response, settings);
                        custModel = orderDetails.data;
                        ViewBag.Transaction = custModel;
                    }
                }
                return View(custModel);
            }
            return View(custModel);
        }
        [HttpGet]
        public async Task<ActionResult> OrderDispatch(string OrderId)
        {
            CustomerOrderDetailsRootObject orderDetails = new CustomerOrderDetailsRootObject();
            List<CustomerOrderModel> custModel = new List<CustomerOrderModel>();
            if (ModelState.IsValid)
            {
                ViewBag.LoginID = Session["LoginID"].ToString();
                ViewBag.Username = Session["Username"].ToString();
                ViewBag.StoreId = Session["StoreId"].ToString();
                ViewBag.OrderId = OrderId;
                string strurl = GetUrl(2);
                strurl = strurl + "UserLogin/DispatchOrder?orderid=" + OrderId + "&StoreId=" + ViewBag.StoreId + "";
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage CustomerresponseMessage = await client.GetAsync(strurl);
                    if (CustomerresponseMessage.IsSuccessStatusCode)
                    {
                        var response = CustomerresponseMessage.Content.ReadAsStringAsync().Result;
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        orderDetails = JsonConvert.DeserializeObject<CustomerOrderDetailsRootObject>(response, settings);
                        custModel = orderDetails.data;
                        ViewBag.Transaction = custModel;
                    }
                }
                return RedirectToAction("Dashboard", "Main");
            }
            return RedirectToAction("Dashboard", "Main");
        }
    }
}