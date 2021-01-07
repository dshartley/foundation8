using System;
using Smart.Platform.Diagnostics;

namespace Smart.Platform.Data.Validation
{
    /// <summary>
    /// A base class for classes that contain validations.
    /// </summary>
    public abstract class HasValidationsBase : IHasValidations
    {
        protected string _validationMessage;

        #region Constructors

        public HasValidationsBase()
        {
        }

        #endregion
        
        #region IHasValidations Members

        public event ValidationEventHandler ValidationPassed;

        public event ValidationEventHandler ValidationFailed;

        public void AfterValidationPassed(int propertyEnum, string message, ValidationResultTypes resultType)
        {
            #region Check Parameters

            if (message == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "message is nothing"));

            #endregion

            // Raise the event
            this.OnValidationPassed(this, propertyEnum, message, resultType);

            this.Reset();
        }

        public void AfterValidationFailed(int propertyEnum, string message, ValidationResultTypes resultType)
        {
            #region Check Parameters

            if (message == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "message is nothing"));

            #endregion

            // Raise the event
            this.OnValidationFailed(this, propertyEnum, message, resultType);
            this.Reset();
        }

        #endregion

        #region Protected Abstract Methods

        public abstract ValidationResultTypes IsValid(int propertyEnum, string value);

        #endregion

        #region Private Methods

        private void Reset()
        {
            _validationMessage = "";
        }

        #endregion

        #region Protected Methods

        protected void OnValidationPassed(IHasValidations item, int propertyEnum, string message, ValidationResultTypes resultType)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));
            if (message == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "message is nothing"));
            
            #endregion

            if (ValidationPassed != null) ValidationPassed(item, propertyEnum, message, resultType);
        }

        protected void OnValidationFailed(IHasValidations item, int propertyEnum, string message, ValidationResultTypes resultType)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));
            if (message == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "message is nothing"));

            #endregion

            if (ValidationFailed != null) ValidationFailed(item, propertyEnum, message, resultType);
        }

        protected ValidationResultTypes CheckNumeric(string value)
        {
            #region Check Parameters

            if (value == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "value is nothing"));

            #endregion

            ValidationResultTypes result = ValidationResultTypes.Passed;

            Decimal d;
            if (!Decimal.TryParse(value, out d))
            {
                result              = ValidationResultTypes.Failed;
                _validationMessage  = Resources.Messages.MustBeNumeric;
            }

            return result;
        }

        protected ValidationResultTypes CheckGreaterThanZero(string value)
        {
            #region Check Parameters

            if (value == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "value is nothing"));

            #endregion

            ValidationResultTypes result = ValidationResultTypes.Passed;

            decimal d = 0;
            decimal.TryParse(value, out d);
            if (d <= 0)
            {
                result = ValidationResultTypes.Failed;
                _validationMessage = Resources.Messages.MustBeGreaterThanZero;
            }

            return result;
        }

        protected ValidationResultTypes CheckGreaterThanOrEqualToZero(string value)
        {
            #region Check Parameters

            if (value == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "value is nothing"));

            #endregion

            ValidationResultTypes result = ValidationResultTypes.Passed;

            decimal d = 0;
            decimal.TryParse(value, out d);
            if (d < 0)
            {
                result = ValidationResultTypes.Failed;
                _validationMessage = Resources.Messages.MustBeZeroOrGreater;
            }

            return result;
        }

        protected ValidationResultTypes CheckIsLength(string value, int length, string propertyName)
        {
            #region Check Parameters

            if (value == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "value is nothing"));
            if (propertyName == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "propertyName is nothing"));

            #endregion

            ValidationResultTypes result = ValidationResultTypes.Passed;

            if (value.Length != length)
            {
                result = ValidationResultTypes.Failed;
                _validationMessage = string.Format(Resources.Messages.MustBeLength, new string[] {propertyName, length.ToString()});
            }

            return result;
        }

        protected ValidationResultTypes CheckMaxLength(string value, int maxLength, string propertyName)
        {
            #region Check Parameters

            if (value == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "value is nothing"));
            if (propertyName == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "propertyName is nothing"));

            #endregion

            ValidationResultTypes result = ValidationResultTypes.Passed;

            if (value.Length > maxLength)
            {
                result = ValidationResultTypes.Failed;
                _validationMessage = string.Format(Resources.Messages.MustBeUpToMaxLength, new string[] { propertyName, maxLength.ToString() });
            }

            return result;
        }

        #endregion
    }
}
