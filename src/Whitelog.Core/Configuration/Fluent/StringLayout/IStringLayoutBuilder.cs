using System;
using Whitelog.Core.Configuration.Fluent.StringLayout.File;
using Whitelog.Core.Loggers;
using Whitelog.Core.PackageDefinitions;
using Whitelog.Core.String;
using Whitelog.Core.String.Layout;

namespace Whitelog.Core.Configuration.Fluent.StringLayout
{
    public interface ILayoutExtensions
    {
        ILayoutExtensions All { get; }
        
        ILayoutExtensions Astrix { get; }
        ILayoutExtensions Title { get; }
        ILayoutExtensions LongDate { get; }
        ILayoutExtensions ThreadId { get; }

        ILayoutExtensions AddCustom(IStringLayoutFactory layoutFactory);
    }

    public interface IStringAppenders
    {
        IStringAppenders File();
        IStringAppenders File(Func<IStringFileAppenderBuilder, object> file);
        IStringAppenders Console();
        IStringAppenders Console(Func<IConsoleBuilder, object> console);
        
        IStringAppenders Custom(IStringAppender stringAppender);
    }

    public interface IStringLayoutBuilder
    {
        IFilterBuilder<IStringLayoutBuilder> Filter { get; }
        IStringLayoutBuilder SetLayout(string layout);
        IStringLayoutBuilder Extensions(Func<ILayoutExtensions, object> extensions);
        IStringLayoutBuilder Map<T>(Func<PackageDefinition<T>, object> define);
        IStringLayoutBuilder Map(IStringPackageDefinition definition);

        IStringAppenders Appenders { get; }
    }
}