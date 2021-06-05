using System;
using Terraria;

namespace TeraInfo
{
    public static class TeraInfoTime
    {
        public const double sec = 1;
        public const double min = sec * 60;
        public const double hour = min * 60;
        public const double daytime = hour * 4 + min * 30;
        public const double night = hour * 19 + min * 30;
        public const double am12 = hour * 24;

        private static double beforeTime;

        public static void Initialize()
        {
            beforeTime = -1;
        }

        public static void CheckTime()
        {
            double fullTime = Get24Time();
            if (beforeTime < 0)
            {
                beforeTime = Get24Time();
            }
            else
            {
                if (fullTime < beforeTime)
                {
                    OnChangeDate();
                }
                beforeTime = fullTime;
            }
        }

        public static void OnChangeDate()
        {
            TeraInfoWorld.elapsedDays++;
        }

        /// <summary>
        /// 現在のゲーム内時間を24時間形式で取得する。
        /// </summary>
        /// <returns>hh:mm</returns>
        public static string GetTime()
        {
            return _GetTime();
        }
        /// <summary>
        /// 現在のゲーム内時間を秒も含めて、24時間形式で取得する。
        /// </summary>
        /// <returns>hh:mm:ss</returns>
        public static string GetTimePlusSecond()
        {
            return _GetTime(true);
        }

        public static string GetDate()
        {
            string result = string.Empty;
            try
            {
                DateTime date = new DateTime(2016, 12, 31);
                date = date.AddDays(TeraInfoWorld.elapsedDays);
                int year = date.Year - 2016;
                result = $"{year}/{date.ToString("MM/dd(ddd)", new System.Globalization.CultureInfo("ja"))}";
            }
            catch (Exception e)
            {
                TeraInfoError.OnError(e);
            }
            return result;
        }

        public static string GetTimeAndElapsedDays()
        {
            return $"{GetTime()} ({TeraInfoWorld.elapsedDays})";
        }

        public static string GetDateTime()
        {
            return $"{GetDate()} {GetTime()}";
        }

        public static string GetDateTimeAndElapsedDays()
        {
            return $"{GetDate()} {GetTime()} ({TeraInfoWorld.elapsedDays})";
        }

        public static double Get24Time()
        {
            //昼なら04時間と30分を加算、夜なら19時間と30分を加算する
            double result = Main.time + (Main.dayTime ? daytime : night);

            //00:00を超える場合、00:00を0からとするため減算
            if (am12 < result)
                result -= am12;

            return result;
        }

        /// <summary>
        /// 現在のゲーム内時間を24時間形式で取得する。
        /// </summary>
        /// <remarks>
        /// ゲーム内時間は、Terraria.Main.time で取得する。
        /// Main.time は 1 で、1秒のため、60 でゲームない時間の 1分となる。
        /// Main.time は 04:30 と 19:30 でリセットされる。
        /// 04:30 - 19:30 が日中、19:31 - 04:29 が夜間とされ、日中は Main.dayTime が true となる。
        /// </remarks>
        /// <param name="getSecond">秒を取得する場合、trueを指定する。デフォルトはfalse</param>
        /// <returns>hh:mm、getSecondにtrueを指定した場合、hh:mm:ss</returns>
        private static string _GetTime(bool getSecond = false)
        {
            string result = string.Empty;
            try
            {
                double time = Get24Time();
                result = $"{(int)(time / hour % 24):D2}:{(int)(time / min % 60):D2}" + (getSecond ? $":{(int)(time % 60):D2}" : "");
            }
            catch (Exception e)
            {
                TeraInfoLogger.Write(e.Message);
            }
            return result;
        }

        /// <summary>
        /// 指定した秒数を経過していればtrueを返す
        /// </summary>
        public static bool CheckElapsedTime(DateTime startTime, DateTime endTime, int elapsedSec)
        {
            bool result = false;
            TimeSpan ts = endTime - startTime;
            result = elapsedSec < ts.TotalSeconds;
            return result;
        }

    }
}
