using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CloudyWing.OrderingSystem.Web.Model.RegisterModel {
    public class RegisterViewModel : IValidatableObject {
        [Display(Name = "電子信箱")]
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "英文姓名")]
        [Required]
        [RegularExpression(@"[A-Za-z,.\s]+", ErrorMessage = "英文姓名 請輸入英文.,或空格。")]
        public string? Name { get; set; }

        [Display(Name = "密碼")]
        [Required]
        [StringLength(30, MinimumLength = 8)]
        public string? Password { get; set; }

        [Display(Name = "密碼確認")]
        [Required]
        [StringLength(30, MinimumLength = 8)]
        [Compare(nameof(Password))]
        public string? ComfirmedPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            if (!Regex.IsMatch(Password, @"[A-Z]+")) {
                yield return CreatePasswordValidationResult("密碼 至少一個大寫字母。");
            }

            if (!Regex.IsMatch(Password, @"[a-z]+")) {
                yield return CreatePasswordValidationResult("密碼 至少一個小寫字母。");
            }

            if (!Regex.IsMatch(Password, @"[0-9]+")) {
                yield return CreatePasswordValidationResult("密碼 至少一個數字。");
            }

            if (!Regex.IsMatch(Password, @"[!,#,$,^,*]+")) {
                yield return CreatePasswordValidationResult("密碼 至少一個特殊符號(!、#、$、^、*)。");
            }
        }

        private static ValidationResult CreatePasswordValidationResult(string message) {
            return new ValidationResult(message, new string[] { "Data." + nameof(Password) });
        }
    }
}
