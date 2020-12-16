// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.GameResources
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.CLASSES
{
  public class GameResources
  {
    public List<XmlDocument> ItemsSplited = new List<XmlDocument>();
    public List<XmlDocument> Maps = new List<XmlDocument>();
    public List<XmlDocument> ShopOffersSplited = new List<XmlDocument>();
    public List<XmlDocument> ConfigsSplited = new List<XmlDocument>();
    public List<XmlDocument> QuickPlayMapListSplited = new List<XmlDocument>();
    public Dictionary<string, List<string>> ShopItemsReged = new Dictionary<string, List<string>>();

    public XDocument Slots { get; private set; } = new XDocument();

    public XmlDocument Items { get; private set; } = new XmlDocument();

    public XmlDocument PvE { get; private set; } = new XmlDocument();

    public XmlDocument QuickPlayMapList { get; private set; } = new XmlDocument();

    public XmlDocument ShopOffers { get; private set; } = new XmlDocument();

    public XmlDocument Configs { get; private set; } = new XmlDocument();
    public XElement ConfigsXML { get; private set; }
    public XmlDocument OnlineVariables { get; private set; } = new XmlDocument();

    public XmlDocument ServerVariables { get; private set; } = new XmlDocument();

    public XmlDocument NewbieItemsXML { get; private set; } = new XmlDocument();

    public XmlDocument NewbieItemsOldXML { get; private set; } = new XmlDocument();

    public XmlDocument ExpCurve { get; private set; } = new XmlDocument();

    public List<Item> NewbieItems { get; private set; } = new List<Item>();

    public GameResources()
    {
      this.Slots = XDocument.Load("Gamefiles/Slots.xml");
      this.Items.Load("Gamefiles/Items.xml");
      this.Configs.Load("Gamefiles/Configs.xml");
      ConfigsXML = XElement.Load("Gamefiles/Configs.xml");
      this.ShopOffers.Load("Gamefiles/ShopOffers.xml");
      this.PvE.Load("Gamefiles/PvE.xml");
      this.OnlineVariables.Load("Gamefiles/OnlineVariables.xml");
      this.NewbieItemsXML.Load("Gamefiles/NewbieItems.xml");
      this.QuickPlayMapList.Load("Gamefiles/QuickPlayMapList.xml");
      this.ExpCurve.Load("Gamefiles/ExpCurve.xml");
      foreach (string file in Directory.GetFiles("Gamefiles/Maps", "*.xml", SearchOption.TopDirectoryOnly))
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(file);
        this.Maps.Add(xmlDocument);
      }
      int id = 1;
      foreach (XmlNode childNode in this.NewbieItemsXML["items"].ChildNodes)
      {
        Item obj = new Item();
        obj.Create((XmlElement) childNode, id);
        ++id;
        this.NewbieItems.Add(obj);
      }
      foreach (string file in Directory.GetFiles("Gamefiles/ShopItems", "*.xml", SearchOption.AllDirectories))
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(file);
        if (xmlDocument["shop_item"] != null || xmlDocument["GameItem"] != null)
        {
          string key = "";
          if (xmlDocument.LastChild["mmo_stats"] != null)
          {
            foreach (XmlElement xmlElement in (XmlNode) xmlDocument.LastChild["mmo_stats"])
            {
              if (xmlElement.Attributes["name"].InnerText == "item_category")
              {
                key = xmlElement.Attributes["value"].InnerText;
                break;
              }
            }
            if (!this.ShopItemsReged.ContainsKey(key))
              this.ShopItemsReged.Add(key, new List<string>());
            this.ShopItemsReged[key].Add(xmlDocument.LastChild.Attributes["name"].InnerText);
          }
        }
      }
      this.SplitGamefiles(this.Items, ref this.ItemsSplited, 250);
      this.SplitGamefiles(this.ShopOffers, ref this.ShopOffersSplited, 250);
      this.SplitGamefiles(this.Configs, ref this.ConfigsSplited, 250);
      this.SplitGamefiles(this.QuickPlayMapList, ref this.QuickPlayMapListSplited, 250);
    }

    public void SplitGamefiles(XmlDocument _From, ref List<XmlDocument> _To, int BlockSize = 250)
    {
      int num1 = 0;
      int num2 = _From["items"].ChildNodes.Count / BlockSize;
      int num3 = 0;
      for (int index1 = 0; index1 <= num2; ++index1)
      {
        XmlDocument xmlDocument = new XmlDocument();
        if (index1 != num2)
        {
          XmlElement element = xmlDocument.CreateElement("items");
          XmlAttribute attribute1 = xmlDocument.CreateAttribute("code");
          XmlAttribute attribute2 = xmlDocument.CreateAttribute("from");
          XmlAttribute attribute3 = xmlDocument.CreateAttribute("to");
          XmlAttribute attribute4 = xmlDocument.CreateAttribute("hash");
          attribute1.Value = "2";
          attribute2.Value = num3.ToString();
          attribute3.Value = string.Format("{0}", (object) (num3 + BlockSize));
          attribute4.Value = "0";
          for (int index2 = 0; index2 < BlockSize; ++index2)
          {
            element.AppendChild(xmlDocument.ImportNode(_From["items"].ChildNodes[num1 + index2], true));
            ++num3;
          }
          element.Attributes.Append(attribute1);
          element.Attributes.Append(attribute2);
          element.Attributes.Append(attribute3);
          element.Attributes.Append(attribute4);
          xmlDocument.AppendChild((XmlNode) element);
          num1 += BlockSize;
        }
        else
        {
          BlockSize = _From["items"].ChildNodes.Count - num3;
          XmlElement element = xmlDocument.CreateElement("items");
          XmlAttribute attribute1 = xmlDocument.CreateAttribute("code");
          XmlAttribute attribute2 = xmlDocument.CreateAttribute("from");
          XmlAttribute attribute3 = xmlDocument.CreateAttribute("to");
          XmlAttribute attribute4 = xmlDocument.CreateAttribute("hash");
          attribute1.Value = "3";
          attribute2.Value = num3.ToString();
          attribute3.Value = string.Format("{0}", (object) (num3 + BlockSize));
          attribute4.Value = "0";
          for (int index2 = 0; index2 < BlockSize; ++index2)
            element.AppendChild(xmlDocument.ImportNode(_From["items"].ChildNodes[num1 + index2], true));
          element.Attributes.Append(attribute1);
          element.Attributes.Append(attribute2);
          element.Attributes.Append(attribute3);
          element.Attributes.Append(attribute4);
          xmlDocument.AppendChild((XmlNode) element);
        }
        _To.Add(xmlDocument);
      }
    }
  }
}
