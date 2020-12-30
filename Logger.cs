/*
    Copyright 2013 Phillip Schichtel

    This file is part of LyricsReloaded.

    LyricsReloaded is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    LyricsReloaded is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with LyricsReloaded. If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using System.Text;
using System.IO;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public class Logger
    {
        private const string LOG_FILE_NAME = "mb_tags-panel.log";

        private readonly MusicBeeApiInterface musicBeeApiInterface;

        private readonly FileInfo fileInfo;
        private StreamWriter writer;

        public Logger(MusicBeeApiInterface musicBeeApiInterface)
        {
            this.musicBeeApiInterface = musicBeeApiInterface;
            fileInfo = new FileInfo(GetLogFilePath());
            writer = null;
        }

        public FileInfo GetFileInfo()
        {
            return fileInfo;
        }

        private void Write(string type, string message, object[] args)
        {
            if (writer == null)
            {
                writer = new StreamWriter(fileInfo.FullName, true, Encoding.UTF8);
                writer.AutoFlush = false;
            }

            DateTime localTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, TimeZoneInfo.Local);
            writer.WriteLine(localTime.ToString("dd/MM/yyyy HH:mm:ss") + " [" + type.ToUpper() + "] " + string.Format(message, args));
            writer.Flush();
        }

        public void Close()
        {
            if (writer != null)
            {
                try
                {
                    writer.Close();
                }
                catch (ObjectDisposedException)
                { }
            }
        }

        public void Debug(string message, params object[] args)
        {
            Write("debug", message, args);
        }

        public void Info(string message, params object[] args)
        {
            Write("info", message, args);
        }

        public void Warn(string message, params object[] args)
        {
            Write("warn", message, args);
        }

        public void Error(string message, params object[] args)
        {
            Write("error", message, args);
        }

        public string GetLogFilePath()
        {
            return System.IO.Path.Combine(musicBeeApiInterface.Setting_GetPersistentStoragePath(), LOG_FILE_NAME);
        }
    }
}
