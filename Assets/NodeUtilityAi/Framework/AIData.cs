using System;
using System.Collections.Generic;
using System.Linq;
using Object = UnityEngine.Object;

namespace NodeUtilityAi.Framework {
    public class AIData : Dictionary<string, Object> {

        public KeyValuePair<string, Object> GetFirstData() {
            if (Count == 0)
                throw new Exception("No Data found");
            return this.First();
        }
        
        public T GetData<T>() where T : Object {
            List<Object> objectList = Values.ToList();
            List<T> dataList = objectList.OfType<T>().ToList();
            if (dataList.Count == 0)
                throw new Exception("No Data found for type " + typeof(T) + 
                                    ", check the type of your input");
            if (dataList.Count > 1)
                throw new Exception("Multiple Data found for type " + typeof(T) + 
                                    ", you should consider using GetData with a dataTag as parameter");
            return dataList[0];
        }

        public T GetData<T>(string dataTag) where T : Object {
            Object data;
            TryGetValue(dataTag, out data);
            if (data == null)
                throw new Exception("No data found for dataTag : " + dataTag + 
                                    ", check the tag of your input");
            if (data.GetType() != typeof(T))
                throw new Exception("The data found for dataTag : " + dataTag + 
                                    " is not from type : " + typeof(T));
            return data as T;
        }

        public override string ToString() {
            return $" ({string.Join(" ", Values.Select(o => o.name))})";
        }
        
    }
}