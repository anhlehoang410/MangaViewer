﻿using MangaViewer.Foundation.Controls;
using MangaViewer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace MangaViewer.Service
{
    public class MangaService
    {
        static string WEBSITEURL = "www.comic131.com";
        WebSiteEnum _webType;
        public MangaService()
        {
            WebType = WebSiteEnum.Comic131;
            //_webType = WebSiteEnum.Local;
        }

        public WebSiteEnum WebType
        {
            get { return _webType; }
            set 
            {
                _webType = value;
                //init menu html
                _menuHtml = "";
            }
        }
        /// <summary>
        ///   get web image and save into temp folder, return local path uri
        /// </summary>
        public Task<string> GetIamgeByImageUrl(MangaPageItem page)
        {
            return Task.Run<string>(() =>
            {
                //Get Image
                MangaPattern mPattern = WebSiteAccess.GetMangaPatternInstance(WebType);
                return mPattern.GetImageByImageUrl(page);
            });
        }

        public Task<ObservableCollection<MangaPageItem>> GetPageList(MangaChapterItem chapter)
        {

            return Task.Run<ObservableCollection<MangaPageItem>>(() =>
            {

                MangaPattern mPattern = WebSiteAccess.GetMangaPatternInstance(WebType);
                List<string> pageUrlList = mPattern.GetPageList(chapter.Url);
                ObservableCollection<MangaPageItem> mangaPageList = new ObservableCollection<MangaPageItem>();

                for (int i = 1; i <= pageUrlList.Count; i++)
                {
                    //string imagePath = mPattern.GetImageUrl(pageUrlList[i-1]);
                    mangaPageList.Add(new MangaPageItem("page-" + i, string.Empty, pageUrlList[i - 1], chapter, i, pageUrlList.Count));

                }
                return mangaPageList;
            });
        }

        public Task<ObservableCollection<MangaChapterItem>> GetChapterList(MangaMenuItem menu)
        {
            return Task.Run<ObservableCollection<MangaChapterItem>>(() =>
            {

                MangaPattern mPattern = WebSiteAccess.GetMangaPatternInstance(WebType);
                List<TitleAndUrl> chapterUrlList = mPattern.GetChapterList(menu.Url);
                ObservableCollection<MangaChapterItem> mangaChapterList = new ObservableCollection<MangaChapterItem>();

                for (int i = 1; i <= chapterUrlList.Count; i++)
                {
                    //string imagePath = mPattern.GetImageUrl(pageUrlList[i-1]);
                    mangaChapterList.Add(new MangaChapterItem("chapter-" + i, chapterUrlList[i - 1].Title, string.Empty, string.Empty, menu, chapterUrlList[i - 1].Url));
                }
                return mangaChapterList;
            });
        }

        //Menu
        private string _menuHtml = "";
        protected string MenuHtml
        {
            get
            {
                if (_menuHtml == string.Empty)
                {
                    MangaPattern mPattern = WebSiteAccess.GetMangaPatternInstance(WebType);
                    _menuHtml = mPattern.GetHtml(WEBSITEURL);
                    
                }
                return _menuHtml;
            }
        }
        public Task<HubMenuGroup> GetTopMangaGroup()
        {
            return Task.Run<HubMenuGroup>(() =>
            {
                var group = new HubMenuGroup("TopGroup", "热门连载", string.Empty, string.Empty, string.Empty);
                ObservableCollection<MangaMenuItem> topMangaMenu = new ObservableCollection<MangaMenuItem>();

                MangaPattern mPattern = WebSiteAccess.GetMangaPatternInstance(WebType);
                List<TitleAndUrl> topMenuList = mPattern.GetTopMangaList(MenuHtml);
                List<Size> sizeArray = new List<Size>() { HubItemSizes.FocusItem, HubItemSizes.SecondarySmallItem, HubItemSizes.SecondarySmallItem, HubItemSizes.SecondarySmallItem };
                List<string> colorArray = new List<string>() { "Red" ,"Black","Black","Black"};
                for (int i = 0; i < topMenuList.Count; i++ )
                {
                    //string imagePath = mPattern.GetImageUrl(pageUrlList[i-1]);
                    if (i >= sizeArray.Count)
                    {
                        //大于则用HubItemSizes.OtherSmallItem
                        topMangaMenu.Add(new MangaMenuItem("menu-" + i, topMenuList[i].Title, topMenuList[i].ImagePath, group, topMenuList[i - 1].Url, HubItemSizes.OtherSmallItem, "Blue"));
                    }
                    else
                    {
                        topMangaMenu.Add(new MangaMenuItem("menu-" + i, topMenuList[i].Title, topMenuList[i].ImagePath, group, topMenuList[i - 1].Url, sizeArray[i], colorArray[i]));
                    }
                }
                return group;
            });
        }

        public Task<ObservableCollection<HubMenuGroup>> GetMainMenu()
        {
            return Task.Run<ObservableCollection<HubMenuGroup>>(() =>
            {
                ObservableCollection<HubMenuGroup> MenuGroups = new ObservableCollection<HubMenuGroup>();

                if (WebType == WebSiteEnum.Local)
                {
                    var group1 = new HubMenuGroup("NewGroup", "最新漫画", string.Empty, string.Empty, string.Empty);
                    group1.Items.Add(new MangaMenuItem("New-1", "海贼王", "http://localhost:8800/image/Hub/", group1, "http://comic.131.com/content/shaonian/2104.html", HubItemSizes.FocusItem, string.Empty));
                    group1.Items.Add(new MangaMenuItem("New-2", "火影", "http://localhost:8800/image/Hub/hub-BizPromotion.png", group1, "http://comic.131.com/content/shaonian/2104.html", HubItemSizes.SecondarySmallItem, "#FF00B1EC"));
                    group1.Items.Add(new MangaMenuItem("New-3", "死神", "http://localhost:8800/image/Hub/hub-announcement.png", group1, "http://comic.131.com/content/shaonian/2104.html", HubItemSizes.SecondarySmallItem, "#FFA80032"));
                    group1.Items.Add(new MangaMenuItem("New-4", "猎人", "http://localhost:8800/image/Hub/hub-News.png", group1, "http://comic.131.com/content/shaonian/2104.html", HubItemSizes.SecondarySmallItem, "#FF45008A"));

                    var group2 = new HubMenuGroup("TopGroup", "热门连载", string.Empty, string.Empty, string.Empty);
                    group2.Items.Add(new MangaMenuItem("Top-1", "海贼王", "http://localhost:8800/image/Hub/hub-perb.png", group2, "http://abchina.azurewebsites.net/onlinebanking.htm", HubItemSizes.FocusItem, string.Empty));
                    group2.Items.Add(new MangaMenuItem("Top-2", "死神", "http://localhost:8800/image/Hub/hub-promotion.png", group2, "http://www.abchina.com/cn/CreditCard/default.htm", HubItemSizes.SecondarySmallItem, "#FFB3020A"));
                    group2.Items.Add(new MangaMenuItem("Top-3", "猎人", "http://localhost:8800/image/Hub/hub-Interest1.png", group2, "http://www.abchina.com/cn/CreditCard/default.htm", HubItemSizes.SecondarySmallItem, "#FFD06112"));

                    var group3 = new HubMenuGroup("OverGroup", "热门完结", string.Empty, string.Empty, string.Empty);
                    group3.Items.Add(new MangaMenuItem("Over-1", "海贼王", "http://localhost:8800/image/Hub/hub-generalloan.png", group3, "http://www.abchina.com/cn/Common/Calculator/loan.htm", HubItemSizes.SecondarySmallItem, string.Empty));
                    group3.Items.Add(new MangaMenuItem("Over-2", "火影", "http://localhost:8800/image/Hub/hub-loancalc.png", group3, "http://www.abchina.com/cn/Common/Calculator/LoanComp.htm", HubItemSizes.SecondarySmallItem, string.Empty));
                    group3.Items.Add(new MangaMenuItem("Over-3", "死神", "http://localhost:8800/image/Hub/hub-housecalc.png", group3, "http://www.abchina.com/cn/Common/Calculator/CalcLoanOrRental.htm", HubItemSizes.SecondarySmallItem, string.Empty));
                    group3.Items.Add(new MangaMenuItem("Over-4", "猎人", "http://localhost:8800/image/Hub/hub-morecalc.png", group3, "http://www.abchina.com/cn/PublicPlate/Calculator/", HubItemSizes.OtherSmallItem, "#FFA42900"));

                    MenuGroups.Add(group1);
                    MenuGroups.Add(group2);
                    MenuGroups.Add(group3);
                    return MenuGroups;
                }

                MenuGroups.Add(GetTopMangaGroup().Result);

                //MenuGroups.Add();
                return MenuGroups;
            });
        }
    }


}
