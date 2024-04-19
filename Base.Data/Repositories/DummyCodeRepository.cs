using Base.Data.Models;
using Base.Domain.Interfaces;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Domain;
using System.Globalization;
using System.Linq.Expressions;
using Microsoft.AspNetCore.StaticFiles;
using System.Data;
using ExcelDataReader;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing.Printing;
using Azure;
using Base.Data.Infrastructure;

namespace Base.Data.Repositories
{
    public interface IDummyCodeRepository : IGenericRepository<DummyCode>
    {

    }

    public class DummyCodeRepository : GenericRepository<DummyCode>, IDummyCodeRepository
    {
        public DummyCodeRepository(Task01Context context) : base(context) { }

        
    }
}
