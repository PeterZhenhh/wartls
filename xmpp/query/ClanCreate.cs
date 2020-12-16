// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.ClanCreate
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using MySql.Data.MySqlClient;
using WARTLS.CLASSES;
using WARTLS.EXCEPTION;
using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class ClanCreate : Stanza
  {
    private Clan Clan;

    public ClanCreate(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      using (MySqlConnection result = SQL.GetConnection().GetAwaiter().GetResult())
      {
        using (MySqlDataReader mySqlDataReader = new MySqlCommand("SELECT * FROM clans WHERE Name='" + this.Query.Attributes["clan_name"].InnerText + "'", result).ExecuteReader())
        {
          if (mySqlDataReader.HasRows)
          {
            mySqlDataReader.Close();
            throw new StanzaException(User, Packet, 4);
          }
          mySqlDataReader.Close();
        }
        this.Clan = new Clan(this.Query.Attributes["clan_name"].InnerText, Encoding.UTF8.GetString(Convert.FromBase64String(this.Query.Attributes["description"].InnerText)), User);
        this.Process();
        if (User.Player.RoomPlayer.Room != null)
          User.Player.RoomPlayer.Room.Sync((Client) null);
                Console.WriteLine(User.Player.Nickname + "clan created with name:" + this.Query.Attributes["clan_name"].InnerText);
                result.Close();
      }
    }

    public override void Process()
    {
      XDocument xDocument = new XDocument();
      XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
      xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
      xelement1.Add((object) new XAttribute((XName) "from", (object) this.To));
      xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
      xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
      XElement xelement2 = new XElement(Stanza.NameSpace + "query");
      XElement xelement3 = new XElement((XName) "clan_create");
      xelement3.Add((object) this.Clan.Serialize(this.User));
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xDocument.Add((object) xelement1);
      this.Compress(ref xDocument);
      this.User.Send(xDocument.ToString());
    }
  }
}
