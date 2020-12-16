// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GetProfilePerformance
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class GetProfilePerformance : Stanza
  {
    private string Channel;

    public GetProfilePerformance(Client User, XmlDocument Packet)
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
      XElement xelement3 = new XElement((XName) "get_profile_performance");
      XElement xelement4 = new XElement((XName) "pvp_modes_to_complete");
      XElement xelement5 = new XElement((XName) "pve_missions_performance");
      xelement4.Add((object) new XElement((XName) "mode", (object) "ctf"));
      xelement4.Add((object) new XElement((XName) "mode", (object) "dst"));
      xelement4.Add((object) new XElement((XName) "mode", (object) "ptb"));
      xelement4.Add((object) new XElement((XName) "mode", (object) "lms"));
      xelement4.Add((object) new XElement((XName) "mode", (object) "ffa"));
      xelement4.Add((object) new XElement((XName) "mode", (object) "stm"));
      xelement4.Add((object) new XElement((XName) "mode", (object) "tbs"));
      xelement4.Add((object) new XElement((XName) "mode", (object) "dmn"));
      xelement4.Add((object) new XElement((XName) "mode", (object) "hnt"));
      xelement4.Add((object) new XElement((XName) "mode", (object) "tdm"));
      xelement3.Add((object) xelement5);
      xelement3.Add((object) xelement4);
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
