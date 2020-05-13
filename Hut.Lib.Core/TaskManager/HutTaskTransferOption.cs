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
using System;
using System.IO;

namespace Hut
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HutTaskTransferOption
    {
        private HutStorageMethod method;
        private HutSearchRule searchrule;
        private HutRenameRule renamerule;
        private string searchtext;
        private string attatchtext;
        private string delimtext;
        private string archivedir;
        private bool isattatchdate;

        public HutTaskTransferOption()
        {
            searchrule = HutSearchRule.None;
            searchtext = string.Empty;
            renamerule = HutRenameRule.None;
            attatchtext = string.Empty;
            delimtext = string.Empty;
            archivedir = string.Empty;
            isattatchdate = false;
        }

        [JsonProperty]
        public HutSearchRule SearchRule { get { return searchrule; } set { searchrule = value; } }

        [JsonProperty]
        public HutStorageMethod StorageMethod { get { return method; } set { method = value; } }

        [JsonProperty]
        public HutRenameRule RenameRule { get { return renamerule; } set { renamerule = value; } }

        [JsonProperty]
        public string SearchText { get { return searchtext; } set { searchtext = value; } }

        [JsonProperty]
        public string AttatchText { get { return attatchtext; } set { attatchtext = value; } }

        [JsonProperty]
        public string DelimText { get { return delimtext; } set { delimtext = value; } }

        [JsonProperty]
        public string ArchiveDir { get { return archivedir; } set { archivedir = value; } }

        [JsonProperty]
        public bool isAttatchDate { get { return isattatchdate; } set { isattatchdate = value; } }

        public string rename(string filename)
        {
            switch (renamerule)
            {
                case HutRenameRule.None:
                    return filename;

                case HutRenameRule.Replace:
                    return attatchtext;

                case HutRenameRule.Prefix:
                    if (isAttatchDate)
                    {
                        return DateTime.Now.ToString(AttatchText) + delimtext + filename;
                    }
                    else
                    {
                        return AttatchText + delimtext + filename;
                    }
                case HutRenameRule.Postfix:
                    if (isAttatchDate)
                    {
                        return Path.GetFileNameWithoutExtension(filename) + delimtext + DateTime.Now.ToString(AttatchText) + Path.GetExtension(filename);
                    }
                    else
                    {
                        return Path.GetFileNameWithoutExtension(filename) + delimtext + AttatchText + Path.GetExtension(filename);
                    }
                default:
                    return filename;
            }
        }
    }
}