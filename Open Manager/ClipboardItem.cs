using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Open_Manager
{
	class ClipboardItem
	{
		public IDataObject dataObject;
		public string typeString;
		enum ViewingType { 
			Image,
			Text
		};
		ViewingType type;
		object data;
		//Bitmap when type is Image, string when type is text
		struct Pair {
			public string dataFormat;
			public string name;
			public Pair(string dataFormat, string name)
			{
				this.dataFormat = dataFormat;
				this.name = name;
			}
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
			this.dataObject = dataObject;
			if ((data = dataObject.GetData(DataFormats.Bitmap)) != null)
			{
				type = ViewingType.Image;
				
			}
			else if ((data = dataObject.GetData(DataFormats.StringFormat)) != null)
			{
				type = ViewingType.Text;
			}
			else
			{
				throw new Exception("cannot make clipboard item");
			}
			foreach (var item in order) {
				if (dataObject.GetFormats().Contains(item.dataFormat))
				{
					typeString = item.name;
					return;
				}
			}
			typeString = "unknown/" + dataObject.GetFormats()[0];
			
			throw new Exception("cannot make clipboard item");
		}

		public Grid MakeControl()
		{
			
			Console.WriteLine("! Copy");
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
			info.Inlines.Add(new Italic(new Run("type="+typeString)));
			stackPanel.Children.Add(info);

			if (type == ViewingType.Text)
			{
				var text = new TextBlock();
				text.FontFamily = new FontFamily("Consolas");
				text.TextWrapping = TextWrapping.Wrap;
				text.Text = (string)data;
				stackPanel.Children.Add(text);
			}
			else if (type == ViewingType.Image)
			{
				var image = new Image();
				image.Source = (InteropBitmap)data;
				stackPanel.Children.Add(image);
			}

			return grid;
			/*
			 * 
			 <Grid>
				<Border BorderBrush="#FFAAAAAA" BorderThickness="1" CornerRadius="5"/>
				<StackPanel Margin="10">
					<TextBlock>
						<Italic>type="text":</Italic>
					</TextBlock>
					<TextBlock FontFamily="Consolas" TextWrapping="Wrap">I am a crippled dog with a spoon</TextBlock>
					or
					<Image Source="http://i.imgur.com/aIf7B0P.jpg" />
				</StackPanel>		
			</Grid>
			*/
		}
	}
}
