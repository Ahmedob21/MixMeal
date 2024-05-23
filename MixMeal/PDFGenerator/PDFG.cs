using iTextSharp.text.pdf;
using iTextSharp.text;
using MixMeal.Models;

namespace MixMeal.PDFGenerator
{
    public class PDFG
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PDFG(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {

            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public string UpdateRecipePdf(Recipe recipe)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string pdfDirectory = Path.Combine(wwwRootPath, "PDF");

            if (!Directory.Exists(pdfDirectory))
            {
                Directory.CreateDirectory(pdfDirectory);
            }

            string fileName = $"Recipe.pdf";
            string path = Path.Combine(pdfDirectory, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                Document document = new Document();
                PdfWriter.GetInstance(document, stream);
                document.Open();

                // Title
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                document.Add(new Paragraph(recipe.Recipename, titleFont));

                // Subtitle
                var subTitleFont = FontFactory.GetFont(FontFactory.HELVETICA, 14);
                document.Add(new Paragraph($"Published on: {recipe.Publishdate.ToString("MMMM dd, yyyy")}", subTitleFont));
                document.Add(new Paragraph($"Price: ${recipe.Price}", subTitleFont));
                document.Add(new Paragraph($"Category: {recipe.Category?.Categoryname}", subTitleFont));
                document.Add(new Paragraph($"Chef: {recipe.Chef?.Firstname} {recipe.Chef?.Lastname}", subTitleFont));
                
                document.Add(new Paragraph("\n"));

                // Description
                var bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                document.Add(new Paragraph("Description:", bodyFont));
                document.Add(new Paragraph(recipe.Recipedescription, bodyFont));
                document.Add(new Paragraph("\n"));

                // Ingredients
                document.Add(new Paragraph("Ingredients:", bodyFont));
                document.Add(new Paragraph(recipe.Ingredientname, bodyFont));
                document.Add(new Paragraph("\n"));

                // Instructions
                document.Add(new Paragraph("Instructions:", bodyFont));
                document.Add(new Paragraph(recipe.Instructions, bodyFont));

                //// Image
                //if (!string.IsNullOrEmpty(recipe.Imagepath))
                //{
                //    try
                //    {
                //        var image = iTextSharp.text.Image.GetInstance(recipe.Imagepath);
                //        image.ScaleToFit(250f, 250f);
                //        image.Alignment = Element.ALIGN_CENTER;
                //        document.Add(image);
                //    }
                //    catch (Exception ex)
                //    {
                //        // Handle image loading exceptions
                //    }
                //}

                document.Close();
            }

            return path;
        }

    }
}
