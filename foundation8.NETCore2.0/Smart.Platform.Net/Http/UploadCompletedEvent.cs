using System;
using Smart.Platform.Diagnostics;

namespace Smart.Platform.Net.Http
{
    /// <summary>
    /// An event that fires when the file upload is completed.
    /// </summary>
    public delegate void UploadCompletedEvent(object sender, UploadCompletedEventArgs args);

    /// <summary>
    /// Encapsulates the arguments for a file upload completed event.
    /// </summary>
    public class UploadCompletedEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadCompletedEventArgs"/> class.
        /// </summary>
        public UploadCompletedEventArgs() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="filePath">The file path.</param>
        public UploadCompletedEventArgs(string fileName, string filePath)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(fileName)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "fileName is nothing"));
            if (string.IsNullOrEmpty(filePath)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "filePath is nothing"));

            #endregion

            _fileName = fileName;
            _filePath = filePath;
        }

        #endregion

        #region Public Methods

        private string _fileName = "";

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        private string _filePath = "";

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        #endregion
    }
}
