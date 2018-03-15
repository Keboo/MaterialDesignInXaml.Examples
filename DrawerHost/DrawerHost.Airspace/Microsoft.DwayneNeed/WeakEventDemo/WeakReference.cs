using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeakEventDemo
{
    public class WeakReference<T> : WeakReference where T : class
    {
        public WeakReference(T target)
            : base(target)
        {
        }

        public new T Target
        {
            get
            {
                return (T) Target;
            }

            set
            {
                Target = value;
            }
        }
    }
}
