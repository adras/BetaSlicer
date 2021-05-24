using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BetaSlicer.NeoDemo
{
    /// <summary>
    /// Custom implementation to get a file out of the Assets directory
    /// </summary>
    class AssetHelper
    {
        const string ASSET_DIRECTORY_NAME = "Assets";

        internal static string GetPath(string fileName)
        {
            string result = Path.Combine(ASSET_DIRECTORY_NAME, fileName);
            
            return result;
        }
    }
}
