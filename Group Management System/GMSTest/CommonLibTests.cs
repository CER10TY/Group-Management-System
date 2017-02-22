using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GMS;

namespace GMSTest
{
    [TestClass]
    public class CommonLibTests
    {
        [TestMethod]
        public void AddStudentsToClass_Test()
        {
            /*
             * Test Conditions:
             * Grade of Class and Student have to be equal for Student to successfully be added to Class.
             */
            // CommonLib
            var gms = new CommonLib();
            // Objects
            var classA = new Class()
            {
                ClassName = "1A",
                Grade = "1",
                Students = new List<Student>()
            };
            var student = new Student()
            {
                StudentName = "Alfred",
                Grade = "1"
            };
            var studentList = new List<Student> {student};

            gms.AddStudentsToClass(studentList, classA);
        }

        [TestMethod]
        public void AddStudentsToGroup_Test()
        {
            /*
             * Test Conditions:
             * Students.Count() may not exceed Max Students.
             * Grade of both Student and Group need to be equal.
             * Student may not be part of group if another group already has same subject
             */
            // CommonLib
            var gms = new CommonLib();
            // Objects
            var subject = new Subject()
            {
                SubjectName = "Maths",
                Grade = "2"
            };
            var group = new Group()
            {
                GroupName = "Maths 1",
                MaxStudents = 2,
                Students = new List<Student>(),
                Subjects = new List<Subject>() {subject},
                Grade = "2"
            };
            var studentA = new Student()
            {
                StudentName = "Alfred",
                Grade = "2",
                Groups = new List<Group>() {},
                Subjects = new List<Subject>() {subject}
            };
            var studentB = new Student()
            {
                StudentName = "James",
                Grade = "2",
                Groups = new List<Group>(),
                Subjects = new List<Subject>()
            };
            var studentList = new List<Student> {studentA, studentB};

            gms.AddStudentsToGroup(studentList, group);
        }

        [TestMethod]
        public void AddSubjectsToStudents_Test()
        {
            /*
             * Test Conditions:
             * Grade of Subject has to be equal to Grade of Student to successfully be added.
             * 
             * - This Unit Test intentionally includes a Grade error in Student Alfred.
             */
            // CommonLib
            var gms = new CommonLib();
            // Objects
            var subjectA = new Subject()
            {
                SubjectName = "Maths",
                Grade = "1"
            };
            var student = new Student()
            {
                StudentName = "Alfred",
                Grade = "1",
                Subjects = new List<Subject>()
            };
            var subjectList = new List<Subject> {subjectA};

            gms.AddSubjectsToStudent(subjectList, student);
        }

        [TestMethod]
        public void AddSubjectsToGroup_Test()
        {
            /*
             * Test Conditions:
             * Grade of Group and Subject has to be equal for Subject to be added to Group.
             */
            // CommonLib
            var gms = new CommonLib();
            // Objects
            var subjectA = new Subject()
            {
                SubjectName = "Maths",
                Grade = "1"
            };
            var subjectList = new List<Subject> {subjectA};
            var group = new Group()
            {
                Grade = "1",
                Subjects = new List<Subject>(),
                GroupName = "Maths 1"
            };

            gms.AddSubjectsToGroup(subjectList, group);
        }

        [TestMethod]
        public void PopulateNewGroup_Test()
        {
            /*
             * Test conditions:
             * Check maxStudent and minStudent so they work properly.
             */
            // CommonLib
            var gms = new CommonLib();
            // Objects
            var subject = new Subject()
            {
                SubjectName = "Maths 1",
                Grade = "1"
            };
            var studentA = new Student()
            {
                StudentName = "Alfred",
                Grade = "1",
                Subjects = new List<Subject> { subject },
                Groups = new List<Group>()
            };
            var classA = new Class()
            {
                ClassName = "1A",
                Grade = "1",
                Students = new List<Student> {studentA}
            };
            // Variables
            var subjectList = new List<Subject> {subject};
            var classList = new List<Class> {classA};
            var teachers = new List<string> {"Maria", "Anna"};
            const int minStudents = 0;
            const int maxStudents = 0;
            const string groupName = "Maths Group";

            gms.PopulateNewGroup(subjectList, minStudents, maxStudents, classList, teachers, groupName);
        }
    }
}
