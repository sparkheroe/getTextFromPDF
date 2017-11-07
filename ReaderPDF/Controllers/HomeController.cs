using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReaderPDF.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public void getTextFromPDF()
        {
            string docPath = @"C:\PDF_Reader\p09.pdf";
            var doc = PdfReader.Open(docPath);

            //string pageText = doc.Pages[0].Contents.Elements.GetDictionary(0).Stream.ToString();
            

            var streamWriter = new StreamWriter(@"C:\PDF_Reader\output.txt", false);

            String outputText = "";

            try
            {
                PdfDocument inputDocument = PdfReader.Open(docPath, PdfDocumentOpenMode.ReadOnly);

                foreach (PdfPage page in inputDocument.Pages)
                {
                    for (int index = 0; index < page.Contents.Elements.Count; index++)
                    {
                        PdfDictionary.PdfStream stream = page.Contents.Elements.GetDictionary(index).Stream;
                        outputText = new PDFTextExtractor().ExtractTextFromPDFBytes(stream.Value);

                        streamWriter.WriteLine(outputText);
                    }
                }

                //splits the result string on parts
                //gets something as 133 words..
                string[] words = outputText.Split(' ');

                string[] documentTypes = new string[]
                    {
                        "DNI",
                        "CE"
                    };

                List<string> listaOcurrencias = new List<string>();

                foreach (string s in documentTypes)
                {
                    foreach (string word in words)
                    {
                        if (word.StartsWith(s))
                        {
                            //Remove founded word from string 
                            string documentNumber = word.Replace(s, "");
                            listaOcurrencias.Add(documentNumber);

                            //Console.WriteLine(s);
                            return;
                        }
                    }                    
                }


            }
            catch (Exception e)
            {

            }
            streamWriter.Close();

        }


        public ActionResult ProcessPDF()
        {
            getTextFromPDF();
            return View();
        }

    }
}