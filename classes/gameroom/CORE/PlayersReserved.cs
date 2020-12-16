// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.GAMEROOM.CORE.PlayersReserved
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using System.Xml.Linq;

namespace WARTLS.CLASSES.GAMEROOM.CORE
{
  public class PlayersReserved
  {
    public int Revision = 2;
    public long UserId;

    public XElement Serialize()
    {
      return new XElement((XName) "playersReserved");
    }
  }
}
