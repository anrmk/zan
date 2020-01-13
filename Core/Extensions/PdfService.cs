using System;
using System.Diagnostics;
using System.IO;
using System.Text;

using Microsoft.Extensions.Configuration;

namespace Core.Extensions {
    public interface IExportService {
        string Title { get; set; }
        string Template { get; set; }
        byte[] Convert(string html, bool intoTemplate = false, bool withHeader = false);
    }

    public class PdfService: IExportService {
        public string Title { get; set; }
        public string Template { get; set; } = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=UTF-8'></head><body>{0}</body></html>";
        public string PathExe;

        public PdfService()  {
            PathExe = "Wkhtmltopdf:PathExe";
        }

        public byte[] Convert(string html, bool intoTemplate = false, bool withHeader = false) {
            try {
                byte[] ret;
                string id = Guid.NewGuid().ToString();
                string inputFile = $"D:\\{id}.html";
                string outputFile = $"D:\\{id}.pdf";
                //string startupPath = Environment.CurrentDirectory + "\\custom_header.html";
                if(intoTemplate) {
                    html = string.Format(Template, html);
                }
                File.WriteAllText(inputFile, html, Encoding.UTF8);
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo {
                    Arguments = $" {inputFile.Replace("\\", "/")} {outputFile.Replace("\\", "/")}",
                    FileName = PathExe,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                p.Start();
                p.WaitForExit();
                ret = File.ReadAllBytes(outputFile);
                return ret;
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
