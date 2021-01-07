using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using Smart.Platform.Diagnostics;

namespace Smart.Platform.Net.Email
{
    /// <summary>
    /// Encapsulates an email.
    /// </summary>
    public class EmailWrapper
    {
        #region Constructors

        private EmailWrapper() { }

        public EmailWrapper(string key,
                            string to,
                            string subject,
                            string body)
        {
            #region Check Parameters

            if (key == string.Empty)        throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));
            if (to == string.Empty)         throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "to is nothing"));
            if (subject == string.Empty)    throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "subject is nothing"));
            if (body == string.Empty)       throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "body is nothing"));

            #endregion

            _key = key;

            // Create the email message
            _mailMessage = new MailMessage("x@x.com", to);
            _mailMessage.IsBodyHtml = false;
            _mailMessage.Subject    = subject;
            _mailMessage.Body       = body;
        }

        #endregion

        #region Public Methods

        private string _key;

        public string Key
        {
            get 
            {
                return _key; 
            }
            set
            {
                _key = value;
            }
        }

        private object _tag;

        public object Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }

        private MailMessage _mailMessage;

        public MailMessage MailMessage
        {
            get { return _mailMessage; }
        }

        private Dictionary<string, Attachment> _attachments = new Dictionary<string, Attachment>();

        public ArrayList AttachmentPaths
        {
            get
            {
                ArrayList paths = new ArrayList();
                foreach (string item in _attachments.Keys)
                {
                    paths.Add(item);
                }

                return paths;
            }
        }

        private bool _successfullySent;

        public bool SuccessfullySent
        {
            get
            {
                return _successfullySent;
            }
            set
            {
                _successfullySent = value;
            }
        }

        private ArrayList _mailboxAccounts = new ArrayList();

        public ArrayList MailboxAccounts
        {
            get
            {
                return _mailboxAccounts;
            }
            set
            {
                _mailboxAccounts = value;
            }
        }

        public void AddAttachment(string path)
        {
            #region Check Parameters

            if (path == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "path is nothing"));
            if (!File.Exists(path)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), ExceptionManager.MessageFileNotFound(path)));

            #endregion

            Attachment attachment = new Attachment(path);

            // Add it to the dictionary so that we can reference it
            _attachments.Add(path, attachment);

            _mailMessage.Attachments.Add(attachment);
        }

        public void DeleteAttachment(string path)
        {
            #region Check Parameters

            if (path == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "path is nothing"));

            #endregion

            // Get the attachment from the dictionary
            Attachment attachment = _attachments[path];

            if (attachment != null)
            {
                _mailMessage.Attachments.Remove(attachment);
            }

            _attachments.Remove(path);
        }

        public bool ContainsMailboxAccount(EmailMailboxAccount mailboxAccount)
        {
            #region Check Parameters

            if (mailboxAccount == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "mailboxAccount is nothing"));

            #endregion

            foreach (EmailMailboxAccount item in _mailboxAccounts)
            {
                if (item.Key == mailboxAccount.Key) return true;
            }

            return false;
        }

        public void AddMailboxAccount(EmailMailboxAccount mailboxAccount)
        {
            #region Check Parameters

            if (mailboxAccount == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "mailboxAccount is nothing"));
            if (this.ContainsMailboxAccount(mailboxAccount)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), String.Format(Resources.EmailWrapper.Messages.MailboxAccountAlreadyExists, mailboxAccount.Key)));
            
            #endregion

            // Add it to the collection
            _mailboxAccounts.Add(mailboxAccount);

            // To be safe, set the from address in the mail message
            _mailMessage.From = new MailAddress(mailboxAccount.HostEmailAddress);
        }

        public void DeleteMailboxAccount(EmailMailboxAccount mailboxAccount)
        {
            #region Check Parameters

            if (mailboxAccount == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "mailboxAccount is nothing"));
            if (!this.ContainsMailboxAccount(mailboxAccount)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), String.Format(Resources.EmailWrapper.Messages.MailboxAccountDoesNotExist, mailboxAccount.Key)));

            #endregion

            // Find it in the collection
            EmailMailboxAccount item = null;
            foreach (EmailMailboxAccount i in _mailboxAccounts)
            {
                if (i.Key == mailboxAccount.Key) item = i;
            }

            // Remove the item
            if (item != null) _mailboxAccounts.Remove(item);
        }

        public void DeleteMailboxAccount(string hostEmailAddress)
        {
            #region Check Parameters

            if (hostEmailAddress == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "hostEmailAddress is nothing"));

            #endregion

            // Find it in the collection
            ArrayList items = new ArrayList();
            foreach (EmailMailboxAccount i in _mailboxAccounts)
            {
                if (i.HostEmailAddress == hostEmailAddress) items.Add(i);
            }

            // Remove the items
            foreach (EmailMailboxAccount i in items)
            {
                _mailboxAccounts.Remove(i);
            }
        }

        #endregion
    }
}
