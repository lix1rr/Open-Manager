using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Open_Manager
{
	class FileItem:Item
	{
		string filename;

		public string ShortDescription => throw new NotImplementedException();

		public string LongDescription => throw new NotImplementedException();

		public Grid MakeControl()
		{
			throw new NotImplementedException();
		}

		public Window MakeWindow()
		{
			throw new NotImplementedException();
		}
	}
}
