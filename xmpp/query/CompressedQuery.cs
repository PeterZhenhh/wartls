// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.CompressedQuery
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System;
using System.Xml;

namespace WARTLS.xmpp.query
{
  public class CompressedQuery : Stanza
  {
    public CompressedQuery(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      this.Uncompress(ref Packet);
      string index = Packet.FirstChild.FirstChild.FirstChild.Name.Replace(":", "_");
      Type packet = Core.MessageFactory.Packets[index];
      if (!(packet != (Type) null))
        return;
      Activator.CreateInstance(packet, (object) User, (object) Packet);
    }
  }
}
