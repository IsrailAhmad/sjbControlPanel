using DropBoxApplication.App_Start;
using DropBoxApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DropBoxApplication.Controllers
{
    public class MenuMasterController : BaseController
    {
        // GET: MenuMaster
        public async Task<ActionResult> Index()
        {
            ViewBag.LoginID = Session["LoginID"].ToString();
            ViewBag.Username = Session["Username"].ToString();
            ViewBag.Message = "Your application Daily Activity page.";
            //return View();
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
                }
            }
            MenuMasterModel menu = (MenuMasterModel)TempData["item"];
            return View(menu);
        }

        [HttpPost]
        public async Task<ActionResult> AddNewMenu(FormCollection fc, HttpPostedFileBase file)
        {
            ViewBag.StoreId = Session["StoreId"].ToString();
            if (ModelState.IsValid)
            {
                int MenuId = 0;
                string storeid = "";
                MenuMasterModel model = new MenuMasterModel();
                string menuid = fc["MenuId"];
                string menuname = fc["MenuName"];
                string menuprice = fc["MenuPrice"];
                if (ViewBag.StoreId == "0")
                {
                    storeid = fc["StoreName"];
                }
                else
                {
                    storeid = ViewBag.StoreId;
                }

                string url = GetUrl(2);
                url = url + "MenuMaster/AddNewMenu?menuid=" + menuid + "&menuname=" + menuname + "&menuprice=" + menuprice + "&storeid=" + storeid + "";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage responseMessage = await client.GetAsync(url);
                    MenuMasterModelSignleRootObject result = new MenuMasterModelSignleRootObject();
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var response = responseMessage.Content.ReadAsStringAsync().Result;
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        result = JsonConvert.DeserializeObject<MenuMasterModelSignleRootObject>(response);
                        if (result.data.MenuId == 0)
                        {
                            MenuId = Convert.ToInt32(menuid);
                        }
                        else
                        {
                            MenuId = result.data.MenuId;
                        }
                        if (MenuId > 0)
                        {
                            try
                            {
                                var allowedExtensions = new[]
                                {
                                 ".Jpg", ".png", ".jpg", "jpeg",".JPG",
                            };
                                //string imagepath = "http://103.233.79.234/Data/SJB_Android/LocalityPictures/";
                                model.ImageUrl = file.ToString(); //getting complete url                         
                                var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                                var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                                if (allowedExtensions.Contains(ext)) //check what type of extension  
                                {
                                    string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                                    string myfile = +MenuId + ext; //appending the name with id  
                                                                   // store the file inside ~/project folder(Img)  
                                                                   //var path = Path.Combine(imagepath, myfile);
                                    string path = @"C:\inetpub\wwwroot\Data\SJB_Android\\MenuPictures\" + Server.HtmlEncode(myfile);
                                    model.ImageUrl = path;
                                    //file.SaveAs(path);
                                    var fInfo = new FileInfo(myfile);
                                    if (!fInfo.Exists)
                                    {
                                        file.SaveAs(path);
                                    }
                                    else
                                    {
                                        System.IO.File.Copy(path, path, true);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                        }
                        else
                        {
                            ViewBag.message = "Please choose only Image file";
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllMenuList()
        {
            List<MenuMasterModel> olist = new List<MenuMasterModel>();
            ViewBag.StoreId = Session["StoreId"].ToString();
            string url = GetUrl(2);
            url = url + "MenuMaster/GetAllMenuList?StoreId=" + ViewBag.StoreId + "";

            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage responseMessage = await client.GetAsync(url);
                MenuMasterModelRootObject result = new MenuMasterModelRootObject();
                if (responseMessage.IsSuccessStatusCode)
                {
                    var response = responseMessage.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    result = JsonConvert.DeserializeObject<MenuMasterModelRootObject>(response);
                    olist = result.data;
                    ViewBag.LocalityList = olist;
                }
            }
            return PartialView("_Menulist", olist);
        }

        [HttpGet]
        public async Task<ActionResult> RemoveMenu(int id)
        {
            //string strint = id.Trim().ToString();
            //var intid = Convert.ToInt32(strint);
            List<MenuMasterModel> olist = new List<MenuMasterModel>();

            MenuMasterModelRootObject obj = new MenuMasterModelRootObject();

            string url = GetUrl(2);
            url = url + "MenuMaster/RemoveMenu?id=" + Convert.ToInt32(id) + "";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var response = responseMessage.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    obj = JsonConvert.DeserializeObject<MenuMasterModelRootObject>(response, settings);
                    olist = obj.data;
                    ViewBag.TransactionList = olist;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> GetMenuById(int id)
        {
            //string strint = id.Trim().ToString();
            //var intid = Convert.ToInt32(strint);
            MenuMasterModel pro = new MenuMasterModel();

            MenuMasterModelSignleRootObject obj = new MenuMasterModelSignleRootObject();

            string url = GetUrl(2);
            url = url + "MenuMaster/GetMenuById?id=" + Convert.ToInt32(id) + "";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var response = responseMessage.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    obj = JsonConvert.DeserializeObject<MenuMasterModelSignleRootObject>(response, settings);
                    pro = obj.data;
                    TempData["item"] = pro;
                }
            }
            return RedirectToAction("Index");
        }

    }
}