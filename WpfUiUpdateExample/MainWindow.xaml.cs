using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WpfUiUpdateExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void AsyncButton_Click(object sender, RoutedEventArgs e)
        {
            // because this method is async and I'm using await below the code in this method waits for 
            // the delay before continuing, but the framework sorts it out so that this method does what it
            // needs to in the background, thus not blocking the UI thread

            // BusyScope changes the mouse cursor when created and changes it back when disposed, which is what the 'using' does
            using (new BusyScope())
            {
                for (var i = 0; i < 10; i++)
                {
                    var newLine = $"Line {i}\n";

                    richTextBox.AppendText(newLine);

                    // this doesn't' block the UI and the textbox gets a chance to update while we still wait for the sleep to happen
                    await Task.Delay(500);
                }
            }
        }

        private void NonAsyncButton_Click(object sender, RoutedEventArgs e)
        {
            // This blocks the UI and the text UI can't update. The method doesn't return until the
            // code is all done, so the UI can't update

            using (new BusyScope())
            {
                for (var i = 0; i < 10; i++)
                {
                    var newLine = $"Line {i}\n";

                    richTextBox.AppendText(newLine);

                    // the sleeps is a stand-in for time consuming work
                    Thread.Sleep(500);
                }
            }
        }

        private async void AsyncWtihWorkInTaskButton_Click(object sender, RoutedEventArgs e)
        {
            using (new BusyScope())
            {
                // Task is used to perform the time consuming work in the background
                await Task.Run(() =>
                {
                    for (var i = 0; i < 10; i++)
                    {
                        var newLine = $"Line {i}\n";

                        // because this whole block is running in a background task, the Dispatcher.Invoke is used 
                        // to update the control on the UI thread
                        Dispatcher.Invoke(new Action(() =>
                        {
                            richTextBox.AppendText(newLine);
                        }));

                        // the sleeps is a stand-in for time consuming work
                        Thread.Sleep(500);
                    }
                });
            }
        }
    }
}
