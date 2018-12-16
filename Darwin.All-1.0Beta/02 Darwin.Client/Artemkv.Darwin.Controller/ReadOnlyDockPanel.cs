using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;

namespace Artemkv.Darwin.Controller
{
	public class ReadOnlyDockPanel : DockPanel
	{
		public static readonly DependencyProperty IsReadOnlyInnerProperty;
		public static readonly DependencyProperty IsEnabledInnerProperty;

		static ReadOnlyDockPanel()
		{
			IsReadOnlyInnerProperty = DependencyProperty.Register("IsReadOnlyInner", typeof(bool), typeof(FrameworkElement));
			IsEnabledInnerProperty = DependencyProperty.Register("IsEnabledInner", typeof(bool), typeof(FrameworkElement));
		}

		public bool IsReadOnlyInner
		{
			set { SetValue(IsReadOnlyInnerProperty, value); }
			get { return (bool)GetValue(IsReadOnlyInnerProperty); }
		}

		public bool IsEnabledInner
		{
			set { SetValue(IsEnabledInnerProperty, value); }
			get { return (bool)GetValue(IsEnabledInnerProperty); }
		}
	}
}
