﻿using FCS_Funding.Models;
using System;
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
    /// Interaction logic for CreateNewDonor.xaml
    /// </summary>
    public partial class CreateNewDonor : Window
    {
        public string DonorAddress1 { get; set; } //
        public string DonorAddress2 { get; set; } //
        public string DonorCity { get; set; } //
        public string DonorState { get; set; } //
        public string DonorType { get; set; } //Not
        public string DonorZip { get; set; } //
        public string OrganizationName { get; set; } //
        public CreateNewDonor()
        {
            InitializeComponent();
        }

        private void CreateDonor(object sender, RoutedEventArgs e)
        {
            Determine_DonorType(this.dType.SelectedIndex);
            //DonorFirstName != null && DonorFirstName != "" && DonorLastName != null && DonorLastName != ""
            if (DonorType != null && DonorType != "")
            {
                FCS_FundingDBModel db = new FCS_FundingDBModel();
                if (DonorType == "Organization" || DonorType == "Government")
                {
                    var OrgName = from d in db.Donors
                                  where d.OrganizationName == OrganizationName
                                  select d;
                    if (OrgName.Count() == 0)
                    {
                        //MessageBox.Show(DonorAddress1 + "\n" + DonorAddress2 + "\n" + DonorCity + "\n" + DonorState + "\n" + DonorZip
                        //    + "\n" + DonorType + "\n" + OrganizationName);
                        Donor d = new Donor();

                        d.DonorType = DonorType;
                        d.OrganizationName = OrganizationName;
                        d.DonorAddress1 = DonorAddress1;
                        d.DonorAddress2 = DonorAddress2;
                        d.DonorState = DonorState;
                        d.DonorCity = DonorCity;
                        d.DonorZip = DonorZip;

                        db.Donors.Add(d);
                        db.SaveChanges();
                        MessageBox.Show("Successfully added Donor!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("There is already an organization with the name you have selected. \nNote: Number your organizations if you have organizations with the same name.");
                    }
                }
                else if(DonorType == "Individual")
                {
                    //MessageBox.Show(DonorAddress1 + "\n" + DonorAddress2 + "\n" + DonorCity + "\n" + DonorState + "\n" + DonorZip
                    //    + "\n" + DonorType + "\n" + OrganizationName);
                    Donor d = new Donor();

                    d.DonorType = DonorType;
                    d.OrganizationName = OrganizationName;
                    d.DonorAddress1 = DonorAddress1;
                    d.DonorAddress2 = DonorAddress2;
                    d.DonorState = DonorState;
                    d.DonorCity = DonorCity;
                    d.DonorZip = DonorZip;

                    CreateIndividualContact cic = new CreateIndividualContact(d);
                    this.Close();
                    cic.Show();
                }
                //its anonymous
                else if(DonorType == "Anonymous")
                {
                    //"Anonymous"
                    int anony = db.Donors.Where(x => x.DonorType == "Anonymous").Select(x => x.DonorType).Count(); //Distinct().First();
                    string Anon = "Anonymous";
                    if (anony < 1)
                    {
                        Donor d = new Donor();
                        d.DonorType = Anon;
                        d.OrganizationName = Anon;
                        d.DonorAddress1 = Anon;
                        d.DonorAddress2 = Anon;
                        d.DonorState = "";
                        d.DonorCity = Anon;
                        d.DonorZip = "";
                        DonorContact dc = new DonorContact();
                        dc.ContactFirstName = Anon;
                        dc.ContactLastName = Anon;
                        dc.ContactPhone = Anon;
                        dc.ContactEmail = Anon;
                        dc.DonorID = d.DonorID;

                        db.Donors.Add(d);
                        db.DonorContacts.Add(dc);
                        db.SaveChanges();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("You already have an Anonymous user");
                    }
                }
            }
            //add both patient and household
            else
            {
                MessageBox.Show("Make sure you select Donor Type.");
            }

        }

        private void Determine_DonorType(int selection)
        {
            switch (selection)
            {
                case 0:
                    DonorType = "Organization"; break;
                case 1:
                    DonorType = "Individual"; break;
                case 2:
                    DonorType = "Anonymous"; break;
                case 3:
                    DonorType = "Government"; break;
            }
        }
    }
}
