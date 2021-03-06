﻿using System;

namespace NextTech.ChaChing123.Common.Models
{
   public class RequestDTO
    {
        public string UserName { get; set; }

        public string SessionKey { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int ID { get; set; }

        private RequestDTO()
        {
            ID = -1;
            PageIndex = 1;
            PageCount = 10;
            UserName = string.Empty;
            SessionKey = string.Empty;
        }
    }
}
