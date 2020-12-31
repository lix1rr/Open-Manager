using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Open_Manager
{
	/// <summary>
	/// Interaction logic for PreviewWindow.xaml
	/// </summary>
	public partial class PreviewWindow : Window
	{
		public PreviewWindow(ClipboardItem item)
		{
			InitializeComponent();
			MouseDown += Window_MouseDown;
			CloseButton.MouseDown += CloseButton_MouseDown;
			CloseButton.MouseEnter += CloseButton_MouseEnter;
			CloseButton.MouseLeave += CloseButton_MouseLeave;
			
			Window_Details.Text = item.longDescription;

			if (item.type == PreviewType.Text)
			{
				PreviewContainer.Children.Remove(ImagePreview);
				TextPreview_TextBox.Text = (string)item.data;
			}
			else if (item.type == PreviewType.Image)
			{
				PreviewContainer.Children.Remove(TextPreview);
				ImagePreview_Image.Source = (InteropBitmap)item.data;
			}
			else
			{
				throw new Exception("Unreachable code has been reached :(");
			}
		}

		private void CloseButton_MouseLeave(object sender, MouseEventArgs e)
		{
			Mouse.OverrideCursor = Cursors.Arrow;
		}

		private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
		{
			Mouse.OverrideCursor = Cursors.Hand;
		}

		private void CloseButton_MouseDown(object sender, RoutedEventArgs e)
		{
			Close();
		}

		
		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				DragMove();
		}
	}
}
