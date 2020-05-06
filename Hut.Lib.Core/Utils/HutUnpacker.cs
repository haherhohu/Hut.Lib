/******************************************************************************
 * Hut Unpacker
 * 
 * - Archive Unpacker
 * 
 * Author : Youngsoo Ryu
 * Version: 1.0.1
 * Update : 2015-04-17
 ******************************************************************************/
using System.IO;
using SevenZipLib;

namespace DeepSouthUtils
{
    public class DeepSouthUnpacker
    {
        public void unpack( string filename, string outputfilename )
        {
            using (SevenZipArchive archive = new SevenZipArchive( filename ) )
            {
//                string targetdir = Path.GetFileNameWithoutExtension(outputfilename);
                string targetdir = Path.GetDirectoryName(Path.GetFullPath(filename));

                if (File.Exists(outputfilename))
                {
                    File.Delete(outputfilename);
                }

                archive.ExtractAll( targetdir );
            }
        }
    }
}
