using Microsoft.VisualBasic;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PracticalWork2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string firstText = this.FirstNumber.Text;
            string secondText = this.SecondNumber.Text;

            String ResultTemplate;
            Boolean isFirstNumeric = Information.IsNumeric(firstText);
            Boolean isSecondNumeric = Information.IsNumeric(secondText);

            if (isFirstNumeric && isSecondNumeric)
            {
                double first = Convert.ToDouble(firstText);
                double second = Convert.ToDouble(secondText);

                double sum, dif, mul, div;
                sum = first + second;
                dif = first - second;
                mul = first * second;
                div = first / second;
                ResultTemplate = $"Результаты:\n{"Cумма:",-15}\t{sum,30}\n{"Разность:",-15}\t{dif,30}\n{"Произведение:",-15}\t{mul,30}\n{"Частное:",-15}\t{div,30}";
            }
            else
            {
                ResultTemplate = $"Ошибка!\n";
                ResultTemplate += isFirstNumeric ? "" : "Первое число не корректно!\n";
                ResultTemplate += isSecondNumeric ? "" : "Второе число не корректно!\n";
            }

            this.textBlock.Text = ResultTemplate;
        }

        private void FirstNumber_GotFocus(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is TextBox)
            {
                TextBox textBox = e.OriginalSource as TextBox;
                if (Information.IsNumeric(textBox.Text))
                {
                    textBox.SelectAll();
                } else
                {
                    textBox.Clear();

                }
            }
        }

        private void FirstNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textBox = sender as TextBox;
                String text = textBox.Text.Trim();
                if (String.IsNullOrEmpty(text) || !Information.IsNumeric(text))
                {
                    textBox.Text = textBox.ToolTip.ToString();
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string cirumferenceLengthText = this.CircumferenceLength.Text;

            String ResultTemplate;
            Boolean isCirumferenceLengthNumeric = Information.IsNumeric(cirumferenceLengthText);

            if (isCirumferenceLengthNumeric)
            {
                double cirumferenceLength = Convert.ToDouble(cirumferenceLengthText);
                double square, radius;
                
                radius = cirumferenceLength / (2 * Math.PI);
                square = Math.PI * Math.Pow(radius, 2);

                ResultTemplate = $"Результаты:\n{"Площадь круга:",-15}\t{square,30}";
            }
            else
            {
                ResultTemplate = $"Ошибка!\nНе корректно введена длина окружности!\n";
            }

            this.textBlock2.Text = ResultTemplate;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            string lengthText = this.Length.Text;

            String ResultTemplate;
            Boolean islengthNumeric = Information.IsNumeric(lengthText);

            if (islengthNumeric)
            {
                double length = Convert.ToDouble(lengthText);
                long meters;

                meters = (long)Math.Floor(length / 100);

                ResultTemplate = $"Результаты:\n{"Количество полных метров:",-25}\t{meters,30}";
            }
            else
            {
                ResultTemplate = $"Ошибка!\nНе корректно введено расстояние!\n";
            }

            this.textBlock3.Text = ResultTemplate;
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            string numberText = this.Number.Text;

            String ResultTemplate;
            Boolean isNumberNumeric = Information.IsNumeric(numberText);

            if (isNumberNumeric)
            {
                char[] numbers = numberText.ToCharArray();

                if (numbers.Length != 3)
                {
                    ResultTemplate = $"Ошибка!\nВведите трёхзначное число!\n";
                } else
                {
                    double sum, mul;
                    sum = 0;
                    mul = 1;

                    foreach (char number in numbers)
                    {
                        sum += Char.GetNumericValue(number);
                        mul *= Char.GetNumericValue(number);
                    }

                    ResultTemplate = $"Результаты:\n{"Cумма:",-15}\t{sum,30}\n{"Произведение:",-15}\t{mul,30}\n";
                }
            }
            else
            {
                ResultTemplate = $"Ошибка!\nНе корректно введено число!!\n";
            }

            this.textBlock4.Text = ResultTemplate;
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            string secondsText = this.Seconds.Text;

            String ResultTemplate;
            Boolean isSecondsTextNumeric = Information.IsNumeric(secondsText);

            if (isSecondsTextNumeric)
            {
                double seconds = (long)Convert.ToDouble(secondsText);

                int lastMinuteSeconds = Convert.ToInt16(seconds % 60);

                ResultTemplate = $"Результаты:\n{"С начала последней минуты прошло: ",-35}\t{lastMinuteSeconds,2}с.";
                
            }
            else
            {
                ResultTemplate = $"Ошибка!\nНе корректно введено количество секунд!\n";
            }

            this.textBlock5.Text = ResultTemplate;
        }
    }
}
