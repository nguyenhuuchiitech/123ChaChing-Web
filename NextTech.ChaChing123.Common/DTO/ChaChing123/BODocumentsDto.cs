﻿using System;

namespace NextTech.ChaChing123.Common.Models
{
   public class BODocumentsItem1DTO
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public int Active { get; set; }

        public int Type { get; set; }
        public int IsAdvanced { get; set; }

        private BODocumentsItem1DTO()
        {
            ID = -1;
            Order = 0;
            Type = 0;
            IsAdvanced = 0;
            Active = 1;
            Title = string.Empty;
            Description = string.Empty;
        }
    }
}
