using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NAudio;
using NAudio.Wave;

namespace Automation_Core.Media
{
    class MusicPlayer
    {
        public IWavePlayer player;
        private Random rnd;
        private List<int> randomList = new List<int>();
        public AudioFileReader audioFileReader;
        private bool manualStopped;
        public int Volume { get; set; }
        public int Pos { get; set; }
        public void ResetRandomization() { randomList.Clear(); Variables.logger.LogLine("Music Randomizer list has been cleared"); }
        public void SetVol()
        {
            if(Volume > 100 || Volume > Variables.MaxVolume)
            {
                Volume = 100;
                Variables.logger.LogLine(3, "Volume level attempted to set to an invalid choice, this is a bug, please submit a bug report.");
                return;
            }
            if (Volume < 0)
            {
                Volume = 0;
                Variables.logger.LogLine(3, "Volume level attempted to set to an invalid choice, this is a bug, please submit a bug report.");
                return;
            }
            Variables.logger.LogLine("Setting the playback volume to " + Volume + "%");
            audioFileReader.Volume = (float)Volume / 100;
        }

        public void setup()
        {
            Variables.logger.LogLine("Begin setup of music player");
            Volume = 20;
            Variables.logger.LogLine("Volume set to 20");
            rnd = new Random();
            Variables.logger.LogLine("rnd generator initialized");
            refreshPlayList();
            Variables.logger.LogLine("Playlist refreshed");
            Variables.logger.LogLine(1, "Music player setup successful.");
        }

        public void refreshPlayList()
        {
            Variables.musicFiles = Directory.GetFiles(Variables.MediaPath);
            randomList.Clear();
            findNext();
        }
        public void playSong(string Filename)
        {
            audioFileReader = new AudioFileReader(Filename);
            player.Init(audioFileReader);
            Play();
        }
        public void Stop()
        {
            manualStopped = true;
            player.Stop();
        }
        public void findNext()
        {
            Variables.logger.LogLine("Begin next song randomizer");
            if (randomList.Count == Variables.musicFiles.Length)
            {
                Variables.logger.LogLine("Song list length is equal to random number list length, random number list will be emptied");
                randomList.Clear();
            }
            int nextSong;
            do
                nextSong = rnd.Next(Variables.musicFiles.Length); 
            while (randomList.Contains(nextSong));
            Variables.logger.LogLine("Found unique song ID: " + nextSong.ToString());
            Variables.curMusicFile = nextSong;
            Variables.logger.LogLine("Song assigned as current file");
            if (!randomList.Contains(Variables.curMusicFile))
            {
                randomList.Add(Variables.curMusicFile);
                Variables.logger.LogLine("Song ID: " + nextSong + " is not found in random list, it will now be added.");
            }
        }
        public void Next()
        {
            findNext();
            BeginPlay();
        }
        public void Prev()
        {
            Variables.curMusicFile--;
            BeginPlay();
        }
        public void BeginPlay()
        {
            Variables.logger.LogLine("Begin song triggered");
            if (player !=null && player.PlaybackState != PlaybackState.Stopped)
            {
                Variables.logger.LogLine("Player exists and is playing, playback will be stopped");
                Stop();
            }
            if(audioFileReader != null)
            {
                Variables.logger.LogLine("Audio file reader exists, disposing of it to recreate it");
                try {
                    audioFileReader.Dispose();
                }
                catch (Exception ex)
                {
                    Variables.logger.LogLine(ex.Message);
                }
            }
            if (player != null)
            {
                Variables.logger.LogLine("Player exists, disposing of it to recreate it.");
                player.Dispose();
                player = null;
            }
            player = new WaveOutEvent();
            Variables.logger.LogLine("New player created");
            audioFileReader = new AudioFileReader(Variables.musicFiles[Variables.curMusicFile]);
            Variables.logger.LogLine("New audio file reader created with the file: " + Variables.musicFiles[Variables.curMusicFile]);
            audioFileReader.Volume = (float)Volume / 100;
            Variables.logger.LogLine("volume float value is equal to: " + audioFileReader.Volume.ToString());
            player.Init(audioFileReader);
            Variables.logger.LogLine("adding the audio file reader to the player");
            player.PlaybackStopped += (sender, evn) => { if(!manualStopped) Next(); Variables.logger.LogLine("Playback has been stopped, manual stop is " + manualStopped.ToString()); };
            Play();
            Variables.logger.LogLine(1, "Playback begining of song file name: " + Variables.musicFiles[Variables.curMusicFile]);
        }
        public void Play()
        {
            manualStopped = false;
            player.Play();
        }
        public void Pause()
        {
            player.Pause();
        }
    }
}
