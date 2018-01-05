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
    public class LocalityMasterController : BaseController
    {
        // GET: LocalityMaster
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
            LocalityMasterModel locality = (LocalityMasterModel)TempData["item"];
            return View(locality);
        }

        [HttpPost]
        public async Task<ActionResult> AddNewLocalityName(FormCollection fc, HttpPostedFileBase file)
        {
            //List<LocalityMasterModel> olist = new List<LocalityMasterModel>();
            if (ModelState.IsValid)
            {
                int localityId = 0;
                LocalityMasterModel model = new LocalityMasterModel();
                string LocalityId = fc["LocalityId"];
                string localityname = fc["LocalityName"];
                string storeid = fc["StoreName"];
                string storeid1 = fc["StoreName1"];
                string storeid2 = fc["StoreName2"];
                string url = GetUrl(2);
                url = url + "Localities/AddNewLocalityName?LocalityId=" + LocalityId + "&localityname=" + localityname + "&storeid=" + storeid + "&storeid1=" + storeid1 + "&storeid2=" + storeid2 + "";

                using (HttpClient client = new HttpClient())
                {

                    HttpResponseMessage responseMessage = await client.GetAsync(url);
                    LocalityMasterModelSingleRootObject result = new LocalityMasterModelSingleRootObject();
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var response = responseMessage.Content.ReadAsStringAsync().Result;
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        result = JsonConvert.DeserializeObject<LocalityMasterModelSingleRootObject>(response);
                        if (result.data.LocalityId == 0)
                        {
                            localityId = Convert.ToInt32(LocalityId);
                        }
                        else
                        {
                            localityId = result.data.LocalityId;
                        }
                        try
                        {
                            if (localityId > 0)
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
                                    string myfile = + localityId + ext; //appending the name with id  
                                                                       // store the file inside ~/project folder(Img)  
                                                                       //var path = Path.Combine(imagepath, myfile);
                                    string path = @"C:\inetpub\wwwroot\Data\SJB_Android\LocalityPictures\" + Server.HtmlEncode(myfile);
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
                            else
                            {
                                ViewBag.message = "Please choose only Image file";
                            }
                        }
                        catch (Exception ex)
                        {

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
        public async Task<ActionResult> GetAllLocalitiesdata()
        {
            List<LocalityMasterModel> olist = new List<LocalityMasterModel>();

            string url = GetUrl(2);
            url = url + "Localities/GetAllLocalitiesWithOutPagging";

            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage responseMessage = await client.GetAsync(url);
                LocalityMasterModelRootObject result = new LocalityMasterModelRootObject();
                if (responseMessage.IsSuccessStatusCode)
                {
                    var response = responseMessage.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    result = JsonConvert.DeserializeObject<LocalityMasterModelRootObject>(response);
                    olist = result.data;
                    ViewBag.LocalityList = olist;
                }
            }
            return PartialView("_Localitieslist", olist);
        }

        [HttpGet]
        public async Task<ActionResult> RemoveLocality(int id)
        {
            //string strint = id.Trim().ToString();
            //var intid = Convert.ToInt32(strint);
            List<LocalityMasterModel> olist = new List<LocalityMasterModel>();

            LocalityMasterModelRootObject obj = new LocalityMasterModelRootObject();

            string url = GetUrl(2);
            url = url + "Localities/RemoveLocality?id=" + Convert.ToInt32(id) + "";
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
                    obj = JsonConvert.DeserializeObject<LocalityMasterModelRootObject>(response, settings);
                    olist = obj.data;
                    ViewBag.TransactionList = olist;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> GetLocalityById(int id)
        {
            //string strint = id.Trim().ToString();
            //var intid = Convert.ToInt32(strint);
            LocalityMasterModel pro = new LocalityMasterModel();

            LocalityMasterModelSingleRootObject obj = new LocalityMasterModelSingleRootObject();

            string url = GetUrl(2);
            url = url + "Localities/GetLocalityById?id=" + Convert.ToInt32(id) + "";
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
                    obj = JsonConvert.DeserializeObject<LocalityMasterModelSingleRootObject>(response, settings);
                    pro = obj.data;
                    TempData["item"] = pro;
                }
            }
            return RedirectToAction("Index");
        }


    }
}