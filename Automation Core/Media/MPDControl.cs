using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libmpc;

namespace Automation_Core.Media
{
    class MPDControl
    {
        private static Mpc mpc = new Mpc();
        private static System.Net.IPAddress IP = System.Net.IPAddress.Parse("172.17.2.57");
        private static System.Net.IPEndPoint IPPort = new System.Net.IPEndPoint(IP, 6600);
        public static int Pos { get; set; }
        public static void SetVol()
        {
            if (!mpc.Connected)
                Connect();
            //check if the requested volume is outside of the acceptable range
            if (Variables.CurVolume > 100 || Variables.CurVolume > Variables.MaxVolume)
            {
                Variables.CurVolume = 100;
                Variables.logger.LogLine(3, "Volume level attempted to set to an invalid choice, this is a bug, please submit a bug report.");
                return;
            }
            if (Variables.CurVolume < 0)
            {
                Variables.CurVolume = 0;
                Variables.logger.LogLine(3, "Volume level attempted to set to an invalid choice, this is a bug, please submit a bug report.");
                return;
            }
            //set the volume
            Variables.logger.LogLine("Setting the playback volume to " + Variables.CurVolume + "%");
            mpc.SetVol(Variables.CurVolume);
        }

        public static void setup()
        {
            //Set the volume to 70, resonable level for mpd
            Variables.CurVolume = 70;
            //Create a connection and assign it to mpc, connect
            mpc.Connection = new MpcConnection();
            mpc.Connection.Server = IPPort;
            Connect();
        }
        public static void Connect()
        {
            //do the connect
            try
            {
                mpc.Connection.Connect();
            }
            catch(Exception ex)
            {
                Variables.logger.LogLine("MPD Connection error: " + ex.Message);
                return;
            }
            //initial operations
            SetVol();
            LoadPL("BGMusic");
        }
        public static void Rescan()
        {
            if (!mpc.Connected)
                Connect();
            //rescan the directories
            mpc.Update();
        }
        public static string GetSong()
        {
            if (!mpc.Connected)
                Connect();
            //get the current song and assign it to the local song variable
            MpdFile song = mpc.CurrentSong();
            //check if song actually exists before returning it
            if (song == null)
                return "No song playing";
            else
                return song.File;
        }
        public static void Stop()
        {
            if (!mpc.Connected)
                Connect();
            //stop playing
            mpc.Stop();
        }
        public static void LoadPL(string Name)
        {
            if (!mpc.Connected)
                Connect();

            //clear the current playlist first
            mpc.Clear();
            //load named playlist into current
            mpc.Load(Name);
            //shuffle the tracks
            mpc.Shuffle();
            //begin playback
            mpc.Play();
        }
        public static void Next()
        {
            if (!mpc.Connected)
                Connect();
            //advance the track
            mpc.Next();
        }
        public static void Prev()
        {
            if (!mpc.Connected)
                Connect();
            //go back a track
            mpc.Previous();
        }
        public static void BeginPlay()
        {
        }
        public static void Play()
        {
            if (!mpc.Connected)
                Connect();
            mpc.Play();
        }
        public static void Pause()
        {
            if (!mpc.Connected)
                Connect();
            //pause
            mpc.Pause(true);
        }
        public static void UnPause()
        {
            if (!mpc.Connected)
                Connect();
            //unpause
            mpc.Pause(false);
        }
    }
}
