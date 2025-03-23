namespace HomeWiki.DAL.Resources.Constants;

public static class TablesMetadata
{
    private const string ForeignKeyPrefix = "fk";
    private const string PrimaryKeyPrefix = "pk";

    public static class Article
    {
        public static string Name = nameof(Article).ToLower();
        public static string PrimaryKeyName = $"{PrimaryKeyPrefix}_{Name}";
        public static string ForeignKeyToCategory = $"{ForeignKeyPrefix}_{Name}_{Category.Name}";
    }

    public static class Category
    {
        public static string Name = nameof(Category).ToLower();
        public static string PrimaryKeyName = $"{PrimaryKeyPrefix}_{Name}";
    }

    public static class Tag
    {
        public static string Name = nameof(Tag).ToLower();
        public static string PrimaryKeyName = $"{PrimaryKeyPrefix}_{Name}";
    }

    public static class ArticleTag
    {
        public static string JoinTableName = $"{Article.Name}_{Tag.Name}";
        public static string ForeignKeyArticle = $"{ForeignKeyPrefix}_{JoinTableName}_{Article.Name}";
        public static string ForeignKeyTag = $"{ForeignKeyPrefix}_{JoinTableName}_{Tag.Name}";
    }
}
