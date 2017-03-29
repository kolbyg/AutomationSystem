using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation_Core.Media;

namespace Automation_Core.Control
{
    class Media
    { 
        public static string Play()
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            if(Variables.InternalMediaPlayerEnabled)
            {
                Variables.musicPlayer.Play();
                return "Media Playing";
            }
            else
            {
                //run command on all nodes
                for (int x = 0; x < Variables.nodes.Length; x++)
                {
                    if (Variables.nodes[x] == null)
                        continue;
                    Play(x);
                }
                return "Internal player disabled, sending command to all nodes";
            }
        }
        public static string Play(int NodeID)
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            switch (Variables.nodes[NodeID].Type)
            {
                case "MPD":
                    MPDControl.Play();
                    return "Media Playing";
            }
            return "Node " + NodeID + " is not a media player, skipping";
        }
        public static string Pause()
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            if (Variables.InternalMediaPlayerEnabled)
            {
                Variables.musicPlayer.Pause();
                return "Media Paused";
            }
            else
            {
                //run command on all nodes
                for (int x = 0; x < Variables.nodes.Length; x++)
                {
                    if (Variables.nodes[x] == null)
                        continue;
                    Pause(x);
                }
                return "Internal player disabled, sending command to all nodes";
            }
        }
        public static string Pause(int NodeID)
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            switch (Variables.nodes[NodeID].Type)
            {
                case "MPD":
                    MPDControl.Pause();
                    return "Media Paused";
            }
            return "Node " + NodeID + " is not a media player, skipping";
        }
        public static string Stop()
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            if (Variables.InternalMediaPlayerEnabled)
            {
                Variables.musicPlayer.Stop();
                return "Media Stopped";
            }
            else
            {
                //run command on all nodes
                for (int x = 0; x < Variables.nodes.Length; x++)
                {
                    if (Variables.nodes[x] == null)
                        continue;
                    Stop(x);
                }
                return "Internal player disabled, sending command to all nodes";
            }
        }
        public static string Stop(int NodeID)
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            switch (Variables.nodes[NodeID].Type)
            {
                case "MPD":
                    MPDControl.Stop();
                    return "Media Stopped";
            }
            return "Node " + NodeID + " is not a media player, skipping";
        }
        public static string Next()
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            if (Variables.InternalMediaPlayerEnabled)
            {
                Variables.musicPlayer.Next();
                return "Media Advanced";
            }
            else
            {
                //run command on all nodes
                for (int x = 0; x < Variables.nodes.Length; x++)
                {
                    if (Variables.nodes[x] == null)
                        continue;
                    Next(x);
                }
                return "Internal player disabled, sending command to all nodes";
            }
        }
        public static string Next(int NodeID)
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            switch (Variables.nodes[NodeID].Type)
            {
                case "MPD":
                    MPDControl.Next();
                    return "Media Advanced";
            }
            return "Node " + NodeID + " is not a media player, skipping";
        }
        public static string Prev()
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            if (Variables.InternalMediaPlayerEnabled)
            {
                return "Function not supported by the internal media player, command ignored.";
            }
            else
            {
                //run command on all nodes
                for (int x = 0; x < Variables.nodes.Length; x++)
                {
                    if (Variables.nodes[x] == null)
                        continue;
                    Prev(x);
                }
                return "Internal player disabled, sending command to all nodes";
            }
        }
        public static string Prev(int NodeID)
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            switch (Variables.nodes[NodeID].Type)
            {
                case "MPD":
                    MPDControl.Prev();
                    return "Media Backed up";
            }
            return "Node " + NodeID + " is not a media player, skipping";
        }
        public static string ResetRandomization()
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            if (Variables.InternalMediaPlayerEnabled)
            {
                Variables.musicPlayer.ResetRandomization();
                return "Randomization reset";
            }
            else
            {
                return "This function is only supported by the internal media player, command ignored.";
            }
        }
        public static string VolUp()
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            if (Variables.CurVolume == Variables.MaxVolume)
                return "Volume is at limit set by volume governer, cannot be set higher";
            if(Variables.CurVolume == 100)
            {
                return "Volume is at 100, cannot be set higher";
            }
            Variables.CurVolume++;
            SetVol(Variables.CurVolume);
            return "Volume turned up, current level: " + Variables.CurVolume;
        }
        public static string InitNodePlayer(int NodeID)
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            if (Variables.nodes[NodeID].Type == "MPD")
            {
                MPDControl.setup(NodeID);
                return "Media player MPD has been started";
            }
            return "Invalid Music Player";
        }
        public static string InitInternalPlayer()
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            Variables.musicPlayer = new Automation_Core.Media.MusicPlayer();
                Variables.musicPlayer.setup();
                Variables.musicPlayer.BeginPlay();
                return "Media player INTERNAL has been started";
        }
        public static string VolDown()
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            if (Variables.CurVolume == 0)
                return "Volume is at 0, cannot be set any lower.";
            Variables.CurVolume--;
            SetVol(Variables.CurVolume);
            return "Volume turned down, current level: " + Variables.CurVolume;
        }
        public static string SetVol(int Value)
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            if (Value > 100 || Value < 0)
            {
                Value = 0;
                return "Chosen volume is invalid, acceptable values are from 0-100";
            }
            if (Value > Variables.MaxVolume)
            {
                Value = Variables.MaxVolume;
                return "Chosen volume is outside the allowable range set by the volume governer, acceptable values are 0-" + Variables.MaxVolume;
            }
            Variables.CurVolume = Value;
            if (Variables.InternalMediaPlayerEnabled)
            {
                Variables.musicPlayer.SetVol();
                return "Volume has been set to " + Variables.CurVolume;
            }
            else
            {
                //run command on all nodes
                for (int x = 0; x < Variables.nodes.Length; x++)
                {
                    if (Variables.nodes[x] == null)
                        continue;
                    SetVol(x, Value);
                }
                return "Internal player disabled, sending command to all nodes";
            }
        }
        public static string SetVol(int NodeID, int Value)
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            if (Value > 100 || Value < 0)
            {
                Value = 0;
                return "Chosen volume is invalid, acceptable values are from 0-100";
            }
            if (Value > Variables.MaxVolume)
            {
                Value = Variables.MaxVolume;
                return "Chosen volume is outside the allowable range set by the volume governer, acceptable values are 0-" + Variables.MaxVolume;
            }
            Variables.CurVolume = Value;
            switch (Variables.nodes[NodeID].Type)
            {
                case "MPD":
                    MPDControl.SetVol();
                    return "Volume has been set to " + Variables.CurVolume;
            }
            return "Node " + NodeID + " is not a media player, skipping";
        }
        public static string LoadPL(string PLName)
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            if (Variables.InternalMediaPlayerEnabled)
            {
                return "Function not supported by the internal media player, command ignored.";
            }
            else
            {
                //run command on all nodes
                for (int x = 0; x < Variables.nodes.Length; x++)
                {
                    if (Variables.nodes[x] == null)
                        continue;
                    LoadPL(x, PLName);
                }
                return "Internal player disabled, sending command to all nodes";
            }
        }
        public static string LoadPL(int NodeID, string PLName)
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            switch (Variables.nodes[NodeID].Type)
            {
                case "MPD":
                    MPDControl.LoadPL(PLName);
                    return "Playlist changed";
            }
            return "Node " + NodeID + " is not a media player, skipping";
        }
        public static int GetVol()
        {
            return Variables.CurVolume;
        }
        public static string GetSong()
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            if (Variables.InternalMediaPlayerEnabled)
            {
                return Variables.musicFiles[Variables.curMusicFile].Substring(Variables.musicFiles[Variables.curMusicFile].LastIndexOf('\\') + 1);
            }
            else
            {
                return "Command error, internal music player is disabled, please send this command with a node id argument to address a node specifically";
            }
        }
        public static string GetSong(int NodeID)
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            if (Variables.nodes[NodeID].Type == "MPD")
            {
                return MPDControl.GetSong();
            }
            return "Invalid Music Player";
        }
        public static string Rescan()
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            if (Variables.InternalMediaPlayerEnabled)
            {
                Variables.musicPlayer.refreshPlayList();
                return "All media folders have been rescanned.";
            }
            else
            {
                //run command on all nodes
                for (int x = 0; x < Variables.nodes.Length; x++)
                {
                    if (Variables.nodes[x] == null)
                        continue;
                    Rescan(x);
                }
                return "Internal player disabled, sending command to all nodes";
            }
        }
        public static string Rescan(int NodeID)
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            switch (Variables.nodes[NodeID].Type)
            {
                case "MPD":
                    MPDControl.Rescan();
                    return "Media rescanned";
            }
            return "Node " + NodeID + " is not a media player, skipping";
        }
        public static string Resume()
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            if (Variables.InternalMediaPlayerEnabled)
            {
                return Play();
            }
            else
            {
                return "This function is only supported by the internal media player, command ignored.";
            }
        }
        public static string Shuffle()
        {
            if (!Variables.MediaPlayerEnabled)
                return "Media player function is not enabled";
            if (Variables.InternalMediaPlayerEnabled)
            {
                return "Function not supported by current media player, command ignored.";
            }
            return "Invalid Music Player";
        }
    }
}
