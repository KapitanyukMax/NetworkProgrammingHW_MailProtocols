﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PropertyChanged;
using MailKit.Net.Smtp;
using Microsoft.Win32;
using MimeKit;
using System.IO;
using System.Windows;

namespace NetworkProgrammingHW_MailProtocols
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel
    {
        public SmtpClient SmtpClient { get; set; } = new SmtpClient();

        public string FromAddress { get; set; } = string.Empty;

        public string LoginStatus { get; set; } = string.Empty;

        public bool IsLoggedIn { get; set; } = false;

        public string ToAddress { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public bool IsImportant { get; set; } = false;

        public string Body { get; set; } = string.Empty;

        private ObservableCollection<string> fileNames =
            new ObservableCollection<string>();

        public IEnumerable<string> FileNames => fileNames;

        public string SelectedFileName { get; set; } = string.Empty;

        private RelayCommand? loginCommand;

        public ICommand LoginCommand => loginCommand ??=
            new RelayCommand(o => Login(), o => !IsLoggedIn);

        private RelayCommand? attachFileCommand;

        public ICommand AttachFileCommand => attachFileCommand ??=
            new RelayCommand(o => AttachFile(), o => IsLoggedIn);

        private RelayCommand? removeFileCommand;

        public ICommand RemoveFileCommand => removeFileCommand ??=
            new RelayCommand(o => fileNames.Remove(SelectedFileName),
                o => fileNames.Count > 0);

        private RelayCommand? sendCommand;

        public ICommand SendCommand => sendCommand ??=
            new RelayCommand(o => SendAsync(), o => CanSend());

        public void Login()
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();

            if (loginWindow.DataContext is LoginViewModel loginViewModel &&
                loginViewModel.SmtpClient.IsAuthenticated)
            {
                SmtpClient = loginViewModel.SmtpClient;
                FromAddress = loginViewModel.Address;
                LoginStatus = $"You are logged in as {loginViewModel.Address}";
                IsLoggedIn = true;
            }
            else
            {
                SmtpClient = new SmtpClient();
                FromAddress = string.Empty;
                LoginStatus = string.Empty;
                IsLoggedIn = false;
            }
        }

        public void AttachFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
                fileNames.Add(dialog.FileName);
        }

        public bool CanSend()
        {
            return !string.IsNullOrWhiteSpace(FromAddress) &&
                   !string.IsNullOrWhiteSpace(ToAddress) &&
                   !string.IsNullOrWhiteSpace(Body);
        }

        public async void SendAsync()
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Max", FromAddress));
            message.To.Add(new MailboxAddress("Test user", ToAddress));
            message.Subject = Subject;
            message.Importance = IsImportant ? MessageImportance.High : MessageImportance.Normal;

            Multipart multipart = new Multipart
            {
                new TextPart("plain") { Text = Body }
            };

            foreach (string fileName in fileNames)
            {
                multipart.Add(new MimePart()
                {
                    Content = new MimeContent(File.OpenRead(fileName)),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    FileName = Path.GetFileName(fileName)
                });
            }

            message.Body = multipart;

            try
            {
                await SmtpClient.SendAsync(message);
            }
            catch (SmtpCommandException)
            {
                MessageBox.Show("Invalid recipient", "Error", MessageBoxButton.OK);
                return;
            }

            MessageBox.Show("Message sent successfully!");
            ToAddress = string.Empty;
            Subject = string.Empty;
            IsImportant = false;
            Body = string.Empty;
            fileNames.Clear();
        }
    }
}