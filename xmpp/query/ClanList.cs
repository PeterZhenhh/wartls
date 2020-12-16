// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.ClanList
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using MySql.Data.MySqlClient;
using WARTLS.CLASSES;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class ClanList : Stanza
  {
    public ClanList(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
      this.Process();
    }

    public override void Process()
    {
      using (MySqlConnection result = SQL.GetConnection().GetAwaiter().GetResult())
      {
        try
        {
          XDocument xDocument = new XDocument();
          XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
          xelement1.Add((object) new XAttribute((XName) "type", (object) "result"));
          xelement1.Add((object) new XAttribute((XName) "from", (object) this.To));
          xelement1.Add((object) new XAttribute((XName) "to", (object) this.User.JID));
          xelement1.Add((object) new XAttribute((XName) "id", (object) this.Id));
          XElement xelement2 = new XElement(Stanza.NameSpace + "query");
          XElement xelement3 = new XElement((XName) "clan_list");
          XElement xelement4 = (XElement) null;
          if (this.User.Player.Clan.ID == 0L)
          {
            xelement4 = new XElement((XName) "clan_performance", (object) new XAttribute((XName) "position", (object) 0));
          }
          else
          {
            using (MySqlDataReader mySqlDataReader = new MySqlCommand("SELECT ID FROM clans ORDER BY Points DESC;", result).ExecuteReader())
            {
              try
              {
                int num = 0;
                while (mySqlDataReader.Read())
                {
                  ++num;
                  if (mySqlDataReader.GetInt64(0) == this.User.Player.Clan.ID)
                  {
                    xelement4 = new XElement((XName) "clan_performance", (object) new XAttribute((XName) "position", (object) num));
                    mySqlDataReader.Close();
                    break;
                  }
                }
                mySqlDataReader.Close();
              }
              catch (Exception ex)
              {
              }
              finally
              {
                mySqlDataReader.Close();
              }
            }
          }
          using (MySqlDataReader mySqlDataReader = new MySqlCommand("SELECT * FROM clans ORDER BY Points DESC LIMIT 10;", result).ExecuteReader())
          {
            try
            {
              while (mySqlDataReader.Read())
              {
                XElement xelement5 = XElement.Parse(mySqlDataReader.GetString(5));
                xelement4.Add((object) new XElement((XName) "clan", new object[8]
                {
                  (object) new XAttribute((XName) "name", (object) mySqlDataReader.GetString(1)),
                  (object) new XAttribute((XName) "clan_id", (object) mySqlDataReader.GetInt64(0)),
                  (object) new XAttribute((XName) "master", (object) mySqlDataReader.GetString(6)),
                  (object) new XAttribute((XName) "clan_points", (object) mySqlDataReader.GetInt32(4)),
                  (object) new XAttribute((XName) "members", (object) xelement5.Elements((XName) "clan_member_info").ToList<XElement>().Count),
                  (object) new XAttribute((XName) "master_badge", (object) "0"),
                  (object) new XAttribute((XName) "master_stripe", (object) "0"),
                  (object) new XAttribute((XName) "master_mark", (object) "0")
                }));
              }
            }
            catch (Exception ex)
            {
            }
            finally
            {
              mySqlDataReader.Close();
            }
          }
          xelement3.Add((object) xelement4);
          xelement2.Add((object) xelement3);
          xelement1.Add((object) xelement2);
          xDocument.Add((object) xelement1);
          this.Compress(ref xDocument);
          this.User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
        }
        catch
        {
        }
        result.Close();
      }
    }
  }
}
