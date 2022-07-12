using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Evolution.Common.Constants;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using HtmlToOpenXml;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace Evolution.Common.Helpers
{
    public static class Utility
    {
        public static bool DeleteFile(string filePath, bool throwError = true)
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                if (throwError)
                    throw ex;
                else
                    return false;
            }

            return true;
        }

        public static bool CheckAndCreateFolder(string folderPath, bool throwError = true)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
            }
            catch (Exception ex)
            {
                if (throwError)
                    throw ex;
                else
                    return false;
            }
            return true;
        }

        public static byte[] GetByteFileData(string filePath)
        {
            byte[] byteFileData = null;

            if (File.Exists(filePath))
            {
                byteFileData = File.ReadAllBytes(filePath);
                using (StreamReader sr = new StreamReader(filePath))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        sr.BaseStream.CopyTo(ms);
                        byteFileData = ms.ToArray();
                    }
                }
            }
            return byteFileData;
        }

        public static T ConvertFromObject<T>(object model) where T : class
        {
            T result = null;
            try
            {
                result = (T)model;
            }
            catch (InvalidCastException ex)
            {
                throw ex;
            }
            return result;
        }

        public static string GetMimeTypes(string extensionName)
        {
            var mimeTypes = new Dictionary<string, string>
            {
               {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".msg", "text/plain"},
                {".zip", "application/zip"},
                {".rar", "application/x-rar-compressed"},
                {".mp4", "video/mp4"},
                {".mp3", "video/mp3"},
                {".avi", "video/avi"},
                {".mvi", "video/mvi"},
                {".wmv", "video/wmv"},
                {".mpeg", "video/mpeg"},
                {".html", "text/html"},
                {".htm", "text/html"},
                {".bmp", "image/bmp"} 
            };

            return mimeTypes.FirstOrDefault(x => x.Key.ToLower() == extensionName.ToLower()).Value;
        }

        public static Encoding GetEncoding(MultipartSection section)
        {
            MediaTypeHeaderValue mediaType;
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out mediaType);
            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
                return Encoding.UTF8;
            else
                return mediaType.Encoding;
        }

        public static RecordStatus GetRecordStatus(ValidationType type)
        {
            RecordStatus status = RecordStatus.Unknown;
            if (type == ValidationType.Add)
                status = RecordStatus.New;
            else if (type == ValidationType.Update)
                status = RecordStatus.Modify;
            else if (type == ValidationType.Delete)
                status = RecordStatus.Delete;

            return status;
        }

        public static ValidationType GetValidationType(string recordStatus)
        {
            ValidationType validationType = ValidationType.None;
            if (recordStatus.ToLower() == RecordStatus.New.FirstChar().ToLower())
                validationType = ValidationType.Add;
            else if (recordStatus.ToLower() == RecordStatus.Modify.FirstChar().ToLower())
                validationType = ValidationType.Update;
            else if (recordStatus.ToLower() == RecordStatus.Delete.FirstChar().ToLower())
                validationType = ValidationType.Delete;

            return validationType;
        }

        public static string GetSqlQuery(SQLModuleType moduleType, SQLModuleActionType actionType)
        {
            string basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                            "SQLQueries",
                                            actionType.ToString(),
                                            moduleType.ToString(),
                                            string.Format("{0}.SQL", actionType.ToString()));

            string extractedQuery = string.Empty;
            using (StreamReader reader = new StreamReader(basePath))
            {
                extractedQuery = reader.ReadToEnd();
            }

            return extractedQuery;
        }

        //public static string GetEmailTemplate(ModuleType moduleType, EmailTemplate emailTemplate)
        //{
        //    string basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
        //                                    "EmailTemplates",
        //                                    moduleType.ToString(),
        //                                    string.Format("{0}.txt", emailTemplate.ToString()));

        //    string extractedQuery = string.Empty;
        //    using (StreamReader reader = new StreamReader(basePath))
        //    {
        //        extractedQuery = reader.ReadToEnd();
        //    }

        //    return extractedQuery;
        //}

        public static object CloneObject(object objSource)
        {
            //step : 1 Get the type of source object and create a new instance of that type
            Type typeSource = objSource.GetType();
            object objTarget = Activator.CreateInstance(typeSource);
            //Step2 : Get all the properties of source object type
            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //Step : 3 Assign all source property to taget object 's properties
            foreach (PropertyInfo property in propertyInfo)
            {
                //Check whether property can be written to
                if (property.CanWrite)
                {
                    //Step : 4 check whether property type is value type, enum or string type
                    if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType == typeof(String))
                    {
                        property.SetValue(objTarget, property.GetValue(objSource, null), null);
                    }
                    //else property type is object/complex types, so need to recursively call this method until the end of the tree is reached
                    else
                    {
                        object objPropertyValue = property.GetValue(objSource, null);
                        if (objPropertyValue == null)
                        {
                            property.SetValue(objTarget, null, null);
                        }
                        else
                        {
                            property.SetValue(objTarget, CloneObject(objPropertyValue), null);
                        }
                    }
                }
            }
            return objTarget;
        }

        /// <summary>
        /// HTML template to Docx file conversion
        /// </summary>
        /// <returns></returns>
        //D793
        public static byte[] HtmlToWord(string htmlString, string intertekTable,string headerText, string techSpecName, string dob,string company,string country, bool isChevron = false)
        {

            using (MemoryStream generatedDocument = new MemoryStream())
            {
                using (WordprocessingDocument package = WordprocessingDocument.Create(
                       generatedDocument, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = package.MainDocumentPart;
                    if (mainPart == null)
                    {
                        mainPart = package.AddMainDocumentPart();
                        new Document(new Body()).Save(mainPart);
                    }

                    if (isChevron)
                    {
                        SectionProperties sectionProperties = new SectionProperties();

                        #region pageBorder
                        PageBorders border = new PageBorders();

                        border.OffsetFrom = PageBorderOffsetValues.Page;

                        TopBorder topBorder = new TopBorder();
                        topBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
                        topBorder.Color = "9c9107";
                        topBorder.Space = 400;
                        topBorder.Size = 20;
                        border.Append(topBorder);
                        BottomBorder bottomBorder = new BottomBorder();
                        bottomBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
                        bottomBorder.Color = "9c9107";
                        bottomBorder.Space = 400;
                        bottomBorder.Size = 20;
                        border.Append(bottomBorder);
                        LeftBorder leftBorder = new LeftBorder();
                        leftBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
                        leftBorder.Color = "9c9107";
                        leftBorder.Space = 400;
                        leftBorder.Size = 20;
                        border.Append(leftBorder);
                        RightBorder rightBorder = new RightBorder();
                        rightBorder.Val = new EnumValue<BorderValues>(BorderValues.Thick);
                        rightBorder.Color = "9c9107";
                        rightBorder.Space = 400;
                        rightBorder.Size = 20;
                        border.Append(rightBorder);
                        sectionProperties.Append(border);
                        #endregion

                        #region pageMargin
                        var pageMargin = new PageMargin()
                        {
                            Top = 400,
                            Right = (UInt32Value)400UL,// (UInt32Value)400UL,
                            Bottom = 400,
                            Left = (UInt32Value)400UL,//(UInt32Value)400UL,
                            Header = (UInt32Value)720UL,
                            Footer = (UInt32Value)720UL,
                            Gutter = (UInt32Value)0UL
                        };
                        sectionProperties.Append(pageMargin);
                        #endregion

                        mainPart.Document.Body.Append(sectionProperties);
                    }

                    #region Header
                    mainPart.DeleteParts(mainPart.HeaderParts);
                    HeaderPart headerPart1 = mainPart.AddNewPart<HeaderPart>();
                    var imgPart = headerPart1.AddImagePart(ImagePartType.Png, "rId999");
                    var imagePartID = headerPart1.GetIdOfPart(imgPart);
                    string imageBasePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Images");

                    if (Directory.Exists(imageBasePath))
                    {
                        imageBasePath = Path.Combine(imageBasePath, "logoCV.png");//D793
                        if (!File.Exists(imageBasePath))
                        {
                            imageBasePath = string.Empty;
                        }
                        else
                        {
                            using (FileStream fs = new FileStream(imageBasePath, FileMode.Open))
                            {
                                imgPart.FeedData(fs);
                            }
                        }
                    }

                    var rId = mainPart.GetIdOfPart(headerPart1);
                    var headerRef = new HeaderReference { Id = rId };
                    SectionProperties sectionProperties1 = mainPart.Document.Body.Descendants<SectionProperties>().FirstOrDefault();
                    if (sectionProperties1 == null)
                    {
                        sectionProperties1 = new SectionProperties() { };
                        var pageMargin = new PageMargin()
                        {
                            Right = (UInt32Value)400UL,// (UInt32Value)400UL,
                            Left = (UInt32Value)600UL,//(UInt32Value)400UL,
                            Header = (UInt32Value)720UL,
                            Footer = (UInt32Value)720UL,
                            Gutter = (UInt32Value)0UL,
                            Top = 400,
                            Bottom = 400,
                        };
                        sectionProperties1.Append(pageMargin);
                        mainPart.Document.Body.Append(sectionProperties1);
                    }
                    sectionProperties1.Append(headerRef);
                    sectionProperties1.Append(new PageSize() { Width = (UInt32Value)11906U, Height = (UInt32Value)16838U });
                    headerPart1.Header = GeneratePageHeaderPart(headerText, imagePartID, techSpecName, dob, "QA-SPI-001 Attachment 4", "Rev 2", isChevron);
                    headerPart1.Header.Save();
                    #endregion

                    #region Footer
                    mainPart.DeleteParts(mainPart.FooterParts);
                    FooterPart FooterPart = mainPart.AddNewPart<FooterPart>();
                    var footerRId = mainPart.GetIdOfPart(FooterPart);
                    var footerRef = new FooterReference { Id = footerRId };
                    sectionProperties1.Append(footerRef);
                    FooterPart.Footer = GeneratePageFooterPart(TechnicalSpecialistConstants.Footer_Text,
                        TechnicalSpecialistConstants.Footer_URL, company, country, techSpecName, isChevron);//D793
                    FooterPart.Footer.Save();
                    #endregion

                    HtmlConverter converter = new HtmlConverter(mainPart);
                    Body body = mainPart.Document.Body;
                    var paragraphs = converter.Parse(htmlString);
                   // AltChunk altChunk= new AltChunk();
                    string intertekWorkChunkId = "intertekWorkChunk";
                    AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart
                (AlternativeFormatImportPartType.Xhtml, intertekWorkChunkId);

            

                    Table table = new Table();
                // // Create a TableProperties object and specify its border information.  
                TableProperties tableProperties = new TableProperties(  
                     new TableBorders(new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Nil), Size = 2 },  
                        new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Nil), Size = 2 },  
                         new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Nil), Size = 2 },  
                         new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Nil), Size = 2 },  
                         new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Nil), Size = 2 },  
                         new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Nil), Size = 2 }), 
                         new Width() { Val = "97%" }
                 );  
  
                 // Append the TableProperties object to the empty table.  
                  table.AppendChild<TableProperties>(tableProperties);
                 

                    TableRow tableRow = new TableRow();
                    TableCell tableCell = new TableCell();
                    tableCell.Append(new TableCellProperties(
                        new NoWrap() { Val = OnOffOnlyValues.Off }));
                    

                    for (int i = 0; i < paragraphs.Count; i++)
                    {
                        if(paragraphs[i].FirstChild != null)
                        {
                            tableCell.Append(new Paragraph(new Run(paragraphs[i])));
                        }
                       // body.Append(paragraphs[i]);
                    }
                    tableRow.Append(tableCell);
                    table.Append(tableRow);
                    body.Append(table);
                    if(intertekTable !=  " "){
                        using (MemoryStream xhtmlStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(intertekTable.ToString())))
                            {
                                chunk.FeedData(xhtmlStream);
                                AltChunk intertekWorkChunk = new AltChunk();
                                intertekWorkChunk.Id = intertekWorkChunkId;
                                body.Append(intertekWorkChunk);
                            }
                    }                    
                    mainPart.Document.Save();
                }

                return generatedDocument.ToArray();
            }
        }

        private static Header GeneratePageHeaderPart(string headerText, string relationshipId, string techSpecName, string dob, string leftText, string rightText, bool isChevron = false)
        {
            #region PosionTab Details
            // set the position to be the Left
            PositionalTab pTabLeft = new PositionalTab()
            {
                Alignment = AbsolutePositionTabAlignmentValues.Left,
                RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin,
                Leader = AbsolutePositionTabLeaderCharValues.None
            };
            // set the position to be the Right
            PositionalTab pTabRight = new PositionalTab()
            {
                Alignment = AbsolutePositionTabAlignmentValues.Right,
                RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin,
                Leader = AbsolutePositionTabLeaderCharValues.None
            };
            // set the position to be the Center
            PositionalTab pTabCenter = new PositionalTab()
            {
                Alignment = AbsolutePositionTabAlignmentValues.Center,
                RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin,
                Leader = AbsolutePositionTabLeaderCharValues.None
            };

            // set the position to be the Left
            PositionalTab pTabLeft1 = new PositionalTab()
            {
                Alignment = AbsolutePositionTabAlignmentValues.Left,
                RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin,
                Leader = AbsolutePositionTabLeaderCharValues.None
            };
            // set the position to be the Right
            PositionalTab pTabRight1 = new PositionalTab()
            {
                Alignment = AbsolutePositionTabAlignmentValues.Right,
                RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin,
                Leader = AbsolutePositionTabLeaderCharValues.None
            };
            // set the position to be the Center
            PositionalTab pTabCenter1 = new PositionalTab()
            {
                Alignment = AbsolutePositionTabAlignmentValues.Center,
                RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin,
                Leader = AbsolutePositionTabLeaderCharValues.None
            };

            #endregion

            Drawing element = null;
            Text textElement = null;
            Header pageHeader = null;

            if (!isChevron)
                element = GetImageContent(relationshipId);

            textElement = GetTextContent(headerText);

            if (!isChevron)
            {
                RunProperties runProperties = new RunProperties();
                Bold bold = new Bold();
                bold.Val = true;
                runProperties.Bold = bold;
                RunFonts runFonts = new RunFonts();
                runFonts.Ascii = "Calibri";
                runProperties.RunFonts = runFonts;
                FontSize fontSize = new FontSize();
                fontSize.Val = "40";
                runProperties.FontSize = fontSize;

                pageHeader = new Header(
                               new Paragraph(
                               new ParagraphProperties(
                               new ParagraphStyleId() { Val = "Header" },
                               new TextAlignment() { Val = VerticalTextAlignmentValues.Top }),
                               new Run(runProperties, pTabLeft, element, pTabRight, textElement)));
            }
            else
            {
                Text techspecTextElement = GetTextContent(string.Format("({0}) - (Intertek)                             ({1})", techSpecName?.ToUpper(), dob?.ToUpper()));//changes for D793 reffered by ALM 10-03-2020 failed doc -----------//D1187 Chevron CV Changes
                Text leftTextElement = GetTextContent(leftText);
                Text rightTextElement = GetTextContent(rightText);

                RunProperties runProperties = new RunProperties();
                Bold bold = new Bold();
                bold.Val = true;
                runProperties.Bold = bold;
                RunFonts runFonts = new RunFonts();
                runFonts.Ascii = "Calibri";
                runProperties.RunFonts = runFonts;
                FontSize fontSize = new FontSize();
                fontSize.Val = "40";
                runProperties.FontSize = fontSize;

                RunProperties runProperties1 = new RunProperties();
                RunFonts runFonts1 = new RunFonts();
                runFonts1.Ascii = "Calibri";
                runProperties1.RunFonts = runFonts1;
                FontSize fontSize1 = new FontSize();
                fontSize1.Val = "26";
                runProperties1.FontSize = fontSize1;

                RunProperties runProperties2 = new RunProperties();
                RunFonts runFonts2 = new RunFonts();
                runFonts2.Ascii = "Calibri";
                runProperties2.RunFonts = runFonts2;
                FontSize fontSize2 = new FontSize();
                fontSize2.Val = "22";
                runProperties2.FontSize = fontSize2;

                pageHeader = new Header(
                             new Paragraph(
                             new ParagraphProperties(
                             new ParagraphStyleId() { Val = "Header" }),
                             new Run(runProperties, pTabCenter, textElement),
                             new Run(new Break()),
                             new Run(runProperties1, pTabCenter1, techspecTextElement),
                             new Run(new Break()),
                             new Run(runProperties2, pTabLeft1, leftTextElement, pTabRight1, rightTextElement)));
            }

            return pageHeader;
        }

        private static Footer GeneratePageFooterPart(string headerText, string headerLocationText, string headerCompanyText,string countyrText, string tsName, bool isChevron = false)//D793
        {
            #region PosionTab Details
            // set the position to be the Left
            PositionalTab pTabLeft = new PositionalTab()
            {
                Alignment = AbsolutePositionTabAlignmentValues.Left,
                RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin,
                Leader = AbsolutePositionTabLeaderCharValues.None
            };
            // set the position to be the Right
            PositionalTab pTabRight = new PositionalTab()
            {
                Alignment = AbsolutePositionTabAlignmentValues.Right,
                RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin,
                Leader = AbsolutePositionTabLeaderCharValues.None
            };
            // set the position to be the Center
            PositionalTab pTabCenter = new PositionalTab()
            {
                Alignment = AbsolutePositionTabAlignmentValues.Center,
                RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin,
                Leader = AbsolutePositionTabLeaderCharValues.None
            };

            // set the position to be the Left
            PositionalTab pTabLeft1 = new PositionalTab()
            {
                Alignment = AbsolutePositionTabAlignmentValues.Left,
                RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin,
                Leader = AbsolutePositionTabLeaderCharValues.None
            };
            // set the position to be the Right
            PositionalTab pTabRight1 = new PositionalTab()
            {
                Alignment = AbsolutePositionTabAlignmentValues.Right,
                RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin,
                Leader = AbsolutePositionTabLeaderCharValues.None
            };
            // set the position to be the Center
            PositionalTab pTabCenter1 = new PositionalTab()
            {
                Alignment = AbsolutePositionTabAlignmentValues.Left,//D793
                RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin,
                Leader = AbsolutePositionTabLeaderCharValues.None
            };
            PositionalTab pTabCenter2 = new PositionalTab()
            {
                Alignment = AbsolutePositionTabAlignmentValues.Center,
                RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin,
                Leader = AbsolutePositionTabLeaderCharValues.None
            };
            //D793
            PositionalTab pTabCenter3 = new PositionalTab()
            {
                Alignment = AbsolutePositionTabAlignmentValues.Left,
                RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin,
                Leader = AbsolutePositionTabLeaderCharValues.None
            };
            PositionalTab pTabRight2 = new PositionalTab()
            {
                Alignment = AbsolutePositionTabAlignmentValues.Right,
                RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin,
                Leader = AbsolutePositionTabLeaderCharValues.None
            };
            #endregion

            Footer pageFooter = null;

            #region TextElements
            Text textElement = GetTextContent(headerText);
            Text textLocationElement = GetTextContent(headerLocationText);
            Text textCompanyElement = GetTextContent(headerCompanyText);
            Text textTsNameElement = GetTextContent(tsName);
            Text textPageElement = GetTextContent("Page ");
            Text textCountryElement = GetTextContent(countyrText);//D793
            #endregion

            #region RunProperties

            RunProperties runProperties = new RunProperties();
            Bold bold = new Bold();
            bold.Val = true;
            runProperties.Bold = bold;
            RunFonts runFonts = new RunFonts();
            runFonts.Ascii = "Calibri";
            runProperties.RunFonts = runFonts;
            FontSize fontSize = new FontSize();
            fontSize.Val = "18";
            runProperties.FontSize = fontSize;
            Italic italic = new Italic();
            italic.Val = true;
            runProperties.Italic = italic;

            #endregion

            if (!isChevron)
            {
                RunProperties runProperties1 = new RunProperties();
                RunFonts runFonts1 = new RunFonts();
                runFonts1.Ascii = "Calibri";
                runProperties1.RunFonts = runFonts1;
                FontSize fontSize1 = new FontSize();
                fontSize1.Val = "18";
                runProperties1.FontSize = fontSize1;
                Italic italic1 = new Italic();
                italic1.Val = true;
                runProperties1.Italic = italic1;

                RunProperties runProperties2 = new RunProperties();
                RunFonts runFonts2 = new RunFonts();
                runFonts2.Ascii = "Calibri";
                runProperties2.RunFonts = runFonts2;
                FontSize fontSize2 = new FontSize();
                fontSize2.Val = "18";
                runProperties2.FontSize = fontSize2;
                //Italic italic2 = new Italic();
                //italic2.Val = true;
                //runProperties2.Italic = italic2;
                Color color = new Color();
                color.Val = "#0000FF";
                runProperties2.Color = color;

                RunProperties runProperties3 = new RunProperties();
                RunFonts runFonts3 = new RunFonts();
                runFonts3.Ascii = "Calibri";
                runProperties3.RunFonts = runFonts3;
                FontSize fontSize3 = new FontSize();
                fontSize3.Val = "18";
                runProperties3.FontSize = fontSize3;
                Italic italic3 = new Italic();
                italic3.Val = true;
                runProperties3.Italic = italic3;

                RunProperties runProperties4 = new RunProperties();
                RunFonts runFonts4 = new RunFonts();
                runFonts4.Ascii = "Calibri";
                runProperties4.RunFonts = runFonts4;
                FontSize fontSize4 = new FontSize();
                fontSize4.Val = "18";
                runProperties4.FontSize = fontSize4;
                Italic italic4 = new Italic();
                italic4.Val = true;
                runProperties4.Italic = italic4;

                pageFooter = new Footer(
                                  new Paragraph(
                                  new ParagraphProperties(
                                  new ParagraphStyleId() { Val = "Footer" },
                                  new Justification() { Val = JustificationValues.Center }),
                                  new Run(runProperties, pTabCenter, textElement),
                                  new Run(new Break()),
                                  new Run(runProperties1, pTabRight1, textTsNameElement),
                                  new Run(new Break()),
                                  new Run(runProperties3, pTabCenter1, textCompanyElement),
                                  new Run(new Break()),
                                  new Run(runProperties4, pTabCenter3, textCountryElement),
                                  new Run(new Break()),
                                  new Run(runProperties2, pTabCenter2, textLocationElement, pTabRight, textPageElement, new PageNumber())
                                  ));
            }
            else
            {
                RunProperties runProperties1 = new RunProperties();
                RunFonts runFonts1 = new RunFonts();
                runFonts1.Ascii = "Calibri";
                runProperties1.RunFonts = runFonts1;
                FontSize fontSize1 = new FontSize();
                fontSize1.Val = "18";
                runProperties1.FontSize = fontSize1;
                //Italic italic1 = new Italic();
                //italic1.Val = true;
                //runProperties1.Italic = italic1;
                Color color = new Color();
                color.Val = "#0000FF";
                runProperties1.Color = color;
                pageFooter = new Footer(
                                   new Paragraph(
                                   new ParagraphProperties(
                                   new ParagraphStyleId() { Val = "Footer" },
                                   new Justification() { Val = JustificationValues.Center }),
                                   new Run(runProperties, pTabCenter, textElement)),
                                   new Paragraph(
                                   new ParagraphProperties(
                                   new ParagraphStyleId() { Val = "Footer" },
                                   new Justification() { Val = JustificationValues.Center }),
                                   new Run(runProperties1, pTabRight, textPageElement, new PageNumber())));
            }
            return pageFooter;
        }

        /// <summary>
        /// Get Image content for header
        /// </summary>
        /// <param name="relationshipId"></param>
        /// <returns></returns>
        private static Drawing GetImageContent(string relationshipId)
        {
            return new Drawing(
                new DW.Inline(
                    //Approx 10000L=0.01``
                    new DW.Extent() { Cx = 1410000L, Cy = 480000L },
                    new DW.EffectExtent()
                    {
                        LeftEdge = 0L,
                        TopEdge = 0L,
                        RightEdge = 0L,
                        BottomEdge = 0L
                    },
                    new DW.DocProperties()
                    {
                        Id = (UInt32Value)1U,
                        Name = "download.png"
                    },
                    new DW.NonVisualGraphicFrameDrawingProperties(
                        new A.GraphicFrameLocks() { NoChangeAspect = true }),
                    new A.Graphic(
                        new A.GraphicData(
                            new PIC.Picture(
                                new PIC.NonVisualPictureProperties(
                                    new PIC.NonVisualDrawingProperties()
                                    {
                                        Id = (UInt32Value)0U,
                                        Name = "download.png"
                                    },
                                    new PIC.NonVisualPictureDrawingProperties()),
                                new PIC.BlipFill(
                                    new A.Blip(
                                        new A.BlipExtensionList(
                                            new A.BlipExtension()
                                            {
                                                Uri =
                                                    "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                            })
                                    )
                                    {
                                        Embed = relationshipId,
                                        CompressionState =
                                            A.BlipCompressionValues.Print
                                    },
                                    new A.Stretch(
                                        new A.FillRectangle())),
                                new PIC.ShapeProperties(
                                    new A.Transform2D(
                                        new A.Offset() { X = 0L, Y = 0L },
                                        //Approx 10000L=0.01``
                                        new A.Extents() { Cx = 1410000L, Cy = 480000L }),
                                    new A.PresetGeometry(
                                        new A.AdjustValueList()
                                    )
                                    { Preset = A.ShapeTypeValues.Rectangle }))
                        )
                        { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                )
                {
                    DistanceFromTop = (UInt32Value)0U,
                    DistanceFromBottom = (UInt32Value)0U,
                    DistanceFromLeft = (UInt32Value)0U,
                    DistanceFromRight = (UInt32Value)0U,
                    EditId = "50D07946"
                });
        }

        /// <summary>
        /// Get text content for header
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static Text GetTextContent(string text)
        {
            var textElement = new Text();
            textElement.Text = text;
            textElement.Space = SpaceProcessingModeValues.Preserve;

            return textElement;
        }
    }
}
