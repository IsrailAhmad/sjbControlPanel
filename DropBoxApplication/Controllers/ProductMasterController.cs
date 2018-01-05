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
    public class ProductMasterController : BaseController
    {
        // GET: ProductMaster
        public async Task<ActionResult> Index()
        {
            ViewBag.LoginID = Session["LoginID"].ToString();
            ViewBag.Username = Session["Username"].ToString();
            ViewBag.StoreId = Session["StoreId"].ToString();
            ViewBag.Message = "Your application Daily Activity page.";


            //string url = GetUrl(2);
            //url = url + "Localities/GetAllLocalitiesWithOutPagging";
            //LocalityMasterModelRootObject obj = new LocalityMasterModelRootObject();
            //using (HttpClient client = new HttpClient())
            //{
            //    HttpResponseMessage responseMessage = await client.GetAsync(url);
            //    if (responseMessage.IsSuccessStatusCode)
            //    {
            //        var result = responseMessage.Content.ReadAsStringAsync().Result;
            //        obj = JsonConvert.DeserializeObject<LocalityMasterModelRootObject>(result);

            //        IList<SelectListItem> LSelectList = new List<SelectListItem>();
            //        foreach (var item in obj.data)
            //        {
            //            LSelectList.Add(new SelectListItem { Text = item.LocalityName, Value = item.LocalityId.ToString() });
            //        }
            //        LSelectList.Insert(0, new SelectListItem() { Value = "", Text = "Select Locality" });

            //        //LSelectList.Insert(1, new SelectListItem() { Value = "0", Text = "All" });

            //        ViewBag.LocalityList = LSelectList;
            //    }
            //}

            //string preurl = GetUrl(2);
            //preurl = preurl + "Store/GetAllStoreList";
            //StoreMasterModelRootObject pobj = new StoreMasterModelRootObject();
            ////List<ProductMasterModel> Prlist = new List<ProductMasterModel>();
            //using (HttpClient prclient = new HttpClient())
            //{
            //    HttpResponseMessage responseMessage = await prclient.GetAsync(preurl);
            //    if (responseMessage.IsSuccessStatusCode)
            //    {
            //        var result = responseMessage.Content.ReadAsStringAsync().Result;
            //        pobj = JsonConvert.DeserializeObject<StoreMasterModelRootObject>(result);
            //        IList<SelectListItem> ProSelectList = new List<SelectListItem>();
            //        foreach (var item in pobj.data)
            //        {
            //            ProSelectList.Add(new SelectListItem { Text = item.StoreName, Value = item.StoreId.ToString() });
            //        }
            //        ProSelectList.Insert(0, new SelectListItem() { Value = "", Text = "Select Store" });
            //        //ProSelectList.Insert(1, new SelectListItem() { Value = "0", Text = "All" });
            //        ViewBag.StoreList = ProSelectList;
            //    }
            //}

            //string murl = GetUrl(2);
            //murl = murl + "MenuMaster/GetAllMenuList";
            //MenuMasterModelRootObject mobj = new MenuMasterModelRootObject();
            ////List<MenuMasterModel> mlist = new List<MenuMasterModel>();
            //using (HttpClient cclient = new HttpClient())
            //{
            //    HttpResponseMessage responseMessage = await cclient.GetAsync(murl);
            //    if (responseMessage.IsSuccessStatusCode)
            //    {
            //        var result = responseMessage.Content.ReadAsStringAsync().Result;
            //        mobj = JsonConvert.DeserializeObject<MenuMasterModelRootObject>(result);
            //        IList<SelectListItem> mSelectList = new List<SelectListItem>();
            //        foreach (var item in mobj.data)
            //        {
            //            mSelectList.Add(new SelectListItem { Text = item.MenuName, Value = item.MenuId.ToString() });
            //        }
            //        mSelectList.Insert(0, new SelectListItem() { Value = "", Text = "Select Menu" });
            //        // mSelectList.Insert(1, new SelectListItem() { Value = "0", Text = "All" });
            //        ViewBag.MenuList = mSelectList;
            //    }
            //}

            string Clienturl = GetUrl(2);
            Clienturl = Clienturl + "Category/GetAllCategoryList?StoreId=" + ViewBag.StoreId + "";
            CategoryMasterModelRootObject cobj = new CategoryMasterModelRootObject();
            //List<CategoryMasterModel> clist = new List<CategoryMasterModel>();
            using (HttpClient cclient = new HttpClient())
            {
                HttpResponseMessage responseMessage = await cclient.GetAsync(Clienturl);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var result = responseMessage.Content.ReadAsStringAsync().Result;
                    cobj = JsonConvert.DeserializeObject<CategoryMasterModelRootObject>(result);
                    IList<SelectListItem> CSelectList = new List<SelectListItem>();
                    foreach (var item in cobj.data)
                    {
                        CSelectList.Add(new SelectListItem { Text = item.CategoryName, Value = item.CategoryId.ToString() });
                    }
                    CSelectList.Insert(0, new SelectListItem() { Value = "", Text = "Select Category" });
                    // CSelectList.Insert(1, new SelectListItem() { Value = "0", Text = "All" });
                    ViewBag.CategoryList = CSelectList;
                }
            }
            //string preurl = GetUrl(2);
            //preurl = preurl + "ProductMaster/GetAllProductList";
            //ProductMasterModelRootObject pobj = new ProductMasterModelRootObject();
            ////List<ProductMasterModel> Prlist = new List<ProductMasterModel>();
            //using (HttpClient prclient = new HttpClient())
            //{
            //    HttpResponseMessage responseMessage = await prclient.GetAsync(preurl);
            //    if (responseMessage.IsSuccessStatusCode)
            //    {
            //        var result = responseMessage.Content.ReadAsStringAsync().Result;
            //        pobj = JsonConvert.DeserializeObject<ProductMasterModelRootObject>(result);
            //        IList<SelectListItem> ProSelectList = new List<SelectListItem>();
            //        foreach (var item in pobj.data)
            //        {
            //            ProSelectList.Add(new SelectListItem { Text = item.ProductName, Value = item.ProductId.ToString() });
            //        }
            //        ProSelectList.Insert(0, new SelectListItem() { Value = "", Text = "Select Product" });
            //        ProSelectList.Insert(1, new SelectListItem() { Value = "0", Text = "All" });
            //        ViewBag.ProductList = ProSelectList;
            //    }
            //}
            ProductMasterModel product = (ProductMasterModel)TempData["item"];
            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> AddNewProduct(FormCollection fc, HttpPostedFileBase file)
        {
            //if (ModelState.IsValid)
            //{
            int ProductId = 0;
            ProductMasterModel model = new ProductMasterModel();
            string productId = fc["ProductId"];
            string CategoryName = fc["CategoryName"];
            string ProductName = fc["ProductName"];
            string UnitPrice = fc["UnitPrice"];
            string GST = fc["GST"];
            string Discount = fc["Discount"];
            string TaxType = fc["TaxType"];
            bool Lock = Convert.ToBoolean(fc["Lock"].Split(',')[0]); ;
            string UOM = fc["UOM"];
            string ProductDetails = fc["ProductDetails"];
            string DeliveryCharge = fc["DeliveryCharge"];

            //string ProductId = Convert.ToString(model.ProductId);
            //string ProductDetails = model.ProductDetails;
            //string CategoryName = model.CategoryName;
            //string ProductName = model.ProductName;
            //string UnitPrice = Convert.ToString(model.UnitPrice);
            //string GST = Convert.ToString(model.GST);
            //string Discount = Convert.ToString(model.Discount);
            //string TaxType = Convert.ToString(model.TaxType);
            ////bool Lock = Convert.ToBoolean(fc["Lock"].Split(',')[0]); ;
            //bool Lock = model.Lock;
            //string UOM = Convert.ToString(model.UOM);
            ////string ProductDetails = fc["ProductDetails"];
            string url = GetUrl(2);
            url = url + "Product/AddNewProduct?ProductId=" + productId + "&CategoryName=" + CategoryName + "&ProductName=" + ProductName + "&UnitPrice=" + UnitPrice + "&GST=" + GST + "&Discount=" + Discount + "&TaxType=" + TaxType + "&Lock=" + Lock + "&UOM=" + UOM + "&ProductDetails=" + ProductDetails + "&DeliveryCharge=" + DeliveryCharge + "";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                ProductMasterModelSingleRootObject result = new ProductMasterModelSingleRootObject();
                if (responseMessage.IsSuccessStatusCode)
                {
                    var response = responseMessage.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    result = JsonConvert.DeserializeObject<ProductMasterModelSingleRootObject>(response, settings);
                    if (result.data.ProductId == 0)
                    {
                        ProductId = Convert.ToInt32(productId);
                    }
                    else
                    {
                        ProductId = result.data.ProductId;
                    }
                    if (ProductId > 0)
                    {
                        try
                        {
                            var allowedExtensions = new[]
                            {
                                 ".Jpg", ".png", ".jpg", "jpeg",".JPG",
                            };
                            //string imagepath = "http://103.233.79.234/Data/SJB_Android/LocalityPictures/";
                            model.ProductPicturesUrl = file.ToString(); //getting complete url                         
                            var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                            var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                            if (allowedExtensions.Contains(ext)) //check what type of extension  
                            {
                                string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                                string myfile = +ProductId + ext; //appending the name with id  
                                                                  // store the file inside ~/project folder(Img)  
                                                                  //var path = Path.Combine(imagepath, myfile);
                                string path = @"C:\inetpub\wwwroot\Data\SJB_Android\ProductPictures\" + Server.HtmlEncode(myfile);
                                model.ProductPicturesUrl = path;
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
            //}
            //else
            //{
            //    return RedirectToAction("Index");
            //}
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProductList()
        {
            List<ProductMasterModel> olist = new List<ProductMasterModel>();
            ViewBag.StoreId = Session["StoreId"].ToString();
            string url = GetUrl(2);
            url = url + "Product/GetAllProductList?StoreId=" + ViewBag.StoreId + "";

            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage responseMessage = await client.GetAsync(url);
                ProductMasterModelRootObject result = new ProductMasterModelRootObject();
                if (responseMessage.IsSuccessStatusCode)
                {
                    var response = responseMessage.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    result = JsonConvert.DeserializeObject<ProductMasterModelRootObject>(response);
                    olist = result.data;
                    ViewBag.LocalityList = olist;
                }
            }
            return PartialView("_Productlist", olist);
        }

        [HttpGet]
        public async Task<ActionResult> RemoveProduct(string id)
        {
            //string strint = id.Trim().ToString();
            var intid = Convert.ToInt32(id);
            List<ProductMasterModel> olist = new List<ProductMasterModel>();

            ProductMasterModelRootObject obj = new ProductMasterModelRootObject();

            string url = GetUrl(2);
            url = url + "Product/RemoveProduct?id=" + Convert.ToInt32(id) + "";
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
                    obj = JsonConvert.DeserializeObject<ProductMasterModelRootObject>(response, settings);
                    olist = obj.data;
                    ViewBag.TransactionList = olist;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> GetProductById(string id)
        {
            //string strint = id.Trim().ToString();
            var intid = Convert.ToInt32(id);
            ProductMasterModel pro = new ProductMasterModel();

            ProductMasterModelSingleRootObject obj = new ProductMasterModelSingleRootObject();

            string url = GetUrl(2);
            url = url + "Product/GetProductById?id=" + Convert.ToInt32(intid) + "";
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
                    obj = JsonConvert.DeserializeObject<ProductMasterModelSingleRootObject>(response, settings);
                    pro = obj.data;
                    TempData["item"] = pro;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> ProductLockOn(string strProductId)
        {
            int ProductId = Convert.ToInt32(strProductId);
            List<ProductMasterModel> olist = new List<ProductMasterModel>();
            ProductMasterModelRootObject obj = new ProductMasterModelRootObject();

            string url = GetUrl(2);
            url = url + "Product/ProductLockOn?id=" + Convert.ToInt32(ProductId) + "";
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
                    obj = JsonConvert.DeserializeObject<ProductMasterModelRootObject>(response, settings);
                    olist = obj.data;
                    ViewBag.TransactionList = olist;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> ProductLockOff(string strProductId)
        {
            int ProductId = Convert.ToInt32(strProductId);
            List<ProductMasterModel> olist = new List<ProductMasterModel>();
            ProductMasterModelRootObject obj = new ProductMasterModelRootObject();

            string url = GetUrl(2);
            url = url + "Product/ProductLockOff?id=" + Convert.ToInt32(ProductId) + "";
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
                    obj = JsonConvert.DeserializeObject<ProductMasterModelRootObject>(response, settings);
                    olist = obj.data;
                    ViewBag.TransactionList = olist;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> GetAllOutOfStockProductList()
        {
            List<ProductMasterModel> olist = new List<ProductMasterModel>();
            ViewBag.StoreId = Session["StoreId"].ToString();
            string url = GetUrl(2);
            url = url + "Product/GetAllOutOfStockProductList?StoreId=" + ViewBag.StoreId + "";

            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage responseMessage = await client.GetAsync(url);
                ProductMasterModelRootObject result = new ProductMasterModelRootObject();
                if (responseMessage.IsSuccessStatusCode)
                {
                    var response = responseMessage.Content.ReadAsStringAsync().Result;
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    result = JsonConvert.DeserializeObject<ProductMasterModelRootObject>(response);
                    olist = result.data;
                    ViewBag.LocalityList = olist;
                }
            }
            return PartialView("_OutOfStockProductList", olist);
        }

    }
}