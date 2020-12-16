// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.GAMEROOM.CORE.Mission
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.CLASSES.GAMEROOM.CORE
{
  public class Mission
  {
    public int Revision = 2;
    public long UserId;
    public XmlDocument Map;
    public XmlNode PvEInfo;

    public string Mode
    {
      get
      {
        return this.Map == null ? (string) null : this.Map.FirstChild.Attributes["game_mode"].InnerText;
      }
    }

    public XElement Serialize(bool IncludeData = false)
    {
      if (this.PvEInfo != null)
      {
        XElement root = XDocument.Parse(this.PvEInfo.OuterXml).Root;
        if (IncludeData)
          root.Add((object) new XAttribute((XName) "data", (object) Convert.ToBase64String(Encoding.UTF8.GetBytes(this.Map.InnerXml))));
        return root;
      }
      XElement xelement = new XElement((XName) "mission");
      try
      {
        xelement.Add((object) new XAttribute((XName) "mission_key", (object) this.Map.FirstChild.Attributes["uid"].InnerText));
        xelement.Add((object) new XAttribute((XName) "no_teams", (object) (this.Mode == "ffa" || this.Mode == "hnt" ? 1 : 0)));
        xelement.Add((object) new XAttribute((XName) "mode", (object) this.Map.FirstChild.Attributes["game_mode"].InnerText));
        xelement.Add((object) new XAttribute((XName) "mode_name", (object) this.Map.FirstChild["UI"]["GameMode"].Attributes["text"].InnerText));
        xelement.Add((object) new XAttribute((XName) "mode_icon", (object) this.Map.FirstChild["UI"]["GameMode"].Attributes["icon"].InnerText));
        xelement.Add((object) new XAttribute((XName) "image", (object) this.Map.FirstChild["UI"]["Description"].Attributes["icon"].InnerText));
        xelement.Add((object) new XAttribute((XName) "description", (object) this.Map.FirstChild["UI"]["Description"].Attributes["text"].InnerText));
        xelement.Add((object) new XAttribute((XName) "name", (object) this.Map.FirstChild.Attributes["name"].InnerText));
        xelement.Add((object) new XAttribute((XName) "difficulty", (object) "normal"));
        xelement.Add((object) new XAttribute((XName) "type", (object) ""));
        xelement.Add((object) new XAttribute((XName) "setting", (object) this.Map.FirstChild["Basemap"].Attributes["name"].InnerText));
        xelement.Add((object) new XAttribute((XName) "time_of_day", (object) this.Map.FirstChild.Attributes["time_of_day"].InnerText));
        xelement.Add((object) new XAttribute((XName) "revision", (object) this.Revision));
        if (!IncludeData)
          return xelement;
        xelement.Add((object) new XAttribute((XName) "data", (object) Convert.ToBase64String(Encoding.UTF8.GetBytes(this.Map.InnerXml))));
        return xelement;
      }
      catch
      {
        return xelement;
      }
    }
  }
}
