﻿using ApollosLibrary.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Moderation.Queries.GetEntryReportQuery
{
    public class GetEntryReportQueryDto
    {
        public int EntryId { get; set; }
        public EntryTypeEnum EntryType { get; set; }
        public Guid ReportedBy { get; set; }
        public DateTime ReportedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
