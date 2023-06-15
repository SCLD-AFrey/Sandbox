﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace Sandbox.UserData
{

    public partial class UserCredential : XPObject
    {
        User fUser;
        [Association(@"UserCredentialReferencesUser")]
        public User User
        {
            get { return fUser; }
            set { SetPropertyValue<User>(nameof(User), ref fUser, value); }
        }
        string fPassword;
        [Size(SizeAttribute.Unlimited)]
        public string Password
        {
            get { return fPassword; }
            set { SetPropertyValue<string>(nameof(Password), ref fPassword, value); }
        }
        byte[] fSalt;
        [MemberDesignTimeVisibility(true)]
        public byte[] Salt
        {
            get { return fSalt; }
            set { SetPropertyValue<byte[]>(nameof(Salt), ref fSalt, value); }
        }
        DateTime fDateAdded;
        public DateTime DateAdded
        {
            get { return fDateAdded; }
            set { SetPropertyValue<DateTime>(nameof(DateAdded), ref fDateAdded, value); }
        }
    }

}
