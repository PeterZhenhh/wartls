// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.GAMEROOM.CORE.TeamColors
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using System.Xml.Linq;

namespace WARTLS.CLASSES.GAMEROOM.CORE
{
  public class TeamColors
  {
    public int Revision = 2;
    public long UserId;

    public XElement Serialize()
    {
      XElement xelement1 = new XElement((XName) "team_colors");
      XElement xelement2 = new XElement((XName) "team_color", new object[2]
      {
        (object) new XAttribute((XName) "id", (object) "1"),
        (object) new XAttribute((XName) "color", (object) "4294907157")
      });
      XElement xelement3 = new XElement((XName) "team_color", new object[2]
      {
        (object) new XAttribute((XName) "id", (object) "2"),
        (object) new XAttribute((XName) "color", (object) "4279655162")
      });
      xelement1.Add((object) xelement2);
      xelement1.Add((object) xelement3);
      return xelement1;
    }
  }
}
