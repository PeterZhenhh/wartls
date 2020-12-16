// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.Channel
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES.GAMEROOM;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace WARTLS.CLASSES
{
  public class Channel
  {
    public string RankGroup = "all";
    public byte MinRank = 1;
    public byte MaxRank = 95;
    public string Bootstrap = "";
    public List<GameRoom> GameRoomList = new List<GameRoom>();
    public List<Client> Users = new List<Client>();
    public string Resource;
    public ushort ServerId;
    public string ChannelType;

    public int Online
    {
      get
      {
        return ArrayList.OnlineUsers.FindAll((Predicate<Client>) (Attribute => !Attribute.Dedicated && Attribute.Channel == this)).Count;
      }
    }

    public double Load
    {
      get
      {
        return (double) this.Online / 100.0;
      }
    }

    public string JID
    {
      get
      {
        return "global." + this.Resource;
      }
    }

    public Channel(
      string Resource,
      ushort ServerId,
      string ChannelType,
      byte MinRank,
      byte MaxRank)
    {
      this.Resource = Resource;
      this.ServerId = ServerId;
      this.ChannelType = ChannelType;
      this.MinRank = MinRank;
      this.MaxRank = MaxRank;
    }

    public XElement Serialize()
    {
      XElement xelement1 = new XElement((XName) "server");
      xelement1.Add((object) new XAttribute((XName) "resource", (object) this.Resource));
      xelement1.Add((object) new XAttribute((XName) "server_id", (object) this.ServerId));
      xelement1.Add((object) new XAttribute((XName) "channel", (object) this.ChannelType));
      xelement1.Add((object) new XAttribute((XName) "rank_group", (object) this.RankGroup));
      xelement1.Add((object) new XAttribute((XName) "load", (object) this.Load));
      xelement1.Add((object) new XAttribute((XName) "online", (object) this.Online));
      xelement1.Add((object) new XAttribute((XName) "min_rank", (object) this.MinRank));
      xelement1.Add((object) new XAttribute((XName) "max_rank", (object) this.MaxRank));
      xelement1.Add((object) new XAttribute((XName) "bootstrap", (object) this.Bootstrap));
      XElement xelement2 = new XElement((XName) "load_stats");
      XElement xelement3 = new XElement((XName) "load_stat");
      xelement3.Add((object) new XAttribute((XName) "type", (object) "quick_play"));
      xelement3.Add((object) new XAttribute((XName) "value", (object) "255"));
      XElement xelement4 = new XElement((XName) "load_stat");
      xelement4.Add((object) new XAttribute((XName) "type", (object) "survival"));
      xelement4.Add((object) new XAttribute((XName) "value", (object) "255"));
      XElement xelement5 = new XElement((XName) "load_stat");
      xelement5.Add((object) new XAttribute((XName) "type", (object) "pve"));
      xelement5.Add((object) new XAttribute((XName) "value", (object) "255"));
      xelement2.Add((object) xelement3);
      xelement2.Add((object) xelement4);
      xelement2.Add((object) xelement5);
      xelement1.Add((object) xelement2);
      return xelement1;
    }
  }
}
