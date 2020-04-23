using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Hut
{
    // INotifyPropertyChanged Observer
    // use IObserver / IObservable
    public class HutNotifyObserver
    {
        protected Dictionary<Enum, PropertyChangedEventHandler> observers = new Dictionary<Enum, PropertyChangedEventHandler>();

        public void Attatch(Enum token, PropertyChangedEventHandler observer)
        {
            if (observers.Keys.Contains(token))
            {
                observers[token] += observer;
            }
            else
            {
                observers.Add(token, observer);
            }
        }

        public void DeAttatch(Enum token, PropertyChangedEventHandler observer)
        {
            if (observers.Keys.Contains(token))
            {
                observers[token] -= observer;
            }
        }

        public void Notify(Enum token, object sender, PropertyChangedEventArgs e)
        {
            observers[token](sender, e);
        }
    }

    public class HutSingleObserver : HutSingleton<HutNotifyObserver>
    {
    }
}

/*
  using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Hut
{
    public class HutObserver<T> : IObserver<T>
    {
        protected List<KeyValuePair<Enum, IObservable<T>>> targets = new List<KeyValuePair<Enum, IObservable<T>>>();

        public void Subscribe(Enum token, IObservable<T> prov)
        {
            targets.Add(new KeyValuePair<Enum, IObservable<T>>(token, prov));
        }

        public void unSubscribe(Enum token, IObservable<T> prov)
        {
            targets.Remove(new KeyValuePair<Enum, IObservable<T>>(token, prov));
        }
        public void OnCompleted()
        {
            targets.Clear();
        }
        public void OnError(Exception error)
        {
        }
        public void OnNext(Enum token, T value)
        {
            foreach (var target in targets.Where(w => w.Key = token))
            {
                // do something;
            }
        }
    }

    public class HutObservable<T> : IObservable<T>
    {
    }
}
*/