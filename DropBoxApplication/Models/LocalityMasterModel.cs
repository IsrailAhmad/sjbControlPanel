using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DropBoxApplication.Models
{

    public class LocalityMasterModel
    {
        public int LocalityId { get; set; }
        [Required(ErrorMessage = "Locality Name is required")]
        public string LocalityName { get; set; }
        [Required(ErrorMessage = "At Least One Store Name is required")]
        public string StoreName { get; set; }
        public int StoreId { get; set; }        
        public string StoreName1 { get; set; }
        public int StoreId1 { get; set; }        
        public string StoreName2 { get; set; }
        public int StoreId2 { get; set; }
        public string ImageUrl { get; set; }        
    }

    public class LocalityMasterModelRootObject
    {
        public List<LocalityMasterModel> data { get; set; }
        // public Response response { get; set; }
    }
    public class LocalityMasterModelSingleRootObject
    {
        public LocalityMasterModel data { get; set; }
        // public Response response { get; set; }
    }

}