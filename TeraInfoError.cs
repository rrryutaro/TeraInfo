using System;
using Microsoft.Xna.Framework.Graphics;

namespace TeraInfo
{
    public static class TeraInfoError
    {
        public static void OnErrorTrace(Exception e)
        {
            TeraInfoLogger.Write(e.StackTrace);
        }

        public static void OnError(Exception e, object[] param = null)
        {
            string msg = $"エラーメッセージ：{e.Message}";
            if (param != null)
            {
                foreach (var x in param)
                {
                    switch (x.GetType().Name)
                    {
                        case "Texture2D":
                            Texture2D texture2D = (Texture2D)x;
                            msg += $"\nパラメーター：{texture2D.Name}";
                            break;
                        default:
                            msg += $"\nパラメーター:{x.ToString()}";
                            break;
                    }
                    //    Type
                }
            }
            msg += $"\nトレース:\n{e.StackTrace}";
            TeraInfoLogger.Write(msg);
        }
    }
}
