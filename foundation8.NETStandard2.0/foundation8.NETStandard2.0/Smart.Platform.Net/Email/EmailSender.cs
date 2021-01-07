using System;
using System.Net;
using System.Net.Mail;
using Smart.Platform.Diagnostics;

namespace Smart.Platform.Net.Email
{
    /// <summary>
    /// Sends emails.
    /// </summary>
    public class EmailSender
    {
        private SmtpClient _client;

        #region Constructors

        public EmailSender() { }

        #endregion

        #region Public Methods

        public bool Send(EmailWrapper email)
        {
            #region Check Parameters

            if (email == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "email is nothing"));
            if (email.MailboxAccounts.Count == 0) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), Resources.EmailSender.Messages.NoMailboxAccounts));

            #endregion

            bool allEmailsSuccessfullySent = true;

            // Go through each mailbox account
            foreach (EmailMailboxAccount mailboxAccount in email.MailboxAccounts)
            {
                try
                {
                    this.SetupClient(mailboxAccount);

                    _client.Send(email.MailMessage);
                    email.SuccessfullySent = true;

                    ConsoleManager.Write(String.Format(Resources.EmailSender.Messages.SentEmail, new string[] {email.Key, mailboxAccount.HostEmailAddress}), ConsoleMessageTypes.Information);
                }
                catch (Exception ex)
                {
                    // Handle the exception here so that the loop continues through the other mailbox accounts
                    ExceptionManager.HandleConsole(String.Format(Resources.EmailSender.Messages.CouldNotSendEmail, new string[] {email.Key, mailboxAccount.HostEmailAddress, ex.Message}));
                    allEmailsSuccessfullySent = false;
                }
            }
            return allEmailsSuccessfullySent;
        }

        #endregion

        #region Private Methods

        private void SetupClient(EmailMailboxAccount mailboxAccount)
        {
            #region Check Parameters

            if (mailboxAccount == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "mailboxAccount is nothing"));

            #endregion

            // Setup the SMTP client
            _client         = new SmtpClient();
            _client.Host    = mailboxAccount.SMTPServer;

            if (!String.IsNullOrEmpty(mailboxAccount.SMTPPort)) _client.Port = Int32.Parse(mailboxAccount.SMTPPort);

            if (!String.IsNullOrEmpty(mailboxAccount.HostUserName) && !String.IsNullOrEmpty(mailboxAccount.HostPassword))
            {
                _client.UseDefaultCredentials = false;
                _client.Credentials = new NetworkCredential(mailboxAccount.HostUserName, mailboxAccount.HostPassword);
            }
            else
            {
                _client.UseDefaultCredentials = true;
            }
        }

        #endregion
    }
}
