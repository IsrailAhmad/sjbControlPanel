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
    public class StoreMasterController : BaseController
    {
        // GET: StoreMaster
        public async Task<ActionResult> Index()
        {
            ViewBag.LoginID = Session["LoginID"].ToString();
            ViewBag.Username = Session["Username"].ToString();
            ViewBag.Message = "Your application Daily Activity page.";
            //return View();

            string url = GetUrl(2);
            url = url + "Localities/GetAllLocalitiesWithOutPagging";
            LocalityMasterModelRootObject obj = new LocalityMasterModelRootObject();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var result = responseMessage.Content.ReadAsStringAsync().Result;
                    obj = JsonConvert.DeserializeObject<LocalityMasterModelRootObject>(result);

                    IList<SelectListItem> LSelectList = new List<SelectListItem>();
                    foreach (var item in obj.data)
                    {
                        LSelectList.Add(new SelectListItem { Text = item.LocalityName, Value = item.LocalityId.ToString() });
                    }
                    LSelectList.Insert(0, new SelectListItem() { Value = "", Text = "Select Locality" });

                    //LSelectList.Insert(1, new SelectListItem() { Value = "0", Text = "All" });

                    ViewBag.LocalityList = LSelectList;
                }
            }
            StoreMasterModel menu = (StoreMasterModel)TempData["item"];
            return View(menu);
        }

        [HttpPost]
        public async Task<ActionResult> AddNewStore(FormCollection fc, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                int StoreId = 0;
                StoreMasterModel model = new StoreMasterModel();
                string storeid = fc["StoreId"];
                string localityid = fc["LocalityName"];
                string storename = fc["StoreName"];
                string phonenumber = fc["StorePhoneNumber"];
                string email = fc["StoreEmailId"];
                string address = fc["StoreAddress"];
                string openingtime = fc["OpeningTime"];
                string closingtime = fc["ClosingTime"];
                ViewBag.StoreId = Session["StoreId"].ToString();
                if (storeid != "0")
                {
                    StoreId = Convert.ToInt32(storeid);
                    SavePictures(StoreId, file);
                }

                string url = GetUrl(2);
                url = url + "Store/AddNewStore?storeid=" + storeid + "&storename=" + storename + "&phonenumber=" + phonenumber + "&email=" + email + "&address=" + address + "&opeingtime=" + openingtime + "&closingtime=" + closingtime + "&localityid=" + localityid + "";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage responseMessage = await client.GetAsync(url);
                    StoreMasterModelSingleRootObject result = new StoreMasterModelSingleRootObject();
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var response = responseMessage.Content.ReadAsStringAsync().Result;
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        result = JsonConvert.DeserializeObject<StoreMasterModelSingleRootObject>(response);
                        if (result.data.StoreId > 0)
                        {
                            StoreId = result.data.StoreId;                            
                            SavePictures(StoreId, file);
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
        public async Task<ActionResult> GetAllStoresList()
        {
            List<StoreMasterModel> olist = new List<StoreMasterModel>();

            string url = GetUrl(2);
            url = url + "Store/GetAllStoreList";
            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage responseMessage = await client.GetAsync(url);
                StoreMasterModelRootObject result = new StoreMasterModelRootObject();
                if (responseMessage.IsSuccessStatusCode)
                {
                    var response = responseMessage.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    result = JsonConvert.DeserializeObject<StoreMasterModelRootObject>(response);
                    olist = result.data;
                    ViewBag.LocalityList = olist;
                }
            }
            return PartialView("_Storeslist", olist);
        }

        [HttpGet]
        public async Task<ActionResult> RemoveStore(int id)
        {
            //string strint = id.Trim().ToString();
            //var intid = Convert.ToInt32(strint);
            List<StoreMasterModel> olist = new List<StoreMasterModel>();

            StoreMasterModelRootObject obj = new StoreMasterModelRootObject();

            string url = GetUrl(2);
            url = url + "Store/RemoveStore?id=" + Convert.ToInt32(id) + "";
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
                    obj = JsonConvert.DeserializeObject<StoreMasterModelRootObject>(response, settings);
                    olist = obj.data;
                    ViewBag.TransactionList = olist;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> GetStoreById(int id)
        {
            //string strint = id.Trim().ToString();
            //var intid = Convert.ToInt32(strint);
            StoreMasterModel pro = new StoreMasterModel();

            StoreMasterModelSingleRootObject obj = new StoreMasterModelSingleRootObject();

            string url = GetUrl(2);
            url = url + "Store/GetStoreById?id=" + Convert.ToInt32(id) + "";
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
                    obj = JsonConvert.DeserializeObject<StoreMasterModelSingleRootObject>(response, settings);
                    pro = obj.data;
                    TempData["item"] = pro;
                }
            }
            return RedirectToAction("Index");
        }

        public static void SavePictures(int StoreId, HttpPostedFileBase file)
        {
            try
            {
                var allowedExtensions = new[]
                {
                                 ".Jpg", ".png", ".jpg", "jpeg",".JPG",
                            };
                //string imagepath = "http://103.233.79.234/Data/SJB_Android/LocalityPictures/";
                // model.StorePicturesUrl = file.ToString(); //getting complete url                         
                var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                if (allowedExtensions.Contains(ext)) //check what type of extension  
                {
                    string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                    string myfile = +StoreId + ext; //appending the name with id  
                                                    // store the file inside ~/project folder(Img)  
                                                    //var path = Path.Combine(imagepath, myfile);
                    string path = @"C:\inetpub\wwwroot\Data\SJB_Android\StorePictures\" + myfile;
                    //model.StorePicturesUrl = path;
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
    }
}