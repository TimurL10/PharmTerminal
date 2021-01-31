using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication5.DAL;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserSettingsController : Controller
    {
        private IDbRepository _dbRepository;

        public UserSettingsController(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Post( UserSettings userSettings)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _dbRepository.PostNetSettings(userSettings);
        //        return Ok(userSettings);
        //    }

        //    return BadRequest(ModelState);

        //}
    }
}
