using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;

namespace Open_Manager
{
	public class ClipboardItem:Item
	{
		// Used as a key in a resource dictionary for a Conrol
		public const ulong clipboardItemKey = 8317878212189737892;

		public IDataObject dataObject;
		public string shortDescription;
		public string longDescription;
		public DateTime dateTime; // Time in UTC when it was created
		
		public PreviewType type;
		public object data;
		//Bitmap when type is ImageSource, string when type is text
		struct Pair {
			public string dataFormat;
			public string name;
			public Pair(string dataFormat, string name)
			{
				this.dataFormat = dataFormat;
				this.name = name;
			}
		}
		
		/*override*/ public string ShortDescription {
			get => shortDescription;
		}
		/*override*/ public string LongDescription {
			get => longDescription;
		}



		// Maps data format strings to more readable MIME like type names, ones at the top have more priority
		static Pair[] order =  {
			new Pair( DataFormats.PenData,             "others/pen data"),
			new Pair( DataFormats.Bitmap,              "image/bitmap" ),
			new Pair( DataFormats.Tiff,                "image/tiff"),
			new Pair( DataFormats.Dib,                 "image/device independent bitmap"),
			new Pair( DataFormats.CommaSeparatedValue, "text/csv"),
			new Pair( DataFormats.Html,                "text/html"),
			new Pair( DataFormats.Rtf,                 "text/rtf"),
			new Pair( DataFormats.Xaml,                "text/xaml"),
			new Pair( DataFormats.Riff,                "audio/riff"),
			new Pair( DataFormats.WaveAudio,           "audio/wav"),
			new Pair( DataFormats.FileDrop,            "others/file drop"),
			new Pair( DataFormats.MetafilePicture,     "others/meta file picture"),
			new Pair( DataFormats.Palette,             "others/palette"),
			new Pair( DataFormats.Serializable,        "others/persistant object"),
			new Pair( DataFormats.SymbolicLink,        "others/symbolic link"),
			new Pair( DataFormats.XamlPackage,         "others/xaml package"),
			new Pair( DataFormats.Text,                "text"),
			new Pair( DataFormats.UnicodeText,         "text/unciode"),
			new Pair( DataFormats.OemText,             "text/oem"),
			new Pair( DataFormats.Locale,              "others/locale"),
			new Pair( DataFormats.Dif,                 "others/data interchange format"),
			new Pair( DataFormats.EnhancedMetafile,    "others/enhanced metafile"),
			new Pair( DataFormats.StringFormat,        "text/system string")
		};

		
		// byte[] when type is Binary
		// Bitmap when type is Image
		// string when type is String
		public ClipboardItem(IDataObject dataObject)
		{
			dateTime = DateTime.UtcNow;
			this.dataObject = dataObject;
			if ((data = (ImageSource)dataObject.GetData(DataFormats.Bitmap)) != null)
			{
				type = PreviewType.Image;
				
			}
			else if ((data = (string)dataObject.GetData(DataFormats.StringFormat)) != null)
			{
				type = PreviewType.Text;
			}
			else
			{
				throw new Exception("cannot make clipboard item");
			}
			shortDescription = "type=";
			foreach (var item in order) {
				if (dataObject.GetFormats().Contains(item.dataFormat))
				{
					shortDescription += item.name;
					goto longDescInit;
				}
			}
			// line runs when a type name couldnt be found
			shortDescription = "unknown/" + dataObject.GetFormats()[0];
			longDescInit:
			longDescription = "Windows data formats: ";
			for (int i = 0; i < dataObject.GetFormats().Count(); i++)
			{
				if (i != 0)
				{
					longDescription += ", ";
				}
				longDescription += dataObject.GetFormats()[i];
			}
			var culture = CultureInfo.CurrentUICulture;
			longDescription += ". Created at " + dateTime.ToString(culture) + " UTC, " + dateTime.ToLocalTime().ToString(culture) + " local time.";
		}

		public Grid MakeControl()
		{
			var grid = new Grid();
			grid.Margin = new Thickness(0, 0, 0, 10);

			var border = new Border();
			border.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xAA, 0xAA, 0xAA));
			border.BorderThickness = new Thickness(1);
			border.CornerRadius = new CornerRadius(5);
			grid.Children.Add(border);

			var stackPanel = new StackPanel();
			stackPanel.Margin = new Thickness(10);
			grid.Children.Add(stackPanel);

			var info = new TextBlock();
			info.TextWrapping = TextWrapping.Wrap;
			info.Inlines.Add(new Italic(new Run(shortDescription)));
			stackPanel.Children.Add(info);

			if (type == PreviewType.Text)
			{
				var text = new TextBox();
				text.BorderBrush = new SolidColorBrush();
				text.BorderThickness = new Thickness(0);
				text.Background = new SolidColorBrush();
				text.TextWrapping = TextWrapping.Wrap;
				text.IsReadOnly = true;
				text.FontFamily = new FontFamily("Consolas");
				text.Text = (string)data;
				stackPanel.Children.Add(text);
			}
			else if (type == PreviewType.Image)
			{
				var image = new Image();
				image.Source = (InteropBitmap)data;
				stackPanel.Children.Add(image);
			}
			grid.Resources.Add(clipboardItemKey, this);
			return grid;	
		}
		public Window MakeWindow()
		{
			return new PreviewWindow(this);
		}
	}
}
