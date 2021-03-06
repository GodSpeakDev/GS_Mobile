﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
    public partial class MyProfilePage : CustomContentPage
    {
        public MyProfilePage ()
        {
            PreventKeyboardOverlap = true;
            InitializeComponent ();
            NavigationPage.SetHasNavigationBar (this, false);

            FirstNameEntry.Completed += (sender, e) => {
                LastNameEntry.Focus ();
            };

            LastNameEntry.Completed += (sender, e) => {
                Countries.Focus ();
            };

            var lastSelectedIndex = 0;
            Countries.Focused += (sender, e) => {
                lastSelectedIndex = (sender as CustomPicker).SelectedIndex;
            };

            Countries.Unfocused += (sender, e) => {
                var actualIndex = (sender as CustomPicker).SelectedIndex;
                if (lastSelectedIndex != actualIndex) {
                    ZipCodeEntry.Focus ();
                }
            };

            ZipCodeEntry.Completed += (sender, e) => {
				ReferringEmailAddressEntry.Focus();
            };

			ReferringEmailAddressEntry.Completed += (sender, e) =>  {
				CurrentPasswordEntry.Focus ();
			};

            CurrentPasswordEntry.Completed += (sender, e) => {
                NewPasswordEntry.Focus ();
            };

            NewPasswordEntry.Completed += (sender, e) => {
                PasswordConfirmEntry.Focus ();
            };

            PasswordConfirmEntry.Completed += (sender, e) => {
                (this.BindingContext as MyProfileViewModel).SaveCommand.Execute ();
            };
        }

        protected override void OnBindingContextChanged ()
        {
            base.OnBindingContextChanged ();

            (this.BindingContext as MvxViewModel).PropertyChanged += Handle_PropertyChanged;
        }

        void Handle_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Countries")
                return;
            var profileViewModel = this.BindingContext as MyProfileViewModel;

            if (profileViewModel != null) {
                Countries.Items.Clear ();
                foreach (var item in profileViewModel.Countries) {
                    Countries.Items.Add (item);
                }

                Countries.SelectedIndex = profileViewModel.SelectedCountryIndex;
            }
        }
    }
}
