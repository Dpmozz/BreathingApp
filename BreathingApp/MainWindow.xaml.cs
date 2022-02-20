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
using System.Windows.Threading;
using System.Diagnostics;

namespace BreathingApp
{
    public partial class MainWindow : Window
    {
        // (1) Inhale -> (2) Hold full -> (3) Exhale -> (4) Hold empty

        int holdFullTick;
        int exhaleTick;
        int holdEmptyTick;
        int inhaleTick;

        int stateMachine = 1;

        DispatcherTimer tickTock;
        Stopwatch stopWatch;

        public MainWindow()
        {
            InitializeComponent();

            GetSettings(); // Load setpoints

            tickTock = new DispatcherTimer();
            tickTock.Interval = TimeSpan.FromSeconds(1);
            tickTock.Tick += TickTock_Tick;
            stopWatch = new Stopwatch();
        }

        private void GetSettings()
        {
            txbInhaleSP.Text = Properties.Settings.Default.Inhale.ToString();
            txbHoldFullSP.Text = Properties.Settings.Default.HoldFull.ToString();
            txbExhaleSP.Text = Properties.Settings.Default.Exhale.ToString();
            txbHoldEmptySP.Text = Properties.Settings.Default.HoldEmpty.ToString();
        }

        private void TickTock_Tick(object sender, EventArgs e)
        {
            lblTOTALTIME.Content = stopWatch.Elapsed.ToString(@"hh\:mm\:ss");

            switch (stateMachine)
            {
                case 1:
                    if (inhaleTick == 0)
                        ClearProgressBars();
                    inhaleTick++;
                    pgbInhale.Value = inhaleTick;
                    if (inhaleTick == int.Parse(txbInhaleSP.Text))
                        stateMachine = 2;
                    break;
                case 2:
                    holdFullTick++;
                    pgbHoldFull.Value = holdFullTick;
                    if (holdFullTick == int.Parse(txbHoldFullSP.Text))
                        stateMachine = 3;
                    break;
                case 3:
                    exhaleTick++;
                    pgbExhale.Value = exhaleTick;
                    if (exhaleTick == int.Parse(txbExhaleSP.Text))
                        stateMachine = 4;
                    break;
                case 4:
                    holdEmptyTick++;
                    pgbHoldEmpty.Value = holdEmptyTick;
                    if (holdEmptyTick == int.Parse(txbHoldEmptySP.Text))
                    {
                        ClearTickCounters();
                        stateMachine = 1;
                    }
                    break;
            }
            Console.WriteLine($"{stateMachine}");
        }
         
        private void pbSTART_Click(object sender, RoutedEventArgs e)
        {
            pbRESET.IsEnabled = false;
            tickTock.Start();
            stopWatch.Start();
        }

        private void pbSTOP_Click(object sender, RoutedEventArgs e)
        {
            pbRESET.IsEnabled = true;
            tickTock.Stop();
            stopWatch.Stop();
        }

        private void pbRESET_Click(object sender, RoutedEventArgs e)
        {
            tickTock.Stop();
            stopWatch.Reset();

            ClearProgressBars();

            ClearTickCounters();

            lblTOTALTIME.Content = "00:00:00";

            stateMachine = 1;
        }

        private void ClearTickCounters()
        {
            inhaleTick = 0;
            holdFullTick = 0;
            exhaleTick = 0;
            holdEmptyTick = 0;
        }

        private void ClearProgressBars()
        {
            pgbInhale.Value = 0;
            pgbHoldFull.Value = 0;
            pgbExhale.Value = 0;
            pgbHoldEmpty.Value = 0;
        }

        private void txbInhaleSP_TextChanged(object sender, TextChangedEventArgs e)
        {
            pgbInhale.Maximum = int.Parse(txbInhaleSP.Text);
            Properties.Settings.Default.Inhale = int.Parse(txbInhaleSP.Text);
            Properties.Settings.Default.Save();
        }

        private void txbHoldFullSP_TextChanged(object sender, TextChangedEventArgs e)
        {
            pgbHoldFull.Maximum = int.Parse(txbHoldFullSP.Text);
            Properties.Settings.Default.HoldFull = int.Parse(txbHoldFullSP.Text);
            Properties.Settings.Default.Save();
        }

        private void txbExhaleSP_TextChanged(object sender, TextChangedEventArgs e)
        {
            pgbExhale.Maximum = int.Parse(txbExhaleSP.Text);
            Properties.Settings.Default.Exhale = int.Parse(txbExhaleSP.Text);
            Properties.Settings.Default.Save();
        }

        private void txbHoldEmptySP_TextChanged(object sender, TextChangedEventArgs e)
        {
            pgbHoldEmpty.Maximum = int.Parse(txbHoldEmptySP.Text);
            Properties.Settings.Default.HoldEmpty = int.Parse(txbHoldEmptySP.Text);
            Properties.Settings.Default.Save();
        }
    }
}
