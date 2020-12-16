// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.GAMEROOM.CORE.Core
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using System;
using System.Xml.Linq;

namespace WARTLS.CLASSES.GAMEROOM.CORE
{
  public class Core
  {
    public long RoomId = 1;
    public byte RoomType = 2;
    public string Name = "NoNameYet";
    public byte MinReadyPlayers = 2;
    public int Revision = 1;
    public int TeamSwitched;
    public bool Private;
    public bool TeamBalanced;
    public int ItemSeed;

    public XElement Serialize(long Master = 0, Players P = null)
    {
      ++this.Revision;
      XElement xelement = new XElement((XName) "core");
      xelement.Add((object) new XAttribute((XName) "teams_switched", (object) this.TeamSwitched));
      xelement.Add((object) new XAttribute((XName) "room_name", (object) this.Name));
      xelement.Add((object) new XAttribute((XName) "private", (object) (this.Private ? 1 : 0)));
      xelement.Add((object) new XAttribute((XName) "players", (object) P.Users.Count));
      xelement.Add((object) new XAttribute((XName) "can_start", 1));
      xelement.Add((object) new XAttribute((XName) "team_balanced", (object) 1));
      xelement.Add((object) new XAttribute((XName) "min_ready_players", (object) this.MinReadyPlayers));
      xelement.Add((object) new XAttribute((XName) "revision", (object) this.Revision));
      return xelement;
    }
  }
}
