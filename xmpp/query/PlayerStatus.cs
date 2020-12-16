// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.PlayerStatus
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class PlayerStatus : Stanza
  {
    public PlayerStatus(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      User.Status = int.Parse(this.Query.Attributes["new_status"].InnerText);
      User.Location = this.Query.Attributes["to"].InnerText;
      this.Process();
      if (User.Player.RoomPlayer.Room != null)
        User.Player.RoomPlayer.Room.Sync((Client) null);
      User.Player.Save();
    }

    public override void Process()
    {
      XDocument xdocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) "k01.warface"));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
