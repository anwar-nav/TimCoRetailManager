﻿using System;

namespace TRMDesktopUI.Library.Models
{
    /// <summary>
    /// This is an Interface of LoggedInUser class
    /// </summary>
    public interface ILoggedInUserModel
    {
        DateTime CreatedDate { get; set; }
        string EmailAddress { get; set; }
        string FirstName { get; set; }
        string Id { get; set; }
        string LastName { get; set; }
        string Token { get; set; }
        void ResetLoggedInUserModel();
    }
}