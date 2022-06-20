using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace PracticalWork5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.Goods = new ObservableCollection<Good>();
            this.Squares = new ObservableCollection<Square>();
            InitializeComponent();
            this.DataContext = this;
        }
        public ObservableCollection<Good> Goods { get; set; }
        public ObservableCollection<Square> Squares { get; set; }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(kilogramPriceTextBox.Text, out double kilogramPrice) | kilogramPrice <= 0)
            {
                kilogramPriceTextBox.Text = "";
                kilogramPriceTextBox.Focus();
                return;
            }

            Thread th = new Thread(() => { CalculatePrices(kilogramPrice); });
            th.Start();
            th.IsBackground = true;
        }

        private void CalculatePrices(double kilogramPrice)
        {
            int count = 10;
            this.Dispatcher.Invoke(delegate
                {
                    button.IsEnabled = false;
                    Goods.Clear();
                }
            );

            for (double index = 1; index <= count; index++)
            {
                Thread.Sleep(50);
                this.Dispatcher.Invoke(delegate
                {
                    progressBar.Value = index / count * 100;
                    Goods.Add(new Good(index, index * kilogramPrice));
                });
            }

            this.Dispatcher.Invoke(delegate
            {
                progressBar.Value = 0;
                button.IsEnabled = true;
            });
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(numberTextBox.Text, out int lastNumber) | lastNumber <= 0)
            {
                numberTextBox.Text = "";
                numberTextBox.Focus();
                return;
            }

            Thread th = new Thread(() => { CalculateSquares(lastNumber); });
            th.Start();
            th.IsBackground = true;
        }

        private void CalculateSquares(int lastNumber)
        {
            this.Dispatcher.Invoke(delegate
            {
                button2.IsEnabled = false;
                Squares.Clear();
            }
            );

            for (int number = 1; number <= lastNumber; number++)
            {
                this.Dispatcher.Invoke(delegate
                {
                    progressBar.Value = Convert.ToDouble(number) / lastNumber * 100;
                    Squares.Add(new Square(number, Math.Pow(number, 2)));
                });
            }

            this.Dispatcher.Invoke(delegate
            {
                button2.IsEnabled = true;
                progressBar.Value = 0;
            });
        }
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(numberTextBox3.Text, out int N) | N <= 0)
            {
                numberTextBox3.Text = "";
                numberTextBox3.Focus();
                return;
            }
            if (!double.TryParse(doubleTextBox3.Text, out double X) | Math.Abs(X) >= 1)
            {
                doubleTextBox3.Text = "";
                doubleTextBox3.Focus();
                return;
            }

            Thread th = new Thread(() => { CalculateExpression(N, X); });
            th.Start();
            th.IsBackground = true;
        }
        private void CalculateExpression(int N, double X)
        {
            this.Dispatcher.Invoke(delegate
            {
                button3.IsEnabled = false;
            }
            );

            double result = X;
            double a = 1, b = 1;
            for (int index = 1; index <= N; index++)
            {
                a *= (2 * index - 1);
                b *= (2 * index);

                result += (a * Math.Pow(X, 2 * index + 1)) / (b * (2 * index + 1));

                this.Dispatcher.Invoke(delegate
                {
                    progressBar.Value = Convert.ToDouble(index) / N * 100;
                });
            }

            this.Dispatcher.Invoke(delegate
            {
                textBlock.Text = String.Format("Значение выражения: {0}", result);
                button3.IsEnabled = true;
                progressBar.Value = 0;
            });
        }
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(numberTextBox4.Text, out int N) | N <= 0)
            {
                numberTextBox4.Text = "";
                numberTextBox4.Focus();
                return;
            }

            Thread th = new Thread(() => { CalculateWhileCycles(N); });
            th.Start();
            th.IsBackground = true;
        }

        private void CalculateWhileCycles(int N)
        {
            this.Dispatcher.Invoke(delegate
            {
                button4.IsEnabled = false;
            }
            );

            int K = 0;
            double square;
            while ((square = Math.Pow(K, 2)) <= N)
            {
                K++;

                this.Dispatcher.Invoke(delegate
                {
                    progressBar.Value = square / N * 33;
                });
            }

            this.Dispatcher.Invoke(delegate
            {
                textBlock4.Text = String.Format("Результаты:\n1. K = {0};\n", K);
                progressBar.Value = 33;
            });

            Thread th = new Thread(() => { CalculateWhileCycle2(N); });
            th.Start();
            th.IsBackground = true;
        }

        private void CalculateWhileCycle2(int N)
        {
            this.Dispatcher.Invoke(delegate
            {
                textBlock4.Text += String.Format("2. Все цифры N (N = N = {0}), начиная с самой правой:\n", N);
            });

            int number = N;
            double numbersCount = N.ToString().Length;

            while (number > 0)
            {
                this.Dispatcher.Invoke(delegate
                {
                    textBlock4.Text += String.Format("{0}\t", number % 10);
                });
                number = number / 10;

                this.Dispatcher.Invoke(delegate
                {
                    progressBar.Value = 33 + ((numbersCount - number.ToString().Length) / numbersCount) * 33;
                });
            }

            this.Dispatcher.Invoke(delegate
            {
                textBlock4.Text += String.Format("\n");
                progressBar.Value = 66;
            });
            Thread th = new Thread(() => { CalculateWhileCycle3(N); });
            th.Start();
            th.IsBackground = true;
        }

        private void CalculateWhileCycle3(int N)
        {
            string error = null;
            if (N <= 1)
            {
                error = "Число должно быть больше чем 1!";
            }

            int firstnumber = 0, secondnumber = 1, result = 0, K = 2;

            do
            {
                result = firstnumber + secondnumber;
                firstnumber = secondnumber;
                secondnumber = result;
                this.Dispatcher.Invoke(delegate
                {
                    progressBar.Value = 66 + ((double)result / N) * 33;
                });
                K++;
            } while (result < N);

            if (N != result)
            {
                error = "N не является числом Фибоначчи!";
            }

            this.Dispatcher.Invoke(delegate
            {
                textBlock4.Text += String.IsNullOrEmpty(error)
                    ? String.Format("3. Порядковый номер числа Фибоначчи: {0};\n", K)
                    : String.Format("3. {0}", error);
                progressBar.Value = 100;
            });

            this.Dispatcher.Invoke(delegate
            {
                button4.IsEnabled = true;
                progressBar.Value = 0;
            });
        }

        private void textBox5N_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!int.TryParse(textBox5N.Text, out int N) | N <= 0)
                {
                    textBox5N.Text = "";
                    textBox5N.Focus();
                    return;
                }

                Thread th = new Thread(() => { CalculateSequenceSum(N); });
                th.Start();
                th.IsBackground = true;
            }
        }

        private void CalculateSequenceSum(int N)
        {
            double result = 0;
            for (int index = 1; index <= N; index++)
            {
                result += (double)(index + 1) / (index * (index + 2) * (index + 3));

                this.Dispatcher.Invoke(delegate
                {
                    progressBar.Value = (double)index / N * 100;
                });
            }

            this.Dispatcher.Invoke(delegate
            {
                textBlock5N.Text = String.Format("N = {0};\nСумма: {1}\n", N, result);
            });
        }

        private void textBox5E_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!Double.TryParse(textBox5E.Text, out double E) | Math.Abs(E) >= 1)
                {
                    textBox5E.Text = "";
                    textBox5E.Focus();
                    return;
                }

                Thread th = new Thread(() => { CalculateSequenceSumMoreThan(E); });
                th.Start();
                th.IsBackground = true;
            }
        }

        private void CalculateSequenceSumMoreThan(double E)
        {
            double result = 0, element;
            int index = 1;
            while ((element = (double)(index + 1) / (index * (index + 2) * (index + 3))) > E)
            {
                result += element;
                index++;

                this.Dispatcher.Invoke(delegate
                {
                    progressBar.Value = E / element * 100;
                });
                Thread.Sleep(250);
            }

            this.Dispatcher.Invoke(delegate
            {
                textBlock5E.Text = String.Format("E = {0};\nСумма: {1}\n", E, result);
                progressBar.Value = 0;
            });
        }
    }

    public class Good
    {
        public Good(double kilograms, double price)
        {
            Kilograms = string.Format("{0} кг", kilograms);
            Price = string.Format("{0} руб", price);
        }

        public string Kilograms { get; set; }
        public string Price { get; set; }
    }

    public class Square
    {
        public Square(int number, double value)
        {
            Number = number;
            Value = value;
        }

        public int Number { get; set; }
        public double Value { get; set; }
    }
}
