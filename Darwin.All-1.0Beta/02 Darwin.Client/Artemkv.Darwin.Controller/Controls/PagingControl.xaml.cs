using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Artemkv.Darwin.Common;

namespace Artemkv.Darwin.Controller.Controls
{
	/// <summary>
	/// Interaction logic for PagingControl.xaml
	/// </summary>
	public partial class PagingControl : UserControl
	{
		#region Class Members

		private IListDataSource _dataSource;
		private DataGrid _dataGrid;
		private ListFilter _filter;

		private int _pageSize = int.MaxValue;
		private int _pageCurrent;
		private int _pagesTotal;

		#endregion Class Members

		#region .Ctor

		public PagingControl()
		{
			InitializeComponent();

			var serviceLocator = ServiceLocator.GetActive();
			_pageSize = serviceLocator.Paging.PageSize;

			UpdateCurrentPage();
		}

		#endregion .Ctor

		#region Public Methods

		public void BindToGridAndDataSource(DataGrid dataGrid, IListDataSource dataSource, ListFilter filter)
		{
			if (dataGrid == null)
				throw new ArgumentNullException("dataGrid");
			if (dataSource == null)
				throw new ArgumentNullException("dataSource");
			if (filter == null)
				throw new ArgumentNullException("filter");

			_dataGrid = dataGrid;
			_dataSource = dataSource;
			_filter = filter;

			UpdateGridItemsSource();
		}

		#endregion Public Methods

		#region Private Methods

		private void UpdateGridItemsSource()
		{
			var result = _dataSource.GetItems(_filter, _pageSize, _pageCurrent);
			_dataGrid.ItemsSource = result.ResultSet;

			UpdatePageCount(result.Count);
			UpdateCurrentPage();

			UpdateEnabledDisable();
		}

		private void UpdateCurrentPage()
		{
			if (_pageCurrent < _pagesTotal)
			{
				PageNoTextBox.Text = (_pageCurrent + 1).ToString();
			}
		}

		private void UpdatePageCount(int itemsTotal)
		{
			_pagesTotal = (int) Math.Ceiling((double)itemsTotal / (double)_pageSize);
			LimitCurrentPage();
			PagesTotalTextBlock.Text = _pagesTotal.ToString();
		}

		private void LimitCurrentPage()
		{
			if (_pageCurrent + 1 >= _pagesTotal)
			{
				_pageCurrent = _pagesTotal - 1;
			}
			if (_pageCurrent < 0)
			{
				_pageCurrent = 0;
			}
		}

		private void UpdateEnabledDisable()
		{
			if (_pageCurrent == 0)
			{
				FirstPageButton.IsEnabled = false;
				PrevPageButton.IsEnabled = false;
			}
			else
			{
				FirstPageButton.IsEnabled = true;
				PrevPageButton.IsEnabled = true;
			}

			if (_pageCurrent + 1 >= _pagesTotal)
			{
				LastPageButton.IsEnabled = false;
				NextPageButton.IsEnabled = false;
			}
			else
			{
				LastPageButton.IsEnabled = true;
				NextPageButton.IsEnabled = true;
			}
		}

		#endregion Private Methods

		#region Event Handlers

		private void FirstPageButton_Click(object sender, RoutedEventArgs e)
		{
			_pageCurrent = 0;
			UpdateGridItemsSource();
		}

		private void PrevPageButton_Click(object sender, RoutedEventArgs e)
		{
			if (_pageCurrent > 0)
			{
				_pageCurrent--;
				UpdateGridItemsSource();
			}
		}

		private void NextPageButton_Click(object sender, RoutedEventArgs e)
		{
			if (_pageCurrent + 1 < _pagesTotal)
			{
				_pageCurrent++;
				UpdateGridItemsSource();
			}
		}

		private void LastPageButton_Click(object sender, RoutedEventArgs e)
		{
			_pageCurrent = _pagesTotal - 1;
			UpdateGridItemsSource();
		}

		private void PageNoTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				int pageToGo = 0;
				if (int.TryParse(PageNoTextBox.Text, out pageToGo))
				{
					_pageCurrent = pageToGo - 1;
					LimitCurrentPage();
					UpdateGridItemsSource();
				}
			}
		}

		#endregion Event Handlers
	}
}
