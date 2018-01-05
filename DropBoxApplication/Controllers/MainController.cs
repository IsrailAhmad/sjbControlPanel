using DropBoxApplication.Models;
using DropBoxApplication.App_Start;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DropBoxApplication.Controllers
{
    public class MainController : BaseController
    {
        // GET: Main
        public ActionResult Dashboard()
        {
            ViewBag.LoginID = Session["LoginID"].ToString();
            ViewBag.Username = Session["Username"].ToString();
            ViewBag.StoreId = Session["StoreId"].ToString();
            ViewBag.StoreStatus = Session["StoreStatus"].ToString();
            ViewBag.Message = "Your application Dashboard page.";
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetOrderStatusCount()
        {
            ViewBag.StoreId = Session["StoreId"].ToString();
            string url = GetUrl(2);
            url = url + "/UserLogin/GetAllOrders?StoreId=" + ViewBag.StoreId + "";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                //List<OrderStatusModel> result = new List<OrderStatusModel>();
                OrderStatusRootObject obj = new OrderStatusRootObject();
                if (responseMessage.IsSuccessStatusCode)
                {
                    var response = responseMessage.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    obj = JsonConvert.DeserializeObject<OrderStatusRootObject>(response, settings);
                    //result = obj.data;
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> StoreOpen()
        {
            ViewBag.StoreId = Session["StoreId"].ToString();
            string url = GetUrl(2);
            url = url + "/Store/StoreOpen?StoreId=" + ViewBag.StoreId + "";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                //List<OrderStatusModel> result = new List<OrderStatusModel>();
                OrderStatusRootObject obj = new OrderStatusRootObject();
                if (responseMessage.IsSuccessStatusCode)
                {
                    var response = responseMessage.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    obj = JsonConvert.DeserializeObject<OrderStatusRootObject>(response, settings);
                    //result = obj.data;
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> StoreClose()
        {
            ViewBag.StoreId = Session["StoreId"].ToString();
            string url = GetUrl(2);
            url = url + "/Store/StoreClose?StoreId=" + ViewBag.StoreId + "";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                //List<OrderStatusModel> result = new List<OrderStatusModel>();
                OrderStatusRootObject obj = new OrderStatusRootObject();
                if (responseMessage.IsSuccessStatusCode)
                {
                    var response = responseMessage.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    obj = JsonConvert.DeserializeObject<OrderStatusRootObject>(response, settings);
                    //result = obj.data;
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetLatestOrderDetails()
        {
            ViewBag.StoreId = Session["StoreId"].ToString();
            string url = GetUrl(2);
            url = url + "/UserLogin/GetLatestOrderDetails?StoreId=" + ViewBag.StoreId + "";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                //List<OrderStatusModel> result = new List<OrderStatusModel>();
                CustomerOrderDetailsSingleRootObject obj = new CustomerOrderDetailsSingleRootObject();
                if (responseMessage.IsSuccessStatusCode)
                {
                    var response = responseMessage.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    obj = JsonConvert.DeserializeObject<CustomerOrderDetailsSingleRootObject>(response, settings);
                    //result = obj.data;

                }
                return Json(obj.data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}