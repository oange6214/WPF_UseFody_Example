using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WPF_From_To_By_KeyFrame
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            //DemoFrom();         // 只寫 From;       From  作為起始值，Width      作為終止值
            //DemoTo();           // 只寫 To;         Width 作為起始值，To         作為終止值
            //DemoBy();           // 只寫 by;         Width 作為起始值，By + Width 作為終止值
            //DemoFromBy();       // From、By 都有;   From  作為起始值，By + From  作為終止值 
            DemoFromTo();       // From、To 都有;   From  作為起始值，To         作為終止值
        }

        //public ObservableCollection<Rectangle> ItemCollection { get; set; } = new ObservableCollection<Rectangle>();

        private ObservableCollection<Rectangle> _itemCollection = new ObservableCollection<Rectangle>();

        public ObservableCollection<Rectangle> ItemCollection
        {
            get { return _itemCollection; }
            set { _itemCollection = value; NotifyPropertyChanged(); }
        }


        private Storyboard myStoryboard;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /*
         * 只有指定動畫的 From 值時，動畫將從屬性指定的值，前進到要進行 From 動畫處理得屬性的值或組合動畫的輸出。
         */
        public void DemoFrom()
        {
            // Create a NameScope for this page so that Storyboards can be used.
            NameScope.SetNameScope(this, new NameScope());

            Rectangle rectangle = new Rectangle
            {
                Height = 10,
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Left,
                Fill = Brushes.Purple
            };

            // Demonstrates the From property used by iteself.
            // Animates the rectangle's Width property from 50 to tis base value (100) over 10 seconds.
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 50,
                Duration = new Duration(TimeSpan.FromSeconds(10))
            };

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(animation);

            string StoryboardName = "fromAnimatedRectangle";

            // Assign the Rectangle a name so that it can be targeted by a Storyboard.
            this.RegisterName(StoryboardName, rectangle);
            Storyboard.SetTargetName(animation, StoryboardName);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Rectangle.WidthProperty));

            GridRoot.Children.Add(rectangle);

            rectangle.MouseLeftButtonDown += (s, e) =>
            {
                myStoryboard.Begin(this);
            };
        }

        public void DemoTo()
        {
            // Create a NameScope for this page so that St oryboards can be used.
            NameScope.SetNameScope(this, new NameScope());

            Rectangle rectangle = new Rectangle
            {
                Height = 10,
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Left,
                Fill = Brushes.Gray
            };

            // Demonstrates the To property used by itself.
            // Animates the Rectangle's Width property from its base value (100) to 300 over 10 seconds.
            DoubleAnimation animation = new DoubleAnimation
            {
                To = 300,
                Duration = new Duration(TimeSpan.FromSeconds(10))
            };

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(animation);

            string StoryboardName = "toAnimatedRectangle";

            // Assign the Rectangle a name so that it can be targeted by a Storyboard.
            this.RegisterName(StoryboardName, rectangle);
            Storyboard.SetTargetName(animation, StoryboardName);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Rectangle.WidthProperty));

            GridRoot.Children.Add(rectangle);

            rectangle.MouseLeftButtonDown += (s, e) =>
            {
                myStoryboard.Begin(this);
            };
        }

        public void DemoBy()
        {
            // Create a NameScope for this page so that Storyboards can be used.
            NameScope.SetNameScope(this, new NameScope());

            // Assign the Rectangle a name so that it can be targeted by a Storyboard.
            Rectangle rectangle = new Rectangle
            {
                Height = 10,
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Left,
                Fill = Brushes.RoyalBlue
            };

            // Demonstrates the To property used by itself.
            // Increments the Rectangle's Width property by 300 over 10 seconds.
            // As a result, the Width prperty is animated from its base value (100) to 400 (100 + 300) over 10 seconds.
            DoubleAnimation animation = new DoubleAnimation
            {
                By = 300,
                Duration = new Duration(TimeSpan.FromSeconds(10))
            };

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(animation);

            string StoryboardName = "byAnimatedRectangle";
            this.RegisterName(StoryboardName, rectangle);

            Storyboard.SetTargetName(animation, StoryboardName);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Rectangle.WidthProperty));

            GridRoot.Children.Add(rectangle);

            rectangle.MouseLeftButtonDown += (s, e) =>
            {
                myStoryboard.Begin(this);
            };
        }

        public void DemoFromBy()
        {
            // Create a NameScope for this page so that Storyboards can be used.
            NameScope.SetNameScope(this, new NameScope());

            // Assign the Rectangle a name so that it can be targeted by a Storyboard.
            Rectangle rectangle = new Rectangle
            {
                Height = 10,
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Left,
                Fill = Brushes.BlueViolet
            };

            // Demonstrates the To property used by itself.
            // Increments the Rectangle's Width property by 300 over 10 seconds.
            // As a result, the Width property is animated from 50
            // to 350 (50 + 300) over 10 seconds
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 50,
                By = 300,
                Duration = new Duration(TimeSpan.FromSeconds(10))
            };

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(animation);

            string StoryboardName = "fromByAnimatedRectangle";
            this.RegisterName(StoryboardName, rectangle);
            Storyboard.SetTargetName(animation, StoryboardName);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Rectangle.WidthProperty));

            GridRoot.Children.Add(rectangle);

            rectangle.MouseLeftButtonDown += (s, e) =>
            {
                myStoryboard.Begin(this);
            };
        }

        /*
         * 同時設置 From 和 To 值時，動畫將從屬性指定的值，前進 From 到屬性指定的值 To.
         */
        public void DemoFromTo()
        {
            // Create a NameScope for this page so that Storyboards can be used.
            NameScope.SetNameScope(this, new NameScope());

            // Assign the Rectangle a name so that it can be targeted by a Storyboard.
            Rectangle rectangle = new Rectangle
            {
                Height = 10,
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Left,
                Fill = Brushes.Black
            };

            // Demonstrates the To property used by itself. Animates the Rectangle's Width property from its base value (100) to 300 over 10 seconds.
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 50,
                To = 300,
                Duration = new Duration(TimeSpan.FromSeconds(10))
            };

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(animation);

            string StoryboardName = "fromToAnimatedRectangle";
            this.RegisterName(StoryboardName, rectangle);
            Storyboard.SetTargetName(animation, StoryboardName);
            Storyboard.SetTargetProperty(animation, new PropertyPath(WidthProperty));

            ItemCollection.Add(rectangle);

            rectangle.MouseLeftButtonDown += (s, e) =>
            {
                myStoryboard.Begin(this);
            };
        }
    }
}
