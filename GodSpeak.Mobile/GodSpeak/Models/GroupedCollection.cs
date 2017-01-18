using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GodSpeak
{
	public class GroupedCollection<T, TKey> : ObservableCollection<T>
	{
		public TKey Key { get; private set; }


		public GroupedCollection()
		{
		}

		public GroupedCollection(TKey key)
			: this(key, null)
		{
		}

		public GroupedCollection(TKey key, IEnumerable<T> items)
		{
			Key = key;

			if (items == null)
			{
				Items.Clear();
			}
			else
			{
				foreach (var item in items)
				{
					Items.Add(item);
				}
			}
		}
	}
}
