using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using LacmusPlugin;
using LacmusPluginsRepository.Helpers;
using LacmusPluginsRepository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LacmusPluginsRepository.Controllers
{
    [Route("plugin-repository/api/v1")]
    public class PluginController : Controller
    {
        private string _packagesBaseDirectory;
        public PluginController(IConfiguration configuration)
        {
            _packagesBaseDirectory = Path.Join(configuration["PluginsBaseDirectory"], "packages");
        }
        // GET
        [HttpGet]
        [Route("pagesCount")]
        public ActionResult<PagesCount> GetMaxPage()
        {
            return Ok(new PagesCount{ Count = 1 });
        }
        
        [HttpGet]
        [Route("plugins")]
        public ActionResult<IEnumerable<IObjectDetectionPlugin>> GetPlugins(
            [FromServices] IEnumerable<IObjectDetectionPlugin> plugins, 
            int page = 0)
        {
            if (page == 0)
                return Ok(plugins);
            return NotFound();
        }
        
        [HttpGet]
        [Route("plugin")]
        public ActionResult Index([Required, MinLength(1)]string tag, 
                                  [Required, Range(2, 2)]int api, 
                                  [Required, Range(0, int.MaxValue)]int major,
                                  [Required, Range(0, int.MaxValue)]int minor)
        {
            var packagePath =Path.Join(_packagesBaseDirectory, $"{tag}_{api}.{major}.{minor}.zip");
            if (!System.IO.File.Exists(packagePath))
                return NotFound("No such plugin");
            return PhysicalFile(packagePath, "application/zip", $"{tag}_{api}.{major}.{minor}.zip");
        }
    }
}