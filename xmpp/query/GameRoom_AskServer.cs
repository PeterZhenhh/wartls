// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.GameRoom_AskServer
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using WARTLS.CLASSES;
using WARTLS.CLASSES.GAMEROOM;
using WARTLS.EXCEPTION;
using System;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
	public class GameRoom_AskServer : Stanza
	{
		private enum AskServerError
		{
			NOT_MASTER = 0,
			INVALID_MISSION = 1,
			NOT_BALANCED = 3,
			ALREADY_STARTED = 7
		}

		private GameRoom Room;

		public GameRoom_AskServer(Client User, XmlDocument Packet)
			: base(User, Packet)
		{
			Room = User.Player.RoomPlayer.Room;
			Room.Players.Users.Count((Client Attribute) => Attribute.Player.RoomPlayer.Status == Status.READY);
			Room.Players.Users.Count(delegate (Client Attribute)
			{
				if (Attribute.Player.RoomPlayer.TeamId == Teams.WARFACE)
				{
					return Attribute.Player.RoomPlayer.Status == Status.READY;
				}
				return false;
			});
			Room.Players.Users.Count(delegate (Client Attribute)
			{
				if (Attribute.Player.RoomPlayer.TeamId == Teams.BLACKWOOD) return Attribute.Player.RoomPlayer.Status == Status.READY;
				return false;
			});
			Room.Dedicated = ArrayList.OnlineUsers.Find(delegate (Client Attribute)
			{
				if (Attribute.Dedicated) return Attribute.Player.RoomPlayer.Room == null;
				return false;
			});
			Room.Players.Users.Count((Client Attribute) => Attribute.Player.RoomPlayer.Status == Status.READY);
			int Warface = Room.Players.Users.Count((Client Attribute) => Attribute.Player.RoomPlayer.TeamId == Teams.WARFACE);
			int Blackwood = Room.Players.Users.Count((Client Attribute) => Attribute.Player.RoomPlayer.TeamId == Teams.BLACKWOOD);
			if (Warface >= 1 && Blackwood >= 1 || User.Player.Privilegie == PrivilegieId.ADMINISTRATOR)
			{
				if (Room.Dedicated == null)
				{
					foreach (Client user in Room.Players.Users)
					{
						user.ShowMessage("Все сервера для запуска игры заняты или отсутствуют. Пожалуйста сообщите администрации если видите данную ошибку", false);
					}
					new StanzaException(User, Packet, 7);
				}
				new Thread(Room.SessionStarter).Start();
				Process();
			}
			else
            {
				new StanzaException(User, Packet, 3);
			}
		}

		public override void Process()
		{
			XDocument xDocument = new XDocument();
			XElement xElement = new XElement(Gateway.JabberNS + "iq");
			xElement.Add(new XAttribute("type", "result"));
			xElement.Add(new XAttribute("from", To));
			xElement.Add(new XAttribute("to", User.JID));
			xElement.Add(new XAttribute("id", Id));
			XElement xElement2 = new XElement(Stanza.NameSpace + "query");
			XElement content = new XElement("gameroom_askserver");
			xElement2.Add(content);
			xElement.Add(xElement2);
			xDocument.Add(xElement);
			User.Send(xDocument.ToString(SaveOptions.DisableFormatting));
		}
	}
}
