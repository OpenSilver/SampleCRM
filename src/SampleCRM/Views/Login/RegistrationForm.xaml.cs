using OpenRiaServices.DomainServices.Client;
using OpenRiaServices.DomainServices.Client.ApplicationServices;
using SampleCRM.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SampleCRM.LoginUI
{
    /// <summary>
    /// Form that presents the <see cref="RegistrationData"/> and performs the registration process.
    /// </summary>
    public partial class RegistrationForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private RegistrationData _registrationData = new RegistrationData();
        private UserRegistrationContext userRegistrationContext = new UserRegistrationContext();
        //private TextBox userNameTextBox;

        /// <summary>
        /// Creates a new <see cref="RegistrationForm"/> instance.
        /// </summary>
        public RegistrationForm()
        {
            InitializeComponent();

            // Set the DataContext of this control to the Registration instance to allow for easy binding.
            DataContext = _registrationData;
            userNameTextBox.LostFocus += UserNameLostFocus;
            _registrationData.PasswordAccessor = () => passwordBox.Password;
            _registrationData.PasswordConfirmationAccessor = () => passwordConfirmationBox.Password;

        }

        /// <summary>
        /// Sets the parent window for the current <see cref="RegistrationForm"/>.
        /// </summary>
        /// <param name="window">The window to use as the parent.</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            parentWindow = window;
        }

        /// <summary>
        /// Wire up the Password and PasswordConfirmation accessors as the fields get generated.
        /// Also bind the Question field to a ComboBox full of security questions, and handle the LostFocus event for the UserName TextBox.
        /// </summary>
        //private void RegisterForm_AutoGeneratingField(object dataForm, DataFormAutoGeneratingFieldEventArgs e)
        //{
        //    // Put all the fields in adding mode
        //    e.Field.Mode = DataFieldMode.AddNew;

        //    if (e.PropertyName == "UserName")
        //    {
        //       userNameTextBox = (TextBox)e.Field.Content;
        //       userNameTextBox.LostFocus +=UserNameLostFocus;
        //    }
        //    else if (e.PropertyName == "Password")
        //    {
        //        PasswordBox passwordBox = new PasswordBox();
        //        e.Field.ReplaceTextBox(passwordBox, PasswordBox.PasswordProperty);
        //       _registrationData.PasswordAccessor = () => passwordBox.Password;
        //    }
        //    else if (e.PropertyName == "PasswordConfirmation")
        //    {
        //        PasswordBox passwordConfirmationBox = new PasswordBox();
        //        e.Field.ReplaceTextBox(passwordConfirmationBox, PasswordBox.PasswordProperty);
        //       _registrationData.PasswordConfirmationAccessor = () => passwordConfirmationBox.Password;
        //    }
        //    else if (e.PropertyName == "Question")
        //    {
        //        ComboBox questionComboBox = new ComboBox();
        //        questionComboBox.ItemsSource = RegistrationForm.GetSecurityQuestions();
        //        e.Field.ReplaceTextBox(questionComboBox, ComboBox.SelectedItemProperty, binding => binding.Converter = new TargetNullValueConverter());
        //    }
        //}

        /// <summary>
        /// The callback for when the UserName TextBox loses focus.
        /// Call into the registration data to allow logic to be processed, possibly setting the FriendlyName field.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void UserNameLostFocus(object sender, RoutedEventArgs e)
        {
            _registrationData.UserNameEntered(((TextBox)sender).Text);
        }

        /// <summary>
        /// Returns a list of the resource strings defined in <see cref="SecurityQuestions" />.
        /// </summary>
        //private static IEnumerable<string> GetSecurityQuestions()
        //{
        //    return new string[]
        //    {
        //        "What is the name of your favorite childhood friend?",
        //        "What was your childhood nickname?",
        //        "What was the color of your first car?",
        //        "What was the make and model of your first car?",
        //        "In what city or town was your first job?",
        //        "Where did you vacation last year?",
        //        "What is your maternal grandmother's maiden name?",
        //        "What is your mother's maiden name?",
        //        "What is your pet's name?",
        //        "What school did you attend for sixth grade?",
        //        "In what year was your father born?"
        //    };
        //}

        /// <summary>
        /// Submit the new registration.
        /// </summary>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // We need to force validation since we are not using the standard OK button from the DataForm.
            // Without ensuring the form is valid, we would get an exception invoking the operation if the entity is invalid.
            //if (this.registerForm.ValidateItem())
            var context = new ValidationContext(_registrationData, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(_registrationData, context, results);
            if (!isValid)
            {
                var totalErrorMessage = string.Join(" ", results.Select(x => $"{x.ErrorMessage}"));
                ErrorWindow.Show(totalErrorMessage);
#if DEBUG
                foreach (var validationResult in results)
                {
                    Console.WriteLine(validationResult.ErrorMessage);
                    //throw new ValidationException(validationResult.ErrorMessage);
                }
#endif
            }
            else
            {
                _registrationData.CurrentOperation = userRegistrationContext.CreateUser(
                    _registrationData,
                    _registrationData.Password,
                    RegistrationOperation_Completed, null);

                parentWindow.AddPendingOperation(_registrationData.CurrentOperation);
            }
        }

        /// <summary>
        /// Completion handler for the registration operation.
        /// If there was an error, an <see cref="ErrorWindow"/> is displayed to the user.
        /// Otherwise, this triggers a login operation that will automatically log in the just registered user.
        /// </summary>
        private void RegistrationOperation_Completed(InvokeOperation<CreateUserStatus> operation)
        {
            if (!operation.IsCanceled)
            {
                if (operation.HasError)
                {
                    parentWindow.DialogResult = false;
                    ErrorWindow.Show(operation.Error);
                    operation.MarkErrorAsHandled();
                }
                else if (operation.Value == CreateUserStatus.Success)
                {
                    _registrationData.CurrentOperation = WebContext.Current.Authentication.Login(this._registrationData.ToLoginParameters(), LoginOperation_Completed, null);
                    parentWindow.AddPendingOperation(this._registrationData.CurrentOperation);
                    parentWindow.DialogResult = true;
                }
                else if (operation.Value == CreateUserStatus.DuplicateUserName)
                {
                    parentWindow.DialogResult = false;
                    _registrationData.ValidationErrors.Add(
                         new ValidationResult("User name already exists. Please enter a different user name.",
                         new string[] { "UserName" }));
                    parentWindow.DialogResult = false;
                    ErrorWindow.Show("User name already exists. Please enter a different user name.");
                    operation.MarkErrorAsHandled();
                }
                else if (operation.Value == CreateUserStatus.DuplicateEmail)
                {
                    _registrationData.ValidationErrors.Add(
                         new ValidationResult("A user name for that e-mail address already exists. Please enter a different e-mail address.",
                         new string[] { "Email" }));
                    parentWindow.DialogResult = false;
                    ErrorWindow.Show("A user name for that e-mail address already exists. Please enter a different e-mail address.");
                    operation.MarkErrorAsHandled();
                }
                else
                {
                    parentWindow.DialogResult = false;
                    ErrorWindow.Show("An unknown error has occurred. Please contact your administrator for help.");
                }
            }
        }

        /// <summary>
        /// Completion handler for the login operation that occurs after a successful registration and login attempt.
        /// This will close the window. If the operation fails, an <see cref="ErrorWindow"/> will display the error message.
        /// </summary>
        /// <param name="loginOperation">The <see cref="LoginOperation"/> that has completed.</param>
        private void LoginOperation_Completed(LoginOperation loginOperation)
        {
            if (!loginOperation.IsCanceled)
            {
                parentWindow.DialogResult = true;

                if (loginOperation.HasError)
                {
                    ErrorWindow.Show(string.Format(System.Globalization.CultureInfo.CurrentUICulture,
                        "Registration was successful but there was a problem while trying to log in with your credentials: {0}",
                        loginOperation.Error.Message));
                    loginOperation.MarkErrorAsHandled();
                }
                else if (loginOperation.LoginSuccess == false)
                {
                    // The operation was successful, but the actual login was not
                    ErrorWindow.Show(string.Format(System.Globalization.CultureInfo.CurrentUICulture,
                        "Registration was successful but there was a problem while trying to log in with your credentials: {0}",
                        "The user name or password is incorrect"));
                }
            }
        }

        /// <summary>
        /// Switches to the login window.
        /// </summary>
        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.NavigateToLogin();
        }

        /// <summary>
        /// If a registration or login operation is in progress and is cancellable, cancel it.
        /// Otherwise, close the window.
        /// </summary>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (this._registrationData.CurrentOperation != null && _registrationData.CurrentOperation.CanCancel)
            {
                _registrationData.CurrentOperation.Cancel();
            }
            else
            {
                parentWindow.DialogResult = false;
            }
        }

        /// <summary>
        /// Maps Esc to the cancel button and Enter to the OK button.
        /// </summary>
        private void RegistrationForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CancelButton_Click(sender, e);
            }
            else if (e.Key == Key.Enter && registerButton.IsEnabled)
            {
                RegisterButton_Click(sender, e);
            }
        }

        /// <summary>
        /// Sets focus to the user name text box.
        /// </summary>
        public void SetInitialFocus()
        {
            userNameTextBox.Focus();
        }
    }
}