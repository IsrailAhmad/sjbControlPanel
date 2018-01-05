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
    public class CategoryMasterController : BaseController
    {
        // GET: CategoryMaster
        public async Task<ActionResult> Index()
        {
            ViewBag.LoginID = Session["LoginID"].ToString();
            ViewBag.Username = Session["Username"].ToString();
            ViewBag.StoreId = Session["StoreId"].ToString();
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

            string murl = GetUrl(2);
            murl = murl + "MenuMaster/GetAllMenuList?StoreId=" + ViewBag.StoreId + "";
            MenuMasterModelRootObject mobj = new MenuMasterModelRootObject();
            //List<MenuMasterModel> mlist = new List<MenuMasterModel>();
            using (HttpClient cclient = new HttpClient())
            {
                HttpResponseMessage responseMessage = await cclient.GetAsync(murl);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var result = responseMessage.Content.ReadAsStringAsync().Result;
                    mobj = JsonConvert.DeserializeObject<MenuMasterModelRootObject>(result);
                    IList<SelectListItem> mSelectList = new List<SelectListItem>();
                    foreach (var item in mobj.data)
                    {
                        mSelectList.Add(new SelectListItem { Text = item.MenuName, Value = item.MenuId.ToString() });
                    }
                    mSelectList.Insert(0, new SelectListItem() { Value = "", Text = "Select Menu" });
                    // mSelectList.Insert(1, new SelectListItem() { Value = "0", Text = "All" });
                    ViewBag.MenuList = mSelectList;
                }
            }
            CategoryMasterModel category = (CategoryMasterModel)TempData["item"];
            return View(category);
        }

        [HttpPost]
        public async Task<ActionResult> AddNewCategory(FormCollection fc, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                ViewBag.StoreId = Session["StoreId"].ToString();
                int categoryid = 0;
                string storeid = "";
                CategoryMasterModel model = new CategoryMasterModel();
                string CategoryId = fc["CategoryId"];
                string categoryname = fc["CategoryName"];
                string menuid = fc["MenuName"];
                if (ViewBag.StoreId == "0")
                {
                    storeid = fc["StoreName"];
                }
                else
                {
                    storeid = ViewBag.StoreId;
                }
                
                string description = fc["CategoryDescription"];
                string url = GetUrl(2);
                url = url + "Category/AddNewCategory?CategoryId=" + CategoryId + "&CategoryName=" + categoryname + "&menuid=" + menuid + "&storeid=" + storeid + "&CategoryDescription=" + description + "";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage responseMessage = await client.GetAsync(url);
                    CategoryMasterModelSingleRootObject result = new CategoryMasterModelSingleRootObject();
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var response = responseMessage.Content.ReadAsStringAsync().Result;
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        result = JsonConvert.DeserializeObject<CategoryMasterModelSingleRootObject>(response);
                        if (result.data.CategoryId == 0)
                        {
                            categoryid = Convert.ToInt32(CategoryId);
                        }
                        else
                        {
                            categoryid = result.data.CategoryId;
                        }
                        
                        if (categoryid > 0)
                        {
                            try
                            {
                                var allowedExtensions = new[]
                                {
                                 ".Jpg", ".png", ".jpg", "jpeg",".JPG",
                            };
                                //string imagepath = "http://103.233.79.234/Data/SJB_Android/CategoryPictures/";
                                model.CategoryPictures = file.ToString(); //getting complete url                         
                                var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                                var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                                if (allowedExtensions.Contains(ext)) //check what type of extension  
                                {
                                    string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                                    string myfile = + categoryid + ext; //appending the name with id  
                                                                       // store the file inside ~/project folder(Img)  
                                                                       //var path = Path.Combine(imagepath, myfile);
                                    string path = @"C:\inetpub\wwwroot\Data\SJB_Android\CategoryPictures\" + Server.HtmlEncode(myfile);
                                    model.CategoryPictures = path;
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
        public async Task<ActionResult> FindMenuListByStore(string StoreId)
        {

            string strurl = GetUrl(2);
            strurl = strurl + "MenuMaster/GetAllCategoryByStoreIdForWeb?StoreId=" + StoreId;
            CategoryMasterModelRootObject mobj = new CategoryMasterModelRootObject();
            IList<SelectListItem> mSelectList = new List<SelectListItem>();
            using (HttpClient cclient = new HttpClient())
            {
                HttpResponseMessage responseMessage = await cclient.GetAsync(strurl);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var result = responseMessage.Content.ReadAsStringAsync().Result;
                    mobj = JsonConvert.DeserializeObject<CategoryMasterModelRootObject>(result);
                    foreach (var item in mobj.data)
                    {
                        mSelectList.Add(new SelectListItem { Text = item.MenuName, Value = item.MenuId.ToString() });
                    }
                    //mSelectList.Insert(0, new SelectListItem() { Value = "", Text = "Select Menu Name" });
                    //mSelectList.Insert(1, new SelectListItem() { Value = "0", Text = "All" });
                    ViewBag.MenuList = mSelectList;
                }
            }
            return new JsonResult { Data = mSelectList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //return View(new CategoryMasterModel());
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCategoryList()
        {
            List<CategoryMasterModel> olist = new List<CategoryMasterModel>();
            ViewBag.StoreId = Session["StoreId"].ToString();
            string url = GetUrl(2);
            url = url + "Category/GetAllCategoryList?StoreId=" + ViewBag.StoreId + "";

            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage responseMessage = await client.GetAsync(url);
                CategoryMasterModelRootObject result = new CategoryMasterModelRootObject();
                if (responseMessage.IsSuccessStatusCode)
                {
                    var response = responseMessage.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    result = JsonConvert.DeserializeObject<CategoryMasterModelRootObject>(response);
                    olist = result.data;
                    ViewBag.LocalityList = olist;
                }
            }
            return PartialView("_Categorylist", olist);
        }

        [HttpGet]
        public async Task<ActionResult> RemoveCategory(int id)
        {
            //string strint = id.Trim().ToString();
            //var intid = Convert.ToInt32(strint);
            List<CategoryMasterModel> olist = new List<CategoryMasterModel>();

            CategoryMasterModelRootObject obj = new CategoryMasterModelRootObject();

            string url = GetUrl(2);
            url = url + "Category/RemoveCategory?id=" + Convert.ToInt32(id) + "";
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
                    obj = JsonConvert.DeserializeObject<CategoryMasterModelRootObject>(response, settings);
                    olist = obj.data;
                    ViewBag.TransactionList = olist;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> GetCategoryById(int id)
        {
            //string strint = id.Trim().ToString();
            //var intid = Convert.ToInt32(strint);
            CategoryMasterModel pro = new CategoryMasterModel();

            CategoryMasterModelSingleRootObject obj = new CategoryMasterModelSingleRootObject();

            string url = GetUrl(2);
            url = url + "Category/GetCategoryById?id=" + Convert.ToInt32(id) + "";
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
                    obj = JsonConvert.DeserializeObject<CategoryMasterModelSingleRootObject>(response, settings);
                    pro = obj.data;
                    TempData["item"] = pro;
                }
            }
            return RedirectToAction("Index");
        }



    }
}