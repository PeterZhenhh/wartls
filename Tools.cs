// Decompiled with JetBrains decompiler
// Type: OnyxServer.Tools
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using Ionic.Zlib;

using WARTLS.CLASSES;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace WARTLS
{
  public class Tools
  {
  
   

    public static string Decode(string base64encoded)
    {
      return Encoding.UTF8.GetString(ZlibStream.UncompressBuffer(Convert.FromBase64String(base64encoded)));
    }

    public static string Encode(string text)
    {
      return Convert.ToBase64String(ZlibStream.CompressBuffer(Encoding.UTF8.GetBytes(text)));
    }

    

  

  

    public static long GetTotalSeconds(string Value)
    {
      try
      {
        long num = long.Parse(new Regex("[0-9]*").Matches(Value)[0].ToString());
        char result = 's';
        char.TryParse(new Regex("[a-z]").Matches(Value)[0].Value, out result);
        switch (result)
        {
          case 'd':
            return (long) TimeSpan.FromDays((double) num).TotalSeconds;
          case 'h':
            return (long) TimeSpan.FromHours((double) num).TotalSeconds;
          case 'm':
            return (long) TimeSpan.FromMinutes((double) num).TotalSeconds;
          default:
            return num;
        }
      }
      catch
      {
        return -1;
      }
    }

  }
}
