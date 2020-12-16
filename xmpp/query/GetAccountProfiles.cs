// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GetAccountProfiles
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class GetAccountProfiles : Stanza
  {
    public GetAccountProfiles(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      try
      {
        this.Process();
      }
      catch
      {
      }
    }

    public override void Process()
    {
      XDocument xdocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) this.To));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "get_account_profiles");
      if (this.User.Player.ProfileCreated)
      {
        XElement xelement4 = new XElement((XName) "profile");
        xelement4.Add((object) new XAttribute((XName) "id", (object) this.User.Player.UserID));
        xelement4.Add((object) new XAttribute((XName) "nickname", (object) this.User.Player.Nickname));
        xelement3.Add((object) xelement4);
      }
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
