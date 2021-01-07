namespace Smart.Platform.Data.Validation
{
    #region Enums

    /// <summary>
    /// Indicates the result of a validation.
    /// </summary>
    public enum ValidationResultTypes
	{
        /// <summary>
        /// The validation passed.
        /// </summary>
        Passed,
        /// <summary>
        /// The validation passed but raised a warning.
        /// </summary>
        Warning,
        /// <summary>
        /// The validation failed.
        /// </summary>
        Failed
	}

    #endregion

    #region Delegates

    /// <summary>
    /// A delegate type for handling validation events
    /// </summary>
    public delegate void ValidationEventHandler(IHasValidations item, int propertyEnum, string message, ValidationResultTypes resultType);

    #endregion

    /// <summary>
    /// Defines a class which has validations.
    /// </summary>
    public interface IHasValidations
    {
        #region Events

        /// <summary>
        /// Occurs when a validation passes.
        /// </summary>
        event ValidationEventHandler ValidationPassed;
        
        /// <summary>
        /// Occurs when a validation fails.
        /// </summary>
        event ValidationEventHandler ValidationFailed;

        #endregion

        /// <summary>
        /// Determines whether the specified property enum value is valid.
        /// </summary>
        /// <param name="propertyEnum">The property enum value.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        ValidationResultTypes IsValid(int propertyEnum, string value);

        /// <summary>
        /// Performed after the validation has passed.
        /// </summary>
        /// <param name="propertyEnum">The property enum.</param>
        /// <param name="message">The message.</param>
        /// <param name="resultType">Type of the result.</param>
        void AfterValidationPassed(int propertyEnum, string message, ValidationResultTypes resultType);
        
        /// <summary>
        /// Performed after the validation has failed.
        /// </summary>
        /// <param name="propertyEnum">The property enum.</param>
        /// <param name="message">The message.</param>
        /// <param name="resultType">Type of the result.</param>
        void AfterValidationFailed(int propertyEnum, string message, ValidationResultTypes resultType);
        }
}
