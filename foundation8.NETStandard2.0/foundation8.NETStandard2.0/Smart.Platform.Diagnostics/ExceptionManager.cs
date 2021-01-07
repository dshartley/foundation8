using System;
using System.Reflection;

namespace Smart.Platform.Diagnostics
{
    /// <summary>
    /// Manages exceptions.
    /// </summary>
    public class ExceptionManager
    {
        #region Constructors

        private ExceptionManager() { }

        #endregion

        #region Public Static Methods

        public static void Handle(Exception ex, IExceptionHandler handler)
        {
            #region Check Parameters

            if (ex == null) return;
            if (handler == null) return;

            #endregion

            ConsoleManager.Write(String.Format(Resources.ExceptionManager.Messages.ErrorOccurred, new string[] { ex.GetType().ToString(), ex.Message }), ConsoleMessageTypes.ProgramError);

            handler.Handle(ex);
        }

        public static void HandleDialog(Exception ex)
        {
            #region Check Parameters

            if (ex == null) return;
            
            #endregion

            ConsoleManager.Write(String.Format(Resources.ExceptionManager.Messages.ErrorOccurred, new string[] {ex.GetType().ToString(), ex.Message}), ConsoleMessageTypes.ProgramError);

            // TODO: Deprecated in .NET Core
            //System.Windows.Forms.MessageBox.Show(ex.Message);

            // Show the exception in an error message dialog
            // TODO:
            //    Dim errorMessageDialog As New ErrorMessageDialog()
            //    errorMessageDialog.ShowDialog(ex)
        }

        public static void HandleConsole(string message)
        {
            if (message == null) message = string.Empty;

            ConsoleManager.Write(message, ConsoleMessageTypes.ProgramError);
        }

        #region Message Strings

        public static string MessageMethodFailed(MethodBase callingMethod, string message)
        {
            #region Check Parameters

            if (callingMethod == null)      throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "callingMethod is nothing"));
            if (message == string.Empty)    throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "message is nothing"));

            #endregion

            return "Method " + callingMethod.DeclaringType.FullName + "." + callingMethod.Name + " failed because " + message + ".";
        }

        public static string MessageFileNotFound(string path)
        {
            #region Check Parameters

            if (path == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "path is nothing"));

            #endregion

            return "The file '" + path + "' does not exist";
        }

        public static string MessageFileAlreadyExists(string path)
        {
            #region Check Parameters

            if (path == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "path is nothing"));

            #endregion

            return "The file '" + path + "' already exists";
        }

        #endregion

        #endregion
    }
}
