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
        private List<IObjectDetectionPlugin> _plugins;
        private readonly PluginManager _pluginManager;
        private string _packagesBaseDirectory;
        
        public PluginController(IConfiguration configuration)
        {
            _pluginManager = new PluginManager(Path.Join(configuration["PluginsBaseDirectory"], "src"));
            _packagesBaseDirectory = Path.Join(configuration["PluginsBaseDirectory"], "packages");
            _plugins = _pluginManager.FindPlugins();
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
        public ActionResult<IEnumerable<IObjectDetectionPlugin>> GetPlugins(int page = 0)
        {
            if (page == 0)
                return Ok(_plugins);
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