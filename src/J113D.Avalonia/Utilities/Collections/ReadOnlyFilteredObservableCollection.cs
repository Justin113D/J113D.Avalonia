using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace J113D.Avalonia.Utilities.Collections
{
	/// <summary>
	/// A readonly observable collection with a built-in filter
	/// </summary>
	/// <typeparam name="TItem">Collection Item type</typeparam>
	/// <typeparam name="TFilter">Filter value type</typeparam>
	public class ReadOnlyFilteredObservableCollection<TItem, TFilter> : ReadOnlyCollection<TItem>, INotifyCollectionChanged, INotifyPropertyChanged where TFilter : notnull
	{
		private TFilter? _filterValue;
		private Func<TItem, TFilter, bool> _filterCallback;

		private readonly ObservableCollection<TItem> _collection;
		private readonly List<TItem> _filteredCollection;

		/// <summary>
		/// Value to filter the collection by
		/// </summary>
		public TFilter? FilterValue
		{
			get => _filterValue;
			set
			{
				TFilter? prev = _filterValue;
				_filterValue = value;
				UpdateFilter(prev == null);
				OnPropertyChanged(new(nameof(FilterValue)));
			}
		}

		/// <summary>
		/// Function to filter items by
		/// </summary>
		public Func<TItem, TFilter, bool> FilterCallback
		{
			get => _filterCallback;
			set
			{
				_filterCallback = value;
				UpdateFilter(_filterValue == null);
			}
		}

		/// <summary>
		/// Create a new filtered observable collection
		/// </summary>
		/// <param name="collection">The collection to wrap and filter</param>
		/// <param name="filterCallback">Function to filter items by</param>
		public ReadOnlyFilteredObservableCollection(ObservableCollection<TItem> collection, Func<TItem, TFilter, bool> filterCallback) : this([.. collection], collection, filterCallback) { }

		private ReadOnlyFilteredObservableCollection(List<TItem> filteredCollection, ObservableCollection<TItem> collection, Func<TItem, TFilter, bool> filterCallback) : base(filteredCollection)
		{
			_collection = collection;
			_filteredCollection = filteredCollection;
			_filterCallback = filterCallback;

			((INotifyCollectionChanged)_collection).CollectionChanged += HandleCollectionChanged;
			((INotifyPropertyChanged)_collection).PropertyChanged += HandlePropertyChanged;
		}

		private void UpdateFilter(bool wasNull)
		{
			if(_filterValue != null)
			{
				_filteredCollection.Clear();
				_filteredCollection.AddRange(_collection.Where(x => FilterCallback(x, _filterValue)));
				CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Reset));
			}
			else if(!wasNull)
			{
				_filteredCollection.Clear();
				_filteredCollection.AddRange(_collection);
				CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Reset));
			}
		}

		/// <summary>
		/// Refreshes filtered values
		/// </summary>
		public void RefreshFilter()
		{
			if(FilterValue != null)
			{
				UpdateFilter(false);
			}
		}

		/// <summary>
		/// CollectionChanged event (per <see cref="INotifyCollectionChanged" />).
		/// </summary>
		event NotifyCollectionChangedEventHandler? INotifyCollectionChanged.CollectionChanged
		{
			add => CollectionChanged += value;
			remove => CollectionChanged -= value;
		}

		/// <summary>
		/// Occurs when the collection changes, either by adding or removing an item.
		/// </summary>
		/// <remarks>
		/// see <seealso cref="INotifyCollectionChanged"/>
		/// </remarks>
		[field: NonSerialized]
		protected virtual event NotifyCollectionChangedEventHandler? CollectionChanged;

		/// <summary>
		/// raise CollectionChanged event to any listeners
		/// </summary>
		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			_filteredCollection.Clear();
			if(FilterValue != null)
			{
				_filteredCollection.AddRange(_collection.Where(x => FilterCallback(x, FilterValue)));
				CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Reset));
			}
			else
			{
				_filteredCollection.AddRange(_collection);
				CollectionChanged?.Invoke(this, args);
			}
		}

		/// <summary>
		/// PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
		/// </summary>
		event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
		{
			add => PropertyChanged += value;
			remove => PropertyChanged -= value;
		}

		/// <summary>
		/// Occurs when a property changes.
		/// </summary>
		/// <remarks>
		/// see <seealso cref="INotifyPropertyChanged"/>
		/// </remarks>
		[field: NonSerialized]
		protected virtual event PropertyChangedEventHandler? PropertyChanged;

		/// <summary>
		/// raise PropertyChanged event to any listeners
		/// </summary>
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
		{
			PropertyChanged?.Invoke(this, args);
		}

		private void HandleCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			OnCollectionChanged(e);
		}

		private void HandlePropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			OnPropertyChanged(e);
		}
	}
}
