// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.AbuseReport
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using MySql.Data.MySqlClient;
using WARTLS.CLASSES;
using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
  public class AbuseReport : Stanza
  {
    private string Channel;

    public AbuseReport(Client User, XmlDocument Packet)
      : base(User, Packet)
    {
            string innerText = this.Query.Attributes["type"].InnerText;
            string innerText2 = this.Query.Attributes["target"].InnerText;
            string text = "";
            if (this.Query.Attributes["comment"].InnerText != "")
            {
                text = Encoding.UTF8.GetString(Convert.FromBase64String(this.Query.Attributes["comment"].InnerText));
            }
            using (MySqlConnection result = SQL.GetConnection().GetAwaiter().GetResult())
            {
                MySqlCommand сmd = new MySqlCommand();
                сmd.Connection = result;
                сmd.Prepare();
                сmd.CommandText = "INSERT INTO reports(initiator, target, reason, comment) VALUES (@Nickname,@Target,@Reason,@Comment)";
                сmd.Parameters.AddWithValue("@Nickname", User.Player.Nickname);
                сmd.Parameters.AddWithValue("@Target", innerText2);
                сmd.Parameters.AddWithValue("@Reason", innerText);
                сmd.Parameters.AddWithValue("@Comment", text);
                сmd.ExecuteNonQuery();
                User.ShowMessage("Ваша жалоба успешно отправлена администрации! Спасибо за содействие.", true);
                result.Close();
            }
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
      XElement xelement3 = new XElement((XName) "abuse_report");
      xelement2.Add((object) xelement3);
      xelement1.Add((object) xelement2);
      xdocument.Add((object) xelement1);
      this.User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
    }
  }
}
