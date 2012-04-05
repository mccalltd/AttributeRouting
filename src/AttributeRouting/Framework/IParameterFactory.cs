using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AttributeRouting.Framework {

    /// <summary>
    /// Factory methods for getting RouteParameters or UrlParameters
    /// </summary>
    public interface IParameterFactory {

        /// <summary>
        /// Optional parameter
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <returns></returns>
        TParameter Optional<TParameter>();

        /// <summary>
        /// Create a new parameter
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <returns></returns>
        TParameter Create<TParameter>();
    }
}
