using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ZDC.Core.Data;
using ZDC.Models;

namespace ZDC.Core.Controllers
{
    [Route("api/[controller]")]
    public class FacilitiesController : Controller
    {
        private readonly ZdcContext _context;

        public FacilitiesController(ZdcContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IList<Facility>> GetFacilities()
        {
            return Ok(_context.Facilities.ToList());
        }
    }
}