// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.GAMEROOM.CORE.RoomMaster
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using System.Xml.Linq;

namespace WARTLS.CLASSES.GAMEROOM.CORE
{
  public class RoomMaster
  {
    public int Revision = 2;
    public long UserId;

    public XElement Serialize()
    {
      XElement xelement = new XElement((XName) "room_master");
      xelement.Add((object) new XAttribute((XName) "master", (object) this.UserId));
      xelement.Add((object) new XAttribute((XName) "revision", (object) this.Revision));
      return xelement;
    }
  }
}
