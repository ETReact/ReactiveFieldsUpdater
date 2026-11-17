using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveFieldsUpdater
{
    /// <summary>
    /// This class can help you to store settings for your plugin
    /// </summary>
    /// <remarks>
    /// This class must be XML serializable
    /// </remarks>
    public class Settings
    {
        public string LastUsedOrganizationWebappUrl { get; set; }
        public string CustomUrl { get; set; } = "https://www.activadigital.it/";

        /// <summary>
        /// Metadata properties that can be edited from the Attributes grid.
        /// </summary>
        public string[] Config_Props { get; set; } = new string[]
        {
            "MaxLength",
            "MaxValue",
            "MinValue"
        };

        /// <summary>
        /// Publish the updated entities after the operations have been applied.
        /// </summary>
        public bool PublishAfterUpdate { get; set; } = false;
    }
}