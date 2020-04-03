﻿using System;

namespace NextTech.ChaChing123.Common.Models
{
   public class BOResponseDto
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }
        
        private BOResponseDto()
        {
            ID = 0;
            Order = 0;
            Title = string.Empty;
            Content = string.Empty;
        }
    }
}
