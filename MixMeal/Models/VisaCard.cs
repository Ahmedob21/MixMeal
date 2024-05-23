using System.ComponentModel.DataAnnotations;

namespace MixMeal.Models
{
    public class VisaCard
    {
        [Required(ErrorMessage = "Cardholder's name is required.")]
        [StringLength(50, ErrorMessage = "Cardholder's name cannot exceed 50 characters.")]
        [Display(Name = "Cardholder's Name")]
        public string Cardname { get; set; } = null!;
        
        
        
        [Required(ErrorMessage = "Card number is required.")]
        [StringLength(16, ErrorMessage = "Card number must be 16 digits.")]
        [CreditCard(ErrorMessage = "Card number is not a valid credit card number.")]
        public string Cardnumber { get; set; } = null!;


        [Required(ErrorMessage = "CVV is required.")]
        [StringLength(3, ErrorMessage = "CVV must be 3 digits.", MinimumLength = 3)]
        [Display(Name = "CVV")]
        public string Cvv { get; set; } = null!;



        [Required(ErrorMessage = "Expiration date is required.")]
        [Display(Name = "Expiration Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM}")]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Expiration date must be in the future.")]
        public DateTime Expiredate { get; set; }
    }


    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date > DateTime.Now)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Expiration date must be in the future.");
                }
            }
            else
            {
                return new ValidationResult("Invalid date format.");
            }
        }
    }

}
