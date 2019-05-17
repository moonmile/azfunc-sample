using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HttpSampleClient
{

    //  形式を選択して貼り付ける
    /*
public class Rootobject
{
    public Class1[] Property1 { get; set; }
}

public class Class1
{
    public int Id { get; set; }
    public string PersonNo { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public DateTime ModifiedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
    */


    public class Rootobject
    {
        public Persons[] Items { get; set; }
    }

    public class Persons
    {
        public int Id { get; set; }
        public string PersonNo { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// ViewModelクラス
    /// </summary>
    public class ViewModel : ObservableObject
    {
        public string PersonNo { get; set; }
        public string Status { get; set; }
        private List<Persons> _Items;
        public List<Persons> Items
        {
            get => _Items;
            set => SetProperty(ref _Items, value, nameof(Items));
        }
    }
    /// <summary>
    /// MVVMパターンのための基底クラス
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        protected bool SetProperty<T>(
            ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
