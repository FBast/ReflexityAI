using System;
using System.Collections.Generic;
using System.Linq;
using NodeUtilityAi.Framework;
using XNode;
using Object = UnityEngine.Object;

namespace NodeUtilityAi.Nodes {
    public abstract class CollectionEntryNode : EntryNode {

        [Input(ShowBackingValue.Never)] public TaggedData DataIn;
        [Output] public CollectionEntryNode LinkedOption;
        [Output] public TaggedData DataOut;
        public string DataTag = "Data";
        public int Index;

        public int CollectionCount => CollectionProvider(_context)?.Count ?? 0;

        protected abstract List<Object> CollectionProvider(AbstractAIComponent context);
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == "LinkedOption") {
                return this;
            }
            if (port.fieldName == "DataOut") {
                List<Object> collection = CollectionProvider(_context);
                if (collection != null && collection.Count > Index)
                    return GetData();
            }
            return null;
        }
        
        public TaggedData GetData() {
            TaggedData taggedData = new TaggedData {
                Data = CollectionProvider(_context)[Index], 
                DataTag = DataTag
            };
            return taggedData;
        }

        protected T GetData<T>() where T : Object {
            List<TaggedData> taggedDatas = GetInputValues<TaggedData>("DataIn").ToList();
            taggedDatas.RemoveAll(data => data == null);
            taggedDatas = taggedDatas.Where(data => data.Data is T).ToList();
            if (taggedDatas.Count > 1)
                throw new Exception("Multiple Data found for type " + typeof(T) + 
                                    ", you should consider using GetData with a dataTag as parameter");
            if (taggedDatas.Count > 0)
                return taggedDatas.First().Data as T;
            return null;
        }

        protected T GetData<T>(string dataTag) where T : Object {
            List<TaggedData> taggedDatas = GetInputValues<TaggedData>("DataIn").ToList();
            taggedDatas.RemoveAll(data => data == null);
            taggedDatas = taggedDatas.Where(data => data.Data is T &&  data.DataTag == dataTag).ToList();
            if (taggedDatas.Count > 1)
                throw new Exception("Multi Data found for type " + typeof(T) + 
                                    " and with tag " + dataTag + ", don't use the same dataTag twice as input");
            if (taggedDatas.Count > 0)
                return taggedDatas.First().Data as T;
            return null;
        }
        
    }

}