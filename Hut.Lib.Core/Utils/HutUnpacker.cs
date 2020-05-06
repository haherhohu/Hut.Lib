/******************************************************************************
 * Hut Unpacker
 *
 * - Archive Unpacker
 *
 * Author : Youngsoo Ryu
 * Version: 1.0.1
 * Update : 2020-05-06
 ******************************************************************************/

using System;
using System.IO;
using System.IO.Compression;

namespace HutUtils
{
    public class HutUnpacker : IDisposable
    {
        public void Dispose()
        {
        }

        public void unpack(string filename, string outputfilename)
        {
            string targetdir = Path.GetDirectoryName(Path.GetFullPath(outputfilename));

            if (Directory.Exists(targetdir))
            {
                Directory.Delete(targetdir);
            }

            ZipFile.ExtractToDirectory(filename, targetdir);
        }
    }
}