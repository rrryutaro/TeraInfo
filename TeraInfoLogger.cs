using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;

namespace TeraInfo
{
    public static class TeraInfoLogger
    {
        public static string logPath = $@"{Main.SavePath}\Logs\TeraInfoLog.txt";
        public static StreamWriter sw = null;
        public static TextWriter tw = null;
        public static DateTime BeforeTime = DateTime.Now;
        public static string BeforeMessage = "";

        public static void Close()
        {
            if (tw != null)
            {
                tw.Close();
            }
            if (sw != null)
            {
                sw.Close();
            }
        }

        public static string GetDateTimeString(DateTime time)
        {
            string result = time.ToString("[yyyy/MM/dd(ddd) HH:mm:ss.ff]", new System.Globalization.CultureInfo("ja"));
            return result;
        }

        public static void Write(object obj)
        {
            Write(Convert(obj));
        }
        public static void Write2(string msg)
        {
            try
            {
                if (sw == null)
                {
                    sw = new StreamWriter(new FileStream(logPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
                    tw = TextWriter.Synchronized(sw);
                }


                string line = $"{GetDateTimeString(DateTime.Now)}{"\t"}{TeraInfoTime.GetDateTimeAndElapsedDays()}{"\t"}{msg}";
                tw.WriteLine(line);
            }
            catch (Exception e)
            {
                //
            }
        }
        public static void Write2(string path, string msg)
        {
            try
            {
                DateTime now = DateTime.Now;

                if (BeforeMessage.Equals(msg) && (now - BeforeTime).TotalMilliseconds < 1)
                {
                    return;
                }

                using (StreamWriter sw = new StreamWriter(new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read)))
                using (TextWriter tw = TextWriter.Synchronized(sw))
                {
                    string line = $"{GetDateTimeString(now)}{"\t"}{TeraInfoTime.GetDateTimeAndElapsedDays()}{"\t"}{msg}";
                    tw.WriteLine(line);
                }
                BeforeTime = now;
                BeforeMessage = msg;
            }
            catch (Exception e)
            {
                //
            }
        }


        public static void Write(string msg, bool time = true)
        {
            try
            {
                if (sw == null)
                {
                    sw = new StreamWriter(new FileStream(logPath, FileMode.Append, FileAccess.Write, FileShare.Read));
                    tw = TextWriter.Synchronized(sw);
                }

                string line;
                if (time)
                    line = $"{GetDateTimeString(DateTime.Now)} {msg}";
                else
                    line = msg;

                tw.WriteLine(line);
            }
            catch (Exception e)
            {
                //
            }
        }

        public static void Write(string path, string msg, bool time = true)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read)))
                using (TextWriter tw = TextWriter.Synchronized(sw))
                {
                    string line;
                    if (time)
                        line = $"{GetDateTimeString(DateTime.Now)} {msg}";
                    else
                        line = msg;

                    tw.WriteLine(line);
                }
            }
            catch (Exception e)
            {
                //
            }
        }

        public static string Convert(object obj)
        {
            string result = obj.ToString();
            switch (obj.GetType().Name)
            {
                case "Vector2":
                    result = GetStringVector2((Vector2)obj);
                    break;

                case "Exception":
                    result = ((Exception)obj).Message;
                    break;
            }
            return result;
        }

        public static string GetStringVector2(Vector2 pos)
        {
            string result = $"{{X: {pos.X}, Y: {pos.Y}}}";
            return result;
        }
        public static string GetStringUIElement(UIElement elem)
        {
            string result = $"{{Top: {elem.Top.Pixels}, Left: {elem.Left.Pixels}, Width: {elem.Width.Pixels}, Height: {elem.Height.Pixels}}}";
            return result;
        }
    }
}
