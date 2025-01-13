using System.Text;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace ParsePdfApp.Utils
{
    public class Product
    {
        public int Code { get; set; }
        public int Quantity { get; set; }
        public float Discount { get; set; }
        public Product() { }
    }

    public class PdfParserPdfPig
    {
        public static string DecodePDF(string base64String)
        {
            byte[] pdfBytes = Convert.FromBase64String(base64String);

            StringBuilder textBuilder = new StringBuilder();

            using (MemoryStream pdfStream = new MemoryStream(pdfBytes))
            {
                using (PdfDocument pdfDocument = PdfDocument.Open(pdfStream))
                {
                    foreach (var page in pdfDocument.GetPages())
                    {
                        string pageText = ContentOrderTextExtractor.GetText(page);
                        textBuilder.AppendLine(pageText);
                    }
                }
            }

            return textBuilder.ToString();
        }

        public static string ParsePdf(string pdfPath)
        {
            PdfDocument pdf = PdfDocument.Open(pdfPath);

            StringBuilder textBuilder = new StringBuilder();

            foreach (var page in pdf.GetPages())
            {
                string pageText = ContentOrderTextExtractor.GetText(page);
                textBuilder.AppendLine(pageText);

            }

            return textBuilder.ToString();
        }

        public static List<Product> ParseProducts(string pdfText)
        {
            List<Product> products = new List<Product>();

            // Set lines list and skip first part
            List<string> lines = pdfText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Skip(23).ToList();

            // Skip Last part
            int poplatekIndex = -1;

            for (int i = lines.Count - 1; i >= 0; i--)
            {
                if (lines[i].Contains("Poplatek"))
                {
                    poplatekIndex = i;
                    break;
                }
            }

            if (poplatekIndex != -1)
            {
                lines.RemoveRange(poplatekIndex, lines.Count - poplatekIndex);
            }

            // Making correct format step 1
            List<string> formatedProductListFirst = new List<string>();

            for (int i = 0, j = 0; i < lines.Count; i++)
            {
                string currentLine = lines[i].Replace(" ", "");

                //if string contains Sleva
                if (currentLine.Contains("Sleva"))
                {
                    formatedProductListFirst[j - 1] += " " + lines[i];
                    continue;
                }

                // if has format
                if (Regex.IsMatch(currentLine, @"^\d+\d+,\d+[Kk][cč]\d+%\d+,\d+[Kk][cč]$")
                    || Regex.IsMatch(currentLine, @"^\d+,\d+kg\d+,\d+[Kk][cč]\d+%\d+,\d+[Kk][cč]"))
                {
                    formatedProductListFirst[j - 1] += " " + lines[i];
                    continue;
                }

                // if string contains only letters
                if (Regex.IsMatch(currentLine, @"^[\p{L}\s'.,%]+$"))
                {
                    formatedProductListFirst[j - 1] += " " + lines[i];
                }
                else
                {
                    formatedProductListFirst.Add(lines[i]);
                    j++;
                }
            }

            // Making correct format step 2
            List<string> formatedProductListSecond = new List<string>();

            for (int i = 0, j = 0; i < formatedProductListFirst.Count; i++)
            {
                string currentLine = formatedProductListFirst[i];

                // doesn't add if contains next
                if (currentLine.Contains("VĚRNOSTNÍ") || currentLine.Contains("DÁREK") || currentLine.Contains("-0,01"))
                {
                    continue;
                }

                if (Regex.IsMatch(currentLine, @"\b\d{8}\b"))
                {
                    formatedProductListSecond.Add(currentLine);
                    j++;
                }
                else
                {
                    formatedProductListSecond[j - 1] += " " + currentLine;
                }
            }

            // Parse and add finnaly into product List
            string noDiscountPattern = @"^(\d{8})(.+?)(\d+(?:,\d+)?)(\d+,\d+)Kč(\d+)%(\d+,\d+)Kč$";
            string discountPattern = @"^(\d{8})(.+?)(\d+(?:,\d+)?)(\d+,\d+)Kč(\d+)%(\d+,\d+)KčSleva(-\d+%)(-\d+,\d+)Kč$";

            foreach (string line in formatedProductListSecond)
            {
                string currentLine = line.Replace(" ", "");

                if (Regex.IsMatch(currentLine, noDiscountPattern))
                {
                    Match match = Regex.Match(currentLine, noDiscountPattern);

                    Product product = new Product
                    {
                        Code = Int32.Parse(match.Groups[1].Value),
                        Quantity = Int32.Parse(match.Groups[3].Value),
                        Discount = 0
                    };

                    products.Add(product);
                }
                else if (Regex.IsMatch(currentLine, discountPattern)) 
                {
                    Match match = Regex.Match(currentLine, discountPattern);

                    Product product = new Product
                    {
                        Code = Int32.Parse(match.Groups[1].Value),
                        Quantity = Int32.Parse(match.Groups[3].Value),
                        Discount = float.Parse(Regex.Match(match.Groups[7].Value, @"\d+").Value)
                    };

                    products.Add(product);
                }
                else
                {
                    Console.WriteLine("String doesn't have required pattern: " + line);
                }
            }

            return products;
        }
    }
}

