using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZarzadzanieHotelem.Utils
{
    static class ClassUtils
    {
        public static T EditElement<T>(T modifiedElement, T element) where T: class
        {
            foreach(var elem in typeof(T).GetProperties())
            {
                if(elem.Name != "Id")
                   elem.SetValue(modifiedElement, elem.GetValue(element, null));
            }
            return modifiedElement;
        }
    }
}
