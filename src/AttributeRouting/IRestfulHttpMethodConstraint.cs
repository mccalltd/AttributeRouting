using System.Collections.Generic;

namespace AttributeRouting {
    public interface IRestfulHttpMethodConstraint {
        ICollection<string> AllowedMethods { get; }
    }
}