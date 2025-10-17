using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintJobInterceptor.UI.Helpers
{
    public enum FeedbackType
    {
        Success,
        Info,
        Warning,
        Critical
    }


    public record FeedbackStyle(Color BackColor, Color IconColor, IconChar Icon);


    public static class FeedbackManager
    {
        public static FeedbackStyle GetStyle(FeedbackType type)
        {
            switch (type)
            {
                case FeedbackType.Success:
                    return new FeedbackStyle(
                        BackColor: Color.FromArgb(57, 61, 27),
                        IconColor: Color.FromArgb(108, 203, 95),
                        Icon: IconChar.CheckCircle);

                case FeedbackType.Info:
                    return new FeedbackStyle(
                        BackColor: Color.FromArgb(39, 39, 39),
                        IconColor: Color.FromArgb(96, 205, 255),
                        Icon: IconChar.CircleInfo);

                case FeedbackType.Warning:
                    return new FeedbackStyle(
                        BackColor: Color.FromArgb(67, 53, 25),
                        IconColor: Color.FromArgb(252, 225, 0),
                        Icon: IconChar.CircleExclamation);

                case FeedbackType.Critical:
                    return new FeedbackStyle(
                        BackColor: Color.FromArgb(68, 39, 38),
                        IconColor: Color.FromArgb(255, 153, 164),
                        Icon: IconChar.CircleXmark);

                default:

                    return GetStyle(FeedbackType.Info);
            }
        }
    }
}
