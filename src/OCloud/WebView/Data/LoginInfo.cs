using System.ComponentModel.DataAnnotations;


namespace OCloud.WebView.Data
{
    public class LoginInfo
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        //[Required, StringLength(100)]
        public string Password { get; set; }

        //[Display(Name = "Remember me")]
        //public bool RememberMe { get; set; }

        //public string ReturnUrl { get; set; }
    }
}
