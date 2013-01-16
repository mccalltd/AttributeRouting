using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AttributeRouting
{
    // indicates that this route is versioned
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RouteVersionedAttribute : Attribute
    {
        public RouteVersionedAttribute(bool isVersioned = true) 
        {
            IsVersioned = isVersioned;
        }

        public bool IsVersioned { get; private set; }

        /// <summary>
        /// Minimum verison supported. Optional, can be overridden by individual route attributes.
        /// </summary>
        public SemanticVersion MinVersion { get; set; }

        /// <summary>
        /// Maximum verison supported. Optional, can be overridden by individual route attributes.
        /// </summary>
        public SemanticVersion MaxVersion { get; set; }


        /// <summary>
        /// Shortcut to set <see cref="MinVersion"/> with a string
        /// </summary>
        public string MinVer
        {
            get { return MinVersion.ToString(); }
            set { MinVersion = SemanticVersion.Parse(value, allowNull:true); }
        }

        /// <summary>
        /// Shortcut to set <see cref="MaxVersion"/> with a string
        /// </summary>
        public string MaxVer
        {
            get { return MaxVersion.ToString(); }
            set { MaxVersion = SemanticVersion.Parse(value, allowNull:true); }
        }

    }
}
