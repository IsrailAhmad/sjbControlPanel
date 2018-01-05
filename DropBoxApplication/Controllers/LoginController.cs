using DropBoxApplication.App_Start;
using DropBoxApplication.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using Newtonsoft.Json.Linq;
using System;

namespace DropBoxApplication.Controllers
{
    public class LoginController : BaseController
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
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
                                ViewBag.Message = "Please Select Store!";
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
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                ViewBag.Message = "Invalid username or password";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}