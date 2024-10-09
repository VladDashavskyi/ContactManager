using ContactManager.BLL.DTO;
using ContactManager.DAL.Context;
using ContactManager.Interfaces;
using ContactManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ContactManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly TenantContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IMainService _mainService;

        public HomeController(TenantContext context, ILogger<HomeController> logger, IMainService mainService)
        {
            _context = context;
            _logger = logger;
            _mainService = mainService;
        }

        public async Task<IActionResult> Index()
        {
            var persons = await _mainService.GetPersonsAsync();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var result = await _mainService.UploadFileAsync(file);
            return View("Index",result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveToDb([FromBody] PersonsDto personsDto)
        {
            List<PersonDto> model = personsDto.personDtos.Where(w => string.IsNullOrEmpty(w.ErrorMessage)).ToList();
            await _mainService.SaveFileToDbAsync(model);
            return Ok();
        }

    }
}
