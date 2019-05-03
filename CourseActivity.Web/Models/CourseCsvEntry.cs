using System;
using System.IO.Pipes;
using SimpleCsvParser;

namespace CourseActivity.Web.Models
{
    public class CourseCsvEntry
    {
        [CsvProperty("User ID")]
        public string UserId { get; set; }

        [CsvProperty("Display Name")]
        public string DisplayName { get; set; }

        [CsvProperty("Sortable Name")]
        public string SortableName { get; set; }

        [CsvProperty("Category")]
        public string Category { get; set; }

        [CsvProperty("Class")]
        public string Class { get; set; }

        [CsvProperty("Title")]
        public string Title { get; set; }

        [CsvProperty("Views")]
        public int? Views { get; set; }

        [CsvProperty("Participations")]
        public int? Participations { get; set; }

        [CsvProperty("Last Access")]
        public DateTime? LastAccess { get; set; }

        [CsvProperty("First Access")]
        public DateTime? FirstAccess { get; set; }

        [CsvProperty("Action")]
        public string Action { get; set; }

        [CsvProperty("Code")]
        public string Code { get; set; }

        [CsvProperty("Group Code")]
        public string GroupCode { get; set; }

        [CsvProperty("Context Type")]
        public string ContextType { get; set; }

        [CsvProperty("Context ID")]
        public string ContextId { get; set; }

        [CsvProperty("Login ID")]
        public string LoginId { get; set; }

        [CsvProperty("Section")]
        public string Section { get; set; }

        [CsvProperty("Section ID")]
        public string SectionId { get; set; }

        [CsvProperty("SIS Course ID")]
        public string SisCourseId { get; set; }

        [CsvProperty("SIS Section ID")]
        public string SisSectionId { get; set; }

        [CsvProperty("SIS User ID")]
        public string SisUserId { get; set; }
    }
}