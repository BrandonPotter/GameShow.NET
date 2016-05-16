using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShow.WpfApp
{
    public class Observable<T>
    {
        private T _mValue;

        public event EventHandler ValueChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyValueChanged()
        {
            ValueChanged?.Invoke(this, new EventArgs());
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
        }

        public T Value
        {
            get
            {
                return _mValue;
            }
            set
            {
                SetValueSilently(value);
                NotifyValueChanged();
            }
        }

        public void SetValueSilently(T value)
        {
            _mValue = value;
        }

        public static implicit operator T(Observable<T> observable)
        {
            return observable.Value;
        }

        //public static T operator =(Observable<T> observable, T value) // Doesn't compile!
        //{
        //    observable.Value = value;
        //    return value;
        //}
    }
}
