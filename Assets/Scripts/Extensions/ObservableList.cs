using System;
using System.Collections.Generic;

namespace Extensions
{
    public class ObservableList<T> : List<T>
    {
        // Events for item added, removed, and list cleared
        public event Action<T> OnItemAdded;
        public event Action<T> OnItemRemoved;
        public event Action OnListCleared;
        
        public new void Add(T item) 
        {
            base.Add(item);
            OnItemAdded?.Invoke(item);
        }

        public new bool Remove(T item) 
        {
            var removed = base.Remove(item);
            if (removed)
            {
                OnItemRemoved?.Invoke(item);
            }
            return removed;
        }

        public new void Clear()
        {
            base.Clear();
            OnListCleared?.Invoke();
        }
    }
}