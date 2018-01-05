using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DropBoxApplication.Models
{
    public class StoreMasterModel
    {
        public int StoreId { get; set; }
        [Required(ErrorMessage = "StoreName is required")]
        public string StoreName { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        [StringLength(10, ErrorMessage = "The Mobile must contains 10 characters", MinimumLength = 10)]
        public string StorePhoneNumber { get; set; }
        [Required(ErrorMessage = "Email Id is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
        ErrorMessage = "Please Enter Correct Email Address")]
        public string StoreEmailId { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string StoreAddress { get; set; }
        public int LocalityId { get; set; }
        public int LocalityName { get; set; }
        public bool FavouriteStore { get; set; }
        public string StorePicturesUrl { get; set; }
        [Required(ErrorMessage = "OpeningTime is required")]
        public object OpeningTime { get; set; }
        [Required(ErrorMessage = "ClosingTime is required")]
        public object ClosingTime { get; set; }
        public bool StoreStatus { get; set; }
        public int LoginId { get; set; }
    }

    public class StoreMasterModelRootObject
    {
        public List<StoreMasterModel> data { get; set; }
        // public Response response { get; set; }
    }

    public class StoreMasterModelSingleRootObject
    {
        public StoreMasterModel data { get; set; }
        // public Response response { get; set; }
    }
}