﻿using FCS_DataTesting;
using System;
//using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

namespace FCS_Funding.Views
{
    /// <summary>
    /// Interaction logic for UpdateInKindItem.xaml
    /// </summary>
    public partial class UpdateInKindItem : Window
    {
        public int DonorID { get; set; }
        public int DonationID { get; set; }
        public int ItemID { get; set; }

        public string ItemName { get; set; }
        public string ItemDescription { get; set; }

        public UpdateInKindItem(InKindItem i)
        {
            DonorID = i.DonorID;
            DonationID = i.DonationID;
            ItemID = i.ItemID;
            ItemName = i.ItemName;
            ItemDescription = i.Description;
            InitializeComponent();
			text_ItemName.Focus();
        }

        private void Update_InKind_Item(object sender, RoutedEventArgs e)
        {

            Models.FCS_DBModel db = new Models.FCS_DBModel();
            var inkinditem = (from p in db.In_Kind_Item
                           where p.ItemID == ItemID
                           select p).First();
            inkinditem.ItemName = ItemName;
            inkinditem.ItemDescription = ItemDescription;

            var donation = (from d in db.Donations
                            where d.DonationID == DonationID
                            select d).First();
            donation.DonationDate = Convert.ToDateTime(DateRecieved.ToString());
            db.SaveChanges();
            
            this.Close();
        }

        private void Delete_InKind_Item(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Delete this In-Kind Item?",
               "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Models.FCS_DBModel db = new Models.FCS_DBModel();
                var inkinditem = (from p in db.In_Kind_Item
                                        where p.ItemID == ItemID
                                        select p).First();

                var donation = (from d in db.Donations
                                where d.DonationID == DonationID
                                select d).First();
                db.In_Kind_Item.Remove(inkinditem);
                db.Donations.Remove(donation);
                db.SaveChanges();
                this.Close();
            }
        }

		private void useEnterAsTab(object sender, System.Windows.Input.KeyEventArgs e)
		{
			CommonControl.IntepretEnterAsTab(sender, e);
		}
	}
}
