// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.GAMEROOM.CORE.Players
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using System.Collections.Generic;
using System.Xml.Linq;

namespace WARTLS.CLASSES.GAMEROOM.CORE
{
  public class Players
  {
    public List<Client> Users = new List<Client>();

    public XElement Serialize()
    {
      XElement xelement = new XElement((XName) "players");
      foreach (Client client in this.Users.ToArray())
        xelement.Add((object) client.ToElement(false));
      return xelement;
    }
  }
}
