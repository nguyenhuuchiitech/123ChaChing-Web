﻿using System;

namespace NextTech.ChaChing123.Common.Models
{
   public class LockAffilateDTO
    {
        
        public string UserName { get; set; }

        public string SessionKey { get; set; }

        public int IsLockAffialate { get; set; }

        private LockAffilateDTO()
        {
            IsLockAffialate = 0;
            UserName = string.Empty;
            SessionKey = string.Empty;
        }
    }
}
