﻿using System;
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

namespace Mastermind2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> availableColors = new List<string> { "Red", "Yellow", "Orange", "White", "Green", "Blue" };
        private List<string> secretCode = new List<string>();
        private int score = 0;

        int pogingen;
        bool debugMode = false;

        int currentAttempt;

        //DispatcherTimer timer;
        //TimeSpan elapsedTime;
        //DateTime clicked;

        public MainWindow()
        {
            InitializeComponent();
            StartGame();
        }


        private void UpdateTitle()
        {
            Title = $"Mastermind Game - Pogingen: {pogingen}";
        }

        private void CheckCode_Click(object sender, RoutedEventArgs e)
        {
            if (pogingen < 10)
            { 
                List<string> playerGuess = new List<string>
                {
                ComboBox1.SelectedItem?.ToString(),
                ComboBox2.SelectedItem?.ToString(),
                ComboBox3.SelectedItem?.ToString(),
                ComboBox4.SelectedItem?.ToString()
                };

                for (int i = 0; i < 4; i++)
                {
                    if (!secretCode.Contains(playerGuess[i]))
                    {
                        (FindName($"Label{i + 1}") as Label).BorderBrush = Brushes.Gray;
                    }
                    else if (secretCode.Contains(playerGuess[i]) && secretCode[i] != playerGuess[i])
                    {
                        (FindName($"Label{i + 1}") as Label).BorderBrush = Brushes.Wheat;
                        score += 1;
                    }
                    else
                    {
                        (FindName($"Label{i + 1}") as Label).BorderBrush = Brushes.DarkRed;
                        score += 2;
                    }
                }
                pogingen++;
                UpdateTitle();
                ScoreText.Text = $"Score: {score}";
            }
            else
            {
                MessageBox.Show("Je hebt de code niet geraden in 10x.");
            }

            History();
        }

        private List<string> GenerateRandomCode()
        {
            Random random = new Random();
            List<string> code = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                string color = availableColors[random.Next(availableColors.Count)];
                code.Add(color);
            }
            return code;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Title = $"Mastermind Game - code: {string.Join(", ", secretCode)}";
            // Title = $"Mastermind Game - Pogingen: {string.Join(", ", pogingen)}";

            codeTextBlock.Text = $"Secret Code: {string.Join(", ", secretCode)}";
            codeTextBlock.Visibility = Visibility.Hidden;

            //clicked = DateTime.Now;
            //timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromMilliseconds(1);
            //timer.Tick += Timer_Tick;
            //timer.Start();
        }

        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    elapsedTime = DateTime.Now - clicked;
        //    timerTextBlock.Text = $"Timer: {elapsedTime.TotalSeconds.ToString("N3")} / 10";
        //    if (elapsedTime.TotalSeconds >= 10)
        //    {
        //        timer.Stop();
        //        pogingen++;
        //        Title = $"Mastermind Game - Pogingen: {pogingen}";
        //        clicked = DateTime.Now;
        //        timer.Start();
        //    }
        //}

        private void PopulateComboBoxes()
        {
            ComboBox[] comboBoxes = { ComboBox1, ComboBox2, ComboBox3, ComboBox4 };
            for (int i = 0; i < comboBoxes.Length; i++)
            {
                comboBoxes[i].ItemsSource = availableColors;
            }
        }

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedItem != null)
            {
                int index = int.Parse(comboBox.Name.Substring(comboBox.Name.Length - 1)) - 1;
                Label label = (Label)FindName($"Label{index + 1}");
                label.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(comboBox.SelectedItem.ToString());
            }
        }

        private void StartGame()
        {
            secretCode = GenerateRandomCode();
            codeTextBlock.Text = $"Secret Code: {string.Join(", ", secretCode)}";
            PopulateComboBoxes();
            score = 0;
            ScoreText.Text = $"Score: {score}";
            ResetLabelBorders();
            ResetLabelBackground();
            pogingen = 0;
            Title = $"Mastermind Game - Pogingen: {pogingen}";
            ResetComboBox();
            currentAttempt = 1;

        }

        private void ResetLabelBorders()
        {
            for (int i = 1; i <= 4; i++)
            {
                Label label = (Label)FindName($"Label{i}");
                label.BorderBrush = Brushes.Transparent;
            }

        }

        private void ResetComboBox()
        {
            ComboBox1.Text = null;
            ComboBox2.Text = null;
            ComboBox3.Text = null;
            ComboBox4.Text = null;
        }

        private void ResetLabelBackground()
        {
            Label1.Background = default;
            Label2.Background = default;
            Label3.Background = default;
            Label4.Background = default;
        }

        private bool ToggleDebug(KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.A)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.A && !debugMode)
            {
                codeTextBlock.Visibility = Visibility.Visible;
                debugMode = true;
            }
            else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.A && debugMode)
            {
                codeTextBlock.Visibility = Visibility.Hidden;
                debugMode = false;
            }
        }

        private void History()
        {

            currentAttempt = pogingen - 1;

            RowDefinition attemptRow = new RowDefinition();
            attemptRow.Height = GridLength.Auto;
            pogingenGrid.RowDefinitions.Add(attemptRow);

            Label attempt1 = new Label();
            attempt1.Background = Label1.Background;
            attempt1.BorderBrush = Label1.BorderBrush;
            attempt1.BorderThickness = Label1.BorderThickness;
            attempt1.Height = 50;
            attempt1.Width = 50;
            attempt1.Margin = new Thickness(5);
            Grid.SetRow(attempt1, currentAttempt);
            Grid.SetColumn(attempt1, 0);

            Label attempt2 = new Label();
            attempt2.Background = Label2.Background;
            attempt2.BorderBrush = Label2.BorderBrush;
            attempt2.BorderThickness = Label2.BorderThickness;
            attempt2.Height = 50;
            attempt2.Width = 50;
            attempt2.Margin = new Thickness(5);
            Grid.SetRow(attempt2, currentAttempt);
            Grid.SetColumn(attempt2, 1);

            Label attempt3 = new Label();
            attempt3.Background = Label3.Background;
            attempt3.BorderBrush = Label3.BorderBrush;
            attempt3.BorderThickness = Label3.BorderThickness;
            attempt3.Height = 50;
            attempt3.Width = 50;
            attempt3.Margin = new Thickness(5);
            Grid.SetRow(attempt3, currentAttempt);
            Grid.SetColumn(attempt3, 2);

            Label attempt4 = new Label();
            attempt4.Background = Label4.Background;
            attempt4.BorderBrush = Label4.BorderBrush;
            attempt4.BorderThickness = Label4.BorderThickness;
            attempt4.Height = 50;
            attempt4.Width = 50;
            attempt4.Margin = new Thickness(5);
            Grid.SetRow(attempt4, currentAttempt);
            Grid.SetColumn(attempt4, 3);

            pogingenGrid.Children.Add(attempt1);
            pogingenGrid.Children.Add(attempt2);
            pogingenGrid.Children.Add(attempt3);
            pogingenGrid.Children.Add(attempt4);

        }
    }
}