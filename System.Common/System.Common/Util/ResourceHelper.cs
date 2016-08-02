using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vestris.ResourceLib;

namespace System
{
    public class ResourceHelper
    {

        /// <summary>
        /// 附加属性
        /// </summary>
        /// <param name="filename"></param>
        public static void SetStringFileInfo(string filePath, Dictionary<string, string> vls)
        {
            VersionResource versionResource = new VersionResource();
            versionResource.LoadFrom(filePath);
            StringFileInfo stringFileInfo = (StringFileInfo)versionResource["StringFileInfo"];

            foreach (string key in vls.Keys)
            {
                stringFileInfo[key] = "{0}\0".SFormat(vls[key]);
            }
            versionResource.SaveTo(filePath);
        }


        public static Dictionary<string, string> GetStringFileInfo(string filePath)
        {
            VersionResource versionResource = new VersionResource();
            versionResource.LoadFrom(filePath);
            StringFileInfo stringFileInfo = (StringFileInfo)versionResource["StringFileInfo"];
            if (stringFileInfo != null && stringFileInfo.Default != null && stringFileInfo.Default.Strings != null)
            {
                Dictionary<string, string> vls = new Dictionary<string, string>();
                foreach (string key in stringFileInfo.Default.Strings.Keys)
                {
                    vls.Add(key, stringFileInfo.Default.Strings[key].StringValue);
                }
                return vls;
            }
            return null;
        }


    }
}
