using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_DoubleAnmation
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DemoDoubleAnmation();
        }

        #region Double Animation

        private Storyboard myStoryboard;

        public void DemoDoubleAnmation()
        {
            var myRectangle = CreateRectangle();
            var myAnimation = CreateAnimation();

            //AnimationClock clock = myDoubleAnimation.CreateClock();

            SetStoryboard(ref myStoryboard, myAnimation, myRectangle);

            GridRoot.Children.Add(myRectangle);
        }

        /// <summary>
        /// 建立 Rectangle 物件
        /// </summary>
        /// <returns></returns>
        public Rectangle CreateRectangle()
        {
            // 建立方塊
            Rectangle rectangle = new Rectangle();
            rectangle.Name = "myRectangle";
            this.RegisterName(rectangle.Name, rectangle);
            rectangle.Width = 100;
            rectangle.Height = 100;
            rectangle.Fill = Brushes.Blue;

            return rectangle;
        }

        /// <summary>
        /// 建立動畫
        /// </summary>
        /// <returns></returns>
        private DoubleAnimation CreateAnimation()
        {
            // 設置 Animation
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 1.0;
            animation.To = 0.0;
            animation.Duration = new Duration(TimeSpan.FromSeconds(3));
            animation.AutoReverse = true;
            animation.RepeatBehavior = RepeatBehavior.Forever;

            return animation;
        }

        /// <summary>
        /// 建立 Storyboard
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="doubleAnimation">物件動畫</param>
        /// <param name="targetObj">目標物件</param>
        /// <returns></returns>
        private void SetStoryboard<T>(ref Storyboard storyboard, DoubleAnimation doubleAnimation, T targetObject) where T : FrameworkElement
        {
            storyboard = new Storyboard();
            storyboard.Children.Add(doubleAnimation);
            Storyboard.SetTargetName(doubleAnimation, targetObject.Name);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Rectangle.OpacityProperty));
        }

        #endregion

        private void BtnBegin_Click(object sender, RoutedEventArgs e)
        {
            myStoryboard.Begin(this, true);
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            myStoryboard.Pause(this);
        }
    }
}
