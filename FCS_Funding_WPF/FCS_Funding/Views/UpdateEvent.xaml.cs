﻿using FCS_DataTesting;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;


namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for UpdateEvent.xaml
    /// </summary>
    public partial class UpdateEvent : Window
    {
        public int DonorID { get; set; }
        public int DonationID { get; set; }
        public int EventID { get; set; }

        public string BeginHour { get; set; }
        public string BeginMinute { get; set; }
        public string EndHour { get; set; }
        public string EndMinute { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string StaffDBRole { get; set; }
        public UpdateEvent(EventsDataGrid p, string staffDBRole)
        {
            StaffDBRole = staffDBRole;
            EventID = p.EventID;
            EventName = p.EventName;
            EventDescription = p.EventDescription;
            BeginMinute = p.EventStartDateTime.Minute.ToString();
            EndMinute = p.EventEndDateTime.Minute.ToString();

            if (BeginMinute.Length == 1)
            {
                BeginMinute = "0" + BeginMinute;
            }
            if (EndMinute.Length == 1)
            {
                EndMinute = "0" + EndMinute;
            }
            if (p.EventStartDateTime.Hour > 12)
            {
                BeginHour = (p.EventStartDateTime.Hour - 12).ToString();
            }
            else if (p.EventStartDateTime.Hour == 0)
            {
                BeginHour = "12";
            }
            else
            {
                BeginHour = p.EventStartDateTime.Hour.ToString();
            }
            if (p.EventEndDateTime.Hour > 12)
            {
                EndHour = (p.EventEndDateTime.Hour - 12).ToString();
            }
            else if (p.EventEndDateTime.Hour == 0)
            {
                EndHour = "12";
            }
            else
            {
                EndHour = p.EventEndDateTime.Hour.ToString();
            }
            InitializeComponent();

			text_EventName.Focus();
        }

        private void Update_Event(object sender, RoutedEventArgs e)
        {
            if (AMPM_Start.SelectedValue.ToString() == "PM" && Convert.ToInt32(BeginHour) != 12)
            {
                BeginHour = (Convert.ToInt32(BeginHour) + 12).ToString();
            }
            if (AMPM_End.SelectedValue.ToString() == "PM" && Convert.ToInt32(EndHour) != 12)
            {
                EndHour = (Convert.ToInt32(EndHour) + 12).ToString();
            }
            if (AMPM_Start.SelectedValue.ToString() == "AM" && Convert.ToInt32(BeginHour) == 12)
            {
                BeginHour = (Convert.ToInt32(BeginHour) - 12).ToString();
            }
            if (AMPM_End.SelectedValue.ToString() == "AM" && Convert.ToInt32(EndHour) == 12)
            {
                EndHour = (Convert.ToInt32(EndHour) - 12).ToString();
            }

            DateTime help = Convert.ToDateTime(DateRecieved.ToString());
            DateTime startDateTime = new DateTime(help.Year, help.Month, help.Day, Convert.ToInt32(BeginHour), Convert.ToInt32(BeginMinute), 0);
            DateTime endDateTime = new DateTime(help.Year, help.Month, help.Day, Convert.ToInt32(EndHour), Convert.ToInt32(EndMinute), 0);
            decimal timeDiff = (decimal)(endDateTime - startDateTime).TotalHours;
            if (EventName != null && EventName != "" && EventDescription != null && EventDescription != "" && timeDiff > 0)
            {
                Models.FCS_DBModel db = new Models.FCS_DBModel();
                //MessageBox.Show(EventName + "\n" + EventDescription + "\n" + startDateTime + "\n" + endDateTime + "\n" + timeDiff);

                var event1 = (from p in db.FundRaisingEvents
                                     where p.EventID == EventID
                                     select p).First();
                event1.EventName = EventName;
                event1.EventDescription = EventDescription;
                event1.EventStartDateTime = startDateTime;
                event1.EventEndDateTime = endDateTime;
                                
                db.SaveChanges();
                
                this.Close();

            }
            else
            {
                MessageBox.Show("Please check the data entered.");
            }
        }

        private void Delete_Event(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Delete this Event?" ,
               "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                FCS_Funding.Models.FCS_DBModel db = new FCS_Funding.Models.FCS_DBModel();
                var event1 = (from d in db.FundRaisingEvents
                                where d.EventID == EventID
                                select d).First();
                var donations = (from p in db.Donations
                                 where p.EventID == EventID
                                 select p);
                foreach (var don in donations)
                {
                    db.Donations.Remove(don);
                }
                db.FundRaisingEvents.Remove(event1);
                db.SaveChanges();
                MessageBox.Show("This Grant has been deleted and the Proposal\nassociated with this grant has been set to Pending.");
                this.Close();
            }
        }
        private void AM_PM_Dropdown(object sender, RoutedEventArgs e)
        {
            var box = sender as ComboBox;
            box.ItemsSource = new List<string>() { "AM", "PM" };
        }


        private void Hour_LostFocus(object sender, RoutedEventArgs e)
        {

            //StartHour
            var textbox = sender as TextBox;
            if (textbox.Text != "" && textbox.Text != null)
            {
                try
                {
                    int value = Convert.ToInt32(textbox.Text);
                    if (value > 12)
                        textbox.Text = "12";
                    else if (value < 1)
                        textbox.Text = "1";
                }
                catch 
                {
                    textbox.Text = "";
                    MessageBox.Show("Please enter a number");
                }
            }

        }
        private void Minute_LostFocus(object sender, RoutedEventArgs e)
        {
            //StartHour
            var textbox = sender as TextBox;
            if (textbox.Text != "" && textbox.Text != null)
            {
                try
                {
                    int value = Convert.ToInt32(textbox.Text);
                    if (textbox.Text.Length == 1)
                    {
                        textbox.Text = "0" + textbox.Text;
                    }
                    else if (value > 59)
                    {
                        textbox.Text = "59";
                    }
                    else if (value < 0)
                    {
                        textbox.Text = "00";
                    }
                }
                catch 
                {
                    textbox.Text = "";
                    MessageBox.Show("Please enter a number");
                }
            }
        }

        private void Donations_Grid(object sender, RoutedEventArgs e)
        {
            Models.FCS_DBModel db = new Models.FCS_DBModel();
            var join1 = (from d in db.Donations
                         join dr in db.Donors on d.DonorID equals dr.DonorID
                         join dc in db.DonorContacts on dr.DonorID equals dc.DonorID
                         where d.EventID == EventID
                         && (dr.DonorType == "Anonymous" || dr.DonorType == "Individual")
                         select new DonationsGrid
                         {
                             DonorFirstName = dc.ContactFirstName,
                             DonorLastName = dc.ContactLastName,
                             OrganizationName = "",
                             DonationAmount = d.DonationAmount,
                             DonationDate = d.DonationDate,
                             DonorID = d.DonorID,
                             DonationID = d.DonationID
                         }).Union(
                         from d in db.Donations
                         join dr in db.Donors on d.DonorID equals dr.DonorID
                         where d.EventID == EventID
                         && (dr.DonorType == "Organization" || dr.DonorType == "Government")
                         select new DonationsGrid
                         {
                             DonorFirstName = "",
                             DonorLastName = "",
                             OrganizationName = dr.OrganizationName,
                             DonationAmount = d.DonationAmount,
                             DonationDate = d.DonationDate,
                             DonorID = d.DonorID,
                             DonationID = d.DonationID
                         });
            // ... Assign ItemsSource of DataGrid.
            var grid = sender as DataGrid;
            grid.ItemsSource = join1.ToList();
        }

        private void EditDonation(object sender, MouseButtonEventArgs e)
        {
                DataGrid dg = sender as DataGrid;

                DonationsGrid p = (DonationsGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
                UpdateDonation up = new UpdateDonation(p);
                up.DeleteDon.IsEnabled = false;
                up.DonationDate.SelectedDate = p.DonationDate;
                up.ShowDialog();
					
        }

        private void Edit_InKindItem(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = sender as DataGrid;

            InKindItem p = (InKindItem)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
            UpdateInKindItem up = new UpdateInKindItem(p);
            up.DeleteItem.IsEnabled = false;
            up.DateRecieved.SelectedDate = p.DateRecieved;
            up.ShowDialog();
				
        }

        private void InKindItemGrid(object sender, RoutedEventArgs e)
        {
            Models.FCS_DBModel db = new Models.FCS_DBModel();
            var join1 = (from p in db.Donors
                         join dc in db.DonorContacts on p.DonorID equals dc.DonorID
                         join d in db.Donations on p.DonorID equals d.DonorID
                         join ki in db.In_Kind_Item on d.DonationID equals ki.DonationID
                         where (p.DonorType == "Anonymous" || p.DonorType == "Individual")
                         && d.EventID == EventID && d.EventID != null
                         select new InKindItem
                         {
                             EventID = d.EventID,
                             DonorID = p.DonorID,
                             ItemID = ki.ItemID,
                             DonationID = d.DonationID,
                             ItemName = ki.ItemName,
                             DonorFirstName = dc.ContactFirstName,
                             DonorLastName = dc.ContactLastName,
                             OrganizationName = "",
                             DateRecieved = d.DonationDate,
                             Description = ki.ItemDescription
                         }).Union(
                        from p in db.Donors
                        join d in db.Donations on p.DonorID equals d.DonorID
                        join ki in db.In_Kind_Item on d.DonationID equals ki.DonationID
                        where (p.DonorType == "Organization" || p.DonorType == "Government")
                        && d.EventID == EventID && d.EventID != null
                        select new InKindItem
                        {
                            EventID = d.EventID,
                            DonorID = p.DonorID,
                            ItemID = ki.ItemID,
                            DonationID = d.DonationID,
                            ItemName = ki.ItemName,
                            DonorFirstName = "",
                            DonorLastName = "",
                            OrganizationName = p.OrganizationName,
                            DateRecieved = d.DonationDate,
                            Description = ki.ItemDescription
                        });
            var grid = sender as DataGrid;
            grid.ItemsSource = join1.ToList();
        }

        private void Edit_InKindService(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = sender as DataGrid;

            InKindService p = (InKindService)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
            UpdateInKindService up = new UpdateInKindService(p);
					
            up.DateRecieved.SelectedDate = p.StartDateTime;

            if (p.StartDateTime.Hour >= 12)
            {
                up.AMPM_Start.SelectedIndex = 1;
            }
            else
            {
                up.AMPM_Start.SelectedIndex = 0;
            }
            if (p.EndDateTime.Hour >= 12)
            {
                up.AMPM_End.SelectedIndex = 1;
            }
            else
            {
                up.AMPM_End.SelectedIndex = 0;
            }
            up.ShowDialog();
        }

        private void InKindServiceGrid(object sender, RoutedEventArgs e)
        {
            Models.FCS_DBModel db = new Models.FCS_DBModel();
            var join1 = (from p in db.Donors
                         join dc in db.DonorContacts on p.DonorID equals dc.DonorID
                         join d in db.Donations on p.DonorID equals d.DonorID
                         join ki in db.In_Kind_Service on d.DonationID equals ki.DonationID
                         where (p.DonorType == "Anonymous" || p.DonorType == "Individual")
                         && d.EventID == EventID
                         select new InKindService
                         {
                             DonorID = p.DonorID,
                             DonationID = d.DonationID,
                             ServiceID = ki.ServiceID,
                             DonorFirstName = dc.ContactFirstName,
                             DonorLastName = dc.ContactLastName,
                             StartDateTime = ki.StartDateTime,
                             EndDateTime = ki.EndDateTime,
                             RatePerHour = ki.RatePerHour,
                             ServiceDescription = ki.ServiceDescription,
                             Length = ki.ServiceLength,
                             Value = ki.ServiceValue
                         });
            var grid = sender as DataGrid;
            grid.ItemsSource = join1.ToList();
        }

        private void AddNewDonation(object sender, RoutedEventArgs e)
        {
            EventDonorDonation cmd = new EventDonorDonation(EventID);
            cmd.ShowDialog();
            cmd.Organization.IsEnabled = false;
        }

        private void AddInKindItem(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Windows.Count <= 2)
            {
                AddInKindItem iki = new AddInKindItem(true, EventID);
                iki.ShowDialog();
                iki.Organization.IsEnabled = false;
            }
        }

        private void AddInKindService(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Windows.Count <= 2)
            {
                AddInKindService iks = new AddInKindService(true, EventID);
                iks.ShowDialog();
                iks.AMPM_End.SelectedIndex = 0;
                iks.AMPM_Start.SelectedIndex = 0;
            }
        }

		private void useEnterAsTab(object sender, System.Windows.Input.KeyEventArgs e)
		{
			CommonControl.IntepretEnterAsTab(sender, e);
		}
	}
}
