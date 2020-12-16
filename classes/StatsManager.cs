// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.StatsManager
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using System.Xml;

namespace WARTLS.CLASSES
{
  public class StatsManager
  {
    private XmlDocument Doc;

    public StatsManager(XmlDocument StatDoc)
    {
      this.Doc = StatDoc;
    }

    public void IncrementPlayerStat(string StatName, long Value)
    {
      foreach (XmlNode childNode in this.Doc["stats"].ChildNodes)
      {
        if (childNode.Attributes["stat"].InnerText == StatName)
        {
          childNode.Attributes[nameof (Value)].InnerText = (long.Parse(childNode.Attributes[nameof (Value)].InnerText) + Value).ToString();
          return;
        }
      }
      XmlElement element = this.Doc.CreateElement("stat");
      XmlAttribute attribute1 = this.Doc.CreateAttribute("stat");
      XmlAttribute attribute2 = this.Doc.CreateAttribute(nameof (Value));
      attribute2.Value = Value.ToString();
      attribute1.Value = StatName;
      element.Attributes.Append(attribute1);
      element.Attributes.Append(attribute2);
      this.Doc["stats"].AppendChild((XmlNode) element);
    }

    public long GetPlayerStat(string Statname)
    {
      foreach (XmlNode childNode in this.Doc["stats"].ChildNodes)
      {
        if (childNode.Attributes["stat"].InnerText == Statname)
          return long.Parse(childNode.Attributes["Value"].InnerText);
      }
      return 0;
    }

    public void ResetPlayerStat(string Statname, long Value)
    {
      foreach (XmlNode childNode in this.Doc["stats"].ChildNodes)
      {
        if (childNode.Attributes["stat"].InnerText == Statname)
          childNode.Attributes[nameof (Value)].InnerText = Value.ToString();
      }
    }

    public void IncrementModePlayerStat(string Mode, string StatName, long Value)
    {
      foreach (XmlNode childNode in this.Doc["stats"].ChildNodes)
      {
        if (childNode.Attributes["stat"].InnerText == StatName && childNode.Attributes["mode"].InnerText == Mode)
        {
          childNode.Attributes[nameof (Value)].InnerText = (long.Parse(childNode.Attributes[nameof (Value)].InnerText) + Value).ToString();
          return;
        }
      }
      XmlElement element = this.Doc.CreateElement("stat");
      XmlAttribute attribute1 = this.Doc.CreateAttribute("mode");
      XmlAttribute attribute2 = this.Doc.CreateAttribute("stat");
      XmlAttribute attribute3 = this.Doc.CreateAttribute(nameof (Value));
      attribute3.Value = Value.ToString();
      attribute1.Value = Mode;
      attribute2.Value = StatName;
      element.Attributes.Append(attribute1);
      element.Attributes.Append(attribute2);
      element.Attributes.Append(attribute3);
      this.Doc["stats"].AppendChild((XmlNode) element);
    }

    public void IncrementClassModePlayerStat(
      string Class,
      string Mode,
      string StatName,
      long Value)
    {
      foreach (XmlNode childNode in this.Doc["stats"].ChildNodes)
      {
        if (childNode.Attributes["stat"].InnerText == StatName && childNode.Attributes["class"].InnerText == Class && childNode.Attributes["mode"].InnerText == Mode)
        {
          childNode.Attributes[nameof (Value)].InnerText = (long.Parse(childNode.Attributes[nameof (Value)].InnerText) + Value).ToString();
          return;
        }
      }
      XmlElement element = this.Doc.CreateElement("stat");
      XmlAttribute attribute1 = this.Doc.CreateAttribute("class");
      XmlAttribute attribute2 = this.Doc.CreateAttribute("mode");
      XmlAttribute attribute3 = this.Doc.CreateAttribute("stat");
      XmlAttribute attribute4 = this.Doc.CreateAttribute(nameof (Value));
      attribute4.Value = Value.ToString();
      attribute1.Value = Class;
      attribute2.Value = Mode;
      attribute3.Value = StatName;
      element.Attributes.Append(attribute1);
      element.Attributes.Append(attribute2);
      element.Attributes.Append(attribute3);
      element.Attributes.Append(attribute4);
      this.Doc["stats"].AppendChild((XmlNode) element);
    }

    public void IncrementDifficultyModePlayerStat(
      string Difficulty,
      string Mode,
      string StatName,
      long Value)
    {
      foreach (XmlNode childNode in this.Doc["stats"].ChildNodes)
      {
        if (childNode.Attributes["stat"].InnerText == StatName && childNode.Attributes["difficulty"].InnerText == Difficulty && childNode.Attributes["mode"].InnerText == Mode)
        {
          childNode.Attributes[nameof (Value)].InnerText = (long.Parse(childNode.Attributes[nameof (Value)].InnerText) + Value).ToString();
          return;
        }
      }
      XmlElement element = this.Doc.CreateElement("stat");
      XmlAttribute attribute1 = this.Doc.CreateAttribute("difficulty");
      XmlAttribute attribute2 = this.Doc.CreateAttribute("mode");
      XmlAttribute attribute3 = this.Doc.CreateAttribute("stat");
      XmlAttribute attribute4 = this.Doc.CreateAttribute(nameof (Value));
      attribute4.Value = Value.ToString();
      attribute1.Value = Difficulty;
      attribute2.Value = Mode;
      attribute3.Value = StatName;
      element.Attributes.Append(attribute1);
      element.Attributes.Append(attribute2);
      element.Attributes.Append(attribute3);
      element.Attributes.Append(attribute4);
      this.Doc["stats"].AppendChild((XmlNode) element);
    }

    public void IncrementWeaponUsage(string ClassName, string Weapon, long Value)
    {
      foreach (XmlNode childNode in this.Doc["stats"].ChildNodes)
      {
        if (childNode.Attributes["stat"].InnerText == "player_wpn_usage" && childNode.Attributes["class"].InnerText == ClassName)
        {
          childNode.Attributes[nameof (Value)].InnerText = (long.Parse(childNode.Attributes[nameof (Value)].InnerText) + Value).ToString();
          break;
        }
      }
    }
  }
}
