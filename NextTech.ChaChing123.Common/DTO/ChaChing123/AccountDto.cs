﻿using System;

namespace NextTech.ChaChing123.Common.Models
{
    public class AccountInfoDTO 
    {
        //public int ID { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        //public string Password { get; set; }

        public int AccountType { get; set; }

        //public string RefCode { get; set; }

        //public string ParentCode { get; set; }

        public int Status { get; set; }

        public bool IsLock { get; set; }

        public bool IsLockAffilate { get; set; }

        //public string ContractNo { get; set; }

        //public int RenewalNo { get; set; }

        //public string TrailDate { get; set; }

       // public string MADA { get; set; }

       // public string SubDomain { get; set; }

        //public string SessionKey { get; set; }

        

        //public string CreatedBy { get; set; }

        //public DateTime CreatedDate { get; set; }

        //public string UpdatedBy { get; set; }

        //public DateTime UpdatedDate { get; set; }

        public AccountInfoDTO()
        {
            this.FullName = string.Empty;
            this.Email = string.Empty;
            this.UserName = string.Empty;
            this.Email = string.Empty;
            //this.YearOfBirth = new DateTime(1900, 01, 01);
            this.Phone = string.Empty;
           // this.RefCode = string.Empty;
            //this.Password = string.Empty;
            //this.ParentCode = string.Empty;
            this.AccountType = 1;
            this.Status = 1;
            //this.CreatedBy = string.Empty;
            //this.CreatedDate = DateTime.Now;
            //this.UpdatedBy = string.Empty;
            //this.UpdatedDate = DateTime.Now;
            //this.XXX = string.Empty;
            //this.SessionKey = string.Empty;
        }

    }
}