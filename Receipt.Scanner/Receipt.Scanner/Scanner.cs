using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace Receipt.Scanner
{
    public static class Scanner
    {
        public static string Scan(Stream imageStream)
        {
            string resultText;
            var uri = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "tessdata");

            bool result = Directory.Exists(uri.ToString());
            using (var engine = new TesseractEngine(uri.ToString(), "eng", EngineMode.Default))
            {
                // have to load Pix via a bitmap since Pix doesn't support loading a stream.
                using (var image = new System.Drawing.Bitmap(imageStream))
                {
                    using (var pix = PixConverter.ToPix(image))
                    {
                        using (var page = engine.Process(pix))
                        {
                            //finalLabel = String.Format("{0:P}", page.GetMeanConfidence());
                            resultText = page.GetText();
                        }
                    }
                }
            }

            return resultText;
        }
    }
}
