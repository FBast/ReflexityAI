using Plugins.ReflexityAI.Framework;
using Plugins.ReflexityAI.Utils.TagList;

namespace Plugins.ReflexityAI.PatternNodes {
    public class SaveHistoricNode : ActionNode {

        [DropdownList(typeof(TagListHelper), nameof(TagListHelper.GetHistoricTags))]
        public string HistoricTag;

        public override void Execute(object context, object[] parameters) {
            // context.SaveInHistoric(HistoricTag);
        }

        public override object GetContext() {
            throw new System.NotImplementedException();
        }

        public override object[] GetParameters() {
            throw new System.NotImplementedException();
        }
    }
}