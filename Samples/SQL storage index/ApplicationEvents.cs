using FormEditor.SqlIndex.Storage;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace FormEditor.SqlIndex
{
	public class ApplicationEvents : ApplicationEventHandler
	{
		protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
		{
			var dbContext = applicationContext.DatabaseContext;
			var db = new DatabaseSchemaHelper(dbContext.Database, applicationContext.ProfilingLogger.Logger, dbContext.SqlSyntax);

			// automatically create the tables for the index on app start
			if (db.TableExist("FormEditorEntries") == false)
			{
				db.CreateTable<Entry>(false);
				// hack to get SQL server to use NVARCHAR(MAX) datatype
				dbContext.Database.Execute("ALTER TABLE FormEditorEntries ALTER COLUMN FieldValues NVARCHAR(MAX)");
			}
			if (db.TableExist("FormEditorFiles") == false)
			{
				db.CreateTable<File>(false);
			}
		}
	}
}