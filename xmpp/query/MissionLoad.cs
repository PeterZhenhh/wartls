// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.MissionLoad
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using WARTLS.CLASSES.GAMEROOM;
using WARTLS.EXCEPTION;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
    internal class MissionLoad : Stanza
    {
        private string Channel;

        private GameRoom Room;

        public MissionLoad(Client User, XmlDocument Packet = null)
            : base(User, Packet)
        {
            try
            {
                if (Type == "result" && Query.Attributes["load_result"].InnerText != "success")
                {
                    new MissionUnload(User);
                }
                else if (Type == null)
                {
                    User.DedicatedTelemetryes = new Dictionary<string, XElement>();
                    byte[] buffer = new byte[4];
                    User.Player.Random.NextBytes(buffer);
                    Room = User.Player.RoomPlayer.Room;
                    Room.Session.Status = 2;
                    Room.Session.Revision++;
                    Room.Session.ID = Room.GetHashCode();
                    Room.Sync();
                }
                User.Player.RoomPlayer.Room.Sync();
            }
            catch { }
        }

        public override void Process()
        {
            if (!(Type == "result"))
            {
                XDocument xDocument = new XDocument();
                XElement xElement = new XElement(Gateway.JabberNS + "iq");
                xElement.Add(new XAttribute("type", "get"));
                xElement.Add(new XAttribute("from", $"masterserver@warface/wartls"));
                xElement.Add(new XAttribute("to", User.JID));
                xElement.Add(new XAttribute("id", User.Player.Random.Next(99999, int.MaxValue)));
                XElement xElement2 = new XElement(Stanza.NameSpace + "query");
                XElement xElement3 = new XElement("mission_load");
                xElement3.Add(new XAttribute("bootstrap_mode", "0"));
                xElement3.Add(new XAttribute("bootstrap_name", "mycom"));
                xElement3.Add(new XAttribute("session_id", Room.Session.ID));
                xElement3.Add(new XAttribute("verbosity_level", "0"));
                xElement3.Add(Room.Serialize(IncludeData: true));
                xElement3.Add(XDocument.Parse(Core.GameResources.OnlineVariables["variables"].OuterXml.Replace("variables", "online_variables")).Root);
                xElement2.Add(xElement3);
                xElement.Add(xElement2);
                xDocument.Add(xElement);
                Compress(ref xDocument);
                User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
            }
        }
    }
}
