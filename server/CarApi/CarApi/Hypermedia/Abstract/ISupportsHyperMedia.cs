using System.Collections.Generic;

namespace CarApi.Hypermedia.Abstract
{
    public interface ISupportsHyperMedia
    {
        List<HyperMediaLink> Links { get; set; }
    }
}
