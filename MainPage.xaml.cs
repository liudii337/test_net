using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace test_net
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            GetList();
        }

        public async static Task GetList_dormitory()
        {
            var http = new HttpClient();
            http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
            var response = await http.GetAsync("http://www.luoo.net/music");
            string result = response.Content.ReadAsStringAsync().Result;

            //初始化文档
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(result);
            //查找节点
            var titleNode = doc.DocumentNode.SelectSingleNode("//div[@class='vol-list']");
            var list = titleNode.SelectNodes("./div[@class='item']");

            foreach (var i in list)
            {
                var Cover = i.SelectSingleNode("//img[@class='cover rounded']").GetAttributeValue("src", "");
                var Node1 = i.SelectSingleNode("//a[@class='name']");
                var VolUrl = Node1.GetAttributeValue("href", "");
                var Title = Node1.GetAttributeValue("title","");

                var a = Node1.InnerText;
                int b1 = a.IndexOf(".");//找a的位置
                int b2 = a.IndexOf(" ");//找b的位置
                a = (a.Substring(b1 + 1)).Substring(0, b2 - b1 - 1);

                var Node2 = i.SelectSingleNode("//span[@class='comments']").InnerText.Replace("\n", "").Replace("\t", ""); 

                var favdcount = i.SelectSingleNode("//span[@class='favs']").InnerText.Replace("\n", "").Replace("\t", "");

            }
        }

        public async Task GetList()
        {
            var http = new HttpClient();
            http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
            var response = await http.GetAsync("http://www.luoo.net/vol/index/1376");
            string result = response.Content.ReadAsStringAsync().Result;

            //初始化文档
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(result);
            //查找节点
            var vol_number = doc.DocumentNode.SelectSingleNode("//span[@class='vol-number rounded']").InnerText;
            var vol_title = doc.DocumentNode.SelectSingleNode("//span[@class='vol-title']").InnerText;
            var vol_img = doc.DocumentNode.SelectSingleNode("//*[@id='volCoverWrapper']/img").GetAttributeValue("src", "");

            var vol_desc0 = doc.DocumentNode.SelectSingleNode("//div[@class='vol-desc']").InnerHtml;
            var vol_desc = Regex.Replace(vol_desc0, "^\n", "").Replace("<p>", "").Replace("</p>", "\n").Replace("<br>", "").Replace(" ", "");

            var vol_date = doc.DocumentNode.SelectSingleNode("//span[@class='vol-date']").InnerText;


            var list = doc.DocumentNode.SelectNodes("//li[@class='track-item rounded']");


            foreach (var i in list)
            {
                var string1 = i.SelectSingleNode("//a[@class='trackname btn-play']").InnerText;
                var index = string1.Substring(0, 2);
                var name = string1.Remove(0,4);

                var imagesrc = i.SelectSingleNode("//a[@class='btn-action-share icon-share']").GetAttributeValue("data-img", "");


            }
            Result.Text = vol_desc;

        }
    }
}
