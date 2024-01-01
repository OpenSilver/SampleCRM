using OpenRiaServices.DomainServices.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SampleCRM.LoginUI
{
    /// <summary>
    /// <see cref="ChildWindow"/> class that controls the registration process.
    /// </summary>
    public partial class LoginRegistrationWindow : ChildWindow
    {
        private IList<OperationBase> _possiblyPendingOperations = new List<OperationBase>();

        /// <summary>
        /// Creates a new <see cref="LoginRegistrationWindow"/> instance.
        /// </summary>
        public LoginRegistrationWindow()
        {
            InitializeComponent();

            registrationForm.SetParentWindow(this);
            loginForm.SetParentWindow(this);

            NavigateToLogin();
        }

        /// <summary>
        /// Ensures the visual state and focus are correct when the window is opened.
        /// </summary>
        protected override void OnOpened()
        {
            base.OnOpened();
            NavigateToLogin();
        }

        /// <summary>
        /// Updates the window title according to which panel (registration / login) is currently being displayed.
        /// </summary>
        private void UpdateTitle(object sender, EventArgs eventArgs)
        {
            Title = (registrationForm.Visibility == Visibility.Visible)
                ? "Register"
                : "Login";
        }

        /// <summary>
        /// Notifies the <see cref="LoginRegistrationWindow"/> window that it can only close if <paramref name="operation"/> is finished or can be cancelled.
        /// </summary>
        /// <param name="operation">The pending operation to monitor</param>
        public void AddPendingOperation(OperationBase operation)
        {
            _possiblyPendingOperations.Add(operation);
        }

        /// <summary>
        /// Causes the <see cref="VisualStateManager"/> to change to the "AtLogin" state.
        /// </summary>
        public virtual void NavigateToLogin()
        {
            loginForm.Visibility = Visibility.Visible;
            registrationForm.Visibility = Visibility.Collapsed;
            UpdateTitle(this, EventArgs.Empty);
        }

        /// <summary>
        /// Causes the <see cref="VisualStateManager"/> to change to the "AtRegistration" state.
        /// </summary>
        public virtual void NavigateToRegistration()
        {
            loginForm.Visibility = Visibility.Collapsed;
            registrationForm.Visibility = Visibility.Visible;
            UpdateTitle(this, EventArgs.Empty);
        }

        /// <summary>
        /// Prevents the window from closing while there are operations in progress
        /// </summary>
        private void LoginWindow_Closing(object sender, CancelEventArgs eventArgs)
        {
            foreach (OperationBase operation in _possiblyPendingOperations)
            {
                if (!operation.IsComplete)
                {
                    if (operation.CanCancel)
                    {
                        operation.Cancel();
                    }
                    else
                    {
                        eventArgs.Cancel = true;
                    }
                }
            }
        }
    }
}