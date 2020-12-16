// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.PersistentSettingsSet
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class PersistentSettingsSet : Stanza
  {
    private string Channel;

    public PersistentSettingsSet(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      XmlNode node = (XmlNode) this.Query["settings"];
      foreach (XmlNode childNode in node.ChildNodes)
      {
        if (User.Player.Settings["settings"] == null)
          User.Player.Settings.AppendChild(User.Player.Settings.ImportNode(node, true));
        if (User.Player.Settings["settings"][childNode.Name] == null)
        {
          User.Player.Settings["settings"].AppendChild(User.Player.Settings.ImportNode(node.FirstChild, true));
        }
        else
        {
          foreach (XmlAttribute attribute in (XmlNamedNodeMap) this.Query["settings"][childNode.Name].Attributes)
          {
            if (User.Player.Settings["settings"][childNode.Name].Attributes[attribute.Name] == null)
              User.Player.Settings["settings"][childNode.Name].SetAttribute(attribute.Name, attribute.Value);
            else
              User.Player.Settings["settings"][childNode.Name].Attributes[attribute.Name].Value = attribute.Value;
          }
        }
      }
      User.Player.Save();
      this.Process();
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
      XElement xelement3 = new XElement((XName) "persistent_settings_set");
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
