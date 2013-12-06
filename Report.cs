using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Vbe.Interop;
using Word = Microsoft.Office.Interop.Word;

namespace IPLab
{
    class Report
    {
        private List<string> detailList = new List<string>(); 

        /// <summary>
        /// generate the final report usinf metadata in the metadata file.
        /// </summary>
        public void generateReport()
        {
            try
            {
                Process[] pro = Process.GetProcessesByName("WINWORD");
                foreach (Process process in pro)
                {
                    process.Kill();
                }
            }
            catch (Exception)
            {

            }

            File.Copy(
                Properties.Resources.workingPath + @"Template.docx",
                Properties.Resources.workingPath + @"Document Verification Results.docx", true);

            //if (File.Exists(@"E:\invoice.pdf"))
            //{
            //    File.Delete(@"E:\invoice.pdf");
            //}
            object fileName = Properties.Resources.workingPath + @"Document Verification Results.docx";

            //Set Missing Value parameter - used to represent
            // a missing value when calling methods through
            // interop.
            object missing = System.Reflection.Missing.Value;

            //Setup the Word.Application class.
            Word.Application wordApp =
                new Word.Application();

            //Setup our Word.Document class we'll use.
            Word.Document aDoc = null;

            List<string> metadata = getResultMetadataList();

            // Check to see that file exists
            if (File.Exists((string)fileName))
            {
                object readOnly = false;
                object isVisible = false;
                object nothin = 1;

                //Set Word to be not visible.
                wordApp.Visible = false;

                //Open the word document
                aDoc = wordApp.Documents.Open(ref fileName, ref missing,
                    ref readOnly, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref isVisible, ref missing, ref missing,
                    ref missing, ref missing);

                // Activate the document
                aDoc.Activate();


                FindAndReplace(wordApp, "<dithering1>", metadata[0]);
                FindAndReplace(wordApp, "<dithering2>", metadata[1]);
                FindAndReplace(wordApp, "<dithering3>", metadata[2]);
                FindAndReplace(wordApp, "<dithering4>", metadata[3]);
                FindAndReplace(wordApp, "<edge1>", metadata[4]);
                FindAndReplace(wordApp, "<Checkerboard1>", metadata[5]);
                FindAndReplace(wordApp, "<Checkerboard2>", metadata[6]);
                FindAndReplace(wordApp, "<Checkerboard3>", metadata[7]);
                FindAndReplace(wordApp, "<Checkerboard4>", metadata[8]);
                FindAndReplace(wordApp, "<result>",
                    "Overall analysis says that the signature is " + finalPrecentage() + "% fogery.");

                Bitmap image = new Bitmap(metadata[9]);
                int width, height;
                double temp;
                width = (int)getWidthAndHeight(image, out temp);
                height = (int)temp;

                aDoc.Shapes.AddPicture(metadata[9], ref missing, ref missing,
                    ref missing, 100, width, height, ref missing);

                //worker.ReportProgress(60);



                //Example of writing to the start of a document.
                //aDoc.Content.InsertBefore("This is at the beginning\r\n\r\n" + invoiceNo);

                //Example of writing to the end of a document.
                //aDoc.Content.InsertAfter("\r\n\r\nThis is at the end" + retailerCode);
            }
            else
            {
                MessageBox.Show("File dose not exist.");
                return;
            }
            object saveAs = Properties.Resources.workingPath + @"Final_Result-" + Path.GetFileName(metadata[9]) + ".pdf";

            //Save the document as the correct file name.
            aDoc.SaveAs(ref saveAs, Word.WdSaveFormat.wdFormatPDF, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing);


            PrintDialog pDialog = new PrintDialog();
            wordApp.ActivePrinter = pDialog.PrinterSettings.PrinterName;

            //Close the document - you have to do this.
            aDoc.Close(ref missing, ref missing, ref missing);

            //worker.ReportProgress(95);

            System.Diagnostics.Process.Start(Properties.Resources.workingPath + @"Final_Result-" + Path.GetFileName(metadata[9]) + ".pdf");

        }

        /// <summary>
        /// calculate the final result precentage.
        /// </summary>
        /// <returns>fnal result</returns>
        private double finalPrecentage()
        {
            double dithering = double.Parse(detailList[3]),
                edge = double.Parse(detailList[4]),
                check = double.Parse(detailList[8]);

            if (edge<50)
            {
                return ((dithering*2) + edge + check)/4;
            }
            return ((edge*2) + dithering + check)/4;
        }
        
        /// <summary>
        /// find a string in a wordfile and then replace it with another string
        /// </summary>
        /// <param name="WordApp">word apllication to be procces with</param>
        /// <param name="findText">string that is to be replace</param>
        /// <param name="replaceWithText">new string</param>
        private void FindAndReplace(Word.Application WordApp, object findText, object replaceWithText)
        {
            object matchCase = true;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundsLike = false;
            object nmatchAllWordForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiacritics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;

            WordApp.Selection.Find.Execute(ref findText,
                ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundsLike,
                ref nmatchAllWordForms, ref forward,
                ref wrap, ref format, ref replaceWithText,
                ref replace, ref matchKashida,
                ref matchDiacritics, ref matchAlefHamza,
                ref matchControl);

        }

        /// <summary>
        /// get the result metadata drom metadata file.
        /// </summary>
        /// <returns>details on metadata file</returns>
        private List<string> getResultMetadataList()
        {
            if (File.Exists(Properties.Resources.workingPath + @"MetadataText.txt"))
            {
                List<string> metadataList = new List<string>();

                string[] lines = File.ReadAllLines(Properties.Resources.workingPath + @"MetadataText.txt");
                foreach (string line in lines)
                {
                    metadataList.Add(line);
                }
                File.Delete(Properties.Resources.workingPath + @"MetadataText.txt");
                File.Create(Properties.Resources.workingPath + @"MetadataText.txt");
                this.detailList = metadataList;
                return metadataList;
            }
            else
            {
                MessageBox.Show("Metadata file cannot be found");
                return null;
            }
        }
        
        /// <summary>
        /// get the maximum width and height of the image, so the image can fit in to the word file.
        /// </summary>
        /// <param name="image">image to be put to the word file</param>
        /// <param name="height">height of the image.</param>
        /// <returns>width of the image</returns>
        private double getWidthAndHeight(Bitmap image, out double height)
        {
            double width = image.Width;
            height = image.Height;

            if ((width < 470 && height < 200) || (width > 470 && height > 200))
            {
                double ratio1 = 470.0 / width;
                double ratio2 = 200.0 / height;

                if (ratio1 < ratio2)
                {
                    width = width * ratio1;
                    height = height * ratio1;
                }
                else
                {
                    width = width * ratio2;
                    height = height * ratio2;
                }
            }
            else if (width < 470 && height > 200)
            {
                double ratio = 200 / height;
                width = width * ratio;
                height = height * ratio;
            }
            else if (width > 470 && height < 200)
            {
                double ratio = 470 / width;
                width = width * ratio;
                height = height * ratio;
            }

            return width;
        }

    }
}
