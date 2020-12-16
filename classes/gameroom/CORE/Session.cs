// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.GAMEROOM.CORE.Session
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using System;
using System.Xml.Linq;

namespace WARTLS.CLASSES.GAMEROOM.CORE
{
  public class Session
  {
    public DateTime StartTime = DateTime.Now;
    public int Revision = 1;
    public long ID = 1;
    public byte Status;
    public byte GameProcess;

    public XElement Serialize()
    {
      XElement xelement = new XElement((XName) "session");
      xelement.Add((object) new XAttribute((XName) "id", (object) this.ID));
      xelement.Add((object) new XAttribute((XName) "status", (object) this.Status));
      xelement.Add((object) new XAttribute((XName) "game_progress", (object) this.GameProcess));
      xelement.Add((object) new XAttribute((XName) "start_time", (object) this.StartTime));
      xelement.Add((object) new XAttribute((XName) "revision", (object) this.Revision));
      return xelement;
    }
  }
}
