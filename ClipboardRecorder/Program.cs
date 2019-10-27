// 28.10.2019 RELEASE
// Author: FORMATC or ARMADILLO-CLD

// Import libraries
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace ClipboardRecorder
{
    static class Program
    {
        // Import dlls
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int keys);
        [STAThread]
        static void Main()
        {
            // Dir config
            string dir = @"C:\Users\Asus\AppData\Roaming\Setts.cfg";
            // Text in config
            string text = "";
            // get the text in the clipboard
            string clipboardThisText = Clipboard.GetText();

            // If there is no config
            if (File.Exists(dir) == false)
            {
                // Create config
                File.Create(dir);
            }

            // Read all text in config
            StreamReader readF = new StreamReader(dir);
            // Read to end :)
            text += readF.ReadToEnd();
            // And close
            readF.Close();

            // if there is no text in the config
            if (text == "")
            {
                // to write in the config text from the clipboard
                StreamWriter writeF = new StreamWriter(dir);
                // and add date to text
                DateTime curDate = DateTime.Now;
                writeF.WriteLine("[" + curDate + "]" + "\n" + clipboardThisText);
                // Close
                writeF.Close();
            }

            // Message with info
            MessageBox.Show("Для запуска логгера буфера обмена нажмите f3. \n" +
                            "Для остановки - f4 \n" +
                            "Для показа логов - f5" +
                            "\nЭто окно можно свободно закрыть");

            // [!] START - F3 - 114 | STOP - F4 - 115 | SHOW - F5 - 116 [!]
            while (!(GetAsyncKeyState(115) != 0 || GetAsyncKeyState(116) != 0))
            {
                // Sleep 100 m\s
                Thread.Sleep(100);
                // If pressed F3 (Start)
                if (GetAsyncKeyState(114) != 0)
                {
                    // Show message with text "Started"
                    MessageBox.Show("Started");
                    // Monitor clipboard until F4 or F5 is pressed
                    while (!(GetAsyncKeyState(115) != 0 || GetAsyncKeyState(116) != 0))
                    {
                        // logs in config
                        string logs = "";

                        // Read logs in config
                        StreamReader readLogs = new StreamReader(dir);
                        // add text from config in "logs"
                        logs += readLogs.ReadToEnd();
                        // close
                        readLogs.Close();

                        // If the text from the clipboard, which was received right now, is not in the logs
                        if (logs.IndexOf(Clipboard.GetText()) == -1)
                        {
                            // Write in logs
                            StreamWriter writeLogs = new StreamWriter(dir);
                            // and add date
                            DateTime curDate = DateTime.Now;
                            writeLogs.WriteLine(logs + "\n" +"[" + curDate + "]" + "\n" + Clipboard.GetText());
                            // close
                            writeLogs.Close();
                        }
                    }
                    // if pressed F4 (STOP)
                    if (GetAsyncKeyState(115) != 0)
                    {
                        // teleport to exit
                        goto exit;
                    }
                    // if pressed F5 (Show logs)
                    else if (GetAsyncKeyState(116) != 0)
                    {
                        // Logs in config
                        string logs = "";

                        // Read text in config
                        StreamReader readLogger = new StreamReader(dir);
                        // add text from config to logs
                        logs += readLogger.ReadToEnd();
                        // close file
                        readLogger.Close();

                        // Message with text "Show logs..."
                        MessageBox.Show("Показываю логи...");
                        // Show logs
                        MessageBox.Show(logs);
                        // Back to the beginning
                        Main();
                    }
                }
                    
            }
            // If pressed F5 (Stop)
            if (GetAsyncKeyState(115) != 0)
            {
                // Exit
                goto exit;

            }
            // If pressed F4 (Show logs)
            else if (GetAsyncKeyState(116) != 0)
            {
                // logs in config
                string logs = "";

                // Read text in config
                StreamReader readLogger = new StreamReader(dir);
                // add text from config in "logs"
                logs += readLogger.ReadToEnd();
                // close file
                readLogger.Close();

                // Message with text "Show logs..."
                MessageBox.Show("Показываю логи...");
                // Show logs
                MessageBox.Show(logs);
                // Back to beginning
                Main();
            }
            // Label for exit
        exit:
            // Message with text "Stopped" and exit program
            MessageBox.Show("Stopped");
        }
    }
}
