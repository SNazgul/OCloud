using System.ComponentModel.DataAnnotations;


namespace OCloud.WebView.Data
{
    public class LoginInfo
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        //[Required, StringLength(100)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Login after creation")]
        public bool LoginAfterCreation { get; set; }

        //[Display(Name = "Remember me")]
        //public bool RememberMe { get; set; }

        //public string ReturnUrl { get; set; }
    }
}
