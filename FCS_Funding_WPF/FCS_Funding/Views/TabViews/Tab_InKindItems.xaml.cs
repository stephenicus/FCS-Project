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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FCS_DataTesting;
using FCS_Funding.Models;

namespace FCS_Funding.Views.TabViews
{
	using Definition;

	/// <summary>
	/// Interaction logic for Tab_InKindItems.xaml
	/// </summary>
	public partial class Tab_InKindItems : UserControl
	{
		public string StaffRole { get; set; }

		public Tab_InKindItems()
		{
			InitializeComponent();

			//	Check for permissions
			StaffRole = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().StaffDBRole;

			if (StaffRole == Definition.Basic || StaffRole == Definition.User)
			{
				AddItem.IsEnabled = false;
			}
			else if (StaffRole == Definition.Admin)
			{
				AddItem.IsEnabled = true;
			}
		}

		private void Edit_InKindItem(object sender, MouseButtonEventArgs e)
		{
			if (StaffRole != Definition.Basic)
			{
				DataGrid dg = sender as DataGrid;

				InKindItem p = (InKindItem)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
				UpdateInKindItem up = new UpdateInKindItem(p);
				if (StaffRole != Definition.Admin)
				{
					up.DeleteItem.IsEnabled = false;
				}
				up.DateRecieved.SelectedDate = p.DateRecieved;
				up.ShowDialog();

			}
		}

		private void InKindItemGrid(object sender, RoutedEventArgs e)
		{
			var db = new FCS_DBModel();
			var join1 = (from p in db.Donors
						 join dc in db.DonorContacts on p.DonorID equals dc.DonorID
						 join d in db.Donations on p.DonorID equals d.DonorID
						 join ki in db.In_Kind_Item on d.DonationID equals ki.DonationID
						 where (p.DonorType == "Anonymous" || p.DonorType == "Individual")
						 && d.EventID == null
						 select new InKindItem
						 {
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
						 && d.EventID == null
						select new InKindItem
						{
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

		private void Add_InKind_Item(object sender, RoutedEventArgs e)
		{
			if (Application.Current.Windows.Count <= 1)
			{
				AddInKindItem iki = new AddInKindItem(false, -1);
				iki.Show();
				iki.Topmost = true;
				iki.Organization.IsEnabled = false;
			}
		}

		private void Refresh_InKind(object sender, RoutedEventArgs e)
		{
			sender = InKind_DataGrid;
			InKindItemGrid(sender, e);
		}
	}
}