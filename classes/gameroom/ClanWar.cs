// Decompiled with JetBrains decompiler
// Type: OnyxServer.classes.gameroom.ClanWar
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using System.Xml.Linq;

namespace WARTLS.classes.gameroom
{
  public class ClanWar
  {
    public string ClanFirst = "";
    public string ClanSecond = "";

    public XElement Serialize()
    {
      XElement xelement = new XElement((XName) "clan_war");
      xelement.Add((object) new XAttribute((XName) "clan_1", (object) this.ClanFirst));
      xelement.Add((object) new XAttribute((XName) "clan_2", (object) this.ClanSecond));
      return xelement;
    }
  }
}
