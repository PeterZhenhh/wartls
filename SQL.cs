using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WARTLS
{
	// Token: 0x02000007 RID: 7
	public class SQL
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00003227 File Offset: 0x00001427
		// (set) Token: 0x0600000C RID: 12 RVA: 0x0000322E File Offset: 0x0000142E
		internal static MySqlConnectionStringBuilder MySqlConnectionString { get; private set; } = new MySqlConnectionStringBuilder();

		// Token: 0x0600000D RID: 13 RVA: 0x00003238 File Offset: 0x00001438
		public SQL()
		{
			try
			{
				SQL.MySqlConnectionString.Server = "localhost";
				SQL.MySqlConnectionString.UserID = "root";
				SQL.MySqlConnectionString.Password = "";
				SQL.MySqlConnectionString.Database = "warface";
				SQL.MySqlConnectionString.CharacterSet = "utf8";
				SQL.MySqlConnectionString.Pooling = true;
				SQL.MySqlConnectionString.Port = 3306;
				SQL.CheckDBConnect();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[{GetType().Name}] Ошибка подключения к MySql Server! [{ex.Message}]", Color.Red);
			}
		}

		private static async void CheckDBConnect()
		{
			Task<MySqlConnection> c = GetConnection();
			try
			{
				await c;
			}
			catch
			{
			}
			if (c.Exception == null)
			{
				using (c)
				{
					Console.WriteLine("[DB] Подключение установлено!", Color.DarkGreen);
					Console.WriteLine("[DB] Сервер базы данных: " + c.Result.ServerVersion, Color.DarkMagenta);
				}
				return;
			}
			Console.WriteLine("[DB] Произошла ошибка при установке подключения", Color.Red);
			Console.WriteLine("[DB] Проверьте корректность введеных данных и повторите попытку", Color.Red);
			Console.WriteLine(c.Exception.InnerException.Message, Color.Red);
		}

	
		internal static async Task<MySqlConnection> GetConnection()
		{
			MySqlConnection sqlConnection = new MySqlConnection(SQL.MySqlConnectionString.ToString());
			Task<MySqlConnection> task = new Task<MySqlConnection>(delegate ()
			{
				sqlConnection.Open();
				return sqlConnection;
			});
			task.Start();
			return await task;
		}
	}
}
