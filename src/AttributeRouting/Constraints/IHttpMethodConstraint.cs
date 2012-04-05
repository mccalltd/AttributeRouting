using System.Collections.Generic;

namespace AttributeRouting.Constraints {
    public interface IHttpMethodConstraint {
        ICollection<string> AllowedMethods { get; }
    }
}