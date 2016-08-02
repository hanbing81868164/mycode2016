using System.Collections.ObjectModel;

namespace System
{
    public class NObservableCollection<T> : ObservableCollection<T> where T : class ,new()
    {
        public NObservableCollection<T> Add(T item)
        {
            base.Add(item);
            return this;
        }
    }
}
