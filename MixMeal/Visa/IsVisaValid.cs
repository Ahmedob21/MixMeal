using MixMeal.Models;
using System.Text.RegularExpressions;

namespace MixMeal.Visa
{
    public class IsVisaValid
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public IsVisaValid(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {

            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        

        

    }
}
