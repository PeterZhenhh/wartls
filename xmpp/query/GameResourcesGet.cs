// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GameResourcesGet
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class GameResourcesGet : Stanza
  {
    private int From;
    private int Left;
    private XmlDocument Selected;

    public GameResourcesGet(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      this.From = int.Parse(this.Query.Attributes["from"] == null ? "0" : this.Query.Attributes["from"].InnerText);
      int index = this.From / 250;
      if (this.Query.Name == "items")
        this.Selected = Core.GameResources.ItemsSplited[index];
      if (this.Query.Name == "get_configs")
        this.Selected = Core.GameResources.ConfigsSplited[index];
      if (this.Query.Name == "shop_get_offers")
        this.Selected = Core.GameResources.ShopOffersSplited[index];
      if (this.Query.Name == "quickplay_maplist")
        this.Selected = Core.GameResources.QuickPlayMapListSplited[index];
      if (this.Query.Name == "missions_get_list")
        this.Selected = Core.GameResources.PvE;
      this.Process();
    }

    public override void Process()
    {
      XDocument xDocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) ("masterserver@warface/" + this.User.Channel.Resource)));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) this.Query.Name);
      if (this.Selected != null)
        xelement3 = XElement.Parse(this.Selected.InnerXml);
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
