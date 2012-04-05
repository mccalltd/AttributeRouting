using System.Collections.Generic;

namespace AttributeRouting.Constraints {
    public interface IRestfulHttpMethodConstraint {
        ICollection<string> AllowedMethods { get; }
    }
}