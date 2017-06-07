using System;
using System.Collections.Generic;

namespace NewsMonitoringSystem.Data.Entities.Generated
{
    public partial class Document : IEqualityComparer<Document>, IEquatable<Document>
    {
        #region IEqualityComparer and IEquatable

        int IEqualityComparer<Document>.GetHashCode(Document obj)
        {
            return obj.DocumentId.GetHashCode()
                   + obj.Title.GetHashCode()
                   + obj.HtmlContent.GetHashCode();
        }

        bool IEqualityComparer<Document>.Equals(Document x, Document y)
        {
            if (x == null || y == null)
                return false;

           if (x.DocumentId != Guid.Empty
               && y.DocumentId != Guid.Empty)
            {
                return x.DocumentId == y.DocumentId;
            }

            return x.Title.Equals(y.Title)
                   && x.HtmlContent.Equals(y.HtmlContent)
                   && x.Link.Equals(y.Link);
        }

        bool IEquatable<Document>.Equals(Document other)
        {
            return ((IEqualityComparer<Document>)this).Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            return ((IEqualityComparer<Document>)this).Equals(this, obj as Document);
        }

        #endregion

    }
}
