﻿using CityWars.APIs;
using CityWars.Common;
using CityWars.Models;
using CityWars.ViewModels;
using Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using ShakeGestures;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace CityWars.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserFighterPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();



        private FighterViewModel CurrentFighter { get; set; }

        public UserFighterPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

        }

        async private void Shaken(object sender, AccelerometerShakenEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var b = 5;
            });
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        private void onSignoutButtonClick(object sender, RoutedEventArgs e)
        {
            ParseUser.LogOut();
            this.Frame.Navigate(typeof(LoginPage));
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!ConnectionInspector.IsOnline())
            {
                var c = 3;
            }

            this.navigationHelper.OnNavigatedTo(e);
            this.LoadFighterDetails();



        }



        private async void LoadFighterDetails()
        {
             var fighter = await new ParseQuery<FighterViewModel>().Where(f => f.UserId == ParseUser.CurrentUser.ObjectId).FirstOrDefaultAsync();
            
            //var CurrentFighter = await new ParseQuery<FighterViewModel>().Where(f => f.UserId == ParseUser.CurrentUser.ObjectId).FirstAsync();
            
            this.FighterName.Text = fighter.FighterName;
            this.FighterType.Text = fighter.FighterType;
            if (fighter.FighterType == "Nacepenata Batka")
            {
                //this.Image.Source = new ImageSource(;
            }
            if (fighter.Health < 0) fighter.Health = 0;
            if (fighter.Health <= 0)
            {
                this.Health.Text = string.Format("Dead! Respawn for 30 gold");
                this.RegenerateHpButton.Content = "Respawn";
            }
            else
            {
                this.Health.Text = string.Format("Health: {0}/100", fighter.Health);
            }
            this.Reputation.Text = string.Format("Reputation: {0}", fighter.Reputation.ToString());
            this.Experience.Text = string.Format("Experience: {0}", fighter.Experience.ToString());
            this.Level.Text = string.Format("Level: {0}", fighter.Level.ToString());

            var maxDmg = fighter.Damage + 5;
            var minDmg = fighter.Damage - 5;

            this.Damage.Text = string.Format("Damage: {0}-{1}",minDmg.ToString(),maxDmg.ToString());
            this.Armor.Text = string.Format("Armor: {0}", fighter.Armor.ToString());
            //this.City.Text = string.Format("City: {0}",fighterToShow.City);
            this.Money.Text = string.Format("Money: {0}", fighter.Money.ToString());

            if (fighter.Message.Length > 1)
            {
                MessageDialog msgbox = new MessageDialog(fighter.Message);
                await msgbox.ShowAsync();
                fighter.Message = "-";                
            }

            await fighter.SaveAsync();
        }



        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void onRegenerateButtonClick(object sender, RoutedEventArgs e)
        {
            if (!ConnectionInspector.IsOnline())
            {
                MessageDialog msgbox = new MessageDialog("Check Your Internet Connection!");
                await msgbox.ShowAsync();
            }
            else
            {
                this.RegenerateFighter();
            }

        }

        private async void RegenerateFighter()
        {
            var fighter = await new ParseQuery<FighterViewModel>().Where(f => f.UserId == ParseUser.CurrentUser.ObjectId).FirstOrDefaultAsync();
            var fighterCurrentMoney = int.Parse(fighter.Money.ToString());

            if (fighter.Health > 0 && fighter.Health < 100)
            {
                if (fighterCurrentMoney < 20)
                {
                    this.RegeneratedMessage.Text = "No enough money!";
                }
                else
                {
                    fighter.Money = fighterCurrentMoney - 20;
                    this.Money.Text = string.Format("Money: {0}", fighter.Money);

                    fighter.Health = 100;
                    await fighter.SaveAsync();
                    this.Health.Text = string.Format("Health: {0}/100", 100);
                    //CurrentFighter.Health = 100;
                }
            }
            else if (fighter.Health <= 0)
            {
                if (fighterCurrentMoney < 30)
                {
                    this.RegeneratedMessage.Text = "No enough money!";
                }
                else
                {
                    fighter.Money = fighterCurrentMoney - 30;
                    this.Money.Text = string.Format("Money: {0}", fighter.Money);
                    this.RegenerateHpButton.Content = "Regenerate";

                    fighter.Health = 100;
                    await fighter.SaveAsync();
                    this.Health.Text = string.Format("Health: {0}/100", 100);
                    var fighterFromDb = await new ParseQuery<FighterViewModel>().Where(f => f.UserId == ParseUser.CurrentUser.ObjectId).FirstOrDefaultAsync();
                    var b = 5;
                    //CurrentFighter.Health = 100;
                }
            }
            await fighter.SaveAsync();
        }

        private async void onFightButtonClick(object sender, RoutedEventArgs e)
        {
            if (!ConnectionInspector.IsOnline())
            {
                MessageDialog msgbox = new MessageDialog("Check Your Internet Connection!");
                await msgbox.ShowAsync();
            }
            else
            {
                this.Frame.Navigate(typeof(AllFightersPage));
            }
        }

        private async void onTopCitiesButtonClick(object sender, RoutedEventArgs e)
        {
            if (!ConnectionInspector.IsOnline())
            {
                MessageDialog msgbox = new MessageDialog("Check Your Internet Connection!");
                await msgbox.ShowAsync();
            }
            else
            {
                this.Frame.Navigate(typeof(TopCitiesPage));
            }
        }

        private async void onImageHolding(object sender, HoldingRoutedEventArgs e)
        {
            if (!ConnectionInspector.IsOnline())
            {
                MessageDialog msgbox = new MessageDialog("Check Your Internet Connection!");
                await msgbox.ShowAsync();
            }
            else
            {
                this.Frame.Navigate(typeof(UserAchievements));
            }
        }

    }
}
