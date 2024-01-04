using OpenRiaServices.DomainServices.Client.ApplicationServices;
using SampleCRM.Web;
using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;

namespace SampleCRM.LoginUI
{
    /// <summary>
    /// Form that presents the login fields and handles the login process.
    /// </summary>
    public partial class LoginForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private readonly LoginInfo _loginInfo = new LoginInfo();
        //private TextBox userNameTextBox;

        /// <summary>
        /// Creates a new <see cref="LoginForm"/> instance.
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();

            // Set the DataContext of this control to the LoginInfo instance to allow for easy binding.
            DataContext = _loginInfo;
            _loginInfo.PasswordAccessor = () => passwordBox.Password;
        }

        /// <summary>
        /// Sets the parent window for the current <see cref="LoginForm"/>.
        /// </summary>
        /// <param name="window">The window to use as the parent.</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            parentWindow = window;
        }

        /// <summary>
        /// Handles <see cref="DataForm.AutoGeneratingField"/> to provide the PasswordAccessor.
        /// </summary>
        //private void LoginForm_AutoGeneratingField(object sender, DataFormAutoGeneratingFieldEventArgs e)
        //{
        //    if (e.PropertyName == "UserName")
        //    {
        //        userNameTextBox = (TextBox)e.Field.Content;
        //    }
        //    else if (e.PropertyName == "Password")
        //    {
        //        PasswordBox passwordBox = new PasswordBox();
        //        e.Field.ReplaceTextBox(passwordBox, PasswordBox.PasswordProperty);
        //        loginInfo.PasswordAccessor = () => passwordBox.Password;
        //    }
        //}

        /// <summary>
        /// Submits the <see cref="LoginOperation"/> to the server
        /// </summary>
        private void LoginButton_Click(object sender, EventArgs e)
        {
            // We need to force validation since we are not using the standard OK button from the DataForm.
            // Without ensuring the form is valid, we get an exception invoking the operation if the entity is invalid.
            //if (loginForm.ValidateItem())
            var context = new ValidationContext(_loginInfo, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(_loginInfo, context, results);
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
                _loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(_loginInfo.ToLoginParameters(), LoginOperation_Completed, null);
                parentWindow.AddPendingOperation(_loginInfo.CurrentLoginOperation);
            }
        }

        /// <summary>
        /// Completion handler for a <see cref="LoginOperation"/>.
        /// If operation succeeds, it closes the window.
        /// If it has an error, it displays an <see cref="ErrorWindow"/> and marks the error as handled.
        /// If it was not canceled, but login failed, it must have been because credentials were incorrect so a validation error is added to notify the user.
        /// </summary>
        private void LoginOperation_Completed(LoginOperation loginOperation)
        {
            if (loginOperation.LoginSuccess)
            {
                parentWindow.DialogResult = true;
            }
            else if (loginOperation.HasError)
            {
                parentWindow.DialogResult = false;
                ErrorWindow.Show(loginOperation.Error);
                loginOperation.MarkErrorAsHandled();
            }
            else if (!loginOperation.IsCanceled)
            {
                _loginInfo.ValidationErrors.Add(new ValidationResult("Bad User Name Or Password" /*ErrorResources.ErrorBadUserNameOrPassword*/, new string[] { "UserName", "Password" }));
                parentWindow.DialogResult = false;
                ErrorWindow.Show("Bad User Name Or Password");
                loginOperation.MarkErrorAsHandled();
            }
        }

        /// <summary>
        /// Switches to the registration form.
        /// </summary>
        private void RegisterNow_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.NavigateToRegistration();
        }

        /// <summary>
        /// If a login operation is in progress and is cancellable, cancel it.
        /// Otherwise, close the window.
        /// </summary>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (_loginInfo.CurrentLoginOperation != null && _loginInfo.CurrentLoginOperation.CanCancel)
            {
                _loginInfo.CurrentLoginOperation.Cancel();
            }
            else
            {
                parentWindow.DialogResult = false;
            }
        }

        /// <summary>
        /// Maps Esc to the cancel button and Enter to the OK button.
        /// </summary>
        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CancelButton_Click(sender, e);
            }
            else if (e.Key == Key.Enter && loginButton.IsEnabled)
            {
                LoginButton_Click(sender, e);
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