/******************************************************************************
* Hut File Transfer Option
*
* - Option for File Transfer Action.
*
* Author : Daegung Kim
* Version: 1.0.0
* Update : 2017-04-12
******************************************************************************/

using Newtonsoft.Json;

namespace Hut
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HutTaskTransferOption
    {
        private HutStorageMethod method;
        private HutSearchRule searchrule;
        private string searchtext;

        public HutTaskTransferOption()
        {
            searchrule = HutSearchRule.None;
            searchtext = string.Empty;
        }

        [JsonProperty]
        public HutSearchRule SearchRule { get { return searchrule; } set { searchrule = value; } }

        [JsonProperty]
        public HutStorageMethod StorageMethod { get { return method; } set { method = value; } }

        [JsonProperty]
        public string SearchText { get { return searchtext; } set { searchtext = value; } }
    }
}