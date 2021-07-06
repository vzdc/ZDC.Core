using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZDC.Core.Data;
using ZDC.Core.Extensions;
using ZDC.Core.Services;
using ZDC.Models;

namespace ZDC.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : Controller
    {
        private readonly AzureService _azureService;
        private readonly ZdcContext _context;

        public FilesController(ZdcContext context, AzureService azureService)
        {
            _context = context;
            _azureService = azureService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<File>>> GetFiles()
        {
            var files = await _context.Files.ToListAsync();
            if (await User.IsStaff(_context))
                return Ok(files);
            if (await User.IsTrainingStaff(_context))
                return Ok(files.Where(x => x.Category != FileCategory.Staff).ToList());
            return Ok(
                files.Where(x => x.Category != FileCategory.Staff && x.Category != FileCategory.Training).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<File>> GetFile(int id)
        {
            var file = await _context.Files.FindAsync(id);
            if (!await User.IsStaff(_context) && file.Category == FileCategory.Staff)
                return NotFound($"File {id} not found");
            if (!await User.IsTrainingStaff(_context) && file.Category == FileCategory.Training)
                return NotFound($"File {id} not found");
            return Ok(file);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,FE,ATA,AFE")]
        [HttpPut]
        public async Task<ActionResult<File>> PutFile([FromBody] File data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid file");
            var file = await _context.Files.FindAsync(data.Id);
            if (file == null)
                return NotFound($"File {data.Id} not found");
            data.Updated = DateTime.UtcNow;
            if (data.FormFile != null)
            {
                await _azureService.DeleteFile(data.Url);
                data.Url = await _azureService.UploadFile(data.FormFile);
            }

            _context.Files.Update(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,FE,ATA,AFE")]
        [HttpPost]
        public async Task<ActionResult<File>> PosFile([FromBody] File data)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid file");
            if (data.FormFile != null)
                data.Url = await _azureService.UploadFile(data.FormFile);
            await _context.Files.AddAsync(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        //[Authorize(Roles = "ATM,DATM,TA,WM,FE,ATA,AFE")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<File>> DeleteFile(int id)
        {
            var file = await _context.Files.FindAsync(id);
            if (file == null)
                return NotFound($"File {id} not found");
            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
            return Ok(file);
        }
    }
}