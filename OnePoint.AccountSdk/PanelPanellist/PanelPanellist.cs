﻿using System;
using System.Collections.Generic;

namespace OnePoint.AccountSdk.PanelPanellist
{
    public class Panel
    {
        public int PanellistCount { get; set; }
        public string MediaUrl { get; set; }
        public string LogoUrl { get; set; }
        public long PanelId { get; set; }
        public long ThemeTemplateId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PanelType { get; set; }
        public string SearchTag { get; set; }
        public string Remark { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public long MediaId { get; set; }
        public long LogoId { get; set; }
    }

    public class Panellist
    {
        public int PanellistId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string DOB { get; set; }
        public string Website { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public string MediaId { get; set; }
        public int CountryCode { get; set; }
        public bool TermsCondition { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedDate { get; set; }
        public string LastUpdatedDate { get; set; }
        public string SearchTag { get; set; }
        public int Gender { get; set; }
        public int MaritalStatus { get; set; }
    }

    public class Country
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string Std { get; set; }
        public string Gmt { get; set; }
        public int CreditRate { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ProfileVariable
    {
        public int VariableId { get; set; }
        public string Name { get; set; }
        public int PanelId { get; set; }
        public int TypeId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsBasic { get; set; }
        public string CreatedDate { get; set; }
        public string LastUpdatedDate { get; set; }
    }

    public class PanellistRootObject
    {
        public List<Panellist> Panellist { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class PanelRootObject
    {
        public List<Panel> Panels { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class ProfileVariableRootObject
    {
        public List<ProfileVariable> ProfileElements { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
    }
}
