using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace Open_Manager
{


	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		static Dictionary<IntPtr, MainWindow> hwndsAndWindow = new Dictionary<IntPtr, MainWindow>();
		List<ClipboardItem> clipboardItems = new List<ClipboardItem>();
		PreviewWindow previewWindow;

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);
		public MainWindow()
		{
			InitializeComponent();
			MouseDown += Window_MouseDown;
			Loaded += Window_Loaded;
			StateChanged += MainWindow_StateChanged;
			Deactivated += MainWindow_Deactivated;
			/*
			CloseButton.MouseDown += CloseButton_MouseDown;
			CloseButton.MouseEnter += CloseButton_MouseEnter;
			CloseButton.MouseLeave += CloseButton_MouseLeave;
			*/
		}

		private void MainWindow_Deactivated(object sender, EventArgs e)
		{
			Close();
		}

		private void MainWindow_StateChanged(object sender, EventArgs e)
		{
			if (WindowState == WindowState.Minimized)
			{
				Close();

			}
		}

		/*
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
*/
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Console.WriteLine("register hook");
			Loaded -= Window_Loaded;
			IntPtr hwnd = new WindowInteropHelper(this).Handle;
			hwndsAndWindow.Add(hwnd, this);
			HwndSource source = HwndSource.FromHwnd(hwnd);
			source.AddHook(new HwndSourceHook(Window_WndProc));
			SetClipboardViewer(hwnd);
		}
		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				DragMove();
		}

		private void OnCopy()
		{
			var dataObject = Clipboard.GetDataObject();
			if (dataObject == null || (clipboardItems.Count > 0 && Clipboard.IsCurrent(clipboardItems[0].dataObject)))
			{
				return;
			}
			ClipboardItem item = new ClipboardItem(dataObject);
			clipboardItems.Add(item);
			var control = ((Item)item).MakeControl();
			if (control != null)
			{
				itemList.Children.Insert(0, control);
				control.MouseDown += ClipboardItem_MouseDown;
			}
			
		}

		private void ClipboardItem_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (previewWindow != null)
			{
				previewWindow.Close();
			}
			var clipboardItem = (ClipboardItem) ((Grid)sender).FindResource(ClipboardItem.clipboardItemKey);
			previewWindow = (PreviewWindow) clipboardItem.MakeWindow();
			previewWindow.Show();
		}

		const int WM_DRAWCLIPBOARD = 0x0308;
		private static IntPtr Window_WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == WM_DRAWCLIPBOARD)
			{
				hwndsAndWindow[hwnd].OnCopy();
			}
			return IntPtr.Zero;
		}
		
	}
}
/*
 * 
 * <<!--
	
	
	
	<Grid>
	<ScrollViewer Margin="0,10,0,0" VerticalScrollBarVisibility="Visible" Height="1000">
		<StackPanel>
			<TextBlock Height="10000">
				as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>as\n<LineBreak/>
			</TextBlock>
		</StackPanel>

	</ScrollViewer>
	</Grid>
	
-->
*/
