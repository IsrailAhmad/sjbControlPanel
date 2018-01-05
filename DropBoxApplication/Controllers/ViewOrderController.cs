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
    public class ViewOrderController : BaseController
    {
        // GET: ViewOrder
        public ActionResult Index()
        {
            ViewBag.LoginID = Session["LoginID"].ToString();
            ViewBag.Username = Session["Username"].ToString();
            ViewBag.StoreId = Session["StoreId"].ToString();
            ViewBag.Message = "Your application Daily Activity page.";

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllOrdersList()
        {
            ViewBag.StoreId = Session["StoreId"].ToString();
            string url = GetUrl(2);
            url = url + "UserLogin/GetAllOrdersByStoreList?StoreId=" + ViewBag.StoreId + "";

            List<CustomerOrderModel> olist = new List<CustomerOrderModel>();
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
            return PartialView("_ViewOrdersList", olist);
        }
    }
}