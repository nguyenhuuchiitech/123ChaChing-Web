﻿using System;

namespace NextTech.ChaChing123.Common.Models
{
   public class RequestOrderListDTO
    {
        public string UserName { get; set; }

        public string SessionKey { get; set; }

        public string KeyWord { get; set; }

        public string PaymentState { get; set; }

        public string AffiliateState { get; set; }

        public string AffiliateAccount { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        private RequestOrderListDTO()
        {
            UserName = string.Empty;
            SessionKey = string.Empty;
            KeyWord = string.Empty;
            PaymentState = "1,2,3";
            AffiliateState = "1,2,3";
            AffiliateAccount = string.Empty;
            PageIndex = 1;
            PageCount = 10;
        }
    }
}
