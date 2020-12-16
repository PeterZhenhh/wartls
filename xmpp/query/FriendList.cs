// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.FriendList
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class FriendList : Stanza
  {
    private string Channel;

    public FriendList(Client User, XmlDocument Packet = null)
      : base(User, Packet)
    {
    }

    public override void Process()
    {
      if (this.Type == "result")
        return;
      XDocument xDocument = new XDocument();
      XElement xelement1 = new XElement((XName) "iq");
      xelement1.Add((object) new XAttribute((XName) "type", this.Type == "get" ? (object) "result" : (object) "get"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) ("masterserver@warface/" + this.User.Channel.Resource)));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", this.Type == "get" ? (object) this.Id : (object) ("uid" + this.User.Player.Random.Next(9999, int.MaxValue).ToString("x8"))));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      xelement2.Add((object) this.User.Player.Friends);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
