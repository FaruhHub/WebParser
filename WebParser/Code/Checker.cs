using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using WebParser.Models;

namespace WebParser.Code
{
    class Checker
    {
        public string WebChecker(string url)
        {
            string http = "http://";
            string https = "https://";
            string lblResult = String.Empty;
            string lblStatusDescription = String.Empty;
            string lblCause = String.Empty;
            string innerTextOfTitle = String.Empty;
            string contentOfTagMeta = String.Empty;
            string tagA = String.Empty;
            string tagImg = String.Empty;
            string innerTextOfH1 = String.Empty;
            string all = String.Empty;
            DateTime date = DateTime.Now;

            Urls urllink = new Urls();
            List<TagA> tagAList = new List<TagA>();
            List<TagImg> tagImgList = new List<TagImg>();
            List<TagH1> tagH1List = new List<TagH1>();

            try
            {
                //------проверяем есть ли ссылка-------------
                if (string.IsNullOrEmpty(url))
                {
                    MessageBox.Show(@"Вы не вввели ссылку !");
                    return null;
                }

                //---делаем проверку на наличие протокола "http://" в ссылке запроса
                if (!(url.StartsWith(http) || url.StartsWith(https)))
                    url = url.Insert(0, http);
                //------------------------------------------------------------------

                // Создаём объект запроса
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                // Получаем ответ с сервера, если запрашиваемый URL не действителен, 
                // переходим к блоку catch, иначе идем дальше.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (request.HaveResponse)
                {
                    lblResult = (response.StatusCode == HttpStatusCode.OK) ? "Сайт доступен" : "Сайт не доступен";
                    lblStatusDescription = response.StatusDescription;
                    lblCause = ((int)response.StatusCode).ToString();

                    string respStream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                    HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                    htmlDoc.LoadHtml(respStream);

                    //------ получаем значение тэга Title -------------
                    var nodeTitle = htmlDoc.DocumentNode.SelectNodes("//title");
                    if (nodeTitle != null)
                    {
                        innerTextOfTitle = nodeTitle["title"].InnerText;
                    }
                    //--------------------------------------------------

                    //------ получаем значение атрибута content тэга meta у которого
                    //------ значение атрибута name является description
                    var nodeMeta = htmlDoc.DocumentNode.SelectNodes("//meta");
                    if (nodeMeta != null)
                    {
                        foreach (var tag in nodeMeta)
                        {
                            if (tag.Attributes["name"] != null && tag.Attributes["name"].Value == "description")
                            {
                                contentOfTagMeta = tag.Attributes["content"].Value;
                            }
                        }
                    }
                    //---------------------------------------------------------------

                    //------ получаем значение атрибута href у тэга A -------------
                    var nodesA = htmlDoc.DocumentNode.SelectNodes("//a");
                    if (nodesA != null)
                    {
                        foreach (var tag in nodesA)
                        {
                            if (tag.Attributes["href"] != null)
                            {
                                var link = tag.Attributes["href"].Value;
                                tagA += link + "\n";
                                TagA insTagA = new TagA
                                {
                                    UrlId = urllink.Id,
                                    Href = link
                                };
                                tagAList.Add(insTagA);
                            }
                        }
                    }
                    //--------------------------------------------------------------

                    //------ получаем значение атрибута src у тэга img -------------
                    var nodesImg = htmlDoc.DocumentNode.SelectNodes("//img");
                    if (nodesImg != null)
                    {
                        foreach (var tag in nodesImg)
                        {
                            if (tag.Attributes["src"] != null)
                            {
                                var src = tag.Attributes["src"].Value;
                                tagImg += src + "\n";
                                TagImg tagImgIns = new TagImg
                                {
                                    UrlId = urllink.Id,
                                    Src = src
                                };
                                tagImgList.Add(tagImgIns);
                            }
                        }
                    }
                    //---------------------------------------------------------------

                    //------ получаем значение атрибута src у тэга img -------------
                    var nodesH1 = htmlDoc.DocumentNode.SelectNodes("//h1");
                    if (nodesH1 != null)
                    {
                        foreach (var tag in nodesH1)
                        {
                            innerTextOfH1 += tag.InnerText + "\n";
                            int index = nodesH1[tag];
                            TagH1 tagH1Ins = new TagH1
                            {
                                UrlId = urllink.Id,
                                H1Text = tag.InnerText
                            };
                            tagH1List.Add(tagH1Ins);
                        }
                    }
                    //---------------------------------------------------------------

                    StringBuilder newContent = new StringBuilder();
                    newContent.AppendLine("URL: " + url +
                                        "\nДата проверки: " + date +
                                        "\nСтатус: " + lblResult +
                                        "\nОписание статуса: " + lblStatusDescription +
                                        "\nКод статуса: " + lblCause +
                                        "\nTitle: " + innerTextOfTitle +
                                        "\nContent: " + contentOfTagMeta + "\n" +
                                        "\nТэги A: \n" + tagA + "\n" +
                                        "\nТэги Img: \n" + tagImg +
                                        "\nТэги H1: \n" + innerTextOfH1 +
                                        "\n");
                    all = newContent.ToString();

                    urllink.Url = url;
                    urllink.Title = innerTextOfTitle;
                    urllink.MetaContent = contentOfTagMeta;
                    urllink.Status = lblResult;
                    urllink.StatusDescription = lblStatusDescription;
                    urllink.StatusCode = lblCause;
                    urllink.DateOfParsing = date;

                    using (var db = new ParsingResultsEntities())
                    {
                        db.Urls.Add(urllink);
                        db.TagH1.AddRange(tagH1List);
                        db.TagA.AddRange(tagAList);
                        db.TagImg.AddRange(tagImgList);
                        db.SaveChanges();
                    }
                }
                response.Close();
                request.Abort();
                return all; //возвращаем полученные данные в интерфейс приложения
            }

            catch (WebException ex)
            {
                lblResult = @"Сайт не доступен";
                lblStatusDescription = ex.Message;

                //---делаем проверку является ли код ошибки HttpStatusCode или WebExceptionStatus
                if (((HttpWebResponse)ex.Response) != null)
                    lblCause = ((int)(((HttpWebResponse)ex.Response).StatusCode)).ToString();
                else
                    lblCause = ((int)(ex.Status)).ToString();
                //-------------------------------------------------------------------------------

                StringBuilder newContent = new StringBuilder();
                newContent.AppendLine("URL: " + url +
                                    "\nДата проверки: " + date +
                                    "\nСтатус: " + lblResult +
                                    "\nОписание статуса: " + lblStatusDescription +
                                    "\nКод статуса: " + lblCause + "\n");
                all = newContent.ToString();

                using (var db = new ParsingResultsEntities())
                {
                    db.Urls.Add(urllink);
                    db.SaveChanges();
                }

                return all;
            }
        }
    }
}
