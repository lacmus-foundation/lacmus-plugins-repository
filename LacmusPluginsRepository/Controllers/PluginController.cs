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
    [Route("api/v1")]
    public class PluginController : Controller
    {
        private List<IObjectDetectionPlugin> _plugins;
        private PluginManager _pluginManager;
        private string _packagesBaseDirectory;
        
        public PluginController(IConfiguration configuration)
        {
            _pluginManager = new PluginManager(Path.Join(configuration["PluginsBaseDirectory"], "src"));
            _packagesBaseDirectory = Path.Join(configuration["PluginsBaseDirectory"], "packages");
            _plugins = _pluginManager.FindPlugins();
        }
        // GET
        [HttpGet]
        [Route("plugins")]
        public ActionResult<PluginsResponse> GetPlugins(int page = 0)
        {
            var response = new PluginsResponse
            {
                Plugins = _plugins,
                MaxPage = 1
            };
            return Ok(response);
        }
        
        [HttpGet]
        [Route("plugin")]
        public ActionResult Index([Required, MinLength(1)]string tag, 
                                  [Required, Range(2, 2)]int api, 
                                  [Required, Range(0, Int32.MaxValue)]int major,
                                  [Required, Range(0, Int32.MaxValue)]int minor)
        {
            var packagePath =Path.Join(_packagesBaseDirectory, $"{tag}_{api}.{major}.{minor}.zip");
            if (!System.IO.File.Exists(packagePath))
                return NotFound("No such plugin");
            return PhysicalFile(packagePath, "application/zip", $"{tag}_{api}.{major}.{minor}.zip");
        }
    }
}