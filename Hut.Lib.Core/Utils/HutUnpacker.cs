/******************************************************************************
 * Hut Unpacker
 *
 * - Archive Unpacker
 *
 * Author : Daegung Kim
 * Version: 1.0.1
 * Update : 2020-05-06
 ******************************************************************************/

using System;
using System.IO;
using System.IO.Compression;

namespace Hut
{
    public class HutUnpacker : IDisposable
    {
        public void Dispose()
        {
        }

        public void Unpack(string filename, string outputfilename)
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