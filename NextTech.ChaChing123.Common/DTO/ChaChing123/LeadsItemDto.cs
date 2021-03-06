﻿using System;

namespace NextTech.ChaChing123.Common.Models
{
   public class LeadsItemDTO
    {
        public int ID { get; set; }

        public string CreatedDate { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        public int LeadType { get; set; }
        public string TypeName { get; set; }
        public string AffialateName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public int SoloID { get; set; }
        public string SoloLink { get; set; }        
        public string SoloPageName { get; set; }
        public int FunnalID { get; set; }
        public string FunnalLink { get; set; }
        public string FunnalPageName { get; set; }
        public string Notes { get; set; }

        private LeadsItemDTO()
        {
            ID = -1;
            SoloID = 0;
            FunnalID = 0;
            CreatedDate = string.Empty;
            Name = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            LeadType = -1;
            TypeName = string.Empty;
            AffialateName = string.Empty;
            Status = -1;
            StatusName = string.Empty;
            SoloLink = string.Empty;
            Notes = string.Empty;
            SoloPageName = string.Empty;
            FunnalPageName = string.Empty;
            FunnalLink = string.Empty;
        }
    }
}
