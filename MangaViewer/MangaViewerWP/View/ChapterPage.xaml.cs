﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MangaViewer.Resources;
using MangaViewer.ViewModel;
using System.Collections.ObjectModel;
using MangaViewer.Model;

namespace MangaViewer.View
{
    public partial class ChapterPage : PhoneApplicationPage
    {
        // Constructor
        public ChapterPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            GetChatperList();
        }

        async void GetChatperList()
        {
            ViewModelLocator.AppViewModel.Main.ChapterList = null;
            //有网络
            ObservableCollection<MangaChapterItem> chapterItem = await MangaViewerWP.App.MangaService.GetChapterList(ViewModelLocator.AppViewModel.Main.SelectedMenu);
            //LoadingStack.Visibility = Visibility.Collapsed;
            ViewModelLocator.AppViewModel.Main.ChapterList = chapterItem;

            ////没网络
            //ViewModelLocator.AppViewModel.Main.PageList = new MangaViewer.Data.PageListData().PageList;
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}