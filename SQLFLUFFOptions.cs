using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SQLDefuzzer
{
    public enum DialectOption
    {
        [Description("ansi Dialect [inherits from 'nothing']")]
        Ansi,
        [Description("athena Dialect [inherits from 'ansi']")]
        Athena,
        [Description("bigquery Dialect [inherits from 'ansi']")]
        Bigquery,
        [Description("clickhouse Dialect [inherits from 'ansi']")]
        Clickhouse,
        [Description("databricks Dialect [inherits from 'sparksql']")]
        Databricks,
        [Description("db2 Dialect [inherits from 'ansi']")]
        Db2,
        [Description("duckdb Dialect [inherits from 'postgres']")]
        Duckdb,
        [Description("exasol Dialect [inherits from 'ansi']")]
        Exasol,
        [Description("greenplum Dialect [inherits from 'postgres']")]
        Greenplum,
        [Description("hive Dialect [inherits from 'ansi']")]
        Hive,
        [Description("materialize Dialect [inherits from 'postgres']")]
        Materialize,
        [Description("mysql Dialect [inherits from 'ansi']")]
        Mysql,
        [Description("oracle Dialect [inherits from 'ansi']")]
        Oracle,
        [Description("postgres Dialect [inherits from 'ansi']")]
        Postgres,
        [Description("redshift Dialect [inherits from 'postgres']")]
        Redshift,
        [Description("snowflake Dialect [inherits from 'ansi']")]
        Snowflake,
        [Description("soql Dialect [inherits from 'ansi']")]
        Soql,
        [Description("sparksql Dialect [inherits from 'ansi']")]
        Sparksql,
        [Description("sqlite Dialect [inherits from 'ansi']")]
        Sqlite,
        [Description("teradata Dialect [inherits from 'ansi']")]
        Teradata,
        [Description("trino Dialect [inherits from 'ansi']")]
        Trino,
        [Description("tsql Dialect [inherits from 'ansi']")]
        Tsql
    }

    public enum TemplaterOption
    {
        [Description("Raw")]
        Raw,
        [Description("Jinja")]
        Jinja,
        [Description("Python")]
        Python,
        [Description("Placeholder")]
        Placeholder
    }

    public enum AliasModalityOption
    {
        [Description("Implicit")]
        Implicit,
        [Description("Explicit")]
        Explicit
    }

    public enum ColumnReferenceModalityOption
    {
        [Description("Consistent")]
        Consistent,
        [Description("Implicit")]
        Implicit,
        [Description("Explicit")]
        Explicit
    }

    public enum StringCaseOption
    {
        [Description("Consistent")]
        Consistent,
        [Description("Upper")]
        Upper,
        [Description("Lower")]
        Lower,
        [Description("Capitalise")]
        Capitalise
    }

    public enum StringCasePascalOption
    {
        [Description("Consistent")]
        Consistent,
        [Description("Upper")]
        Upper,
        [Description("Lower")]
        Lower,
        [Description("Pascal")]
        Pascal,
        [Description("Capitalise")]
        Capitalise
    }

    public enum QuotedLiteralsOption
    {
        [Description("Consistent")]
        Consistent,
        [Description("Single Quotes")]
        SingleQuotes,
        [Description("Double Quotes")]
        DoubleQuotes
    }

    public enum CastingStyleOption
    {
        [Description("Consistent")]
        Consistent,
        [Description("Shorthand")]
        Shorthand,
        [Description("Convert")]
        Convert,
        [Description("Cast")]
        Cast
    }

    public enum ConsistentReferencesOption
    {
        [Description("Consistent")]
        Consistent,
        [Description("Qualified")]
        Qualified,
        [Description("Unqualified")]
        Unqualified
    }

    public enum QuotedIdentifiersReferencesToFlagOption
    {
        [Description("All")]
        All,
        [Description("Aliases")]
        Aliases,
        [Description("Column Aliases")]
        ColumnAliases,
        [Description("None")]
        None
    }

    public enum UnQuotedIdentifiersReferencesToFlagOption
    {
        [Description("All")]
        All,
        [Description("Aliases")]
        Aliases,
        [Description("Column Aliases")]
        ColumnAliases
    }

    public enum JoinFromClauseOption
    {
        [Description("Join")]
        Join,
        [Description("From")]
        From,
        [Description("Both")]
        Both
    }

    public enum JoinConditionOrderOption
    {
        [Description("Earlier")]
        Earlier,
        [Description("Later")]
        Later
    }

    public enum FullyQualifyJoinTypesOption
    {
        [Description("Inner")]
        Inner,
        [Description("Outer")]
        Outer,
        [Description("Both")]
        Both
    }

    public enum SelectTrailingCommaOption
    {
        [Description("Forbid")]
        Forbid,
        [Description("Require")]
        Require
    }

    public enum SelectTargetsOption
    {
        [Description("Single")]
        Single,
        [Description("Multiple")]
        Multiple
    }

    public enum CommentOrderingOption
    {
        [Description("Forbid")]
        Forbid,
        [Description("Require")]
        Require
    }

    public enum CommentOrderOption
    {
        [Description("Before")]
        Before,
        [Description("After")]
        After
    }

    public enum IndentUnitOption
    {
        [Description("Space")]
        Space,
        [Description("Tab")]
        Tab
    }

    [UseDescription]
    public enum LinePositionOption
    {
        [Description("Trailing")]
        Trailing,
        [Description("Leading")]
        Leading,
        [Description("Alone")]
        Alone,
        [Description("Trailing:Strict")]
        TrailingStrict,
        [Description("Leading:Strict")]
        LeadingStrict,
        [Description("Alone:Strict")]
        AloneStrict
    }

    [UseDescription]
    public enum SpacingOption
    {
        [Description("Touch")]
        Touch,
        [Description("Touch:Inline")]
        TouchInline,
        [Description("Single")]
        Single,
        [Description("Single:Inline")]
        SingleInline,
        [Description("Any")]
        Any
    }

    public class EnumOptionConverter<T> : EnumConverter
    {
        public EnumOptionConverter(Type type) : base(type) { }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(Enum.GetValues(typeof(T)));
        }
    }

    internal class SQLFLUFFOptions : DialogPage
    {
        [Category("core")]
        [DisplayName("Verbose")]
        [Description("Verbose is an integer (0-2) indicating the level of log output.")]
        [SQLFluffField("verbose")]
        public int Verbose { get; set; } = 0;

        [Category("core")]
        [DisplayName("No Color")]
        [Description("Turn off color formatting of output.")]
        [SQLFluffField("nocolor")]
        public bool NoColor { get; set; } = false;

        [Category("core")]
        [DisplayName("Dialect")]
        [Description("Supported dialects https://docs.sqlfluff.com/en/stable/dialects.html")]
        [SQLFluffField("dialect")]
        [TypeConverter(typeof(EnumOptionConverter<DialectOption>))]
        public DialectOption Dialect { get; set; } = DialectOption.Ansi;

        [Category("core")]
        [DisplayName("Templater")]
        [Description("One of [raw|jinja|python|placeholder]")]
        [SQLFluffField("templater")]
        [TypeConverter(typeof(EnumOptionConverter<TemplaterOption>))]
        public TemplaterOption Templater { get; set; } = TemplaterOption.Raw;

        [Category("core")]
        [DisplayName("Rules")]
        [Description("Comma separated list of rules to check, default to all.")]
        [SQLFluffField("rules")]
        public string Rules { get; set; } = "all";

        [Category("core")]
        [DisplayName("Exclude Rules")]
        [Description("Comma separated list of rules to exclude, or None.")]
        [SQLFluffField("exclude_rules")]
        public string ExcludeRules { get; set; } = null;

        [Category("core")]
        [DisplayName("Output Line Length")]
        [Description("Below controls SQLFluff output, see max_line_length for SQL output.")]
        [SQLFluffField("output_line_length")]
        public int OutputLineLength { get; set; } = 80;

        [Category("core")]
        [DisplayName("Runaway Limit")]
        [Description("Number of passes to run before admitting defeat.")]
        [SQLFluffField("runaway_limit")]
        public int RunawayLimit { get; set; } = 10;

        [Category("core")]
        [DisplayName("Ignore")]
        [Description("Ignore errors by category (one or more of the following, separated by commas: lexing,linting,parsing,templating).")]
        [SQLFluffField("ignore")]
        public string Ignore { get; set; } = null;

        [Category("core")]
        [DisplayName("Warnings")]
        [Description("Warn only for rule codes (one of more rule codes, seperated by commas: e.g. LT01,LT02). Also works for templating and parsing errors by using TMP or PRS.")]
        [SQLFluffField("warnings")]
        public string Warnings { get; set; } = null;

        [Category("core")]
        [DisplayName("Warn Unused Ignores")]
        [Description("Whether to warn about unneeded '-- noqa:' comments.")]
        [SQLFluffField("warn_unused_ignores")]
        public bool WarnUnusedIgnores { get; set; } = false;

        [Category("core")]
        [DisplayName("Ignore Templated Areas")]
        [Description("Ignore linting errors found within sections of code coming directly from templated code (e.g. from within Jinja curly braces. Note that it does not ignore errors from literal code found within template loops.")]
        [SQLFluffField("ignore_templated_areas")]
        public bool IgnoreTemplatedAreas { get; set; } = true;

        [Category("core")]
        [DisplayName("Encoding")]
        [Description("Can either be autodetect or a valid encoding e.g. utf-8, utf-8-sig.")]
        [SQLFluffField("encoding")]
        public string Encoding { get; set; } = "autodetect";

        [Category("core")]
        [DisplayName("Disable Noqa")]
        [Description("Ignore inline overrides (e.g. to test if still required).")]
        [SQLFluffField("disable_noqa")]
        public bool DisableNoqa { get; set; } = false;

        [Category("core")]
        [DisplayName("SQL File Extensions")]
        [Description("Comma separated list of file extensions to lint. NB: This config will only apply in the root folder.")]
        [SQLFluffField("sql_file_exts")]
        public string SQLFileExts { get; set; } = ".sql,.sql.j2,.dml,.ddl";

        [Category("core")]
        [DisplayName("Fix Even Unparsable")]
        [Description("Allow fix to run on files, even if they contain parsing errors. Note altering this is NOT RECOMMENDED as can corrupt SQL.")]
        [SQLFluffField("fix_even_unparsable")]
        public bool FixEvenUnparsable { get; set; } = false;

        [Category("core")]
        [DisplayName("Large File Skip Char Limit")]
        [Description("Very large files can make the parser effectively hang. The more efficient check is the _byte_ limit check which is enabled by default. The previous _character_ limit check is still present for backward compatibility. This will be removed in a future version. Set either to 0 to disable.")]
        [SQLFluffField("large_file_skip_char_limit")]
        public int LargeFileSkipCharLimit { get; set; } = 0;

        [Category("core")]
        [DisplayName("Large File Skip Byte Limit")]
        [Description("Very large files can make the parser effectively hang. The more efficient check is the _byte_ limit check which is enabled by default. The previous _character_ limit check is still present for backward compatibility. This will be removed in a future version. Set either to 0 to disable.")]
        [SQLFluffField("large_file_skip_byte_limit")]
        public int LargeFileSkipByteLimit { get; set; } = 20000;

        [Category("core")]
        [DisplayName("Processes")]
        [Description("CPU processes to use while linting. If positive, just implies number of processes. If negative or zero, implies number_of_cpus - specified_number. e.g. -1 means use all processors but one. 0  means all cpus.")]
        [SQLFluffField("processes")]
        public int Processes { get; set; } = 1;

        [Category("core")]
        [DisplayName("Max Line Length")]
        [Description("Max line length is set by default to be in line with the dbt style guide. https://github.com/dbt-labs/corp/blob/main/dbt_style_guide.md Set to zero or negative to disable checks.")]
        [SQLFluffField("max_line_length")]
        public int MaxLineLength { get; set; } = 80;

        [Category("indentation")]
        [DisplayName("Indent Unit")]
        [Description("Indent Unit. Refer to https://docs.sqlfluff.com/en/stable/layout.html#configuring-indent-locations")]
        [SQLFluffField("indent_unit")]
        [TypeConverter(typeof(EnumOptionConverter<IndentUnitOption>))]
        public IndentUnitOption IndentUnitType { get; set; } = IndentUnitOption.Space;

        [Category("indentation")]
        [DisplayName("Tab Space Size")]
        [Description("Tab Space Size.")]
        [SQLFluffField("tab_space_size")]
        public int TabSpaceSize { get; set; } = 4;

        [Category("indentation")]
        [DisplayName("Indented Joins")]
        [Description("Indented Joins.")]
        [SQLFluffField("indented_joins")]
        public bool IndentedJoins { get; set; } = false;

        [Category("indentation")]
        [DisplayName("Indented CTEs")]
        [Description("Indented CTEs.")]
        [SQLFluffField("indented_ctes")]
        public bool IndentedCTEs { get; set; } = false;

        [Category("indentation")]
        [DisplayName("Indented Using On")]
        [Description("Indented Using On.")]
        [SQLFluffField("indented_using_on")]
        public bool IndentedUsingOn { get; set; } = true;

        [Category("indentation")]
        [DisplayName("Indented On Contents")]
        [Description("Indented On Contents.")]
        [SQLFluffField("indented_on_contents")]
        public bool IndentedOnContents { get; set; } = true;

        [Category("indentation")]
        [DisplayName("Indented Then")]
        [Description("Indented Then.")]
        [SQLFluffField("indented_then")]
        public bool IndentedThen { get; set; } = true;

        [Category("indentation")]
        [DisplayName("Indented Then Contents")]
        [Description("Indented Then Contents.")]
        [SQLFluffField("indented_then_contents")]
        public bool IndentedThenContents { get; set; } = true;

        [Category("indentation")]
        [DisplayName("Allow Implicit Indents")]
        [Description("Allow Implicit Indents.")]
        [SQLFluffField("allow_implicit_indents")]
        public bool AllowImplicitIndents { get; set; } = false;

        [Category("indentation")]
        [DisplayName("Template Blocks Indent")]
        [Description("Template Blocks Indent.")]
        [SQLFluffField("template_blocks_indent")]
        public bool TemplateBlocksIndent { get; set; } = true;

        [Category("indentation")]
        [DisplayName("Skip Indentation In")]
        [Description("Skip Indentation In.")]
        [SQLFluffField("skip_indentation_in")]
        public string SkipIndentationIn { get; set; } = "script_content";

        //[Category("indentation")]
        //[DisplayName("Trailing Comments")]
        //[Description("Trailing Comments.")]
        //[SQLFluffField("trailing_comments")]
        //[TypeConverter(typeof(EnumOptionConverter<CommentOrderingOption>))]
        //public CommentOrderingOption TrailingComments { get; set; } = CommentOrderingOption.Forbid;

        [Category("indentation")]
        [DisplayName("Trailing Comments")]
        [Description("Trailing Comments.")]
        [SQLFluffField("trailing_comments")]
        [TypeConverter(typeof(EnumOptionConverter<CommentOrderOption>))]
        public CommentOrderOption TrailingCommentsOrder { get; set; } = CommentOrderOption.Before;

        [Category("layout:type:comma")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption CommaSpacingBefore { get; set; } = SpacingOption.Touch;

        [Category("layout:type:comma")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        [TypeConverter(typeof(EnumOptionConverter<LinePositionOption>))]
        public LinePositionOption CommaLinePosition { get; set; } = LinePositionOption.Trailing;

        [Category("layout:type:binary_operator")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption BinaryOperatorSpacingWithin { get; set; } = SpacingOption.Touch;

        [Category("layout:type:binary_operator")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        [TypeConverter(typeof(EnumOptionConverter<LinePositionOption>))]
        public LinePositionOption BinaryOperatorLinePosition { get; set; } = LinePositionOption.Leading;

        [Category("layout:type:statement_terminator")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption StatementTerminatorSpacingBefore { get; set; } = SpacingOption.Touch;

        [Category("layout:type:statement_terminator")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        [TypeConverter(typeof(EnumOptionConverter<LinePositionOption>))]
        public LinePositionOption StatementTerminatorLinePosition { get; set; } = LinePositionOption.Trailing;

        [Category("layout:type:end_of_file")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption EndOfFileSpacingBefore { get; set; } = SpacingOption.Touch;

        [Category("layout:type:set_operator")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        [TypeConverter(typeof(EnumOptionConverter<LinePositionOption>))]
        public LinePositionOption SetOperatorLinePosition { get; set; } = LinePositionOption.AloneStrict;

        [Category("layout:type:start_bracket")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption StartBracketSpacingAfter { get; set; } = SpacingOption.Touch;

        [Category("layout:type:end_bracket")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption EndBracketSpacingBefore { get; set; } = SpacingOption.Touch;

        [Category("layout:type:start_square_bracket")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption StartSquareBracketSpacingAfter { get; set; } = SpacingOption.Touch;

        [Category("layout:type:end_square_bracket")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption EndSquareBracketSpacingBefore { get; set; } = SpacingOption.Touch;

        [Category("layout:type:start_angle_bracket")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption StartAngleBracketSpacingAfter { get; set; } = SpacingOption.Touch;

        [Category("layout:type:end_angle_bracket")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption EndAngleBracketSpacingBefore { get; set; } = SpacingOption.Touch;

        [Category("layout:type:casting_operator")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption CastingOperatorSpacingBefore { get; set; } = SpacingOption.Touch;

        [Category("layout:type:casting_operator")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption CastingOperatorSpacingAfter { get; set; } = SpacingOption.TouchInline;

        [Category("layout:type:slice")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption SliceSpacingBefore { get; set; } = SpacingOption.Touch;

        [Category("layout:type:slice")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption SliceSpacingAfter { get; set; } = SpacingOption.Touch;

        [Category("layout:type:dot")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption DotSpacingBefore { get; set; } = SpacingOption.Touch;

        [Category("layout:type:dot")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption DotSpacingAfter { get; set; } = SpacingOption.Touch;

        [Category("layout:type:comparison_operator")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption ComparisonOperatorSpacingWithin { get; set; } = SpacingOption.Touch;

        [Category("layout:type:comparison_operator")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        [TypeConverter(typeof(EnumOptionConverter<LinePositionOption>))]
        public LinePositionOption ComparisonOperatorLinePosition { get; set; } = LinePositionOption.Leading;

        [Category("layout:type:assignment_operator")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption AssignmentOperatorSpacingWithin { get; set; } = SpacingOption.Touch;

        [Category("layout:type:assignment_operator")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        [TypeConverter(typeof(EnumOptionConverter<LinePositionOption>))]
        public LinePositionOption AssignmentOperatorLinePosition { get; set; } = LinePositionOption.Leading;

        [Category("layout:type:object_reference")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption ObjectReferenceSpacingWithin { get; set; } = SpacingOption.TouchInline;

        [Category("layout:type:numeric_literal")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption NumericLiteralSpacingWithin { get; set; } = SpacingOption.TouchInline;

        [Category("layout:type:sign_indicator")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption SignIndicatorSpacingAfter { get; set; } = SpacingOption.TouchInline;

        [Category("layout:type:tilde")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption TildeSpacingAfter { get; set; } = SpacingOption.TouchInline;

        [Category("layout:type:function_name")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption FunctionNameSpacingWithin { get; set; } = SpacingOption.TouchInline;

        [Category("layout:type:function_name")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption FunctionNameSpacingAfter { get; set; } = SpacingOption.TouchInline;

        [Category("layout:type:array_type")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption ArrayTypeSpacingWithin { get; set; } = SpacingOption.TouchInline;

        [Category("layout:type:typed_array_literal")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption TypedArrayLiteralSpacingWithin { get; set; } = SpacingOption.Touch;

        [Category("layout:type:sized_array_type")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption SizedArrayTypeSpacingWithin { get; set; } = SpacingOption.Touch;

        [Category("layout:type:struct_type")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption StructTypeSpacingWithin { get; set; } = SpacingOption.TouchInline;

        [Category("layout:type:bracketed_arguments")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption BracketedArgumentsSpacingBefore { get; set; } = SpacingOption.TouchInline;

        [Category("layout:type:typed_struct_literal")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption TypedStructLiteralSpacingWithin { get; set; } = SpacingOption.Touch;

        [Category("layout:type:semi_structured_expression")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption SemiStructuredExpressionSpacingWithin { get; set; } = SpacingOption.TouchInline;

        [Category("layout:type:semi_structured_expression")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption SemiStructuredExpressionSpacingBefore { get; set; } = SpacingOption.TouchInline;

        [Category("layout:type:array_accessor")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption ArrayAccessorSpacingBefore { get; set; } = SpacingOption.TouchInline;

        [Category("layout:type:colon")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption ColonSpacingBefore { get; set; } = SpacingOption.Touch;

        [Category("layout:type:colon_delimiter")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption ColonDelimiterSpacingBefore { get; set; } = SpacingOption.Touch;

        [Category("layout:type:colon_delimiter")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption ColonDelimiterSpacingAfter { get; set; } = SpacingOption.Touch;

        [Category("layout:type:path_segment")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption PathSegmentSpacingWithin { get; set; } = SpacingOption.Touch;

        [Category("layout:type:sql_conf_option")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption SQLConfOptionSpacingWithin { get; set; } = SpacingOption.Touch;

        [Category("layout:type:sqlcmd_operator")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption SQLCmdOperatorSpacingBefore { get; set; } = SpacingOption.Touch;

        [Category("layout:type:comment")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption CommentSpacingBefore { get; set; } = SpacingOption.Any;

        [Category("layout:type:comment")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption CommentSpacingAfter { get; set; } = SpacingOption.Any;

        [Category("layout:type:pattern_expression")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption PatternExpressionSpacingWithin { get; set; } = SpacingOption.Any;

        [Category("layout:type:placeholder")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption PlaceholderSpacingBefore { get; set; } = SpacingOption.Any;

        [Category("layout:type:placeholder")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption PlaceholderSpacingAfter { get; set; } = SpacingOption.Any;

        [Category("layout:type:common_table_expression")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        [TypeConverter(typeof(EnumOptionConverter<SpacingOption>))]
        public SpacingOption CommonTableExpressionSpacingWithin { get; set; } = SpacingOption.SingleInline;

        [Category("layout:type:select_clause")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        [TypeConverter(typeof(EnumOptionConverter<LinePositionOption>))]
        public LinePositionOption SelectClauseLinePosition { get; set; } = LinePositionOption.Alone;

        [Category("layout:type:where_clause")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        [TypeConverter(typeof(EnumOptionConverter<LinePositionOption>))]
        public LinePositionOption WhereClauseLinePosition { get; set; } = LinePositionOption.Alone;

        [Category("layout:type:from_clause")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        [TypeConverter(typeof(EnumOptionConverter<LinePositionOption>))]
        public LinePositionOption FromClauseLinePosition { get; set; } = LinePositionOption.Alone;

        [Category("layout:type:join_clause")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        [TypeConverter(typeof(EnumOptionConverter<LinePositionOption>))]
        public LinePositionOption JoinClauseLinePosition { get; set; } = LinePositionOption.Alone;

        [Category("layout:type:groupby_clause")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        [TypeConverter(typeof(EnumOptionConverter<LinePositionOption>))]
        public LinePositionOption GroupByClauseLinePosition { get; set; } = LinePositionOption.Alone;

        [Category("layout:type:orderby_clause")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        [TypeConverter(typeof(EnumOptionConverter<LinePositionOption>))]
        public LinePositionOption OrderByClauseLinePosition { get; set; } = LinePositionOption.Leading;

        [Category("layout:type:having_clause")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        [TypeConverter(typeof(EnumOptionConverter<LinePositionOption>))]
        public LinePositionOption HavingClauseLinePosition { get; set; } = LinePositionOption.Alone;

        [Category("layout:type:limit_clause")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        [TypeConverter(typeof(EnumOptionConverter<LinePositionOption>))]
        public LinePositionOption LimitClauseLinePosition { get; set; } = LinePositionOption.Alone;

        [Category("templater")]
        [DisplayName("Unwrap Wrapped Queries")]
        [Description("Unwrap Wrapped Queries.")]
        [SQLFluffField("unwrap_wrapped_queries")]
        public bool UnwrapWrappedQueries { get; set; } = true;

        [Category("templater:jinja")]
        [DisplayName("Apply DBT Builtins")]
        [Description("Use builtin dbt templater.")]
        [SQLFluffField("apply_dbt_builtins")]
        public bool ApplyDbtBuiltins { get; set; } = true;

        [Category("rules")]
        [DisplayName("Allow Scalar")]
        [Description("Allow Scalar.")]
        [SQLFluffField("allow_scalar")]
        public bool AllowScalar { get; set; } = true;

        [Category("rules")]
        [DisplayName("Single Table References")]
        [Description("Single Table References.")]
        [SQLFluffField("single_table_references")]
        [TypeConverter(typeof(EnumOptionConverter<ConsistentReferencesOption>))]
        public ConsistentReferencesOption SingleTableReferences { get; set; } = ConsistentReferencesOption.Consistent;

        [Category("rules")]
        [DisplayName("Unquoted Identifiers Policy")]
        [Description("Unquoted Identifiers Policy.")]
        [SQLFluffField("unquoted_identifiers_policy")]
        [TypeConverter(typeof(EnumOptionConverter<UnQuotedIdentifiersReferencesToFlagOption>))]
        public UnQuotedIdentifiersReferencesToFlagOption UnquotedIdentifiersPolicy { get; set; } = UnQuotedIdentifiersReferencesToFlagOption.All;

        [Category("rules:capitalisation.keywords")]
        [DisplayName("Capitalisation Policy")]
        [Description("Keywords.")]
        [SQLFluffField("capitalisation_policy")]
        [TypeConverter(typeof(EnumOptionConverter<StringCaseOption>))]
        public StringCaseOption KeywordsCapitalisationPolicy { get; set; } = StringCaseOption.Consistent;

        [Category("rules:capitalisation.keywords")]
        [DisplayName("Ignore Words")]
        [Description("Comma separated list of words to ignore for this rule.")]
        [SQLFluffField("ignore_words")]
        public string KeywordsIgnoreWords { get; set; } = null;

        [Category("rules:capitalisation.keywords")]
        [DisplayName("Ignore Words Regex")]
        [Description("Comma separated list of words to ignore for this rule.")]
        [SQLFluffField("ignore_words_regex")]
        public string KeywordsIgnoreWordsRegex { get; set; } = null;

        [Category("rules:capitalisation.identifiers")]
        [DisplayName("Extended Capitalisation Policy")]
        [Description("Unquoted identifiers.")]
        [SQLFluffField("extended_capitalisation_policy")]
        [TypeConverter(typeof(EnumOptionConverter<StringCasePascalOption>))]
        public StringCasePascalOption IdentifiersExtendedCapitalisationPolicy { get; set; } = StringCasePascalOption.Consistent;

        [Category("rules:capitalisation.identifiers")]
        [DisplayName("Unquoted Identifiers Policy")]
        [Description("Unquoted identifiers.")]
        [SQLFluffField("unquoted_identifiers_policy")]
        [TypeConverter(typeof(EnumOptionConverter<UnQuotedIdentifiersReferencesToFlagOption>))]
        public UnQuotedIdentifiersReferencesToFlagOption IdentifiersUnquotedIdentifiersPolicy { get; set; } = UnQuotedIdentifiersReferencesToFlagOption.All;

        [Category("rules:capitalisation.identifiers")]
        [DisplayName("Ignore Words")]
        [Description("Comma separated list of words to ignore for this rule.")]
        [SQLFluffField("ignore_words")]
        public string IdentifiersIgnoreWords { get; set; } = null;

        [Category("rules:capitalisation.identifiers")]
        [DisplayName("Ignore Words Regex")]
        [Description("Comma separated list of words to ignore for this rule.")]
        [SQLFluffField("ignore_words_regex")]
        public string IdentifiersIgnoreWordsRegex { get; set; } = null;

        [Category("rules:capitalisation.functions")]
        [DisplayName("Extended Capitalisation Policy")]
        [Description("Function names.")]
        [SQLFluffField("extended_capitalisation_policy")]
        [TypeConverter(typeof(EnumOptionConverter<StringCasePascalOption>))]
        public StringCasePascalOption FunctionsExtendedCapitalisationPolicy { get; set; } = StringCasePascalOption.Consistent;

        [Category("rules:capitalisation.functions")]
        [DisplayName("Ignore Words")]
        [Description("Comma separated list of words to ignore for this rule.")]
        [SQLFluffField("ignore_words")]
        public string FunctionsIgnoreWords { get; set; } = null;

        [Category("rules:capitalisation.functions")]
        [DisplayName("Ignore Words Regex")]
        [Description("Comma separated list of words to ignore for this rule.")]
        [SQLFluffField("ignore_words_regex")]
        public string FunctionsIgnoreWordsRegex { get; set; } = null;

        [Category("rules:capitalisation.literals")]
        [DisplayName("Capitalisation Policy")]
        [Description("Null & Boolean Literals.")]
        [SQLFluffField("capitalisation_policy")]
        [TypeConverter(typeof(EnumOptionConverter<StringCaseOption>))]
        public StringCaseOption LiteralsCapitalisationPolicy { get; set; } = StringCaseOption.Consistent;

        [Category("rules:capitalisation.literals")]
        [DisplayName("Ignore Words")]
        [Description("Comma separated list of words to ignore for this rule.")]
        [SQLFluffField("ignore_words")]
        public string LiteralsIgnoreWords { get; set; } = null;

        [Category("rules:capitalisation.literals")]
        [DisplayName("Ignore Words Regex")]
        [Description("Comma separated list of words to ignore for this rule.")]
        [SQLFluffField("ignore_words_regex")]
        public string LiteralsIgnoreWordsRegex { get; set; } = null;

        [Category("rules:capitalisation.types")]
        [DisplayName("Extended Capitalisation Policy")]
        [Description("Data Types.")]
        [SQLFluffField("extended_capitalisation_policy")]
        [TypeConverter(typeof(EnumOptionConverter<StringCasePascalOption>))] 
        public StringCasePascalOption TypesExtendedCapitalisationPolicy { get; set; } = StringCasePascalOption.Consistent;

        [Category("rules:capitalisation.types")]
        [DisplayName("Ignore Words")]
        [Description("Comma separated list of words to ignore for this rule.")]
        [SQLFluffField("ignore_words")]
        public string TypesIgnoreWords { get; set; } = null;

        [Category("rules:capitalisation.types")]
        [DisplayName("Ignore Words Regex")]
        [Description("Comma separated list of words to ignore for this rule.")]
        [SQLFluffField("ignore_words_regex")]
        public string TypesIgnoreWordsRegex { get; set; } = null;

        [Category("rules:ambiguous.join")]
        [DisplayName("Fully Qualify Join Types")]
        [Description("Fully qualify JOIN clause.")]
        [SQLFluffField("fully_qualify_join_types")]
        [TypeConverter(typeof(EnumOptionConverter<FullyQualifyJoinTypesOption>))]
        public FullyQualifyJoinTypesOption FullyQualifyJoinTypes { get; set; } = FullyQualifyJoinTypesOption.Inner;

        [Category("rules:ambiguous.column_references")]
        [DisplayName("Group By And Order By Style")]
        [Description("GROUP BY/ORDER BY column references.")]
        [SQLFluffField("group_by_and_order_by_style")]
        [TypeConverter(typeof(EnumOptionConverter<ColumnReferenceModalityOption>))]
        public ColumnReferenceModalityOption GroupByAndOrderByStyle { get; set; } = ColumnReferenceModalityOption.Consistent;

        [Category("rules:aliasing.table")]
        [DisplayName("Aliasing")]
        [Description("Aliasing preference for tables.")]
        [SQLFluffField("aliasing")]
        [TypeConverter(typeof(EnumOptionConverter<AliasModalityOption>))]
        public AliasModalityOption TableAliasing { get; set; } = AliasModalityOption.Explicit;

        [Category("rules:aliasing.column")]
        [DisplayName("Aliasing")]
        [Description("Aliasing preference for columns.")]
        [SQLFluffField("aliasing")]
        [TypeConverter(typeof(EnumOptionConverter<AliasModalityOption>))]
        public AliasModalityOption ColumnAliasing { get; set; } = AliasModalityOption.Explicit;

        [Category("rules:aliasing.length")]
        [DisplayName("Min Alias Length")]
        [Description("Min Alias Length.")]
        [SQLFluffField("min_alias_length")]
        public string MinAliasLength { get; set; } = null;

        [Category("rules:aliasing.length")]
        [DisplayName("Max Alias Length")]
        [Description("Max Alias Length.")]
        [SQLFluffField("max_alias_length")]
        public string MaxAliasLength { get; set; } = null;

        [Category("rules:aliasing.forbid")]
        [DisplayName("Force Enable")]
        [Description("Avoid table aliases in from clauses and join conditions. Disabled by default for all dialects unless explicitly enabled. We suggest instead using aliasing.length (AL06) in most cases.")]
        [SQLFluffField("force_enable")]
        public bool ForbidForceEnable { get; set; } = false;

        [Category("rules:convention.select_trailing_comma")]
        [DisplayName("Select Clause Trailing Comma")]
        [Description("Trailing commas.")]
        [SQLFluffField("select_clause_trailing_comma")]
        [TypeConverter(typeof(EnumOptionConverter<SelectTrailingCommaOption>))]
        public SelectTrailingCommaOption SelectClauseTrailingComma { get; set; } = SelectTrailingCommaOption.Forbid;

        [Category("rules:convention.count_rows")]
        [DisplayName("Prefer Count 1")]
        [Description("Consistent syntax to count all rows.")]
        [SQLFluffField("prefer_count_1")]
        public bool PreferCount1 { get; set; } = false;

        [Category("rules:convention.count_rows")]
        [DisplayName("Prefer Count 0")]
        [Description("Consistent syntax to count all rows.")]
        [SQLFluffField("prefer_count_0")]
        public bool PreferCount0 { get; set; } = false;

        [Category("rules:convention.terminator")]
        [DisplayName("Multiline Newline")]
        [Description("Semi-colon formatting approach.")]
        [SQLFluffField("multiline_newline")]
        public bool MultilineNewline { get; set; } = false;

        [Category("rules:convention.terminator")]
        [DisplayName("Require Final Semicolon")]
        [Description("Semi-colon formatting approach.")]
        [SQLFluffField("require_final_semicolon")]
        public bool RequireFinalSemicolon { get; set; } = false;

        [Category("rules:convention.blocked_words")]
        [DisplayName("Blocked Words")]
        [Description("Comma separated list of blocked words that should not be used.")]
        [SQLFluffField("blocked_words")]
        public string BlockedWords { get; set; } = null;

        [Category("rules:convention.blocked_words")]
        [DisplayName("Blocked Regex")]
        [Description("Comma separated list of blocked words that should not be used.")]
        [SQLFluffField("blocked_regex")]
        public string BlockedRegex { get; set; } = null;

        [Category("rules:convention.blocked_words")]
        [DisplayName("Match Source")]
        [Description("Comma separated list of blocked words that should not be used.")]
        [SQLFluffField("match_source")]
        public bool MatchSource { get; set; } = false;

        [Category("rules:convention.quoted_literals")]
        [DisplayName("Preferred Quoted Literal Style")]
        [Description("Consistent usage of preferred quotes for quoted literals.")]
        [SQLFluffField("preferred_quoted_literal_style")]
        [TypeConverter(typeof(EnumOptionConverter<QuotedLiteralsOption>))]
        public QuotedLiteralsOption PreferredQuotedLiteralStyle { get; set; } = QuotedLiteralsOption.Consistent;


        [Category("rules:convention.quoted_literals")]
        [DisplayName("Force Enable")]
        [Description("Disabled for dialects that do not support single and double quotes for quoted literals (e.g. Postgres).")]
        [SQLFluffField("force_enable")]
        public bool QuotedLiteralsForceEnable { get; set; } = false;

        [Category("rules:convention.casting_style")]
        [DisplayName("Preferred Type Casting Style")]
        [Description("SQL type casting.")]
        [SQLFluffField("preferred_type_casting_style")]
        [TypeConverter(typeof(EnumOptionConverter<CastingStyleOption>))]
        public CastingStyleOption PreferredTypeCastingStyle { get; set; } = CastingStyleOption.Consistent;

        [Category("rules:references.from")]
        [DisplayName("Force Enable")]
        [Description("References must be in FROM clause. Disabled for some dialects (e.g. bigquery).")]
        [SQLFluffField("force_enable")]
        public bool ReferencesFromForceEnable { get; set; } = false;

        [Category("rules:references.qualification")]
        [DisplayName("Ignore Words")]
        [Description("Comma separated list of words to ignore for this rule.")]
        [SQLFluffField("ignore_words")]
        public string ReferencesQualificationIgnoreWords { get; set; } = null;

        [Category("rules:references.qualification")]
        [DisplayName("Ignore Words Regex")]
        [Description("Comma separated list of words to ignore for this rule.")]
        [SQLFluffField("ignore_words_regex")]
        public string ReferencesQualificationIgnoreWordsRegex { get; set; } = null;

        [Category("rules:references.consistent")]
        [DisplayName("Force Enable")]
        [Description("References must be consistently used. Disabled for some dialects (e.g. bigquery).")]
        [SQLFluffField("force_enable")]
        public bool ReferencesConsistentForceEnable { get; set; } = false;

        [Category("rules:references.keywords")]
        [DisplayName("Unquoted Identifiers Policy")]
        [Description("Keywords should not be used as identifiers.")]
        [SQLFluffField("unquoted_identifiers_policy")]
        [TypeConverter(typeof(EnumOptionConverter<UnQuotedIdentifiersReferencesToFlagOption>))]
        public UnQuotedIdentifiersReferencesToFlagOption ReferencesKeywordsUnquotedIdentifiersPolicy { get; set; } = UnQuotedIdentifiersReferencesToFlagOption.All;

        [Category("rules:references.keywords")]
        [DisplayName("Quoted Identifiers Policy")]
        [Description("Keywords should not be used as identifiers.")]
        [SQLFluffField("quoted_identifiers_policy")]
        [TypeConverter(typeof(EnumOptionConverter<QuotedIdentifiersReferencesToFlagOption>))]
        public QuotedIdentifiersReferencesToFlagOption ReferencesKeywordsQuotedIdentifiersPolicy { get; set; } = QuotedIdentifiersReferencesToFlagOption.None;

        [Category("rules:references.keywords")]
        [DisplayName("Ignore Words")]
        [Description("Comma separated list of words to ignore for this rule.")]
        [SQLFluffField("ignore_words")]
        public string ReferencesKeywordsIgnoreWords { get; set; } = null;

        [Category("rules:references.keywords")]
        [DisplayName("Ignore Words Regex")]
        [Description("Comma separated list of words to ignore for this rule.")]
        [SQLFluffField("ignore_words_regex")]
        public string ReferencesKeywordsIgnoreWordsRegex { get; set; } = null;

        [Category("rules:references.special_chars")]
        [DisplayName("Unquoted Identifiers Policy")]
        [Description("Special characters in identifiers.")]
        [SQLFluffField("unquoted_identifiers_policy")]
        [TypeConverter(typeof(EnumOptionConverter<UnQuotedIdentifiersReferencesToFlagOption>))]
        public UnQuotedIdentifiersReferencesToFlagOption ReferencesSpecialCharsUnquotedIdentifiersPolicy { get; set; } = UnQuotedIdentifiersReferencesToFlagOption.All;

        [Category("rules:references.special_chars")]
        [DisplayName("Quoted Identifiers Policy")]
        [Description("Special characters in identifiers.")]
        [SQLFluffField("quoted_identifiers_policy")]
        [TypeConverter(typeof(EnumOptionConverter<QuotedIdentifiersReferencesToFlagOption>))]
        public QuotedIdentifiersReferencesToFlagOption ReferencesSpecialCharsQuotedIdentifiersPolicy { get; set; } = QuotedIdentifiersReferencesToFlagOption.All;

        [Category("rules:references.special_chars")]
        [DisplayName("Allow Space In Identifier")]
        [Description("Special characters in identifiers.")]
        [SQLFluffField("allow_space_in_identifier")]
        public bool AllowSpaceInIdentifier { get; set; } = false;

        [Category("rules:references.special_chars")]
        [DisplayName("Additional Allowed Characters")]
        [Description("Special characters in identifiers.")]
        [SQLFluffField("additional_allowed_characters")]
        public string AdditionalAllowedCharacters { get; set; } = null;

        [Category("rules:references.special_chars")]
        [DisplayName("Ignore Words")]
        [Description("Special characters in identifiers.")]
        [SQLFluffField("ignore_words")]
        public string ReferencesSpecialCharsIgnoreWords { get; set; } = null;

        [Category("rules:references.special_chars")]
        [DisplayName("Ignore Words Regex")]
        [Description("Special characters in identifiers.")]
        [SQLFluffField("ignore_words_regex")]
        public string ReferencesSpecialCharsIgnoreWordsRegex { get; set; } = null;

        [Category("rules:references.quoting")]
        [DisplayName("Prefer Quoted Identifiers")]
        [Description("Policy on quoted and unquoted identifiers.")]
        [SQLFluffField("prefer_quoted_identifiers")]
        public bool PreferQuotedIdentifiers { get; set; } = false;

        [Category("rules:references.quoting")]
        [DisplayName("Prefer Quoted Keywords")]
        [Description("Policy on quoted and unquoted identifiers.")]
        [SQLFluffField("prefer_quoted_keywords")]
        public bool PreferQuotedKeywords { get; set; } = false;

        [Category("rules:references.quoting")]
        [DisplayName("Ignore Words")]
        [Description("Policy on quoted and unquoted identifiers.")]
        [SQLFluffField("ignore_words")]
        public string ReferencesQuotingIgnoreWords { get; set; } = null;

        [Category("rules:references.quoting")]
        [DisplayName("Ignore Words Regex")]
        [Description("Policy on quoted and unquoted identifiers.")]
        [SQLFluffField("ignore_words_regex")]
        public string ReferencesQuotingIgnoreWordsRegex { get; set; } = null;

        [Category("rules:references.quoting")]
        [DisplayName("Force Enable")]
        [Description("Policy on quoted and unquoted identifiers.")]
        [SQLFluffField("force_enable")]
        public bool ReferencesQuotingForceEnable { get; set; } = false;

        [Category("rules:layout.long_lines")]
        [DisplayName("Ignore Comment Lines")]
        [Description("Line length.")]
        [SQLFluffField("ignore_comment_lines")]
        public bool IgnoreCommentLines { get; set; } = false;

        [Category("rules:layout.long_lines")]
        [DisplayName("Ignore Comment Clauses")]
        [Description("Line length.")]
        [SQLFluffField("ignore_comment_clauses")]
        public bool IgnoreCommentClauses { get; set; } = false;

        [Category("rules:layout.select_targets")]
        [DisplayName("Wildcard Policy")]
        [Description("Wildcard Policy.")]
        [SQLFluffField("wildcard_policy")]
        [TypeConverter(typeof(EnumOptionConverter<SelectTargetsOption>))]
        public SelectTargetsOption WildcardPolicy { get; set; } = SelectTargetsOption.Single;

        [Category("rules:structure.subquery")]
        [DisplayName("Forbid Subquery In")]
        [Description("By default, allow subqueries in from clauses, but not join clauses.")]
        [SQLFluffField("forbid_subquery_in")]
        [TypeConverter(typeof(EnumOptionConverter<JoinFromClauseOption>))]
        public JoinFromClauseOption ForbidSubqueryIn { get; set; } = JoinFromClauseOption.Join;

        [Category("rules:structure.join_condition_order")]
        [DisplayName("Preferred First Table In Join Clause")]
        [Description("Preferred First Table In Join Clause.")]
        [SQLFluffField("preferred_first_table_in_join_clause")]
        [TypeConverter(typeof(EnumOptionConverter<JoinConditionOrderOption>))]
        public JoinConditionOrderOption PreferredFirstTableInJoinClause { get; set; } = JoinConditionOrderOption.Earlier;

        public Dictionary<string, object> BuildConfiguration()
        {
            var config = new Dictionary<string, object>();

            // Create an instance of the options class with default values
            var defaultOptions = Activator.CreateInstance(GetType());

            foreach (var property in GetType().GetProperties())
            {
                var categoryAttribute = (CategoryAttribute)property.GetCustomAttributes(typeof(CategoryAttribute), false).FirstOrDefault();
                var displayNameAttribute = (DisplayNameAttribute)property.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault();
                var fluffFieldAttribute = (SQLFluffFieldAttribute)property.GetCustomAttributes(typeof(SQLFluffFieldAttribute), false).FirstOrDefault();

                if (categoryAttribute != null && (displayNameAttribute != null || fluffFieldAttribute != null))
                {
                    var categoryName = categoryAttribute.Category;
                    var propertyName = fluffFieldAttribute?.FieldName ?? displayNameAttribute?.DisplayName;

                    var value = property.GetValue(this);
                    var defaultValue = property.GetValue(defaultOptions);

                    if (categoryName != null && propertyName != null && value != null && !value.Equals(defaultValue))
                    {
                        var nestedKeys = categoryName.Split(':');

                        // Create nested structure based on colons
                        var nestedDict = config;
                        foreach (var key in nestedKeys)
                        {
                            if (!nestedDict.ContainsKey(key))
                            {
                                nestedDict[key] = new Dictionary<string, object>();
                            }

                            nestedDict = (Dictionary<string, object>)nestedDict[key];
                        }

                        // Check if the property type is an enum
                        if (property.PropertyType.IsEnum)
                        {
                            // Map enum value to its string representation
                            value = MapEnumOption((Enum)value);
                        }

                        // Assign the value to the last nested key
                        nestedDict[propertyName] = value;
                    }
                }
            }

            return config;
        }

        public string ConvertToCfgFormat(Dictionary<string, object> config)
        {
            StringBuilder cfgBuilder = new StringBuilder();

            foreach (var category in config)
            {
                cfgBuilder.AppendLine($"[{category.Key}]");

                var properties = (Dictionary<string, object>)category.Value;
                foreach (var property in properties)
                {
                    cfgBuilder.AppendLine($"{property.Key} = {property.Value}");
                }

                cfgBuilder.AppendLine();
            }

            return cfgBuilder.ToString();
        }

        public void resetOptions()
        {
            // Create an instance of the options class with default values
            var defaultOptions = Activator.CreateInstance(GetType());

            foreach (var property in GetType().GetProperties())
            {
                var categoryAttribute = (CategoryAttribute)property.GetCustomAttributes(typeof(CategoryAttribute), false).FirstOrDefault();
                if (categoryAttribute != null)
                {
                    var defaultValue = property.GetValue(defaultOptions);

                    property.SetValue(this, defaultValue);
                }
            }
        }

        public static string MapEnumOption<TEnum>(TEnum enumValue) where TEnum : Enum
        {
            // Check if the UseDescription attribute is applied to the enum type
            bool useDescription = Attribute.IsDefined(typeof(TEnum), typeof(UseDescriptionAttribute));

            if (useDescription)
            {
                // Get the field info for this type
                FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

                // Get the description attribute
                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                // Return the description if it exists
                if (attributes != null && attributes.Length > 0)
                    return attributes[0].Description.ToLower();
            }

            // Convert the enum value to its string representation in lowercase
            return enumValue.ToString().ToLower();
        }
    }
}
