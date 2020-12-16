// Decompiled with JetBrains decompiler
// Type: OnyxServer.Program
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using NLog;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Threading;

namespace WARTLS
{
    internal class Program
    {
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        public static void WriteLine(string Text, ConsoleColor Color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = Color;
            Console.WriteLine(Text);
            Console.ResetColor();
        }
        public static Logger logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            ServicePointManager.ServerCertificateValidationCallback = ((object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true);
            Console.Title = $"WarTLS 4.1 Fun Version";
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($@" __      __             ___________.____       _________
/  \    /  \_____ ______\__    ___/|    |     /   _____/
\   \/\/   /\__  \\_  __ \|    |   |    |     \_____  \ 
 \        /  / __ \|  | \/|    |   |    |___  /        \
  \__/\  /  (____  /__|   |____|   |_______ \/_______  /
       \/        \/                        \/        \/   ");
            Console.ResetColor();
            // Не удалять.
            Console.WriteLine("Создатель эмулятора - Илья Петров. Доработал его - n1kodim, lovecode(myrka).");
            Console.WriteLine("Скачано с портала alt-v.ru");
            new Core();
            Thread.Sleep(-1);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
        }
    }
}

