// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System.Collections.Generic;
    using System.Web;
    using System.Xml.Schema;

    public interface IXmlValidationRenderer
    {
        void Render(HttpContext context, IEnumerable<ValidationEventArgs> errors);
    }
}
