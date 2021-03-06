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
	/// Interaction logic for Tab_Admin.xaml
	/// </summary>
	public partial class Tab_Admin : UserControl
	{

		public Tab_Admin()
		{
			InitializeComponent();
		}

		private void Refresh_AdminGrid(object sender, RoutedEventArgs e)
		{
			var db = new FCS_DBModel();
			var join1 = (from p in db.Staff
						 select new AdminDataGrid
						 {
							 StaffID = p.StaffID,
							 StaffUserName = p.StaffUserName,
							 StaffFirstName = p.StaffFirstName,
							 StaffLastName = p.StaffLastName,
							 StaffTitle = p.StaffTitle,
							 StaffDBRole = p.StaffDBRole
						 });

			Admin_DataGrid.ItemsSource = join1.ToList();
		}

		private void CreateNewAccount(object sender, RoutedEventArgs e)
		{
			CreateNewAccount cna = new CreateNewAccount();
			cna.ShowDialog();
			cna.UserRole.SelectedIndex = 0;

			Refresh_AdminGrid(sender, e);
		}

		private void EditAccount(object sender, MouseButtonEventArgs e)
		{
			try
			{
				DataGrid dg = sender as DataGrid;

				AdminDataGrid p = (AdminDataGrid)dg.SelectedItems[0]; // OR:  Patient p = (Patient)dg.SelectedItem;
				UpdateAccount up = new UpdateAccount(p);

				up.UserRole.SelectedItem = p.StaffDBRole;
				up.ShowDialog();
			}
			catch (Exception error)
			{
			}

			Refresh_AdminGrid(sender, e);
		}
	}
}
