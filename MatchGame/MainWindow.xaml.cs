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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchGame
{
    using System.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    using System.Windows.Threading;
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            TimeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                TimeTextBlock.Text = TimeTextBlock.Text + " - Play again?";
            }
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>(){ //8쌍의 이모지가 든 목록을 생성한다
                "🦑", "🦑",
                "🐡", "🐡",
                "🐘", "🐘",
                "🐳", "🐳",
                "🐫", "🐫",
                "🦕", "🦕",
                "🦘", "🦘",
                "🦔", "🦔",
            };
            Random random = new Random(); //임의의 숫자를 만들어 내는 생성기를 만든다
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>()) //mainGrid에 포함된 모든 TextBlock을 찾아 각 TextBlock마다 명령문을 실행한다
            {
                if (textBlock.Name != "TimeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count); //0부터 이모지 목록에 남은 이모지 개수중 임의의 숫자를 택해 index라는 이름을 붙인다
                    string nextEmoji = animalEmoji[index]; //index라는 이름이 붙은 임의의 숫자를 사용해 목록에서 임의의 이모지를 꺼낸다
                    textBlock.Text = nextEmoji; //TextBlock의 텍스트를 이모지 목록으로 변경한다
                    animalEmoji.RemoveAt(index); //목록에서 이모지를 제거한다
                }
            }
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility |= Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8) 
            {
                SetUpGame();
            }
        }
    }
}
