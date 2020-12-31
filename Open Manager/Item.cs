using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Open_Manager
{
	interface Item
	{
		string ShortDescription { get; }
		string LongDescription { get; }
		Grid MakeControl();
		Window MakeWindow();
	}
	public enum PreviewType
	{
		Image,
		Text
	};
}
