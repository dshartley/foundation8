using System;
using Smart.Platform.Diagnostics;

namespace Smart.Platform.Net.Email
{
    /// <summary>
    /// Encapsulates an email mailbox account.
    /// </summary>
    public class EmailMailboxAccount
    {
        #region Constructors

        private EmailMailboxAccount() { }

        public EmailMailboxAccount(string key,
                            string hostEmailAddress,
                            string smtpServer)
        {
            #region Check Parameters

            if (key == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));
            if (hostEmailAddress == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "hostEmailAddress is nothing"));
            if (smtpServer == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "smtpServer is nothing"));

            #endregion

            _key = key;
            _hostEmailAddress = hostEmailAddress;
            _smtpServer = smtpServer;
        }

        public EmailMailboxAccount( string key,
                                    string hostEmailAddress,
                                    string smtpServer,
                                    string smtpPort)
        {
            #region Check Parameters

            if (key == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));
            if (hostEmailAddress == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "hostEmailAddress is nothing"));
            if (smtpServer == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "smtpServer is nothing"));
            if (smtpPort == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "smtpPort is nothing"));

            #endregion

            _key = key;
            _hostEmailAddress = hostEmailAddress;
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
        }

        public EmailMailboxAccount( string key,
                                    string hostEmailAddress,
                                    string hostUserName,
                                    string hostPassword,
                                    string smtpServer,
                                    string smtpPort)
        {
            #region Check Parameters

            if (key == string.Empty)                throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));
            if (hostEmailAddress == string.Empty)   throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "hostEmailAddress is nothing"));
            if (hostUserName == string.Empty)       throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "hostUserName is nothing"));
            if (hostPassword == string.Empty)       throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "hostPassword is nothing"));
            if (smtpServer == string.Empty)         throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "smtpServer is nothing"));
            if (smtpPort == string.Empty)           throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "smtpPort is nothing"));

            #endregion

            _key                = key;
            _hostEmailAddress   = hostEmailAddress;
            _hostUserName       = hostUserName;
            _hostPassword       = hostPassword;
            _smtpServer         = smtpServer;
            _smtpPort           = smtpPort;
        }

        #endregion

        #region Public Methods

        private string _key;

        public string Key
        {
            get { return _key; }
        }

        private string _hostEmailAddress;

        public string HostEmailAddress
        {
            get 
            {
                return _hostEmailAddress;
            }
            set 
            {
                _hostEmailAddress = value;
            }
        }

        private string _hostUserName;

        public string HostUserName
        {
            get
            {
                return _hostUserName;
            }
            set
            {
                _hostUserName = value;
            }
        }

        private string _hostPassword;

        public string HostPassword
        {
            get
            {
                return _hostPassword;
            }
            set
            {
                _hostPassword = value;
            }
        }

        private string _smtpServer;

        public string SMTPServer
        {
            get
            {
                return _smtpServer;
            }
            set
            {
                _smtpServer = value;
            }
        }

        private string _smtpPort;

        public string SMTPPort
        {
            get
            {
                return _smtpPort;
            }
            set
            {
                _smtpPort = value;
            }
        }

        #endregion
    }
}
