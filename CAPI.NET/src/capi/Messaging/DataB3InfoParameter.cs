
namespace Mommosoft.Capi.Messaging {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.Specialized;

    public class DataB3InfoParameter : Parameter<short> {
        protected static readonly BitVector32.Section QualifierSection = BitVector32.CreateSection(1);
        protected static readonly BitVector32.Section MoreDataSection = BitVector32.CreateSection(1, QualifierSection);
        protected static readonly BitVector32.Section DeliveryConfirmationSection = BitVector32.CreateSection(1, MoreDataSection);
        protected static readonly BitVector32.Section ExpeditedDataSection = BitVector32.CreateSection(1, DeliveryConfirmationSection);
        protected static readonly BitVector32.Section BreakUISection = BitVector32.CreateSection(1, ExpeditedDataSection);
        protected static readonly BitVector32.Section FramingErrorSection = BitVector32.CreateSection(1, DeliveryConfirmationSection);
    }
}
