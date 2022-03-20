using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace WPF_Custom_Animation
{
    public enum EdgeBehaviorEnum
    {
        EaseIn,
        EaseOut,
        EaseInOut
    }

    public class BounceDoubleAnimation : DoubleAnimation
    {
        public EdgeBehaviorEnum EdgeBehavior
        {
            get { return (EdgeBehaviorEnum)GetValue(EdgeBehaviorProperty); }
            set { SetValue(EdgeBehaviorProperty, value); }
        }

        public static readonly DependencyProperty EdgeBehaviorProperty =
            DependencyProperty.Register(nameof(EdgeBehavior), typeof(EdgeBehaviorEnum), typeof(BounceDoubleAnimation), new PropertyMetadata(EdgeBehaviorEnum.EaseIn));

        /// <summary>
        /// 重複彈跳次數
        /// </summary>
        public double Bounciness
        {
            get { return (double)GetValue(BouncinessProperty); }
            set { SetValue(BouncinessProperty, value); }
        }

        public static readonly DependencyProperty BouncinessProperty =
            DependencyProperty.Register(nameof(Bounciness), typeof(double), typeof(BounceDoubleAnimation), new PropertyMetadata(default(double)));

        /// <summary>
        /// 彈跳次數
        /// </summary>
        public int Bounces
        {
            get { return (int)GetValue(BouncesProperty); }
            set { SetValue(BouncesProperty, value); }
        }

        public static readonly DependencyProperty BouncesProperty =
            DependencyProperty.Register(nameof(Bounces), typeof(int), typeof(BounceDoubleAnimation), new PropertyMetadata(default(int)));

        /// <summary>
        /// GetCurrentValueCore 方法返回動畫的當前值
        /// 它採用 三個參數：建議的起始值、建議的結束值和 AnimationClock，用於確定動畫的進度。
        /// </summary>
        /// <param name="defaultOriginValue">建議的起始值</param>
        /// <param name="defaultDestinationValue">建議的結束值和</param>
        /// <param name="animationClock"></param>
        /// <returns></returns>
        protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue, AnimationClock animationClock)
        {
            double returnValue;
            var start = From ?? defaultOriginValue;
            var delta = To - start ?? defaultOriginValue - start;

            switch(EdgeBehavior)
            {
                case EdgeBehaviorEnum.EaseIn:
                    returnValue = EaseIn(animationClock.CurrentProgress.Value, start, delta, Bounciness, Bounces);
                    break;
                case EdgeBehaviorEnum.EaseOut:
                    returnValue = EaseOut(animationClock.CurrentProgress.Value, start, delta, Bounciness, Bounces);
                    break;
                default:
                    returnValue = EaseInOut(animationClock.CurrentProgress.Value, start, delta, Bounciness, Bounces);
                    break;
            }
            return returnValue;
        }

        /// <summary>
        /// 如果該類不使用依賴屬性儲存其資料，或者它在創建後需要額外初始化，則可能需要重寫其他方法。
        /// </summary>
        /// <returns></returns>
        protected override Freezable CreateInstanceCore()
        {
            return new BounceDoubleAnimation();
        }

        private static double EaseIn(double timeFraction, double start, double delta, double bounciness, int bounces)
        {
            double returnValue = Math.Abs(Math.Pow((timeFraction), bounciness) * Math.Cos(2 * Math.PI * timeFraction * bounces));
            returnValue *= delta;
            returnValue += start;
            return returnValue;
        }

        private static double EaseOut(double timeFraction, double start, double delta, double bounciness, int bounces)
        {
            double returnValue = Math.Abs(Math.Pow((1 - timeFraction), bounciness) * Math.Cos(2 * Math.PI * timeFraction * bounces));
            returnValue = delta - (returnValue * delta);
            returnValue += start;
            return returnValue;
        }

        private static double EaseInOut(double timeFraction, double start, double delta, double bounciness, int bounces)
        {
            double returnValue = 0.0;

            if (timeFraction <= 0.5)
            {
                returnValue = EaseIn(timeFraction * 2, start, delta / 2, bounciness, bounces);
            }
            else
            {
                returnValue = EaseOut((timeFraction - 0.5) * 2, start, delta / 2, bounciness, bounces);
                returnValue += delta / 2;
            }
            return returnValue;
        }

        private static double BounceOut(double x)
        {
            if (x < 1d / 2.75d) return 7.5625 * x;
            else if (x < 2d / 2.75d) return 7.5625 * (x -= 1.5d / 2.75d) * x + 0.75;
            else if (x < 2.5d / 2.75d) return 7.5625 * (x -= 2.25d / 2.75d) * x + 0.9375;
            else return 7.5625 * (x - 2.625d / 2.75d) * x + 0.984375;
        }
    }
}
