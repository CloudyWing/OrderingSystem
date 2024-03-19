using System.ComponentModel.DataAnnotations;

namespace CloudyWing.OrderingSystem.Web.Model.LoginModel {
    public class LoginViewModel {
        [Display(Name = "電子信箱")]
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "密碼")]
        [Required]
        public string? Password { get; set; }
    }
}
