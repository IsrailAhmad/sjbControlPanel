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
    public class HomeController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            string preurl = GetUrl(2);
            preurl = preurl + "Store/GetAllStoreList";
            StoreMasterModelRootObject pobj = new StoreMasterModelRootObject();
            //List<ProductMasterModel> Prlist = new List<ProductMasterModel>();
            using (HttpClient prclient = new HttpClient())
            {
                HttpResponseMessage responseMessage = await prclient.GetAsync(preurl);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var result = responseMessage.Content.ReadAsStringAsync().Result;
                    pobj = JsonConvert.DeserializeObject<StoreMasterModelRootObject>(result);
                    IList<SelectListItem> ProSelectList = new List<SelectListItem>();
                    foreach (var item in pobj.data)
                    {
                        ProSelectList.Add(new SelectListItem { Text = item.StoreName, Value = item.StoreId.ToString() });
                    }
                    ProSelectList.Insert(0, new SelectListItem() { Value = "", Text = "Select Store" });
                    //ProSelectList.Insert(1, new SelectListItem() { Value = "0", Text = "All" });
                    ViewBag.StoreList = ProSelectList;
                    ViewBag.Message = TempData["item"];
                }
            }
            return View(new UserLoginViewModel());
            //return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<ActionResult> Userlogin(UserLoginViewModel login, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                string url = GetUrl(2);

                url = url + "UserLogin/WebsiteLogin?username=" + login.UserName + "&password=" + login.Password + "&storeid=" + login.StoreName + "";
                UserRootObject lRole = new UserRootObject();
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage responseMessage = await client.GetAsync(url);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var result = responseMessage.Content.ReadAsStringAsync().Result;
                        lRole = JsonConvert.DeserializeObject<UserRootObject>(result);
                        if (lRole.response.isSuccess == true)
                        {
                            if (lRole.data.StoreId == 0 && lRole.data.FirstName == "Admin")
                            {
                                Session["StoreId"] = lRole.data.StoreId;
                                Session["StoreStatus"] = lRole.data.StoreStatus;
                                Session["LoginID"] = lRole.data.UserID;
                                Session["Username"] = lRole.data.FirstName + ' ' + lRole.data.LastName;
                                return RedirectToAction("Dashboard", "Main");
                            }
                            else if (lRole.data.StoreId == Convert.ToInt32(login.StoreName))
                            {
                                Session["StoreId"] = lRole.data.StoreId;
                                Session["StoreStatus"] = lRole.data.StoreStatus;
                                Session["LoginID"] = lRole.data.UserID;
                                Session["Username"] = lRole.data.FirstName + ' ' + lRole.data.LastName;
                                return RedirectToAction("Dashboard", "Main");
                            }
                            else
                            {
                                ViewBag.Message = "Select Store Name/Use Admin as Name";
                                TempData["item"] = ViewBag.Message;
                                return RedirectToAction("Index");
                            }

                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        //ModelState.AddModelError("", "Invalid username or password");
                        //return View("Index", login);
                        ViewBag.Message = "Invalid username or password";
                        TempData["item"] = ViewBag.Message;
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                ViewBag.Message = "Invalid username or password";
            }            
            TempData["item"] = ViewBag.Message;
            return RedirectToAction("Index");            
        }
    }
}