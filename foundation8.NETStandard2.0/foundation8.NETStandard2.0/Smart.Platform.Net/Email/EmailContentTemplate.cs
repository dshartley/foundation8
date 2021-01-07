using System;
using System.Collections;
using System.IO;
using Smart.Platform.Diagnostics;

namespace Smart.Platform.Net.Email
{
    /// <summary>
    /// Encapsulates the content of an email.
    /// </summary>
    public class EmailContentTemplate
    {
        private const string    _fieldPrefix = "[smart:]";
        private ArrayList       _bodyTemplatePaths = new ArrayList();
        private string          _subjectTemplatePath;

        #region Constructors

        private EmailContentTemplate() { }

        public EmailContentTemplate(    string bodyTemplatePath,
                                        string subjectTemplatePath)
        {
            #region Check Parameters

            if (bodyTemplatePath == string.Empty)       throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "bodyTemplatePath is nothing"));
            if (!File.Exists(bodyTemplatePath))         throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "bodyTemplatePath is invalid"));
            if (subjectTemplatePath == string.Empty)    throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "subjectTemplatePath is nothing"));
            if (!File.Exists(subjectTemplatePath))      throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "subjectTemplatePath is invalid"));

            #endregion

            _bodyTemplatePaths.Add(bodyTemplatePath);
            _subjectTemplatePath    = subjectTemplatePath;

            this.SetBodyText();
            _subject = this.GetText(_subjectTemplatePath);
        }

        public EmailContentTemplate(ArrayList   bodyTemplatePaths, 
                                    string      subjectTemplatePath)
        {
            #region Check Parameters

            if (bodyTemplatePaths == null)              throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "bodyTemplatePaths is nothing"));
            if (subjectTemplatePath == string.Empty)    throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "subjectTemplatePath is nothing"));
            if (!File.Exists(subjectTemplatePath))      throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "subjectTemplatePath is invalid"));

            #endregion

            _bodyTemplatePaths      = bodyTemplatePaths;
            _subjectTemplatePath    = subjectTemplatePath;

            this.SetBodyText();
            _subject = this.GetText(_subjectTemplatePath);
        }

        #endregion

        #region Public Methods

        private string _body;

        public string Body
        {
            get { return _body; }
        }

        private string _subject;

        public string Subject
        {
            get { return _subject; }
        }

        public void SetBodyField(string name, string value)
        {
            #region Check Parameters

            if (name == string.Empty)   throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "name is nothing"));
            if (value == null)          throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "value is nothing"));

            #endregion

            _body = this.SetField(name, value, _body);
        }

        public void SetSubjectField(string name, string value)
        {
            #region Check Parameters

            if (name == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "name is nothing"));
            if (value == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "value is nothing"));

            #endregion

            _subject = this.SetField(name, value, _subject);
        }

        #endregion

        #region Private Methods

        private string SetField(string name, string value, string content)
        {
            #region Check Parameters

            if (name == string.Empty)   throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "name is nothing"));
            if (value == null)          throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "value is nothing"));
            if (content == null)        throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "content is nothing"));

            #endregion

            // Make sure there is at least a space
            if (value.Length == 0) value = " ";

            // Replace each occurence of the field with the value
            content = content.Replace(_fieldPrefix + name, value);

            return content;
        }

        private void SetBodyText()
        {
            _body = "";
            if (_bodyTemplatePaths == null || _bodyTemplatePaths.Count == 0)
            {
                // Go through each item in the array list
                foreach (string item in _bodyTemplatePaths)
                {
                    // If the file exists then concatenate it to the body
                    if (File.Exists(item)) _body += this.GetText(item);
                }
            }
        }

        private string GetText(string path)
        {
            StreamReader    reader  = new StreamReader(path, System.Text.Encoding.Default);
            string          text    = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            return text;
        }

        #endregion
    }
}
