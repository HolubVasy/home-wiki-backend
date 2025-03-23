﻿using home_wiki_backend.DAL.Common.Models.Bases;

namespace home_wiki_backend.DAL.Common.Models.Entities
{
    public sealed class Tag : ModelBase
    {
        #region Entity relationships

        public HashSet<Article>? Articles { get; init; }

        #endregion
    }
}
