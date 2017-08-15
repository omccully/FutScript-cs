using System;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Media;

namespace FutScriptFunctions.Helpers
{
    public class BotHelpers
    {
        static SoundPlayer player = new SoundPlayer();

        /// <summary>
        /// Play a PCM WAV file
        /// </summary>
        /// <param name="path">Path to PCM WAV file</param>
        public static void PlaySound(string path)
        {
            player.SoundLocation = path;
            player.Play();
        }

        /// <summary>
        /// Gets the current unix timestamp
        /// </summary>
        /// <returns>The current unix timestamp</returns>
        public static long UnixTime()
        {
            return (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
        }

        public static void Wait(int ms)
        {
            Thread.Sleep(Math.Max(0, ms));
        }

        /// <summary>
        /// Ends all processes called <paramref name="name"/>
        /// </summary>
        /// <param name="name">Process name to end</param>
        public static void EndProcess(string name)
        {
            if (name == "this")
            {
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                foreach (Process proc in Process.GetProcessesByName(name.Replace(".exe", "")))
                {
                    proc.Kill();
                }
            }
        }

        /// <summary>
        /// Starts process from path
        /// </summary>
        /// <param name="path">Path to peocess</param>
        public static void StartProcess(string path)
        {
            Process.Start(path);
        }

        #region Networking
        /// <summary>
        /// Sends a simple HTTP query to <paramref name="addr"/>. 
        /// This method is for sending messages to a web server.
        /// It does not do anything with the response.
        /// </summary>
        /// <param name="addr">HTTP address</param>
        /// <returns>True if success, false if failed.</returns>
        public static bool HTTPQuery(string addr)
        {
            try
            {
                WebRequest request = WebRequest.Create(addr);
                request.Timeout = 1000;
                WebResponse response = request.GetResponse();
            }
            catch (WebException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Opens a TCP connection and sends <paramref name="message"/>.
        /// </summary>
        /// <param name="addr">IP address of TCP server</param>
        /// <param name="port">Port of TCP server</param>
        /// <param name="message">Message to send</param>
        /// <returns>True if success, false if failed.</returns>
        public static bool TCPQuery(string addr, int port, string message)
        {
            try
            {
                TcpClient tc = new TcpClient();
                tc.Connect(addr, port);
                Socket s = tc.Client;
                s.Send(System.Text.Encoding.ASCII.GetBytes(message));
                s.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
