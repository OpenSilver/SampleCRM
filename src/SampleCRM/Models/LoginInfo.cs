using OpenRiaServices.DomainServices.Client;
using OpenRiaServices.DomainServices.Client.ApplicationServices;
using System;
using System.ComponentModel.DataAnnotations;

namespace SampleCRM.LoginUI
{
    /// <summary>
    /// This internal entity is used to ease the binding between the UI controls (DataForm and the label displaying a validation error) and the log on credentials entered by the user.
    /// </summary>
    public class LoginInfo : ComplexObject
    {
        private string _userName;
        //private string _password;
        private bool _rememberMe;
        private LoginOperation _currentLoginOperation;

        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        [Display(Name = "User name")]
        [Required]
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                if (_userName != value)
                {
                    ValidateProperty("UserName", value);
                    _userName = value;
                    RaisePropertyChanged("UserName");
                }
            }
        }

        /// <summary>
        /// Gets or sets a function that returns the password.
        /// </summary>
        internal Func<string> PasswordAccessor { get; set; }


        /// <summary>
        /// Gets and sets the password.
        /// </summary>
        [Required]
        public string Password
        {
            get
            {
                return (PasswordAccessor == null) ? string.Empty : PasswordAccessor();
                //return _password;
            }
            set
            {
                ValidateProperty("Password", value);

                // Do not store the password in a private field as it should not be stored in memory in plain-text.
                // Instead, the supplied PasswordAccessor serves as the backing store for the value.

                RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// Gets and sets the value indicating whether the user's authentication information should be recorded for future logins.
        /// </summary>
        [Display(Name = "Keep me signed in")]
        public bool RememberMe
        {
            get
            {
                return _rememberMe;
            }
            set
            {
                if (_rememberMe != value)
                {
                    ValidateProperty("RememberMe", value);
                    _rememberMe = value;
                    RaisePropertyChanged("RememberMe");
                }
            }
        }

        /// <summary>
        /// Gets or sets the current login operation.
        /// </summary>
        internal LoginOperation CurrentLoginOperation
        {
            get
            {
                return _currentLoginOperation;
            }
            set
            {
                if (_currentLoginOperation != value)
                {
                    if (_currentLoginOperation != null)
                    {
                        _currentLoginOperation.Completed -= (s, e) => CurrentLoginOperationChanged();
                    }

                    _currentLoginOperation = value;

                    if (_currentLoginOperation != null)
                    {
                        _currentLoginOperation.Completed += (s, e) => CurrentLoginOperationChanged();
                    }

                    CurrentLoginOperationChanged();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user is presently being logged in.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public bool IsLoggingIn
        {
            get
            {
                return CurrentLoginOperation != null && !CurrentLoginOperation.IsComplete;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user can presently log in.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public bool CanLogIn
        {
            get
            {
                return !IsLoggingIn;
            }
        }

        /// <summary>
        /// Raises operation-related property change notifications when the current login operation changes.
        /// </summary>
        private void CurrentLoginOperationChanged()
        {
            RaisePropertyChanged("IsLoggingIn");
            RaisePropertyChanged("CanLogIn");
        }

        /// <summary>
        /// Creates a new <see cref="LoginParameters"/> instance using the data stored in this entity.
        /// </summary>
        public LoginParameters ToLoginParameters()
        {
            return new LoginParameters(UserName, Password, RememberMe, null);
        }
    }
}