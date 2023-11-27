using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDefuzzer
{
    public enum DialectOption
    {
        [Description("ansi Dialect [inherits from 'nothing']")]
        ansi,
        [Description("athena Dialect [inherits from 'ansi']")]
        athena,
        [Description("bigquery Dialect [inherits from 'ansi']")]
        bigquery,
        [Description("clickhouse Dialect [inherits from 'ansi']")]
        clickhouse,
        [Description("databricks Dialect [inherits from 'sparksql']")]
        databricks,
        [Description("db2 Dialect [inherits from 'ansi']")]
        db2,
        [Description("duckdb Dialect [inherits from 'postgres']")]
        duckdb,
        [Description("exasol Dialect [inherits from 'ansi']")]
        exasol,
        [Description("greenplum Dialect [inherits from 'postgres']")]
        greenplum,
        [Description("hive Dialect [inherits from 'ansi']")]
        hive,
        [Description("materialize Dialect [inherits from 'postgres']")]
        materialize,
        [Description("mysql Dialect [inherits from 'ansi']")]
        mysql,
        [Description("oracle Dialect [inherits from 'ansi']")]
        oracle,
        [Description("postgres Dialect [inherits from 'ansi']")]
        postgres,
        [Description("redshift Dialect [inherits from 'postgres']")]
        redshift,
        [Description("snowflake Dialect [inherits from 'ansi']")]
        snowflake,
        [Description("soql Dialect [inherits from 'ansi']")]
        soql,
        [Description("sparksql Dialect [inherits from 'ansi']")]
        sparksql,
        [Description("sqlite Dialect [inherits from 'ansi']")]
        sqlite,
        [Description("teradata Dialect [inherits from 'ansi']")]
        teradata,
        [Description("trino Dialect [inherits from 'ansi']")]
        trino,
        [Description("tsql Dialect [inherits from 'ansi']")]
        tsql
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
        public DialectOption Dialect { get; set; } = DialectOption.tsql;

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
        public string IndentUnit { get; set; } = "space";

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

        [Category("indentation")]
        [DisplayName("Trailing Comments")]
        [Description("Trailing Comments.")]
        [SQLFluffField("trailing_comments")]
        public string TrailingComments { get; set; } = "before";

        [Category("layout:type:comma")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        public string CommaSpacingBefore { get; set; } = "touch";

        [Category("layout:type:comma")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        public string CommaLinePosition { get; set; } = "trailing";

        [Category("layout:type:binary_operator")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        public string BinaryOperatorSpacingWithin { get; set; } = "touch";

        [Category("layout:type:binary_operator")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        public string BinaryOperatorLinePosition { get; set; } = "leading";

        [Category("layout:type:statement_terminator")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        public string StatementTerminatorSpacingBefore { get; set; } = "touch";

        [Category("layout:type:statement_terminator")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        public string StatementTerminatorLinePosition { get; set; } = "trailing";

        [Category("layout:type:end_of_file")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        public string EndOfFileSpacingBefore { get; set; } = "touch";

        [Category("layout:type:set_operator")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        public string SetOperatorLinePosition { get; set; } = "alone:strict";

        [Category("layout:type:start_bracket")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        public string StartBracketSpacingAfter { get; set; } = "touch";

        [Category("layout:type:end_bracket")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        public string EndBracketSpacingBefore { get; set; } = "touch";

        [Category("layout:type:start_square_bracket")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        public string StartSquareBracketSpacingAfter { get; set; } = "touch";

        [Category("layout:type:end_square_bracket")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        public string EndSquareBracketSpacingBefore { get; set; } = "touch";

        [Category("layout:type:start_angle_bracket")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        public string StartAngleBracketSpacingAfter { get; set; } = "touch";

        [Category("layout:type:end_angle_bracket")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        public string EndAngleBracketSpacingBefore { get; set; } = "touch";

        [Category("layout:type:casting_operator")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        public string CastingOperatorSpacingBefore { get; set; } = "touch";

        [Category("layout:type:casting_operator")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        public string CastingOperatorSpacingAfter { get; set; } = "touch:inline";

        [Category("layout:type:slice")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        public string SliceSpacingBefore { get; set; } = "touch";

        [Category("layout:type:slice")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        public string SliceSpacingAfter { get; set; } = "touch";

        [Category("layout:type:dot")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        public string DotSpacingBefore { get; set; } = "touch";

        [Category("layout:type:dot")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        public string DotSpacingAfter { get; set; } = "touch";

        [Category("layout:type:comparison_operator")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        public string ComparisonOperatorSpacingWithin { get; set; } = "touch";

        [Category("layout:type:comparison_operator")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        public string ComparisonOperatorLinePosition { get; set; } = "leading";

        [Category("layout:type:assignment_operator")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        public string AssignmentOperatorSpacingWithin { get; set; } = "touch";

        [Category("layout:type:assignment_operator")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        public string AssignmentOperatorLinePosition { get; set; } = "leading";

        [Category("layout:type:object_reference")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        public string ObjectReferenceSpacingWithin { get; set; } = "touch:inline";

        [Category("layout:type:numeric_literal")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        public string NumericLiteralSpacingWithin { get; set; } = "touch:inline";

        [Category("layout:type:sign_indicator")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        public string SignIndicatorSpacingAfter { get; set; } = "touch:inline";

        [Category("layout:type:tilde")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        public string TildeSpacingAfter { get; set; } = "touch:inline";

        [Category("layout:type:function_name")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        public string FunctionNameSpacingWithin { get; set; } = "touch:inline";

        [Category("layout:type:function_name")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        public string FunctionNameSpacingAfter { get; set; } = "touch:inline";

        [Category("layout:type:array_type")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        public string ArrayTypeSpacingWithin { get; set; } = "touch:inline";

        [Category("layout:type:typed_array_literal")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        public string TypedArrayLiteralSpacingWithin { get; set; } = "touch";

        [Category("layout:type:sized_array_type")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        public string SizedArrayTypeSpacingWithin { get; set; } = "touch";

        [Category("layout:type:struct_type")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        public string StructTypeSpacingWithin { get; set; } = "touch:inline";

        [Category("layout:type:bracketed_arguments")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        public string BracketedArgumentsSpacingBefore { get; set; } = "touch:inline";

        [Category("layout:type:typed_struct_literal")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        public string TypedStructLiteralSpacingWithin { get; set; } = "touch";

        [Category("layout:type:semi_structured_expression")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        public string SemiStructuredExpressionSpacingWithin { get; set; } = "touch:inline";

        [Category("layout:type:semi_structured_expression")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        public string SemiStructuredExpressionSpacingBefore { get; set; } = "touch:inline";

        [Category("layout:type:array_accessor")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        public string ArrayAccessorSpacingBefore { get; set; } = "touch:inline";

        [Category("layout:type:colon")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        public string ColonSpacingBefore { get; set; } = "touch";

        [Category("layout:type:colon_delimiter")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        public string ColonDelimiterSpacingBefore { get; set; } = "touch";

        [Category("layout:type:colon_delimiter")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        public string ColonDelimiterSpacingAfter { get; set; } = "touch";

        [Category("layout:type:path_segment")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        public string PathSegmentSpacingWithin { get; set; } = "touch";

        [Category("layout:type:sql_conf_option")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        public string SQLConfOptionSpacingWithin { get; set; } = "touch";

        [Category("layout:type:sqlcmd_operator")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        public string SQLCmdOperatorSpacingBefore { get; set; } = "touch";

        [Category("layout:type:comment")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        public string CommentSpacingBefore { get; set; } = "any";

        [Category("layout:type:comment")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        public string CommentSpacingAfter { get; set; } = "any";

        [Category("layout:type:pattern_expression")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        public string PatternExpressionSpacingWithin { get; set; } = "any";

        [Category("layout:type:placeholder")]
        [DisplayName("Spacing Before")]
        [SQLFluffField("spacing_before")]
        public string PlaceholderSpacingBefore { get; set; } = "any";

        [Category("layout:type:placeholder")]
        [DisplayName("Spacing After")]
        [SQLFluffField("spacing_after")]
        public string PlaceholderSpacingAfter { get; set; } = "any";

        [Category("layout:type:common_table_expression")]
        [DisplayName("Spacing Within")]
        [SQLFluffField("spacing_within")]
        public string CommonTableExpressionSpacingWithin { get; set; } = "single:inline";

        [Category("layout:type:select_clause")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        public string SelectClauseLinePosition { get; set; } = "alone";

        [Category("layout:type:where_clause")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        public string WhereClauseLinePosition { get; set; } = "alone";

        [Category("layout:type:from_clause")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        public string FromClauseLinePosition { get; set; } = "alone";

        [Category("layout:type:join_clause")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        public string JoinClauseLinePosition { get; set; } = "alone";

        [Category("layout:type:groupby_clause")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        public string GroupByClauseLinePosition { get; set; } = "alone";

        [Category("layout:type:orderby_clause")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        public string OrderByClauseLinePosition { get; set; } = "leading";

        [Category("layout:type:having_clause")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        public string HavingClauseLinePosition { get; set; } = "alone";

        [Category("layout:type:limit_clause")]
        [DisplayName("Line Position")]
        [SQLFluffField("line_position")]
        public string LimitClauseLinePosition { get; set; } = "alone";

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
        public string SingleTableReferences { get; set; } = "consistent";

        [Category("rules")]
        [DisplayName("Unquoted Identifiers Policy")]
        [Description("Unquoted Identifiers Policy.")]
        [SQLFluffField("unquoted_identifiers_policy")]
        public string UnquotedIdentifiersPolicy { get; set; } = "all";

        [Category("rules:capitalisation.keywords")]
        [DisplayName("Capitalisation Policy")]
        [Description("Keywords.")]
        [SQLFluffField("capitalisation_policy")]
        public string KeywordsCapitalisationPolicy { get; set; } = "consistent";

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
        public string IdentifiersExtendedCapitalisationPolicy { get; set; } = "consistent";

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
        public string FunctionsExtendedCapitalisationPolicy { get; set; } = "consistent";

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
        public string LiteralsCapitalisationPolicy { get; set; } = "consistent";

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
        public string TypesExtendedCapitalisationPolicy { get; set; } = "consistent";

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
        public string FullyQualifyJoinTypes { get; set; } = "inner";

        [Category("rules:ambiguous.column_references")]
        [DisplayName("Group By And Order By Style")]
        [Description("GROUP BY/ORDER BY column references.")]
        [SQLFluffField("group_by_and_order_by_style")]
        public string GroupByAndOrderByStyle { get; set; } = "consistent";

        [Category("rules:aliasing.table")]
        [DisplayName("Aliasing")]
        [Description("Aliasing preference for tables.")]
        [SQLFluffField("aliasing")]
        public string TableAliasing { get; set; } = "explicit";

        [Category("rules:aliasing.column")]
        [DisplayName("Aliasing")]
        [Description("Aliasing preference for columns.")]
        [SQLFluffField("aliasing")]
        public string ColumnAliasing { get; set; } = "explicit";

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
        public string SelectClauseTrailingComma { get; set; } = "forbid";

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
        public string PreferredQuotedLiteralStyle { get; set; } = "consistent";

        [Category("rules:convention.quoted_literals")]
        [DisplayName("Force Enable")]
        [Description("Disabled for dialects that do not support single and double quotes for quoted literals (e.g. Postgres).")]
        [SQLFluffField("force_enable")]
        public bool QuotedLiteralsForceEnable { get; set; } = false;

        [Category("rules:convention.casting_style")]
        [DisplayName("Preferred Type Casting Style")]
        [Description("SQL type casting.")]
        [SQLFluffField("preferred_type_casting_style")]
        public string PreferredTypeCastingStyle { get; set; } = "consistent";

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
        public string ReferencesKeywordsUnquotedIdentifiersPolicy { get; set; } = "aliases";

        [Category("rules:references.keywords")]
        [DisplayName("Quoted Identifiers Policy")]
        [Description("Keywords should not be used as identifiers.")]
        [SQLFluffField("quoted_identifiers_policy")]
        public string ReferencesKeywordsQuotedIdentifiersPolicy { get; set; } = "none";

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
        public string ReferencesSpecialCharsUnquotedIdentifiersPolicy { get; set; } = "all";

        [Category("rules:references.special_chars")]
        [DisplayName("Quoted Identifiers Policy")]
        [Description("Special characters in identifiers.")]
        [SQLFluffField("quoted_identifiers_policy")]
        public string ReferencesSpecialCharsQuotedIdentifiersPolicy { get; set; } = "all";

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
        public string WildcardPolicy { get; set; } = "single";

        [Category("rules:structure.subquery")]
        [DisplayName("Forbid Subquery In")]
        [Description("By default, allow subqueries in from clauses, but not join clauses.")]
        [SQLFluffField("forbid_subquery_in")]
        public string ForbidSubqueryIn { get; set; } = "join";

        [Category("rules:structure.join_condition_order")]
        [DisplayName("Preferred First Table In Join Clause")]
        [Description("Preferred First Table In Join Clause.")]
        [SQLFluffField("preferred_first_table_in_join_clause")]
        public string PreferredFirstTableInJoinClause { get; set; } = "earlier";

        public object BuildConfiguration()
        {
            var config = new Dictionary<string, object>();

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

                    if (categoryName != null && propertyName != null && value != null)
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

        private static string MapEnumOption<TEnum>(TEnum enumValue) where TEnum : Enum
        {
            // Convert the enum value to its string representation in lowercase
            return enumValue.ToString().ToLower();
        }
    }
}
