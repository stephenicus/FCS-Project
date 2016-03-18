﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for AddInKindItem.xaml
    /// </summary>
    public partial class AddInKindItem : Window
    {
         public string ItemName { get; set; }
         public string ItemDescription { get; set; }

        public AddInKindItem()
        {
            InitializeComponent();
        }

        private void Individual_DropDown(object sender, RoutedEventArgs e)
        {
            Models.FCS_FundingContext db = new Models.FCS_FundingContext();
            var query = (from o in db.Donors
                         join c in db.DonorContacts on o.DonorID equals c.DonorID
                         where o.DonorType == "Individual" || o.DonorType == "Anonymous"
                         orderby c.ContactLastName
                         select c.ContactFirstName + "\t" + c.ContactLastName + "\t" + c.ContactPhone).ToList();

            var box = sender as ComboBox;
            box.ItemsSource = query;
        }

        private void Organization_DropDown(object sender, RoutedEventArgs e)
        {
            Models.FCS_FundingContext db = new Models.FCS_FundingContext();
            var query = (from o in db.Donors
                         where o.OrganizationName != null && o.OrganizationName != ""
                         orderby o.OrganizationName
                         select o.OrganizationName).ToList();

            var box = sender as ComboBox;
            box.ItemsSource = query;
        }

        private void Add_InKind_Item(object sender, RoutedEventArgs e)
        {
            if (ItemName != null && ItemName != "" && ItemDescription != null && ItemDescription != "")
            {
                Models.FCS_FundingContext db = new Models.FCS_FundingContext();
                //Then its an organization
                if (OrgOrIndividual.IsChecked.Value && Organization.SelectedIndex != -1)
                {
                    string Organiz = Organization.SelectedValue.ToString();
                    MessageBox.Show(ItemName + "\n" + ItemDescription + "\n" + DateRecieved + "\n" + Organiz + "\n" + "You got HERE");
                    var donorID = (from d in db.Donors
                                   where d.OrganizationName == Organiz
                                   select d.DonorID).Distinct().First();
                    MessageBox.Show(donorID.ToString());
                    Models.Donation donation = new Models.Donation(donorID, false, true, 0M, Convert.ToDateTime(DateRecieved.ToString()));
                    db.Donations.Add(donation);
                    db.SaveChanges();
                    MessageBox.Show(donation.DonationID.ToString());
                    Models.In_Kind_Item inKind = new Models.In_Kind_Item(donation.DonationID, ItemName, ItemDescription);
                    db.In_Kind_Item.Add(inKind);
                }
                //then its an individual
                else if(Individual.SelectedIndex != -1)
                {
                    string Indiv = Individual.SelectedValue.ToString();
                    MessageBox.Show(ItemName + "\n" + ItemDescription + "\n" + DateRecieved + "\n" + Indiv);
                    string[] words = Indiv.Split('\t');
                    string FName = words[0]; string LName = words[1]; string FNumber = words[2];
                    MessageBox.Show("|" + FName + "|");
                    MessageBox.Show("|" + LName + "|");
                    MessageBox.Show("|" + FNumber + "|");
                    var donorID = (from dc in db.DonorContacts
                                   where dc.ContactFirstName == FName && dc.ContactLastName == LName && dc.ContactPhone == FNumber
                                   select dc.DonorID).Distinct().FirstOrDefault();

                    //int householdID = db.Patients.Where(x => x.PatientOQ == familyOQNumber).Select(x => x.HouseholdID).Distinct().First();
                    Models.Donation donation = new Models.Donation(donorID, false, true, 0M, Convert.ToDateTime(DateRecieved.ToString()));
                    Models.In_Kind_Item inKind = new Models.In_Kind_Item(donation.DonationID, ItemName, ItemDescription);
                    db.Donations.Add(donation);
                    db.In_Kind_Item.Add(inKind);
                }
                else
                {
                    MessageBox.Show("Make sure to select an organization or an individual");
                    return;
                }
                db.SaveChanges();
                MessageBox.Show("Successfully added In_Kind Item");
                this.Close();
                
            }
            //add both patient and household
            else
            {
                MessageBox.Show("Make sure you input correct data.");
            }
        }

        private void Change_Organization_Individual(object sender, RoutedEventArgs e)
        {
            if(OrgOrIndividual.IsChecked.Value)
            {
                Individual.IsEnabled = false;
                Organization.IsEnabled = true;
            }
            else
            {
                Individual.IsEnabled = true;
                Organization.IsEnabled = false;
            }
        }
    }
}
