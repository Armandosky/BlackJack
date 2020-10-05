using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfLibrary1;

namespace Informe1
{
    /// <summary>
    /// Lógica de interacción para Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        Player myPlayer = new Player();
        Dealer myDealer = new Dealer();

        public Game()
        {
            InitializeComponent();
        }

        public int Check(List<Card> hand)
        {
            int value = 0;
            foreach (Card c in hand)
            {
                if (value + c.Score > 21 && c.Score == 11)
                {
                    value += 1;
                }
                else
                {
                    value += c.Score;
                }
            }
            return value;
        }

        private void btn_Card_Click(object sender, RoutedEventArgs e)
        {
            if (myPlayer.Hand == null || myPlayer.Hand.Count == 0)
            {
                myDealer.Generate();
                myDealer.Randomize();
                myDealer.Init();

                Card c1 = myDealer.Deal();
                Card c2 = myDealer.Deal();

                myPlayer.Init(c1, c2);
                RefreshPlayerCards();
            }
            else
            {
                Card c = myDealer.Deal();
                myPlayer.AddCard(c);
                RefreshPlayerCards();
            }
            int handValue = Check(myPlayer.Hand);
            lblPlayerScore.Content = handValue;

            if (handValue >= 22)
            {
                btn_Card.IsEnabled = false;
                btn_Stop.IsEnabled = false;
                MessageBox.Show("Dealer Win");
            }
        }
        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            txtDealer.Text = "";

            foreach (Card c in myDealer.Hand)
            {
                txtDealer.Text += c.Symbol + c.Suit + "\n";
            }

            while (Check(myDealer.Hand) < Check(myPlayer.Hand))
            {
                Card c = myDealer.Deal();
                myDealer.AddCard(c);

                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    txtDealer.Text += c.Symbol + c.Suit + "\n";
                    lblDealerScore.Content = Check(myDealer.Hand);
                });
            }
            int DealerValue = Check(myDealer.Hand);
            lblDealerScore.Content = DealerValue;

            int handValue = Check(myPlayer.Hand);
            lblPlayerScore.Content = handValue;

            if (DealerValue >= 22)
            {
                btn_Card.IsEnabled = false;
                btn_Stop.IsEnabled = false;
                MessageBox.Show("Player Win");
            }

            if (handValue > DealerValue && handValue <= 21)
            {
                btn_Card.IsEnabled = false;
                btn_Stop.IsEnabled = false;
                MessageBox.Show("Player Win");
            }
            else
            {
                if (handValue == DealerValue)
                {
                    btn_Card.IsEnabled = false;
                    btn_Stop.IsEnabled = false;
                    MessageBox.Show("Tie");
                }
                else
                {
                    btn_Card.IsEnabled = false;
                    btn_Stop.IsEnabled = false;
                    MessageBox.Show("Dealer Win");
                }
            }
        }

        private string GetText()
        {
            return txtDealer.Text;
        }
        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            txtDealer.Text = "";
            txtPlayer.Text = "";
            lblDealerScore.Content = "";
            lblPlayerScore.Content = "";
            btn_Card.IsEnabled = true;
            btn_Stop.IsEnabled = true;

            Inicio w = (Inicio)Window.GetWindow(this);
            w.frameMain.NavigationService.Navigate(new Page1());
        }
        private void RefreshPlayerCards()
        {
            txtPlayer.Text = "";

            foreach (Card c in myPlayer.Hand)
            {
                txtPlayer.Text += c.Symbol + c.Suit + "\n";
            }
        }
    }
}
    
